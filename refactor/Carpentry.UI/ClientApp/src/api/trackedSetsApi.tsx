import {Get, Post} from './apiHandler'

export const trackedSetsApi = {
  async getTrackedSets(showUntracked: boolean): Promise<NormalizedList<TrackedSetDto>> {
    const endpoint = `api/TrackedSets?showUntracked=${showUntracked}`;
    return  await Get(endpoint);
  },
  async addTrackedSet(setId: number): Promise<void> {
    const endpoint = `api/TrackedSets/AddTrackedSet`;
    return await Post(endpoint, setId);
  },
  async updateTrackedSet(setId: number): Promise<void> {
    const endpoint = `api/TrackedSets/UpdateTrackedSet`;
    return await Post(endpoint, setId);
  },
  async removeTrackedSet(setId: number): Promise<void> {
    const endpoint = `api/TrackedSets/RemoveTrackedSet`;
    return await Post(endpoint, setId);
  },
}
