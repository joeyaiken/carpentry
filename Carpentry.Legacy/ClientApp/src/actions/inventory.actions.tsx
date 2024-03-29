import { Dispatch } from 'redux'
import { AppState } from '../reducers';
// import { NetworkWifiTwoTone, ContactSupportOutlined } from '@material-ui/icons';
// import { arrowFunctionExpression } from '@babel/types';
import { api_Inventory_AddBatch, api_Inventory_Search, api_Inventory_GetByName, api_Inventory_Delete, api_Inventory_Update } from './api';
// import { appBarAddClicked } from './core.actions';
import { apiDataRequested, apiDataReceived } from './data.actions';

/**
 * Actions related to the Inventory container
 */

// export const INVENTORY_FILTER_CHANGED = 'INVENTORY_FILTER_CHANGED';
// export const inventoryFilterChanged = (filter: string, value: string | boolean): ReduxAction => ({
//     type: INVENTORY_FILTER_CHANGED,
//     payload: {
//         filter: filter, 
//         value: value
//     }
// });

export const INVENTORY_SEARCH_METHOD_CHANGED = 'INVENTORY_SEARCH_METHOD_CHANGED';
export const inventorySearchMethodChanged = (method: string): ReduxAction => ({
    type: INVENTORY_SEARCH_METHOD_CHANGED,
    payload: method
});

export const requestAddCardsFromSearch = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        addCardsFromSearch(dispatch, getState());
    }
}
export const CARD_SEARCH_SAVE_PENDING_CARDS = 'CARD_SEARCH_SAVE_PENDING_CARDS';
export const cardSearchSavingPendingCards = (): ReduxAction => ({
    type: CARD_SEARCH_SAVE_PENDING_CARDS
});

function addCardsFromSearch(dispatch: Dispatch, state: AppState){
    if(state.app.cardSearch.pendingCardsSaving){
        return;
    }

    dispatch(cardSearchSavingPendingCards())

    // console.log('addingCards');
    // console.log(state.cardSearch.pendingCards);

    //cards to add from pending
    //var cardsToTryAdding = state.cardSearch.cardSearchPendingCards

    //for each of the keys in pending cards...

    let newCards: InventoryCard[] = [];

    //Object.keys(state.cardSearch.pendingCards).forEach((key: string) => {
    Object.keys(state.data.cardSearchPendingCards.pendingCards).forEach((key: string) => {

        //need to rethink this


        //see if it exists in the current inventory

        let itemToAdd: PendingCardsDto = state.data.cardSearchPendingCards.pendingCards[key];
        
        itemToAdd.cards.forEach(card => {
            const newCard: InventoryCard = {
                id: 0,
                isFoil: card.isFoil,
                multiverseId: card.multiverseId,
                statusId: card.statusId,
                variantName: card.variantName,
                deckCards: [],
                name: card.name,
                set: card.set,
            }

            newCards.push(newCard);

        })
    });
    api_Inventory_AddBatch(newCards).then(() => {
        dispatch(inventoryAddComplete());
        dispatch(requestInventoryItems());
    });

}

export const INVENTORY_ADD_COMPLETE = 'INVENTORY_ADD_COMPLETE';
export const inventoryAddComplete = (): ReduxAction => ({
    type: INVENTORY_ADD_COMPLETE
});

/**
 * 
 * Inventory api stuff
 * 
 */

//Thunks

//InventoryItems = Overview
export const requestInventoryItems = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getInventoryItems(dispatch, getState());
    }
}

