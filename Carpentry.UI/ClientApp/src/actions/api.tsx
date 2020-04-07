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

        async searchSet(filters: CardFilterProps): Promise<MagicCard[]> {
            const endpoint = `api/CardSearch/SearchSet`;
            const result = await Post(endpoint, filters);
            return result || [];
        },

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

        async getFilterValues(): Promise<FilterOptionDto> {
            const endpoint = `api/Core/GetFilterValues`;
            const result = await Get(endpoint);
            return result;
        },

        ////Backup DB
        ////should this be a POST since it could/should include filepath info?
        //[HttpGet("[action]")]
        //public async Task<ActionResult> BackupDatabase()
        //{
        //    throw new NotImplementedException();
        //}

        ////Restore DB
        //[HttpGet("[action]")]
        //public async Task<ActionResult> RestoreDatabase()
        //{
        //    throw new NotImplementedException();
        //}

        ////Get Set|Data Update Status
        //[HttpGet("[action]")]
        //public async Task<ActionResult> GetDatabaseUpdateStatus()
        //{
        //    throw new NotImplementedException();
        //}


        ////Update Set Scry Data
        //[HttpGet("[action]")]
        //public async Task<ActionResult> UpdateScryfallSet(string setCode)
        //{
        //    throw new NotImplementedException();
        //}

        ////Update Set Card Data
        //[HttpGet("[action]")]
        //public async Task<ActionResult> UpdateSetData(string setCode)
        //{
        //    throw new NotImplementedException();
        //}

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

        async search(filters: FilterDescriptor[]): Promise<DeckProperties[]> {
            const endpoint = `api/Decks/Search`;
            const result = await Post(endpoint, filters);
            return result;
        },

        async get(deckId: number): Promise<DeckDto> {
            const endpoint = `api/Decks/Get`;
            const url = `${endpoint}?deckId=${deckId}`;
            const result = await Get(url);
            return result;
        },

        async addCard(deckCardProps: DeckCardDto): Promise<void> {
            const endpoint = `api/Decks/AddCard`;
            const result = await Post(endpoint, deckCardProps);
            return result;
        },

        //This DTO is wrong, not DeckCardDto
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

        async api_Inventory_Add(dto: InventoryCard): Promise<void> {
            const url = `api/Inventory/Add`;
            await Post(url, dto);
            return;
        },
        
        async api_Inventory_AddBatch(dto: InventoryCard[]): Promise<void> {
            const url = `api/Inventory/AddBatch`;
            await Post(url, dto);
            return;
        },
        
        async api_Inventory_Update(dto: InventoryCard): Promise<void> {
            const url = `api/Inventory/Update`;
            await Post(url, dto);
            return;
        },
        
        async api_Inventory_Delete(id: number): Promise<void> {
            const endpoint = `api/Inventory/Delete`;
            const url = `${endpoint}?id=${id}`;
            await Get(url);
            return;
        },
        
        async api_Inventory_Search(param: InventoryQueryParameter): Promise<InventoryOverviewDto[]> {
            const endpoint = `api/Inventory/Search`;
            const result = await Post(endpoint, param);
            return result;
        },

        async api_Inventory_GetByName(name: string): Promise<InventoryDetailDto> {
            const endpoint = `api/Inventory/GetByName?name=${name}`;
            const result = await Get(endpoint);
            return result;
        },

    },
}

async function Get(url: string): Promise<any> {
    const response = await fetch(url);
    if (response.status === 202) {
        return;
    }
    const result = await response.json();
    return result;
}

async function Post(endpoint: string, payload: any): Promise<any> {
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

