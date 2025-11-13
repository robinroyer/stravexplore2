import { useQuery } from '@tanstack/react-query';
import { trendsApi } from '../api/stravaClient';

export const useTrendingSegments = (region = 'paris', topN = 20, options = {}) => {
  return useQuery({
    queryKey: ['trends', 'segments', region, topN],
    queryFn: () => trendsApi.getTrendingSegments(region, topN),
    select: (response) => response.data,
    ...options,
  });
};

export const useSegmentTrend = (segmentId, options = {}) => {
  return useQuery({
    queryKey: ['trends', 'segment', segmentId],
    queryFn: () => trendsApi.getSegmentTrend(segmentId),
    select: (response) => response.data,
    enabled: !!segmentId,
    ...options,
  });
};

export const useEventSegments = (bounds, options = {}) => {
  return useQuery({
    queryKey: ['trends', 'events', bounds],
    queryFn: () => trendsApi.findEvents(bounds),
    select: (response) => response.data,
    enabled: bounds && bounds.length === 4,
    ...options,
  });
};
