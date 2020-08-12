// import Redux, { Store, Dispatch, compose, combineReducers } from 'redux'

// export default class Api {
//     async api_Cards_SearchSet(filters: CardFilterProps): Promise<MagicCard[]>{
//         const endpoint = `api/CardSearch/SearchSet`;
//         const result = await Post(endpoint, filters);
//         return result || [];
//     }
// }
// export const requestCardSearchInventory = (card: MagicCard): any => {
//     return (dispatch: Dispatch, getState: any) => {
//         return searchCardSearchInventory(dispatch, getState(), card);
//     }
// }


export const api = {

    cardSearch: {

        async searchInventory(filters: InventoryQueryParameter): Promise<MagicCard[]> {
            const endpoint = `api/CardSearch/SearchInventory`;
            const result = await Post(endpoint, filters);
            return result || [];
        },

        // async searchSet(filters: CardSearchQueryParameter): Promise<MagicCard[]> {
        //     console.log('searchSet');
        //     console.log(filters);
        //     const endpoint = `api/CardSearch/SearchSet`;
        //     const result = await Post(endpoint, filters);
        //     return result || [];
        // },

        async searchWeb(name: string, exclusive: boolean): Promise<MagicCard[]> {
            const endpoint = `api/CardSearch/SearchWeb`;
            const payload = {
                name: name,
                exclusive: exclusive
            }
            const result = await Post(endpoint, payload);
            return result || [];
        }

    },

    core: {

        async getFilterValues(): Promise<AppFiltersDto> {
            //console.log('why no filter values?')
            const endpoint = `api/Core/GetFilterValues`;
            const result = await Get(endpoint);
            return result;
        },

        async getTrackedSets(showUntracked: boolean, update: boolean): Promise<SetDetailDto> {
            const endpoint = `api/Core/GetTrackedSets?showUntracked=${showUntracked}&update=${update}`;
            const result = await Get(endpoint);
            return result;
        },

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

        // async getUntrackedSets(): Promise<SetDetailDto> {
        //     const endpoint = `api/Core/GetUntrackedSets`;
        //     const result = await Get(endpoint);
        //     return result;
        // },

        ////Backup DB
        ////should this be a POST since it could/should include filepath info?
        //[HttpGet("[action]")]
        //public async Task<ActionResult> BackupDatabase()

        ////Restore DB
        //[HttpGet("[action]")]
        //public async Task<ActionResult> RestoreDatabase()

        ////Get Set|Data Update Status
        //[HttpGet("[action]")]
        //public async Task<ActionResult> GetDatabaseUpdateStatus()

        ////Update Set Scry Data
        //[HttpGet("[action]")]
        //public async Task<ActionResult> UpdateScryfallSet(string setCode)

        ////Update Set Card Data
        //[HttpGet("[action]")]
        //public async Task<ActionResult> UpdateSetData(string setCode)

    },

    Decks: {

        async add(deckProps: DeckProperties): Promise<number> {
            const endpoint = `api/Decks/Add`;
            const result = await Post(endpoint, deckProps);
            return result;
        },

        async update(deckProps: DeckProperties): Promise<void> {
            const endpoint = `api/Decks/Update`;
            await Post(endpoint, deckProps);
            return;
        },

        async delete(deckId: number): Promise<void> {
            const endpoint = `api/Decks/Delete`;
            const url = `${endpoint}?deckId=${deckId}`;
            await Get(url);
            return;
        },

        async getOverviews(): Promise<DeckOverviewDto[]> {
            //console.log('')
            const endpoint = `api/Decks/GetDeckOverviews`;
            const result = await Get(endpoint);
            return result;
        },

        async getDetail(deckId: number): Promise<DeckDetailDto> {
            const endpoint = `api/Decks/GetDeckDetail`;
            const url = `${endpoint}?deckId=${deckId}`;
            const result = await Get(url);
            return result;
        },

        async addCard(deckCardProps: DeckCardDto): Promise<void> {
            const endpoint = `api/Decks/AddCard`;
            const result = await Post(endpoint, deckCardProps);
            return result;
        },

        async updateCard(dto: DeckCardDto): Promise<void> {
            const endpoint = `api/Decks/UpdateCard`;
            const result = await Post(endpoint, dto);
            return result;
        },

        async removeCard(deckCardId: number): Promise<void> {
            const endpoint = `api/Decks/RemoveCard`;
            const url = `${endpoint}?id=${deckCardId}`;
            await Get(url);
            return;
        },

    },

    Inventory: {

        // async api_Inventory_Add(dto: InventoryCard): Promise<void> {
        //     const url = `api/Inventory/Add`;
        //     await Post(url, dto);
        //     return;
        // },
        
        //TODO - whatever calls this needs to not clear pending cards when an error occurrs
        async AddBatch(dto: InventoryCard[]): Promise<void> {
            const url = `api/Inventory/AddCardBatch`;
            await Post(url, dto);
            return;
        },
        
        // async api_Inventory_Update(dto: InventoryCard): Promise<void> {
        //     const url = `api/Inventory/Update`;
        //     await Post(url, dto);
        //     return;
        // },
        
        // async api_Inventory_Delete(id: number): Promise<void> {
        //     const endpoint = `api/Inventory/Delete`;
        //     const url = `${endpoint}?id=${id}`;
        //     await Get(url);
        //     return;
        // },
        
        async searchCards(param: InventoryQueryParameter): Promise<InventoryOverviewDto[]> {
            const endpoint = `api/Inventory/SearchCards`;
            const result = await Post(endpoint, param);
            return result;
        },

        async getCardsByName(name: string): Promise<InventoryDetailDto> {
            const endpoint = `api/Inventory/GetCardsByName?name=${name}`;
            const result = await Get(endpoint);
            return result;
        },

        async exportInventoryBackup(): Promise<any> {
            const endpoint = `api/Inventory/ExportInventoryBackup`;
            const result = await Get(endpoint);
            return result;
        }
    },
}

