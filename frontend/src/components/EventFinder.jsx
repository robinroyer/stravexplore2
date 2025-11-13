import { useState } from 'react';
import { useEventSegments } from '../hooks/useTrends';
import Map from './Map';

export default function EventFinder() {
  const [bounds, setBounds] = useState(null);
  const [activityType, setActivityType] = useState('All');

  const { data: events, isLoading } = useEventSegments(bounds, {
    enabled: !!bounds
  });

  const filteredEvents = events?.filter(
    event => activityType === 'All' || event.activityType === activityType
  ) || [];

  const markers = filteredEvents.map(event => ({
    id: event.segmentId,
    position: event.startLocation || [48.8566, 2.3522],
    popup: (
      <div className="p-2">
        <h3 className="font-bold">{event.name}</h3>
        <p className="text-sm">Participants: {event.estimatedParticipants}</p>
        <p className="text-sm">Stars: {event.starCount}</p>
      </div>
    ),
    data: event
  }));

  return (
    <div className="h-screen flex flex-col">
      <div className="bg-white shadow-md p-4">
        <h1 className="text-2xl font-bold mb-4">Event Finder</h1>
        <div className="flex gap-4">
          <div>
            <label className="block text-sm font-medium mb-1">Activity Type</label>
            <select
              value={activityType}
              onChange={(e) => setActivityType(e.target.value)}
              className="border rounded px-3 py-2"
            >
              <option value="All">All</option>
              <option value="Ride">Ride</option>
              <option value="Run">Run</option>
            </select>
          </div>
          <div className="flex items-end">
            <div className="text-sm">
              {isLoading && <span className="text-blue-600">Loading events...</span>}
              {filteredEvents.length > 0 && (
                <span className="text-green-600">Found {filteredEvents.length} popular segments</span>
              )}
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
            onBoundsChange={setBounds}
            height="100%"
          />
        </div>

        <div className="w-96 bg-white shadow-lg p-6 overflow-y-auto">
          <h2 className="text-xl font-bold mb-4">Popular Segments / Events</h2>

          {isLoading && <p>Loading...</p>}

          {filteredEvents.length > 0 ? (
            <div className="space-y-4">
              {filteredEvents.map((event) => (
                <div key={event.segmentId} className="border rounded-lg p-4 hover:shadow-md transition-shadow">
                  <h3 className="font-bold text-lg mb-2">{event.name}</h3>

                  <div className="space-y-2 mb-3">
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-600">Distance:</span>
                      <span className="font-medium">{(event.distance / 1000).toFixed(2)} km</span>
                    </div>
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-600">Est. Participants:</span>
                      <span className="font-medium">{event.estimatedParticipants}</span>
                    </div>
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-600">PR Count:</span>
                      <span className="font-medium">{event.prCount}</span>
                    </div>
                    <div className="flex justify-between text-sm">
                      <span className="text-gray-600">Stars:</span>
                      <span className="font-medium">{event.starCount}</span>
                    </div>
                  </div>

                  <div className="flex gap-2">
                    <span className="text-xs bg-blue-100 text-blue-800 px-2 py-1 rounded">
                      {event.activityType}
                    </span>
                    {event.city && (
                      <span className="text-xs bg-gray-100 text-gray-800 px-2 py-1 rounded">
                        {event.city}
                      </span>
                    )}
                  </div>
                </div>
              ))}
            </div>
          ) : (
            !isLoading && <p className="text-gray-500">Move the map to find popular segments</p>
          )}
        </div>
      </div>
    </div>
  );
}
