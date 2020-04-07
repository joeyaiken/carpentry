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
    async core_GetFilterOptions(): Promise<FilterOptionDto> {
        const endpoint = `api/Core/GetFilterOptions`;
        // const url = `${endpoint}?deckId=${deckId}`;
        console.log(`requesting endpoint: ${endpoint}`);
        const result = await Get(endpoint);
        return result;
    },
    core: {
        async getFilterOptions(): Promise<FilterOptionDto> {
            const endpoint = `api/Core/GetFilterOptions`;
            // const url = `${endpoint}?deckId=${deckId}`;
            console.log(`requesting endpoint: ${endpoint}`);
            const result = await Get(endpoint);
            return result;
        },
    }

}

// export class _api {
//     static async core_GetFilterOptions(): Promise<FilterOptionDto> {
//         const endpoint = `api/Core/GetFilterOptions`;
//         // const url = `${endpoint}?deckId=${deckId}`;
//         console.log(`requesting endpoint: ${endpoint}`);
//         const result = await Get(endpoint);
//         return result;
//     };

// }


export async function api_Core_GetFilterOptions(): Promise<FilterOptionDto> {

    const endpoint = `api/Core/GetFilterOptions`;
    // const url = `${endpoint}?deckId=${deckId}`;
    console.log(`requesting endpoint: ${endpoint}`);
    const result = await Get(endpoint);
    return result;
}

export async function api_Cards_SearchSet(filters: CardFilterProps): Promise<MagicCard[]>{
    const endpoint = `api/CardSearch/SearchSet`;
    const result = await Post(endpoint, filters);
    return result || [];
}

export async function api_Cards_SearchWeb(name: string, exclusive: boolean): Promise<MagicCard[]>{
    const endpoint = `api/CardSearch/SearchWeb`;
    const payload = {
        name: name,
        exclusive: exclusive
    }
    const result = await Post(endpoint, payload);
    return result || [];
}

export async function api_Cards_SearchInventory(filters: InventoryQueryParameter): Promise<MagicCard[]>{
    const endpoint = `api/CardSearch/SearchInventory`;
    const result = await Post(endpoint, filters);
    return result || [];
}

export async function api_Decks_Add(deckProps: DeckProperties): Promise<number> {
    const endpoint = `api/Decks/Add`;
    const result = await Post(endpoint, deckProps);
    return result;    
}

export async function api_Decks_Update(deckProps: DeckProperties): Promise<void> {
    const endpoint = `api/Decks/Update`;
    await Post(endpoint, deckProps);
    return;

}

export async function api_Decks_Delete(deckId: number): Promise<void> {
    const endpoint = `api/Decks/Delete`;
    const url = `${endpoint}?deckId=${deckId}`;
    await Get(url);
    return;
}

export async function api_Decks_Search(filters: FilterDescriptor[]): Promise<DeckProperties[]> {
    const endpoint = `api/Decks/Search`;
    const result = await Post(endpoint, filters);
    return result;
}

export async function api_Decks_Get(deckId: number): Promise<DeckDto> {
    const endpoint = `api/Decks/Get`;
    const url = `${endpoint}?deckId=${deckId}`;
    const result = await Get(url);
    return result;
}

export async function api_Decks_AddCard(deckCardProps: DeckCardDto): Promise<void> {
    const endpoint = `api/Decks/AddCard`;
    const result = await Post(endpoint, deckCardProps);
    return result;    
}

//This DTO is wrong, not DeckCardDto
export async function api_Decks_UpdateCard(dto: DeckCardDto): Promise<void> {
    const endpoint = `api/Decks/UpdateCard`;
    const result = await Post(endpoint, dto);
    return result;    
}

export async function api_Decks_RemoveCard(deckCardId: number): Promise<void> {
    const endpoint = `api/Decks/RemoveCard`;
    const url = `${endpoint}?id=${deckCardId}`;
    await Get(url);
    return;
}

// Inventory
// Add
export async function api_Inventory_Add(dto: InventoryCard): Promise<void> {
    const url = `api/Inventory/Add`;
    await Post(url, dto);
    return;
}
// AddBatch
export async function api_Inventory_AddBatch(dto: InventoryCard[]): Promise<void> {
    const url = `api/Inventory/AddBatch`;
    await Post(url, dto);
    return;
}
// Update
export async function api_Inventory_Update(dto: InventoryCard): Promise<void> {
    const url = `api/Inventory/Update`;
    await Post(url, dto);
    return;
}
// Delete

export async function api_Inventory_Delete(id: number): Promise<void> {
    const endpoint = `api/Inventory/Delete`;
    const url = `${endpoint}?id=${id}`;
    await Get(url);
    return;
}
// Search
export async function api_Inventory_Search(param: InventoryQueryParameter): Promise<InventoryOverviewDto[]> {
    const endpoint = `api/Inventory/Search`;
    const result = await Post(endpoint, param);
    return result;
}

export async function api_Inventory_GetByName(name: string): Promise<InventoryDetailDto> {
    const endpoint = `api/Inventory/GetByName?name=${name}`;
    const result = await Get(endpoint);
    return result;
}


async function Get(url: string): Promise<any> {
    const response = await fetch(url);
    if(response.status === 202){
        return;
    }
    const result = await response.json();
    return result;
}

async function Post(endpoint: string, payload: any): Promise<any> {
    const bodyToAdd = JSON.stringify(payload);
    const response = await fetch(endpoint,{
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
