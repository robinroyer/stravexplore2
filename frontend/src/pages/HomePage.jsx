import { Link } from 'react-router-dom';

export default function HomePage() {
  const features = [
    {
      title: 'Segment Explorer',
      description: 'Discover and explore Strava segments on an interactive map. View leaderboards and segment details.',
      link: '/explorer',
      icon: '🗺️'
    },
    {
      title: 'Difficulty Analyzer',
      description: 'Analyze segments by difficulty level. Filter by grade, elevation, and challenge rating.',
      link: '/analytics',
      icon: '📊'
    },
    {
      title: 'Route Recommender',
      description: 'Find popular routes in your area. Get recommendations based on distance and elevation.',
      link: '/routes',
      icon: '🚴'
    },
    {
      title: 'Trends Dashboard',
      description: 'Track trending segments and view popularity analytics over time.',
      link: '/trends',
      icon: '📈'
    },
    {
      title: 'Event Finder',
      description: 'Discover popular segments and potential event locations based on activity.',
      link: '/events',
      icon: '🏆'
    }
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-orange-50 to-orange-100">
      <div className="container mx-auto px-4 py-12">
        <div className="text-center mb-16">
          <h1 className="text-5xl font-bold text-gray-800 mb-4">
            Strava Explorer
          </h1>
          <p className="text-xl text-gray-600 max-w-2xl mx-auto">
            Explore, analyze, and discover Strava segments, routes, and trends with powerful visualization tools.
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 mb-12">
          {features.map((feature, index) => (
            <Link
              key={index}
              to={feature.link}
              className="bg-white rounded-lg shadow-lg p-8 hover:shadow-xl transition-shadow duration-300 transform hover:-translate-y-1"
            >
              <div className="text-5xl mb-4">{feature.icon}</div>
              <h2 className="text-2xl font-bold text-gray-800 mb-3">
                {feature.title}
              </h2>
              <p className="text-gray-600">
                {feature.description}
              </p>
              <div className="mt-4 text-orange-600 font-medium">
                Explore →
              </div>
            </Link>
          ))}
        </div>

        <div className="bg-white rounded-lg shadow-lg p-8 max-w-4xl mx-auto">
          <h2 className="text-2xl font-bold text-gray-800 mb-4">About This Application</h2>
          <p className="text-gray-600 mb-4">
            This application consumes public data from the Strava API to provide insights into segments, routes, and cycling/running trends. Built with modern web technologies:
          </p>
          <div className="grid grid-cols-2 gap-4 text-sm">
            <div>
              <h3 className="font-semibold mb-2">Backend</h3>
              <ul className="space-y-1 text-gray-600">
                <li>• .NET 8 Core Web API</li>
                <li>• Memory Caching</li>
                <li>• Polly Retry Policies</li>
              </ul>
            </div>
            <div>
              <h3 className="font-semibold mb-2">Frontend</h3>
              <ul className="space-y-1 text-gray-600">
                <li>• React 18 with Vite</li>
                <li>• TailwindCSS</li>
                <li>• React Query & Leaflet</li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
