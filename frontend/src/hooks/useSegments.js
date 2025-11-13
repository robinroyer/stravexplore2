import { useQuery, useMutation } from '@tanstack/react-query';
import { segmentsApi } from '../api/stravaClient';

export const useExploreSegments = (bounds, activityType, minCat, maxCat, options = {}) => {
  return useQuery({
    queryKey: ['segments', 'explore', bounds, activityType, minCat, maxCat],
    queryFn: () => segmentsApi.explore(bounds, activityType, minCat, maxCat),
    select: (response) => response.data,
    enabled: bounds && bounds.length === 4,
    ...options,
  });
};

export const useSegmentDetails = (segmentId, options = {}) => {
  return useQuery({
    queryKey: ['segments', 'details', segmentId],
    queryFn: () => segmentsApi.getDetails(segmentId),
    select: (response) => response.data,
    enabled: !!segmentId,
    ...options,
  });
};

export const useSegmentLeaderboard = (segmentId, page = 1, pageSize = 10, options = {}) => {
  return useQuery({
    queryKey: ['segments', 'leaderboard', segmentId, page, pageSize],
    queryFn: () => segmentsApi.getLeaderboard(segmentId, page, pageSize),
    select: (response) => response.data,
    enabled: !!segmentId,
    ...options,
  });
};

export const useAnalyzeByDifficulty = (bounds, activityType, difficulty, options = {}) => {
  return useQuery({
    queryKey: ['segments', 'difficulty', bounds, activityType, difficulty],
    queryFn: () => segmentsApi.analyzeByDifficulty(bounds, activityType, difficulty),
    select: (response) => response.data,
    enabled: bounds && bounds.length === 4,
    ...options,
  });
};

export const useCompareSegments = () => {
  return useMutation({
    mutationFn: (segmentIds) => segmentsApi.compare(segmentIds),
  });
};
