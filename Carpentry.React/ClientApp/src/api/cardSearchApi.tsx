import {Post} from './apiHandler'

// This really should eventually be merged into the inventory controller or something
export const cardSearchApi = {
  async searchInventory(filters: CardSearchQueryParameter): Promise<CardSearchResultDto[]> {
    const endpoint = `api/CardSearch/SearchInventory`;
    const result = await Post(endpoint, filters);
    return result || [];
  },
}
