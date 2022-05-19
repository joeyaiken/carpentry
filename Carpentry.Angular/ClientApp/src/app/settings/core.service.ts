import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {HttpService} from "../common/HttpService";
import {InventoryTotalsByStatusResult, SetDetailDto} from "./models";

@Injectable({
    providedIn: 'root',
})
export class CoreService extends HttpService
{
    constructor(http: HttpClient) {
        super(http);
    }

    //Don't think this will be used by angular
    // getCoreData(): Observable<AppFiltersDto> {
    //     const endpoint = `api/Core/GetCoreData`;
    //     return this.http.get<AppFiltersDto>(endpoint);
    // }

    getTrackedSets(showUntracked: boolean, update: boolean): Observable<SetDetailDto[]> {
        const endpoint = `api/Core/GetTrackedSets?showUntracked=${showUntracked}&update=${update}`;
        return this.http.get<SetDetailDto[]>(endpoint);
    }

    addTrackedSet(setId: number): Observable<void> {
        const endpoint = `api/Core/AddTrackedSet`;
        return this.http.post<void>(endpoint, setId);
    }

    updateTrackedSet(setId: number): Observable<void> {
        const endpoint = `api/Core/UpdateTrackedSet`;
        return this.http.post<void>(endpoint, setId);
    }

    removeTrackedSet(setId: number): Observable<void> {
        const endpoint = `api/Core/RemoveTrackedSet`;
        return this.http.post<void>(endpoint, setId);
    }

    getCollectionTotals(): Observable<InventoryTotalsByStatusResult[]> {
        const endpoint = 'api/Core/GetCollectionTotals';
        return this.http.get<InventoryTotalsByStatusResult[]>(endpoint);
    }
}
