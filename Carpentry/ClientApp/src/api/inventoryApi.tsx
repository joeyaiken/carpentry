import { 
    Get, 
    // GetFile, 
    Post 
} from '../api/apiHandler'

export const inventoryApi = {
    async addInventoryCard(dto: InventoryCard): Promise<void> {
        const url = `api/Inventory/AddInventoryCard`;
        await Post(url, dto);
        return;
    },
    //TODO - whatever calls this needs to not clear pending cards when an error occurrs
    async addInventoryCardBatch(dto: InventoryCard[]): Promise<void> {
        const url = `api/Inventory/AddInventoryCardBatch`;
        await Post(url, dto);
        return;
    },
    async updateInventoryCard(dto: InventoryCard): Promise<void> {
        const url = `api/Inventory/UpdateInventoryCard`;
        await Post(url, dto);
        return;
    },
    async UpdateInventoryCardBatch(dtos: InventoryCard[]): Promise<void> {
        const url = `api/Inventory/UpdateInventoryCardBatch`;
        await Post(url, dtos);
        return;
    },
    async deleteInventoryCard(id: number): Promise<void> {
        const endpoint = `api/Inventory/DeleteInventoryCard`;
        const url = `${endpoint}?id=${id}`;
        await Get(url);
        return;
    },
    async deleteInventoryCardBatch(ids: number[]): Promise<void> {
        const endpoint = `api/Inventory/DeleteInventoryCardBatch`;
        await Post(endpoint, ids);
        return;
    },

    async searchCards(param: InventoryQueryParameter): Promise<InventoryOverviewDto[]> {
        const endpoint = `api/Inventory/SearchCards`;
        const result = await Post(endpoint, param);
        return result;
    },
    async getInventoryDetail(cardId: number): Promise<InventoryDetailDto> {
        const endpoint = `api/Inventory/GetInventoryDetail?cardId=${cardId}`;
        const result = await Get(endpoint);
        return result;
    },

    // async getCollectionBuilderSuggestions(): Promise<InventoryOverviewDto[]> {
    //     const endpoint = `api/Inventory/GetCollectionBuilderSuggestions`;
    //     const result = await Get(endpoint);
    //     return result;
    // },
    // async hideCollectionBuilderSuggestion(dto: InventoryOverviewDto): Promise<void> {
    //     const endpoint = `api/Inventory/HideCollectionBuilderSuggestion`;
    //     const result = await Post(endpoint, dto);
    //     return result;
    // },
    
    // async getTrimmingTips(): Promise<InventoryOverviewDto[]> {
    //     const endpoint = `api/Inventory/GetTrimmingTips`;
    //     const result = await Get(endpoint);
    //     return result;
    // },
    // async hideTrimmingTip(dto: InventoryOverviewDto): Promise<void> {
    //     const endpoint = `api/Inventory/HideTrimmingTip`;
    //     const result = await Post(endpoint, dto);
    //     return result;
    // },

    async getTrimmingToolCards(dto: TrimmingToolRequest): Promise<InventoryOverviewDto[]> {
        const endpoint = `api/Inventory/GetTrimmingToolCards`;
        const result = await Post(endpoint, dto);
        return result;
    },

    async trimCards(cardsToTrim: TrimmedCardDto[]): Promise<void> {
        const endpoint = `api/Inventory/TrimCards`;
        await Post(endpoint, cardsToTrim);
        return;  
    },

    async validateCarpentryImport(dto: CardImportDto): Promise<ValidatedCarpentryImportDto> {
        const endpoint = `api/Inventory/ValidateCarpentryImport`;
        const result = await Post(endpoint, dto);
        return result;
    },
    async addValidatedCarpentryImport(dto: ValidatedCarpentryImportDto): Promise<void> {
        const endpoint = `api/Inventory/AddValidatedCarpentryImport`;
        const result = await Post(endpoint, dto);
        return result;
    },
    async exportInventoryBackup(): Promise<any> {
        const endpoint = `api/Inventory/ExportInventoryBackup`;
        const result = await Get(endpoint);
        return result;
    }

}
