import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export const stravaClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Segments API
export const segmentsApi = {
  explore: (bounds, activityType = 'Ride', minCat, maxCat) =>
    stravaClient.get('/segments/explore', {
      params: {
        minLat: bounds[0],
        minLng: bounds[1],
        maxLat: bounds[2],
        maxLng: bounds[3],
        activityType,
        minCat,
        maxCat,
      },
    }),

  getDetails: (segmentId) =>
    stravaClient.get(`/segments/${segmentId}/details`),

  getLeaderboard: (segmentId, page = 1, pageSize = 10) =>
    stravaClient.get(`/segments/${segmentId}/leaderboard`, {
      params: { page, pageSize },
    }),

  analyzeByDifficulty: (bounds, activityType = 'Ride', difficulty) =>
    stravaClient.get('/segments/difficulty-analyzer', {
      params: {
        minLat: bounds[0],
        minLng: bounds[1],
        maxLat: bounds[2],
        maxLng: bounds[3],
        activityType,
        difficulty,
      },
    }),

  compare: (segmentIds) =>
    stravaClient.post('/segments/compare', segmentIds),
};

// Routes API
export const routesApi = {
  getPopular: (lat, lng, radiusKm = 10) =>
    stravaClient.get('/routes/popular', {
      params: { lat, lng, radiusKm },
    }),

  getDetails: (routeId) =>
    stravaClient.get(`/routes/${routeId}/details`),

  getStats: (routeId) =>
    stravaClient.get(`/routes/${routeId}/stats`),

  filter: (bounds, minElevation = 0, maxElevation = 5000) =>
    stravaClient.get('/routes/filter', {
      params: {
        minLat: bounds[0],
        minLng: bounds[1],
        maxLat: bounds[2],
        maxLng: bounds[3],
        minElevation,
        maxElevation,
      },
    }),
};

// Trends API
export const trendsApi = {
  getTrendingSegments: (region = 'paris', topN = 20) =>
    stravaClient.get('/trends/segments', {
      params: { region, topN },
    }),

  getSegmentTrend: (segmentId) =>
    stravaClient.get(`/trends/segment/${segmentId}/history`),

  findEvents: (bounds) =>
    stravaClient.get('/trends/events', {
      params: {
        minLat: bounds[0],
        minLng: bounds[1],
        maxLat: bounds[2],
        maxLng: bounds[3],
      },
    }),
};

// Health API
export const healthApi = {
  check: () => stravaClient.get('/health'),
};
