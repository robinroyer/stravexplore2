import { useState } from 'react';
import { usePopularRoutes } from '../hooks/useRoutes';

export default function RouteRecommender() {
  const [location, setLocation] = useState({ lat: 48.8566, lng: 2.3522 });
  const [radius, setRadius] = useState(10);

  const { data: routes, isLoading } = usePopularRoutes(
    location.lat,
    location.lng,
    radius
  );

  return (
    <div className="h-screen flex flex-col p-6">
      <div className="bg-white shadow-md rounded p-6 mb-6">
        <h1 className="text-2xl font-bold mb-4">Route Recommender</h1>

        <div className="flex gap-4 mb-4">
          <div>
            <label className="block text-sm font-medium mb-1">Latitude</label>
            <input
              type="number"
              value={location.lat}
              onChange={(e) => setLocation({ ...location, lat: parseFloat(e.target.value) })}
              className="border rounded px-3 py-2 w-32"
              step="0.0001"
            />
          </div>
          <div>
            <label className="block text-sm font-medium mb-1">Longitude</label>
            <input
              type="number"
              value={location.lng}
              onChange={(e) => setLocation({ ...location, lng: parseFloat(e.target.value) })}
              className="border rounded px-3 py-2 w-32"
              step="0.0001"
            />
          </div>
          <div>
            <label className="block text-sm font-medium mb-1">Radius (km)</label>
            <input
              type="number"
              value={radius}
              onChange={(e) => setRadius(parseInt(e.target.value))}
              className="border rounded px-3 py-2 w-24"
              min="1"
              max="50"
            />
          </div>
        </div>
      </div>

      <div className="flex-1 bg-white shadow-md rounded p-6 overflow-y-auto">
        <h2 className="text-xl font-bold mb-4">Popular Routes</h2>

        {isLoading && <p>Loading routes...</p>}

        {routes && routes.length > 0 ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {routes.map((route) => (
              <div key={route.id} className="border rounded-lg p-4 hover:shadow-lg transition-shadow">
                <h3 className="font-bold text-lg mb-2">{route.name}</h3>
                {route.description && (
                  <p className="text-sm text-gray-600 mb-3">{route.description}</p>
                )}
                <div className="space-y-1 text-sm mb-3">
                  <p><span className="font-medium">Distance:</span> {(route.distance / 1000).toFixed(2)} km</p>
                  <p><span className="font-medium">Elevation Gain:</span> {route.elevationGain?.toFixed(0)} m</p>
                  <p><span className="font-medium">Type:</span> {route.type}</p>
                  <p><span className="font-medium">Users:</span> {route.userCount}</p>
                </div>
                <div className="flex justify-between items-center">
                  <span className="text-xs bg-blue-100 text-blue-800 px-2 py-1 rounded">
                    {route.difficultyLevel}
                  </span>
                  <span className="text-xs text-gray-500">
                    Score: {route.popularityScore?.toFixed(1)}
                  </span>
                </div>
              </div>
            ))}
          </div>
        ) : (
          !isLoading && <p className="text-gray-500">No routes found in this area</p>
        )}
      </div>
    </div>
  );
}
