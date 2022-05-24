import {Get, Post} from './apiHandler';

export const coreApi = {
  async getCoreData(): Promise<AppFiltersDto> {
    const endpoint = `api/Core/GetCoreData`;
    return await Get(endpoint);
  },

  async getTrackedSets(showUntracked: boolean, update: boolean): Promise<SetDetailDto[]> {
    const endpoint = `api/Core/GetTrackedSets?showUntracked=${showUntracked}&update=${update}`;
    return await Get(endpoint);
  },

  async addTrackedSet(setId: number): Promise<void> {
    const endpoint = `api/Core/AddTrackedSet`;
    await Post(endpoint, setId);
    return;
  },
  async updateTrackedSet(setId: number): Promise<void> {
    const endpoint = `api/Core/UpdateTrackedSet`;
    await Post(endpoint, setId);
    return;
  },
  async removeTrackedSet(setId: number): Promise<void> {
    const endpoint = `api/Core/RemoveTrackedSet`;
    await Post(endpoint, setId);
    return;
  },
}
