import { useState } from 'react';
import Map from '../components/Map';
import { useExploreSegments, useSegmentLeaderboard } from '../hooks/useSegments';

export default function ExplorerPage() {
  const [bounds, setBounds] = useState(null);
  const [activityType, setActivityType] = useState('Ride');
  const [selectedSegment, setSelectedSegment] = useState(null);

  const { data: segments, isLoading, error } = useExploreSegments(
    bounds,
    activityType,
    null,
    null,
    { enabled: !!bounds }
  );

  const { data: leaderboard } = useSegmentLeaderboard(
    selectedSegment?.id,
    1,
    10,
    { enabled: !!selectedSegment }
  );

  const markers = segments?.map(segment => ({
    id: segment.id,
    position: segment.startLatlng || [48.8566, 2.3522],
    popup: (
      <div className="p-2">
        <h3 className="font-bold text-lg">{segment.name}</h3>
        <p className="text-sm">Distance: {(segment.distance / 1000).toFixed(2)} km</p>
        <p className="text-sm">Avg Grade: {segment.avgGrade?.toFixed(1)}%</p>
        <p className="text-sm">Stars: {segment.starCount}</p>
      </div>
    ),
    data: segment
  })) || [];

  const handleMarkerClick = (marker) => {
    setSelectedSegment(marker.data);
  };

  return (
    <div className="h-screen flex flex-col">
      <div className="bg-white shadow-md p-4">
        <h1 className="text-2xl font-bold mb-4">Segment Explorer</h1>
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
          <div className="flex items-end">
            <div className="text-sm">
              {isLoading && <span className="text-blue-600">Loading segments...</span>}
              {error && <span className="text-red-600">Error loading segments</span>}
              {segments && <span className="text-green-600">Found {segments.length} segments</span>}
            </div>
          </div>
        </div>
      </div>

      <div className="flex-1 flex">
        <div className="flex-1">
          <Map
            center={[48.8566, 2.3522]}
            zoom={12}
            markers={markers}
            onMarkerClick={handleMarkerClick}
            onBoundsChange={setBounds}
            height="100%"
          />
        </div>

        {selectedSegment && (
          <div className="w-96 bg-white shadow-lg p-6 overflow-y-auto">
            <h2 className="text-xl font-bold mb-4">{selectedSegment.name}</h2>

            <div className="mb-6">
              <h3 className="font-semibold mb-2">Segment Details</h3>
              <div className="space-y-2 text-sm">
                <p><span className="font-medium">Distance:</span> {(selectedSegment.distance / 1000).toFixed(2)} km</p>
                <p><span className="font-medium">Avg Grade:</span> {selectedSegment.avgGrade?.toFixed(1)}%</p>
                <p><span className="font-medium">Elevation:</span> {selectedSegment.elevDifference?.toFixed(0)} m</p>
                <p><span className="font-medium">Category:</span> {selectedSegment.climbCategoryDesc}</p>
                <p><span className="font-medium">Stars:</span> {selectedSegment.starCount}</p>
              </div>
            </div>

            {leaderboard && (
              <div>
                <h3 className="font-semibold mb-2">Leaderboard</h3>
                <div className="space-y-2">
                  {leaderboard.entries?.map((entry, index) => (
                    <div key={index} className="border-b pb-2">
                      <div className="flex justify-between items-center">
                        <span className="font-medium">#{entry.rank} {entry.athleteName}</span>
                        <span className="text-sm text-gray-600">
                          {Math.floor(entry.elapsedTime / 60)}:{String(entry.elapsedTime % 60).padStart(2, '0')}
                        </span>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>
        )}
      </div>
    </div>
  );
}
