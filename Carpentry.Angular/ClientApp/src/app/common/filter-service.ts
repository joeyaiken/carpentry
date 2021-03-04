import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpService } from "../common/HttpService";
import { FilterOption } from "../settings/models";

@Injectable({
    providedIn: 'root',
})
export class FilterService extends HttpService {
    constructor(http: HttpClient) {
        super(http);
    }

    getCardSetFilters(): Observable<FilterOption[]> {
        const endpoint = 'api/Core/GetCardSetFilters';
        return this.http.get<FilterOption[]>(endpoint);
    }

    getCardTypeFilters(): Observable<FilterOption[]> {
        const endpoint = 'api/Core/GetCardTypeFilters';
        return this.http.get<FilterOption[]>(endpoint);
    }

    getFormatFilters(): Observable<FilterOption[]> {
        const endpoint = 'api/Core/GetFormatFilters';
        return this.http.get<FilterOption[]>(endpoint);
    }

    getManaTypeFilters(): Observable<FilterOption[]> {
        const endpoint = 'api/Core/GetManaTypeFilters';
        return this.http.get<FilterOption[]>(endpoint);
    }
    
    getRarityFilters(): Observable<FilterOption[]> {
        const endpoint = 'api/Core/GetRarityFilters';
        return this.http.get<FilterOption[]>(endpoint);
    }
    
    getStatusFilters(): Observable<FilterOption[]> {
        const endpoint = 'api/Core/GetStatusFilters';
        return this.http.get<FilterOption[]>(endpoint);
    }
    
    getCardGroupFilters(): Observable<FilterOption[]> {
        const endpoint = 'api/Core/GetCardGroupFilters';
        return this.http.get<FilterOption[]>(endpoint);
    }
    
    getInventorySortOptions(): Observable<FilterOption[]> {
        const endpoint = 'api/Core/GetInventorySortOptions';
        return this.http.get<FilterOption[]>(endpoint);
    }

    getInventoryGroupOptions(): Observable<FilterOption[]> {
        const endpoint = 'api/Core/GetInventoryGroupOptions';
        return this.http.get<FilterOption[]>(endpoint);
    }
}