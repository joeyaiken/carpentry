import {Get, Post} from './apiHandler'

export const inventoryApi = {
  async addInventoryCard(dto: InventoryCard): Promise<void> {
    const url = `api/Inventory/AddInventoryCard`;
    return await Post(url, dto);
  },
  //TODO - whatever calls this needs to not clear pending cards when an error occurs
  async addInventoryCardBatch(dto: NewInventoryCard[]): Promise<void> {
    const url = `api/Inventory/AddInventoryCardBatch`;
    return await Post(url, dto);
  },
  async updateInventoryCard(dto: InventoryCard): Promise<void> {
    const url = `api/Inventory/UpdateInventoryCard`;
    return await Post(url, dto);
  },
  async UpdateInventoryCardBatch(dtos: InventoryCard[]): Promise<void> {
    const url = `api/Inventory/UpdateInventoryCardBatch`;
    return await Post(url, dtos);
  },
  async deleteInventoryCard(id: number): Promise<void> {
    const endpoint = `api/Inventory/DeleteInventoryCard`;
    const url = `${endpoint}?id=${id}`;
    return await Get(url);
  },
  async deleteInventoryCardBatch(ids: number[]): Promise<void> {
    const endpoint = `api/Inventory/DeleteInventoryCardBatch`;
    return await Post(endpoint, ids);
  },

  async searchCards(param: InventoryQueryParameter): Promise<InventoryOverviewDto[]> {
    const endpoint = `api/Inventory/SearchCards`;
    return await Post(endpoint, param);
  },
  async getInventoryDetail(cardId: number): Promise<InventoryDetailDto> {
    const endpoint = `api/Inventory/GetInventoryDetail?cardId=${cardId}`;
    return await Get(endpoint);
  },

  async getTrimmingToolCards(dto: TrimmingToolRequest): Promise<TrimmingToolResult[]> {
    const endpoint = `api/TrimmingTool/GetTrimmingToolCards`;
    return await Post(endpoint, dto);
  },

  async trimCards(cardsToTrim: TrimmedCardDto[]): Promise<void> {
    const endpoint = `api/TrimmingTool/TrimCards`;
    return await Post(endpoint, cardsToTrim);
  },

  async validateCarpentryImport(dto: CardImportDto): Promise<ValidatedCarpentryImportDto> {
    const endpoint = `api/Inventory/ValidateCarpentryImport`;
    return await Post(endpoint, dto);
  },
  async addValidatedCarpentryImport(dto: ValidatedCarpentryImportDto): Promise<void> {
    const endpoint = `api/Inventory/AddValidatedCarpentryImport`;
    return await Post(endpoint, dto);
  },
  async exportInventoryBackup(): Promise<any> {
    const endpoint = `api/Inventory/ExportInventoryBackup`;
    return await Get(endpoint);
  }
}
