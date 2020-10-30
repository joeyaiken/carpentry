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
        const endpoint = `api/Decks/GetDeckOverviews`;
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
    async addValidatedDeckImport(dto: ValidatedDeckImportDto): Promise<void> {
        const endpoint = `api/Decks/AddValidatedDeckImport`;
        await Post(endpoint, dto);
        return;
    },
    async exportDeckList(deckId: number): Promise<string> {
        const endpoint = `api/Decks/ExportDeckList`;
        const url = `${endpoint}?deckId=${deckId}`;
        const result = await Get(url);
        return result;
    }
}
