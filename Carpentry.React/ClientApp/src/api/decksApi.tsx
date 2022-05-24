import {Get, Post} from './apiHandler'

export const decksApi = {
  async addDeck(deckProps: DeckPropertiesDto): Promise<number> {
    const endpoint = `api/Decks/AddDeck`;
    return await Post(endpoint, deckProps);
  },
  async updateDeck(deckProps: DeckPropertiesDto): Promise<void> {
    const endpoint = `api/Decks/UpdateDeck`;
    return await Post(endpoint, deckProps);
  },
  async deleteDeck(deckId: number): Promise<void> {
    const endpoint = `api/Decks/DeleteDeck`;
    const url = `${endpoint}?deckId=${deckId}`;
    return await Get(url);
  },

  async cloneDeck(deckId: number): Promise<void> {
    const url = `api/Decks/CloneDeck?deckId=${deckId}`;
    return await Get(url);
  },

  async disassembleDeck(deckId: number): Promise<void> {
    const url = `api/Decks/DissassembleDeck?deckId=${deckId}`;
    return await Get(url);
  },

  async addDeckCard(deckCardProps: DeckCardDto): Promise<void> {
    const endpoint = `api/Decks/AddDeckCard`;
    return await Post(endpoint, deckCardProps);
  },
  async updateDeckCard(dto: DeckCardDto): Promise<void> {
    const endpoint = `api/Decks/UpdateDeckCard`;
    return await Post(endpoint, dto);
  },
  async removeDeckCard(deckCardId: number): Promise<void> {
    const endpoint = `api/Decks/RemoveDeckCard`;
    const url = `${endpoint}?id=${deckCardId}`;
    return await Get(url);
  },

  async getDeckOverviews(): Promise<DeckOverviewDto[]> {
    const format = "";
    const sort = "";
    const includeDissasembled = true;
    const endpoint = `api/Decks/GetDeckOverviews?format=${format}&sortBy=${sort}&includeDissasembled=${includeDissasembled}`;
    return await Get(endpoint);
  },
  async getDeckDetail(deckId: number): Promise<DeckDetailDto> {
    const endpoint = `api/Decks/GetDeckDetail`;
    const url = `${endpoint}?deckId=${deckId}`;
    return await Get(url);
  },

  async validateDeckImport(dto: CardImportDto): Promise<ValidatedDeckImportDto> {
    const endpoint = `api/Decks/ValidateDeckImport`;
    return await Post(endpoint, dto);
  },
  async addValidatedDeckImport(dto: ValidatedDeckImportDto): Promise<number> {
    const endpoint = `api/Decks/AddValidatedDeckImport`;
    return await Post(endpoint, dto);
  },
  async exportDeckList(deckId: number, exportType: DeckExportType): Promise<string> {
    const endpoint = `api/Decks/ExportDeckList`;
    const url = `${endpoint}?deckId=${deckId}&exportType=${exportType}`;
    return await Get(url);
  },

  async getCardTagDetails(deckId: number, cardId: number): Promise<CardTagDetailDto> {
    const endpoint = `api/Decks/GetCardTagDetails`;
    const url = `${endpoint}?deckId=${deckId}&cardId=${cardId}`;
    return await Get(url);
  },
  async addCardTag(dto: CardTagDto): Promise<void> {
    const endpoint = `api/Decks/AddCardTag`;
    return await Post(endpoint, dto);
  },
  async removeCardTag(cardTagId: number): Promise<CardTagDetailDto> {
    const endpoint = `api/Decks/RemoveCardTag`;
    const url = `${endpoint}?cardTagId=${cardTagId}`;
    return await Get(url);
  },

}
