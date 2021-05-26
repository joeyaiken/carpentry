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
        http: HttpClient
        ) {
        super(http);
    }

    addDeck(deckProps: DeckPropertiesDto): Observable<number> {
        const endpoint = `api/Decks/AddDeck`;
        return this.http.post<number>(endpoint, deckProps);
    }

    updateDeck(deckProps: DeckPropertiesDto): Observable<void> {
        const url = `api/Decks/UpdateDeck`;
        return this.http.post<void>(url, deckProps);
    }; 

    deleteDeck(deckId: number): Observable<void> {
        const url = `api/Decks/DeleteDeck?deckId=${deckId}`;
        return this.http.get<void>(url);
    };

    cloneDeck(deckId: number): Observable<void> {
        const url = `api/Decks/CloneDeck?deckId=${deckId}`;
        return this.http.get<void>(url);
    };
    
    dissassembleDeck(deckId: number): Observable<void> {
        const url = `api/Decks/DissassembleDeck?deckId=${deckId}`;
        return this.http.get<void>(url);
    };

    addDeckCard(deckCardProps: DeckCardDto): Observable<void> {
        const endpoint = `api/Decks/AddDeckCard`;
        return this.http.post<void>(endpoint, deckCardProps);
    };

    updateDeckCard(dto: DeckCardDto): Observable<void> {
        const endpoint = `api/Decks/UpdateDeckCard`;
        return this.http.post<void>(endpoint, dto);
    };

    removeDeckCard(deckCardId: number): Observable<void> {
        const url = `api/Decks/RemoveDeckCard?id=${deckCardId}`;
        return this.http.get<void>(url);
    };

    getDeckOverviews(format: string, sort: string, includeDissasembled: boolean): Observable<DeckOverviewDto[]> {
        const endpoint = `api/Decks/GetDeckOverviews?format=${format}&sortBy=${sort}&includeDissasembled=${includeDissasembled}`;
        return this.http.get<DeckOverviewDto[]>(endpoint);
    }

    getDeckDetail(deckId: number): Observable<DeckDetailDto> {
        const url = `api/Decks/GetDeckDetail?deckId=${deckId}`;
        return this.http.get<DeckDetailDto>(url);
    };

    validateDeckImport(dto: CardImportDto): Observable<ValidatedDeckImportDto> {
        const endpoint = `api/Decks/ValidateDeckImport`;
        return this.http.post<ValidatedDeckImportDto>(endpoint, dto);
    };
    addValidatedDeckImport(dto: ValidatedDeckImportDto): Observable<number> {
        const endpoint = `api/Decks/AddValidatedDeckImport`;
        return this.http.post<number>(endpoint, dto);
    };
    
    exportDeckList(deckId: number, exportType: string): Observable<string> {
        const url = `api/Decks/ExportDeckList?deckId=${deckId}&exportType=${exportType}`;
        return this.http.get<string>(url);
    };

    getCardTagDetails(deckId: number, cardId: number): Observable<CardTagDetailDto> {
        const url = `api/Decks/GetCardTagDetails?deckId=${deckId}&cardId=${cardId}`;
        return this.http.get<CardTagDetailDto>(url);
    };

    addCardTag(dto: CardTagDto): Observable<void> {
        const endpoint = `api/Decks/AddCardTag`;
        return this.http.post<void>(endpoint, dto);
    };

    removeCardTag(cardTagId: number): Observable<CardTagDetailDto> {
        const url = `api/Decks/RemoveCardTag?cardTagId=${cardTagId}`;
        return this.http.get<CardTagDetailDto>(url);
    };
}