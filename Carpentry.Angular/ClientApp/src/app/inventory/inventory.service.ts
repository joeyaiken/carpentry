import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpService } from "../common/HttpService";
import {
  CardImportDto,
  InventoryCard,
  InventoryDetailDto,
  InventoryOverviewDto,
  InventoryQueryParameter,
  NewInventoryCard,
  TrimmedCardDto,
  TrimmingToolRequest, TrimmingToolResult,
  ValidatedCarpentryImportDto
} from "./models";

@Injectable({
    providedIn: 'root',
})
export class InventoryService extends HttpService
{
    constructor(
        http: HttpClient
        ) {
        super(http);
    }

    addInventoryCard(dto: InventoryCard): Observable<void> {
        const url = `api/Inventory/AddInventoryCard`;
        return this.http.post<void>(url, dto);
    }

    addInventoryCardBatch(dto: NewInventoryCard[]): Observable<void> {
        const url = `api/Inventory/AddInventoryCardBatch`;
        return this.http.post<void>(url, dto);
    }

    updateInventoryCard(dto: InventoryCard): Observable<void> {
        const url = `api/Inventory/UpdateInventoryCard`;
        return this.http.post<void>(url, dto);
    }

    UpdateInventoryCardBatch(dtos: InventoryCard[]): Observable<void> {
        const url = `api/Inventory/UpdateInventoryCardBatch`;
        return this.http.post<void>(url, dtos);
    }

    deleteInventoryCard(id: number): Observable<void> {
        const url = `api/Inventory/DeleteInventoryCard?id=${id}`;
        return this.http.get<void>(url);
    }

    deleteInventoryCardBatch(ids: number[]): Observable<void> {
        const url = `api/Inventory/DeleteInventoryCardBatch`;
        return this.http.post<void>(url, ids);
    }

    searchCards(param: InventoryQueryParameter): Observable<InventoryOverviewDto[]> {
        const endpoint = `api/Inventory/SearchCards`;
        return this.http.post<InventoryOverviewDto[]>(endpoint, param);
    }

    getInventoryDetail(cardId: number): Observable<InventoryDetailDto> {
        const endpoint = `api/Inventory/GetInventoryDetail?cardId=${cardId}`;
        return this.http.get<InventoryDetailDto>(endpoint);
    }

    getTrimmingToolCards(dto: TrimmingToolRequest): Observable<TrimmingToolResult[]> {
        const endpoint = `api/TrimmingTool/GetTrimmingToolCards`;
        return this.http.post<TrimmingToolResult[]>(endpoint, dto);
    }

    trimCards(cardsToTrim: TrimmedCardDto[]): Observable<void> {
        const endpoint = `api/TrimmingTool/TrimCards`;
        return this.http.post<void>(endpoint, cardsToTrim);
    }

    validateCarpentryImport(dto: CardImportDto): Observable<ValidatedCarpentryImportDto> {
        const endpoint = `api/Inventory/ValidateCarpentryImport`;
        return this.http.post<ValidatedCarpentryImportDto>(endpoint, dto);
    }

    addValidatedCarpentryImport(dto: ValidatedCarpentryImportDto): Observable<void> {
        const endpoint = `api/Inventory/AddValidatedCarpentryImport`;
        return this.http.post<void>(endpoint, dto);
    }

    exportInventoryBackup(): Observable<any> {
        const endpoint = `api/Inventory/ExportInventoryBackup`;
        return this.http.get<any>(endpoint);
    }
}
