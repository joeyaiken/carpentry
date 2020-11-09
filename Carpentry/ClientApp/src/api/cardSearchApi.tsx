import { 
    // Get, 
    // GetFile, 
    Post 
} from '../api/apiHandler'

export const cardSearchApi = {
    async searchInventory(filters: CardSearchQueryParameter): Promise<CardSearchResultDto[]> {
        const endpoint = `api/CardSearch/SearchInventory`;
        const result = await Post(endpoint, filters);
        return result || [];
    },
    async searchWeb(name: string, exclusive: boolean): Promise<CardSearchResultDto[]> {
        const endpoint = `api/CardSearch/SearchWeb`;
        const payload = {
            name: name,
            exclusive: exclusive
        }
        const result = await Post(endpoint, payload);
        return result || [];
    }
}
