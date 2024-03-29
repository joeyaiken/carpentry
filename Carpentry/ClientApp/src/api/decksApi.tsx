import { 
    Get, 
    // GetFile, 
    Post 
} from '../api/apiHandler'

export const decksApi = {
    async addDeck(deckProps: DeckPropertiesDto): Promise<number> {
        const endpoint = `api/Decks/AddDeck`;
        const result = await Post(endpoint, deckProps);
        return result;
    },
    async updateDeck(deckProps: DeckPropertiesDto): Promise<void> {
        const endpoint = `api/Decks/UpdateDeck`;
        await Post(endpoint, deckProps);
        return;
    },
    async deleteDeck(deckId: number): Promise<void> {
        const endpoint = `api/Decks/DeleteDeck`;
        const url = `${endpoint}?deckId=${deckId}`;
        await Get(url);
        return;
    },

    async cloneDeck(deckId: number): Promise<void> {
        const url = `api/Decks/CloneDeck?deckId=${deckId}`;
        await Get(url);
        return;
    },
    
    async dissassembleDeck(deckId: number): Promise<void> {
        const url = `api/Decks/DissassembleDeck?deckId=${deckId}`;
        await Get(url);
        return;
    },

    async addDeckCard(deckCardProps: DeckCardDto): Promise<void> {
        const endpoint = `api/Decks/AddDeckCard`;
        const result = await Post(endpoint, deckCardProps);
        return result;
    },
    async updateDeckCard(dto: DeckCardDto): Promise<void> {
        const endpoint = `api/Decks/UpdateDeckCard`;
        const result = await Post(endpoint, dto);
        return result;
    },
    async removeDeckCard(deckCardId: number): Promise<void> {
        const endpoint = `api/Decks/RemoveDeckCard`;
        const url = `${endpoint}?id=${deckCardId}`;
        await Get(url);
        return;
    },

    async getDeckOverviews(): Promise<DeckOverviewDto[]> {
        const format = "";
        const sort = "";
        const includeDissasembled = true;
        const endpoint = `api/Decks/GetDeckOverviews?format=${format}&sortBy=${sort}&includeDissasembled=${includeDissasembled}`;
        //const endpoint = `api/Decks/GetDeckOverviews`;
        const result = await Get(endpoint);
        return result;
    },
    async getDeckDetail(deckId: number): Promise<DeckDetailDto> {
        const endpoint = `api/Decks/GetDeckDetail`;
        const url = `${endpoint}?deckId=${deckId}`;
        const result = await Get(url);
        return result;
    },

    async validateDeckImport(dto: CardImportDto): Promise<ValidatedDeckImportDto> {
        const endpoint = `api/Decks/ValidateDeckImport`;
        const result = await Post(endpoint, dto);
        return result;
    },
    async addValidatedDeckImport(dto: ValidatedDeckImportDto): Promise<number> {
        const endpoint = `api/Decks/AddValidatedDeckImport`;
        var newId = await Post(endpoint, dto);
        return newId;
    },
    async exportDeckList(deckId: number, exportType: string): Promise<string> {
        const endpoint = `api/Decks/ExportDeckList`;
        const url = `${endpoint}?deckId=${deckId}&exportType=${exportType}`;
        const result = await Get(url);
        return result;
    },

    async getCardTagDetails(deckId: number, cardId: number): Promise<CardTagDetailDto> {
        const endpoint = `api/Decks/GetCardTagDetails`;
        const url = `${endpoint}?deckId=${deckId}&cardId=${cardId}`;
        const result = await Get(url);
        return result;
    },
    async addCardTag(dto: CardTagDto): Promise<void> {
        const endpoint = `api/Decks/AddCardTag`;
        await Post(endpoint, dto);
        return;
    },
    async removeCardTag(cardTagId: number): Promise<void> {
        const endpoint = `api/Decks/RemoveCardTag`;
        const url = `${endpoint}?cardTagId=${cardTagId}`;
        await Get(url);
        return;
    },

}
