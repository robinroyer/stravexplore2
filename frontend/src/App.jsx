import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import HomePage from './pages/HomePage';
import ExplorerPage from './pages/ExplorerPage';
import AnalyticsPage from './pages/AnalyticsPage';
import RouteRecommender from './components/RouteRecommender';
import TrendsDashboard from './components/TrendsDashboard';
import EventFinder from './components/EventFinder';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 1,
      staleTime: 5 * 60 * 1000, // 5 minutes
    },
  },
});

function Navigation() {
  return (
    <nav className="bg-orange-600 text-white shadow-lg">
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-between h-16">
          <Link to="/" className="text-xl font-bold">
            Strava Explorer
          </Link>
          <div className="flex space-x-4">
            <Link to="/explorer" className="hover:bg-orange-700 px-3 py-2 rounded">
              Explorer
            </Link>
            <Link to="/analytics" className="hover:bg-orange-700 px-3 py-2 rounded">
              Analytics
            </Link>
            <Link to="/routes" className="hover:bg-orange-700 px-3 py-2 rounded">
              Routes
            </Link>
            <Link to="/trends" className="hover:bg-orange-700 px-3 py-2 rounded">
              Trends
            </Link>
            <Link to="/events" className="hover:bg-orange-700 px-3 py-2 rounded">
              Events
            </Link>
          </div>
        </div>
      </div>
    </nav>
  );
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <Router>
        <div className="min-h-screen bg-gray-50">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route
              path="/explorer"
              element={
                <>
                  <Navigation />
                  <ExplorerPage />
                </>
              }
            />
            <Route
              path="/analytics"
              element={
                <>
                  <Navigation />
                  <AnalyticsPage />
                </>
              }
            />
            <Route
              path="/routes"
              element={
                <>
                  <Navigation />
                  <RouteRecommender />
                </>
              }
            />
            <Route
              path="/trends"
              element={
                <>
                  <Navigation />
                  <TrendsDashboard />
                </>
              }
            />
            <Route
              path="/events"
              element={
                <>
                  <Navigation />
                  <EventFinder />
                </>
              }
            />
          </Routes>
        </div>
      </Router>
    </QueryClientProvider>
  );
}

export default App;
