import { 
    Get, 
    // GetFile, 
    // Post 
} from '../api/apiHandler'

export const coreApi = {
    async getFilterValues(): Promise<AppFiltersDto> {
        const endpoint = `api/Core/GetFilterValues`;
        const result = await Get(endpoint);
        return result;
    },

    async getTrackedSets(showUntracked: boolean, update: boolean): Promise<SetDetailDto> {
        const endpoint = `api/Core/GetTrackedSets?showUntracked=${showUntracked}&update=${update}`;
        const result = await Get(endpoint);
        return result;
    },

    //TODO - These next 3 should be POST not GET
    async addTrackedSet(setId: number): Promise<void> {
        const endpoint = `api/Core/AddTrackedSet?setId=${setId}`;
        console.log('updating tracked sets ping 4');
        await Get(endpoint);
        console.log('updating tracked sets ping 5');
        return;
    },
    async updateTrackedSet(setId: number): Promise<void> {
        const endpoint = `api/Core/UpdateTrackedSet?setId=${setId}`;
        await Get(endpoint);
        return;
    },
    async removeTrackedSet(setId: number): Promise<void> {
        const endpoint = `api/Core/RemoveTrackedSet?setId=${setId}`;
        await Get(endpoint);
        return;
    },
}
