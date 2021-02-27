import { ContentObserver } from "@angular/cdk/observers";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
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

    async getCoreDataAsync(): Promise<AppFiltersDto> {
        const endpoint = `api/Core/GetCoreData`;
        const result = await this.GetAsync(endpoint);
        return result;
    }

    async getTrackedSetsAsync(showUntracked: boolean, update: boolean): Promise<SetDetailDto> {
        const endpoint = `api/Core/GetTrackedSets?showUntracked=${showUntracked}&update=${update}`;
        const result = await this.GetAsync(endpoint);
        return result;
    }

    //TODO - These next 3 should be POST not GET
    async addTrackedSetAsync(setId: number): Promise<void> {
        const endpoint = `api/Core/AddTrackedSet?setId=${setId}`;
        await this.GetAsync(endpoint);
        return;
    }
    async updateTrackedSetAsync(setId: number): Promise<void> {
        const endpoint = `api/Core/UpdateTrackedSet?setId=${setId}`;
        await this.GetAsync(endpoint);
        return;
    }
    async removeTrackedSetAsync(setId: number): Promise<void> {
        const endpoint = `api/Core/RemoveTrackedSet?setId=${setId}`;
        await this.GetAsync(endpoint);
        return;
    }

    async getCollectionTotalsAsync(): Promise<InventoryTotalsByStatusResult[]> {
        const endpoint = 'api/Core/GetCollectionTotals';
        const result = await this.GetAsync(endpoint);
        return result;
    }


}