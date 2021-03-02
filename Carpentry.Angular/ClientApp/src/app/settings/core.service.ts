import { ContentObserver } from "@angular/cdk/observers";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HttpService } from "../common/HttpService";
import { AppFiltersDto, InventoryTotalsByStatusResult, SetDetailDto } from "./models";

@Injectable({
    providedIn: 'root',
})
export class CoreService extends HttpService 
{
    constructor(http: HttpClient) {
        super(http);
    }

    getCoreData(): Observable<AppFiltersDto> {
        const endpoint = `api/Core/GetCoreData`;
        return this.http.get<AppFiltersDto>(endpoint);
    }

    getTrackedSets(showUntracked: boolean, update: boolean): Observable<SetDetailDto[]> {
        const endpoint = `api/Core/GetTrackedSets?showUntracked=${showUntracked}&update=${update}`;
        return this.http.get<SetDetailDto[]>(endpoint);
    }

    //TODO - These next 3 should be POST not GET
    addTrackedSet(setId: number): Observable<void> {
        const endpoint = `api/Core/AddTrackedSet?setId=${setId}`;
        return this.http.get<void>(endpoint);
    }

    updateTrackedSet(setId: number): Observable<void> {
        const endpoint = `api/Core/UpdateTrackedSet?setId=${setId}`;
        return this.http.get<void>(endpoint);
    }
    
    removeTrackedSet(setId: number): Observable<void> {
        const endpoint = `api/Core/RemoveTrackedSet?setId=${setId}`;        
        return this.http.get<void>(endpoint);
    }
    //

    getCollectionTotals(): Observable<InventoryTotalsByStatusResult[]> {
        const endpoint = 'api/Core/GetCollectionTotals';
        return this.http.get<InventoryTotalsByStatusResult[]>(endpoint);
    }
}