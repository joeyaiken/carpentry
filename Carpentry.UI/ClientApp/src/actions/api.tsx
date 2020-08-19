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

        async searchInventory(filters: CardSearchQueryParameter): Promise<CardSearchResultDto[]> {
            const endpoint = `api/CardSearch/SearchInventory`;
            const result = await Post(endpoint, filters);
            return result || [];
        },

        async searchWeb(name: string, exclusive: boolean): Promise<CardSearchResultDto[]> {
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

    },

    decks: {

        async addDeck(deckProps: DeckPropertiesDto): Promise<number> {
            const endpoint = `api/Decks/AddDeck`;
            const result = await Post(endpoint, deckProps);
            return result;
        },
        async updateDeck(deckProps: DeckPropertiesDto): Promise<void> {
            const endpoint = `api/Decks/UpdateDeck`;
            await Post(endpoint, deckProps);
            return;
        },
        async deleteDeck(deckId: number): Promise<void> {
            const endpoint = `api/Decks/DeleteDeck`;
            const url = `${endpoint}?deckId=${deckId}`;
            await Get(url);
            return;
        },

        async addDeckCard(deckCardProps: DeckCardDto): Promise<void> {
            const endpoint = `api/Decks/AddDeckCard`;
            const result = await Post(endpoint, deckCardProps);
            return result;
        },
        async updateDeckCard(dto: DeckCardDto): Promise<void> {
            const endpoint = `api/Decks/UpdateDeckCard`;
            const result = await Post(endpoint, dto);
            return result;
        },
        async removeDeckCard(deckCardId: number): Promise<void> {
            const endpoint = `api/Decks/RemoveDeckCard`;
            const url = `${endpoint}?id=${deckCardId}`;
            await Get(url);
            return;
        },

        async getDeckOverviews(): Promise<DeckOverviewDto[]> {
            const endpoint = `api/Decks/GetDeckOverviews`;
            const result = await Get(endpoint);
            return result;
        },
        async getDeckDetail(deckId: number): Promise<DeckDetailDto> {
            const endpoint = `api/Decks/GetDeckDetail`;
            const url = `${endpoint}?deckId=${deckId}`;
            const result = await Get(url);
            return result;
        },

        async validateDeckImport(dto: CardImportDto): Promise<ValidatedDeckImportDto> {
            const endpoint = `api/Decks/ValidateDeckImport`;
            //const url = `${endpoint}?deckId=${deckId}`;
            const result = await Post(endpoint, dto);
            return result;
        },
        async addValidatedDeckImport(dto: ValidatedDeckImportDto): Promise<void> {
            const endpoint = `api/Decks/AddValidatedDeckImport`;
            //const url = `${endpoint}?deckId=${deckId}`;
            await Post(endpoint, dto);
            return;
        },
        async exportDeckList(deckId: number): Promise<string> {
            const endpoint = `api/Decks/ExportDeckList`;
            const url = `${endpoint}?deckId=${deckId}`;
            const result = await Get(url);
            return result;
        }

    },

    inventory: {

        async addInventoryCard(dto: InventoryCard): Promise<void> {
            const url = `api/Inventory/AddInventoryCard`;
            await Post(url, dto);
            return;
        },
        //TODO - whatever calls this needs to not clear pending cards when an error occurrs
        async addInventoryCardBatch(dto: InventoryCard[]): Promise<void> {
            const url = `api/Inventory/AddInventoryCardBatch`;
            await Post(url, dto);
            return;
        },
        async updateInventoryCard(dto: InventoryCard): Promise<void> {
            const url = `api/Inventory/UpdateInventoryCard`;
            await Post(url, dto);
            return;
        },
        async UpdateInventoryCardBatch(dtos: InventoryCard[]): Promise<void> {
            const url = `api/Inventory/UpdateInventoryCardBatch`;
            await Post(url, dtos);
            return;
        },
        async deleteInventoryCard(id: number): Promise<void> {
            const endpoint = `api/Inventory/DeleteInventoryCard`;
            const url = `${endpoint}?id=${id}`;
            await Get(url);
            return;
        },
        async deleteInventoryCardBatch(ids: number[]): Promise<void> {
            const endpoint = `api/Inventory/DeleteInventoryCardBatch`;
            await Post(endpoint, ids);
            return;
        },

        async searchCards(param: InventoryQueryParameter): Promise<InventoryOverviewDto[]> {
            const endpoint = `api/Inventory/SearchCards`;
            const result = await Post(endpoint, param);
            return result;
        },
        async getInventoryDetail(cardId: number): Promise<InventoryDetailDto> {
            const endpoint = `api/Inventory/GetInventoryDetail?cardId=${cardId}`;
            const result = await Get(endpoint);
            return result;
        },

        async getCollectionBuilderSuggestions(): Promise<InventoryOverviewDto[]> {
            const endpoint = `api/Inventory/GetCollectionBuilderSuggestions`;
            const result = await Get(endpoint);
            return result;
        },
        async hideCollectionBuilderSuggestion(dto: InventoryOverviewDto): Promise<void> {
            const endpoint = `api/Inventory/HideCollectionBuilderSuggestion`;
            const result = await Post(endpoint, dto);
            return result;
        },
        
        async getTrimmingTips(): Promise<InventoryOverviewDto[]> {
            const endpoint = `api/Inventory/GetTrimmingTips`;
            const result = await Get(endpoint);
            return result;
        },
        async hideTrimmingTip(dto: InventoryOverviewDto): Promise<void> {
            const endpoint = `api/Inventory/HideTrimmingTip`;
            const result = await Post(endpoint, dto);
            return result;
        },

        async validateCarpentryImport(dto: CardImportDto): Promise<ValidatedCarpentryImportDto> {
            const endpoint = `api/Inventory/ValidateCarpentryImport`;
            const result = await Post(endpoint, dto);
            return result;
        },
        async addValidatedCarpentryImport(dto: ValidatedCarpentryImportDto): Promise<void> {
            const endpoint = `api/Inventory/AddValidatedCarpentryImport`;
            const result = await Post(endpoint, dto);
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

