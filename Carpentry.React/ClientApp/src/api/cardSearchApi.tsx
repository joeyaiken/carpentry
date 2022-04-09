import { 
    // Get, 
    // GetFile, 
    Post 
} from '../api/apiHandler'

export const cardSearchApi = {
    //This should be replaced with ... idk yet

    async searchInventory(filters: CardSearchQueryParameter): Promise<CardSearchResultDto[]> {
        const endpoint = `api/CardSearch/SearchInventory`;
        const result = await Post(endpoint, filters);
        return result || [];
    },
    //I can't remove this until Inventory is updated like Decks
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
