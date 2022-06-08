import {Get, Post} from './apiHandler'

export const coreApi = {
  async getCoreData(): Promise<AppFiltersDto> {
    const endpoint = `api/Core/GetCoreData`;
    return await Get(endpoint);
  },

  async GetCollectionTotals(): Promise<NormalizedList<InventoryTotalsByStatusResult>> {
    const endpoint = `api/Core/GetCollectionTotals`;
    return await Get(endpoint);
  }
}
