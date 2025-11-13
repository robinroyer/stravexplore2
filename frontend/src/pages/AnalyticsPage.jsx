import { useState } from 'react';
import { useAnalyzeByDifficulty } from '../hooks/useSegments';
import Map from '../components/Map';

export default function AnalyticsPage() {
  const [bounds, setBounds] = useState(null);
  const [activityType, setActivityType] = useState('Ride');
  const [difficulty, setDifficulty] = useState('');

  const { data: segments, isLoading } = useAnalyzeByDifficulty(
    bounds,
    activityType,
    difficulty || undefined,
    { enabled: !!bounds }
  );

  const getDifficultyColor = (level) => {
    const colors = {
      Easy: 'bg-green-100 text-green-800',
      Moderate: 'bg-yellow-100 text-yellow-800',
      Hard: 'bg-orange-100 text-orange-800',
      'Very Hard': 'bg-red-100 text-red-800',
      Extreme: 'bg-purple-100 text-purple-800',
    };
    return colors[level] || 'bg-gray-100 text-gray-800';
  };

  const markers = segments?.map(segment => ({
    id: segment.id,
    position: segment.startLatlng || [48.8566, 2.3522],
    data: segment
  })) || [];

  return (
    <div className="h-screen flex flex-col">
      <div className="bg-white shadow-md p-4">
        <h1 className="text-2xl font-bold mb-4">Difficulty Analyzer</h1>
        <div className="flex gap-4">
          <div>
            <label className="block text-sm font-medium mb-1">Activity Type</label>
            <select
              value={activityType}
              onChange={(e) => setActivityType(e.target.value)}
              className="border rounded px-3 py-2"
            >
              <option value="Ride">Ride</option>
              <option value="Run">Run</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium mb-1">Difficulty</label>
            <select
              value={difficulty}
              onChange={(e) => setDifficulty(e.target.value)}
              className="border rounded px-3 py-2"
            >
              <option value="">All</option>
              <option value="Easy">Easy</option>
              <option value="Moderate">Moderate</option>
              <option value="Hard">Hard</option>
              <option value="Very Hard">Very Hard</option>
              <option value="Extreme">Extreme</option>
            </select>
          </div>
        </div>
      </div>

      <div className="flex-1 flex">
        <div className="flex-1">
          <Map
            center={[48.8566, 2.3522]}
            zoom={12}
            markers={markers}
            onBoundsChange={setBounds}
            height="100%"
          />
        </div>

        <div className="w-96 bg-white shadow-lg p-6 overflow-y-auto">
          <h2 className="text-xl font-bold mb-4">Segments by Difficulty</h2>

          {isLoading && <p>Loading...</p>}

          {segments && segments.length > 0 ? (
            <div className="space-y-3">
              {segments.map((segment) => (
                <div key={segment.id} className="border rounded p-3">
                  <h3 className="font-semibold mb-2">{segment.name}</h3>
                  <div className="space-y-1 text-sm mb-2">
                    <p>Distance: {(segment.distance / 1000).toFixed(2)} km</p>
                    <p>Avg Grade: {segment.averageGrade?.toFixed(1)}%</p>
                    <p>Max Grade: {segment.maximumGrade?.toFixed(1)}%</p>
                    <p>Score: {segment.difficultyScore?.toFixed(1)}</p>
                  </div>
                  <span className={`inline-block px-2 py-1 rounded text-xs font-medium ${getDifficultyColor(segment.difficultyLevel)}`}>
                    {segment.difficultyLevel}
                  </span>
                </div>
              ))}
            </div>
          ) : (
            !isLoading && <p className="text-gray-500">Move the map to explore segments</p>
          )}
        </div>
      </div>
    </div>
  );
}
