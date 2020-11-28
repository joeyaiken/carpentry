import { Dispatch } from "redux";
import { cardSearchApi } from "../../../api/cardSearchApi";
import { inventoryApi } from "../../../api/inventoryApi";
import { AppState } from "../../../configureStore";

export const requestCardSearch = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return searchCards(dispatch, getState());
    }   
}

export const CARD_SEARCH_REQUESTED = 'CARD_SEARCH_REQUESTED';
export const cardSearchRequested = (): ReduxAction => ({
    type: CARD_SEARCH_REQUESTED,
});

export const CARD_SEARCH_RECEIVED = 'CARD_SEARCH_RECEIVED'
export const cardSearchReceived = (results: CardSearchResultDto[]): ReduxAction => ({
    type: CARD_SEARCH_RECEIVED,
    payload: results,

});

function searchCards(dispatch: Dispatch, state: AppState): any{
    // const _localApiScope: ApiScopeOption = "cardSearchResults";
    const searchInProgress: boolean = state.cardSearch.data.searchResults.isLoading; //state.cardSearch.isLoading;

    //const cardSearchMethod = state.cardSearch.cardSearchMethod;
    //const cardSearchMethod = state.app.cardSearch.cardSearchMethod;
    const cardSearchMethod = state.cardSearch.state.cardSearchMethod;

    if(searchInProgress){
        return;
    }
    dispatch(cardSearchRequested());
    //why TF am I treating this like a bool?
    if(cardSearchMethod === "web"){
        const { cardName, exclusiveName } = state.cardSearch.state.searchFilter;
        
        cardSearchApi.searchWeb(cardName, exclusiveName).then((results) =>{
            dispatch(cardSearchReceived(results));
        });
    // }else if(cardSearchMethod === "inventory"){

    //     const param: InventoryQueryParameter = {
    //         groupBy: "mid",
    //         text: state.ui.cardSearchFilterProps.text,
    //         colors: state.ui.cardSearchFilterProps.colorIdentity,
    //         types: [],
    //         skip: 0,
    //         take: 500,
    //         format: state.ui.cardSearchFilterProps.format,
    //         sort: '',
    //         set: state.ui.cardSearchFilterProps.set,
    //         // setId: state.ui.cardSearchFilterProps.setId,
    //         exclusiveColorFilters: state.ui.cardSearchFilterProps.exclusiveColorFilters,
    //         multiColorOnly: state.ui.cardSearchFilterProps.multiColorOnly,
    //         maxCount:0,
    //         minCount:0,
    //         type: state.ui.cardSearchFilterProps.type,
    //         rarity: state.ui.cardSearchFilterProps.rarity,
    //         sortDescending: false,
    //     }

    //     api.cardSearch.searchInventory(param).then((results) => {
    //         dispatch(apiDataReceived(_localApiScope, results));
    //     });

    } else {
        const currentFilterProps = state.cardSearch.state.searchFilter;
        //CardSearchQueryParameter
        const param: CardSearchQueryParameter = {
            colorIdentity: currentFilterProps.colorIdentity,
            exclusiveColorFilters: currentFilterProps.exclusiveColorFilters,
            multiColorOnly: currentFilterProps.multiColorOnly,
            rarity: currentFilterProps.rarity,
            // set: currentFilterProps.set,
            set: currentFilterProps.set,
            // setId: currentFilterProps.setId,
            type: currentFilterProps.type,
            searchGroup: currentFilterProps.group,
            excludeUnowned: false,
        }

        // console.log('serch by set')
        // console.log(param);

        cardSearchApi.searchInventory(param).then((results) => {
            dispatch(cardSearchReceived(results));
        })

        // api.cardSearch.searchSet(param).then((results) => {
        //     dispatch(apiDataReceived(_localApiScope, results));
        // })
    }
}


//This should probably be a singular action (cardSearchSelectCard)
//cardSerchLoadInventory
export const requestCardSearchInventory = (card: CardSearchResultDto): any => {
    return (dispatch: Dispatch, getState: any) => {
        return searchCardSearchInventory(dispatch, getState(), card);
    }
}

export const CARD_SEARCH_INVENTORY_REQUESTED = 'CARD_SEARCH_INVENTORY_REQUESTED';
export const cardSearchInventoryRequested = (): ReduxAction => ({
    type: CARD_SEARCH_INVENTORY_REQUESTED,
});

export const CARD_SEARCH_INVENTORY_RECEIVED = 'CARD_SEARCH_INVENTORY_RECEIVED';
export const cardSearchInventoryReceived = (payload: InventoryDetailDto): ReduxAction => ({
    type: CARD_SEARCH_INVENTORY_RECEIVED,
    payload: payload,
});


//I don't really want to solve this tonight
function searchCardSearchInventory(dispatch: Dispatch, state: AppState, card: CardSearchResultDto): any{
    // const _localApiScope: ApiScopeOption = "cardSearchInventoryDetail";
    //need another bool for isSearching
    const searchInProgress: boolean = state.cardSearch.data.inventoryDetail.isLoading;
    if(searchInProgress){
        console.log('DAMNIT THERE IS A SEARCH IN PROGRESS');
        return;
    }
    dispatch(cardSearchInventoryRequested());

    //need to figure out what API call I'm using, should re-use the existing inventory one
    inventoryApi.getInventoryDetail(card.cardId).then((results) =>{
        dispatch(cardSearchInventoryReceived(results));
    });

}

