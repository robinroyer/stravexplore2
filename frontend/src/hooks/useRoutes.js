import { useQuery } from '@tanstack/react-query';
import { routesApi } from '../api/stravaClient';

export const usePopularRoutes = (lat, lng, radiusKm = 10, options = {}) => {
  return useQuery({
    queryKey: ['routes', 'popular', lat, lng, radiusKm],
    queryFn: () => routesApi.getPopular(lat, lng, radiusKm),
    select: (response) => response.data,
    enabled: !!lat && !!lng,
    ...options,
  });
};

export const useRouteDetails = (routeId, options = {}) => {
  return useQuery({
    queryKey: ['routes', 'details', routeId],
    queryFn: () => routesApi.getDetails(routeId),
    select: (response) => response.data,
    enabled: !!routeId,
    ...options,
  });
};

export const useRouteStats = (routeId, options = {}) => {
  return useQuery({
    queryKey: ['routes', 'stats', routeId],
    queryFn: () => routesApi.getStats(routeId),
    select: (response) => response.data,
    enabled: !!routeId,
    ...options,
  });
};

export const useFilterRoutes = (bounds, minElevation, maxElevation, options = {}) => {
  return useQuery({
    queryKey: ['routes', 'filter', bounds, minElevation, maxElevation],
    queryFn: () => routesApi.filter(bounds, minElevation, maxElevation),
    select: (response) => response.data,
    enabled: bounds && bounds.length === 4,
    ...options,
  });
};
