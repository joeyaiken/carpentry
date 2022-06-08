import {Get, Post} from './apiHandler'

export const trackedSetsApi = {
  async getTrackedSets(showUntracked: boolean, update: boolean): Promise<NormalizedList<SetDetailDto>> {
    const endpoint = `api/TrackedSets?showUntracked=${showUntracked}&update=${update}`;
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
