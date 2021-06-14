import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { forkJoin, Observable, of } from "rxjs";
import { map, tap } from "rxjs/operators";
import { HttpService } from "../common/HttpService";
import { AppFiltersDto, FilterOption } from "../settings/models";

@Injectable({
    providedIn: 'root',
})
export class FilterService extends HttpService {

    //Can I attempt to store cached values of commonly-used things?...
    private formatFilters: FilterOption[] = [];

    
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

        if(this.formatFilters.length === 0){
            const endpoint = 'api/Core/GetFormatFilters';
            return this.http.get<FilterOption[]>(endpoint)
                .pipe(tap(
                    data => this.formatFilters = data,
                    error => console.log(error),
                ))
        }
        else {
            return of(this.formatFilters)
        }
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

    getAppFilterOptions(): Observable<AppFiltersDto> {
        return forkJoin({
            sets: this.getCardSetFilters(),
            types: this.getCardTypeFilters(),
            formats: this.getFormatFilters(),
            colors: this.getManaTypeFilters(),
            groupBy: this.getInventoryGroupOptions(),
            rarities: this.getRarityFilters(),
            searchGroups: this.getCardGroupFilters(),
            sortBy: this.getInventorySortOptions(),
            statuses: this.getStatusFilters(),
        })
        .pipe(
            map(response => {
                let filters: AppFiltersDto = {
                    types: response.types,
                    sets: response.sets,
                    formats: response.formats,
                    colors: response.colors,
                    groupBy: response.groupBy,
                    rarities: response.rarities,
                    searchGroups: response.searchGroups,
                    sortBy: response.sortBy,
                    statuses: response.statuses,
                };
                return filters;
            })
        );
    }
}