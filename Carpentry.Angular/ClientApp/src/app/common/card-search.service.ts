import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CardSearchResultDto } from "../inventory/models";
import { HttpService } from "./HttpService";
import { CardSearchQueryParameter } from "./models";

@Injectable({
    providedIn: 'root',
})
export class CardSearchService extends HttpService {
    constructor(http: HttpClient) {
        super(http);
    }

    searchInventory(filters: CardSearchQueryParameter): Observable<CardSearchResultDto[]>{
        const endpoint = `api/CardSearch/SearchInventory`;
        return this.http.post<CardSearchResultDto[]>(endpoint, filters);
    }
}