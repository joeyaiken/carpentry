import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpService } from "../common/HttpService";
import { CardImportDto, CardTagDetailDto, CardTagDto, DeckCardDto, DeckDetailDto, DeckOverviewDto, DeckPropertiesDto, ValidatedDeckImportDto } from "./models";



@Injectable({
    providedIn: 'root',
})
export class DecksService extends HttpService 
{
    constructor(
        // http: HttpClient
        ) {
        super();
    }

    //TODO - replace all promises with observables


    // getDeckOverviews(): Observable<any> { }
    
    // async addDeck(deckProps: DeckPropertiesDto): Promise<number> {
    //     const endpoint = `api/Decks/AddDeck`;
    //     const result = await this.Post(endpoint, deckProps);
    //     return result;
    // }

    // // addDeck(deckProps:DeckPropertiesDto): Observable<number> {

    // // }

    // async updateDeck(deckProps: DeckPropertiesDto): Promise<void> {
    //     const endpoint = `api/Decks/UpdateDeck`;
    //     await this.Post(endpoint, deckProps);
    //     return;
    // }; 

    // async deleteDeck(deckId: number): Promise<void> {
    //     const endpoint = `api/Decks/DeleteDeck`;
    //     const url = `${endpoint}?deckId=${deckId}`;
    //     await this.Get(url);
    //     return;
    // };

    // async cloneDeck(deckId: number): Promise<void> {
    //     const url = `api/Decks/CloneDeck?deckId=${deckId}`;
    //     await this.Get(url);
    //     return;
    // };
    
    // async dissassembleDeck(deckId: number): Promise<void> {
    //     const url = `api/Decks/DissassembleDeck?deckId=${deckId}`;
    //     await this.Get(url);
    //     return;
    // };

    // async addDeckCard(deckCardProps: DeckCardDto): Promise<void> {
    //     const endpoint = `api/Decks/AddDeckCard`;
    //     const result = await this.Post(endpoint, deckCardProps);
    //     return result;
    // };
    // async updateDeckCard(dto: DeckCardDto): Promise<void> {
    //     const endpoint = `api/Decks/UpdateDeckCard`;
    //     const result = await this.Post(endpoint, dto);
    //     return result;
    // };
    // async removeDeckCard(deckCardId: number): Promise<void> {
    //     const endpoint = `api/Decks/RemoveDeckCard`;
    //     const url = `${endpoint}?id=${deckCardId}`;
    //     await this.Get(url);
    //     return;
    // };

    async getDeckOverviews(): Promise<DeckOverviewDto[]> {
        console.log('get deck overviews')
        const endpoint = `api/Decks/GetDeckOverviews`;
        const result = await this.Get(endpoint);
        return result;
    };
    // async getDeckDetail(deckId: number): Promise<DeckDetailDto> {
    //     const endpoint = `api/Decks/GetDeckDetail`;
    //     const url = `${endpoint}?deckId=${deckId}`;
    //     const result = await this.Get(url);
    //     return result;
    // };

    // async validateDeckImport(dto: CardImportDto): Promise<ValidatedDeckImportDto> {
    //     const endpoint = `api/Decks/ValidateDeckImport`;
    //     const result = await this.Post(endpoint, dto);
    //     return result;
    // };
    // async addValidatedDeckImport(dto: ValidatedDeckImportDto): Promise<number> {
    //     const endpoint = `api/Decks/AddValidatedDeckImport`;
    //     var newId = await this.Post(endpoint, dto);
    //     return newId;
    // };
    // //async exportDeckList(deckId: number, exportType: DeckExportType): Promise<string> {
    //     async exportDeckList(deckId: number, exportType: string): Promise<string> {
    //     const endpoint = `api/Decks/ExportDeckList`;
    //     const url = `${endpoint}?deckId=${deckId}&exportType=${exportType}`;
    //     const result = await this.Get(url);
    //     return result;
    // };

    // async getCardTagDetails(deckId: number, cardId: number): Promise<CardTagDetailDto> {
    //     const endpoint = `api/Decks/GetCardTagDetails`;
    //     const url = `${endpoint}?deckId=${deckId}&cardId=${cardId}`;
    //     const result = await this.Get(url);
    //     return result;
    // };
    // async addCardTag(dto: CardTagDto): Promise<void> {
    //     const endpoint = `api/Decks/AddCardTag`;
    //     await this.Post(endpoint, dto);
    //     return;
    // };
    // async removeCardTag(cardTagId: number): Promise<CardTagDetailDto> {
    //     const endpoint = `api/Decks/RemoveCardTag`;
    //     const url = `${endpoint}?cardTagId=${cardTagId}`;
    //     const result = await this.Get(url);
    //     return result;
    // };
}