function getInventoryItems(dispatch: Dispatch, state: AppState): any {
    const _localApiScope: ApiScopeOption = "inventoryOverview";
    // console.log('actions - getInventoryItems START');
    //const dataQueryInProgress = state.inventory.overviewIsLoading;
    const dataQueryInProgress = state.data.isLoading.inventoryOverview;
    if(dataQueryInProgress){
        return;
    }
    // console.log('actions - getInventoryItems - calling inventoryItemsRequested');

    //dispatch(inventoryItemsRequested());
    dispatch(apiDataRequested(_localApiScope));
    
    // console.log('actions - getInventoryItems - calling api_getAllInventoryItems');

    const param: InventoryQueryParameter = {
        //groupBy: state.inventory.searchFilter.searchMethod,
        //groupBy: state.inventory.searchMethod,
        groupBy: state.app.inventory.searchMethod,
        text: state.ui.inventoryFilterProps.text,
        //sets: [],
        colors: state.ui.inventoryFilterProps.colorIdentity,
        types: [],
        skip: 0,
        take: 100,
        format: state.ui.inventoryFilterProps.format,
        sort: '',//state.inventory.searchFilter.sort,
        //sort: 'name',
        //set: 'eld'
        set: state.ui.inventoryFilterProps.set,
        exclusiveColorFilters: state.ui.inventoryFilterProps.exclusiveColorFilters,
        multiColorOnly: state.ui.inventoryFilterProps.multiColorOnly,
        maxCount: state.ui.inventoryFilterProps.maxCount || 0,
        minCount: state.ui.inventoryFilterProps.minCount || 0,
        type: state.ui.inventoryFilterProps.type,
        //rarity: state.cardSearch.cardSearchFilter.props.rarity,
        rarity: state.ui.inventoryFilterProps.rarity,
        
    }


    api_Inventory_Search(param).then((result) => {

        //InventoryQueryResult[]

        //dispatch(inventoryItemsReceived(result));
        dispatch(apiDataReceived(_localApiScope,result));
    });

}
export const requestInventoryDetail = (name: string | null): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getInventoryDetail(dispatch, getState(), name);
    }
}

function getInventoryDetail(dispatch: Dispatch, state: AppState, name: string | null): any {
    const _localApiScope: ApiScopeOption = "inventoryDetail";

    //const queryInProgress = state.inventory.detailIsLoading;
    const queryInProgress = state.data.isLoading.inventoryDetail;

    if(queryInProgress){
        return;
    }
    
    if(!name){
        //dispatch(inventoryDetailReceived(null));
        dispatch(apiDataReceived(_localApiScope, null));
    } else {
        // dispatch(inventoryDetailRequested());
        dispatch(apiDataRequested(_localApiScope));

        api_Inventory_GetByName(name).then((result) => {
            dispatch(apiDataReceived(_localApiScope, result));
        });
    }
}

export const requestUpdateInventoryCard = (card: InventoryCard, statusId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        return updateInventoryCard(dispatch, getState(), card, statusId);
    }   
}

function updateInventoryCard(dispatch: Dispatch, state: AppState, card: InventoryCard, statusId: number): any {
    //TODO: Decide if this should be blocking (probably not?)
    const updatedCard: InventoryCard = {
        ...card,
        statusId: statusId,
    }
    
    api_Inventory_Update(updatedCard).then(() => {
        dispatch(requestInventoryItems());
        //I HOPE this won't break, will prob require some null catches

        //dispatch(requestInventoryDetail(updatedCard.name));
        dispatch(requestInventoryDetail(state.app.inventory.selectedDetailItemName));


        // if(state.inventory.selectedDetailItem){
        //     dispatch(requestInventoryDetail(state.inventory.selectedDetailItem.cards[0].name));
        // }
    });
}

export const requestDeleteInventoryCard = (cardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        return deleteInventoryCard(dispatch, getState(), cardId);
    }   
}

function deleteInventoryCard(dispatch: Dispatch, state: AppState, cardId: number): any {
    //TODO: Decide if this should be blocking (probably not?)
    
    api_Inventory_Delete(cardId).then(() => {
        dispatch(requestInventoryItems());
        //I HOPE this won't break, will prob require some null catches

        //I need "selectedInventoryDetailName" or "selectedInventoryDetailId" as an AppState property, not just "Selected card"

        if(state.app.inventory.selectedDetailItemName){
            dispatch(requestInventoryDetail(state.app.inventory.selectedDetailItemName));
        }
    });
}
