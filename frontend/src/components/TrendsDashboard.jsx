import { useState } from 'react';
import { useTrendingSegments, useSegmentTrend } from '../hooks/useTrends';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

export default function TrendsDashboard() {
  const [region, setRegion] = useState('paris');
  const [selectedSegmentId, setSelectedSegmentId] = useState(null);

  const { data: trendingSegments, isLoading } = useTrendingSegments(region, 20);
  const { data: segmentTrend } = useSegmentTrend(selectedSegmentId, {
    enabled: !!selectedSegmentId
  });

  return (
    <div className="h-screen flex flex-col p-6">
      <div className="bg-white shadow-md rounded p-6 mb-6">
        <h1 className="text-2xl font-bold mb-4">Trends Dashboard</h1>

        <div className="flex gap-4">
          <div>
            <label className="block text-sm font-medium mb-1">Region</label>
            <select
              value={region}
              onChange={(e) => setRegion(e.target.value)}
              className="border rounded px-3 py-2"
            >
              <option value="paris">Paris</option>
              <option value="lyon">Lyon</option>
              <option value="marseille">Marseille</option>
            </select>
          </div>
        </div>
      </div>

      <div className="flex-1 flex gap-6 overflow-hidden">
        <div className="w-1/2 bg-white shadow-md rounded p-6 overflow-y-auto">
          <h2 className="text-xl font-bold mb-4">Top Trending Segments</h2>

          {isLoading && <p>Loading...</p>}

          {trendingSegments && trendingSegments.length > 0 ? (
            <div className="space-y-3">
              {trendingSegments.map((segment, index) => (
                <div
                  key={segment.segmentId}
                  className={`border rounded p-3 cursor-pointer transition-colors ${
                    selectedSegmentId === segment.segmentId
                      ? 'bg-blue-50 border-blue-300'
                      : 'hover:bg-gray-50'
                  }`}
                  onClick={() => setSelectedSegmentId(segment.segmentId)}
                >
                  <div className="flex justify-between items-start mb-2">
                    <h3 className="font-semibold">
                      #{index + 1} {segment.name}
                    </h3>
                    <span className="text-xs bg-green-100 text-green-800 px-2 py-1 rounded">
                      Score: {segment.trendScore?.toFixed(1)}
                    </span>
                  </div>
                  <div className="grid grid-cols-2 gap-2 text-sm text-gray-600">
                    <p>Distance: {(segment.distance / 1000).toFixed(2)} km</p>
                    <p>Grade: {segment.averageGrade?.toFixed(1)}%</p>
                    <p>Stars: {segment.starCount}</p>
                    <p>Efforts: {segment.effortCount}</p>
                  </div>
                  {segment.city && (
                    <p className="text-xs text-gray-500 mt-2">
                      {segment.city}, {segment.country}
                    </p>
                  )}
                </div>
              ))}
            </div>
          ) : (
            !isLoading && <p className="text-gray-500">No trending segments found</p>
          )}
        </div>

        <div className="w-1/2 bg-white shadow-md rounded p-6">
          <h2 className="text-xl font-bold mb-4">Segment Trend Analysis</h2>

          {selectedSegmentId ? (
            segmentTrend ? (
              <div>
                <div className="mb-4">
                  <h3 className="font-semibold text-lg">{segmentTrend.name}</h3>
                  <p className="text-sm text-gray-600">
                    Overall Trend: {segmentTrend.overallTrend?.toFixed(1)}%
                  </p>
                </div>

                <ResponsiveContainer width="100%" height={300}>
                  <LineChart data={segmentTrend.trendData}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis
                      dataKey="date"
                      tickFormatter={(date) => new Date(date).toLocaleDateString()}
                    />
                    <YAxis />
                    <Tooltip
                      labelFormatter={(date) => new Date(date).toLocaleDateString()}
                    />
                    <Legend />
                    <Line
                      type="monotone"
                      dataKey="effortCount"
                      stroke="#8884d8"
                      name="Efforts"
                    />
                    <Line
                      type="monotone"
                      dataKey="athleteCount"
                      stroke="#82ca9d"
                      name="Athletes"
                    />
                  </LineChart>
                </ResponsiveContainer>

                <div className="mt-6 grid grid-cols-2 gap-4">
                  <div className="border rounded p-3">
                    <p className="text-sm text-gray-600">Total Efforts (30 days)</p>
                    <p className="text-2xl font-bold">
                      {segmentTrend.trendData?.reduce((sum, d) => sum + d.effortCount, 0) || 0}
                    </p>
                  </div>
                  <div className="border rounded p-3">
                    <p className="text-sm text-gray-600">Unique Athletes (30 days)</p>
                    <p className="text-2xl font-bold">
                      {segmentTrend.trendData?.reduce((sum, d) => sum + d.athleteCount, 0) || 0}
                    </p>
                  </div>
                </div>
              </div>
            ) : (
              <p>Loading trend data...</p>
            )
          ) : (
            <p className="text-gray-500">Select a segment to view trend analysis</p>
          )}
        </div>
      </div>
    </div>
  );
}