async function Get(url: string): Promise<any> {
    // console.log(`get fetching URL ${url}`)
    const response = await fetch(url);
    // console.log('updating tracked sets ping 7?');
    // console.log(response);

    const contentType = response.headers.get("content-type");
    if(contentType && contentType.indexOf("application/json") !== -1){
        const result = await response.json();
        return result;
    }
    return;

    // if (response.status === 202) {
    //     return;
    // }
    // const result = await response.json();
    // return result;
}

async function GetFile(url: string): Promise<any> {
    // console.log(`get fetching URL ${url}`)
    const response = await fetch(url);
    // console.log('updating tracked sets ping 7?');
    // console.log(response);

    // const contentType = response.headers.get("content-type");
    // if(contentType && contentType.indexOf("application/json") !== -1){
    //     const result = await response.json();
    //     return result;
    // }

    if(response.status != 200){
        //error
        return;
    } else {
        return response.blob();
    }


    /*
    fetch('api/zip')
        .then((response) => {
            if (response.status != 200) {
                let errorMessage = "Error processing the request... (" + response.status + " " + response.statusText + ")";
                throw new Error(errorMessage);
            } else {
                return response.blob();
            }
        })
        .then((blob: any) => {
            // !!! see next code block !!!
            downloadData('geojsons.zip', blob);
        })
        .catch(error => {
            console.error(error);
        });
    
    */



    // if (response.status === 202) {
    //     return;
    // }
    // const result = await response.json();
    // return result;
}

async function Post(endpoint: string, payload: any): Promise<any> {
    // console.log('post');
    // console.log(payload);
    const bodyToAdd = JSON.stringify(payload);
    const response = await fetch(endpoint, {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: bodyToAdd
    });
    const result = await response.json().catch(() => {
        return;
    });
    return result;
}

