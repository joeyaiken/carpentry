import { Dispatch } from 'redux'
import { AppState } from '../_reducers';
// import { NetworkWifiTwoTone, ContactSupportOutlined } from '@material-ui/icons';
// import { arrowFunctionExpression } from '@babel/types';
// import { api } from './api';
// import { appBarAddClicked } from './core.actions';
import { apiDataRequested, apiDataReceived } from './data.actions';
import { inventoryApi } from '../api/inventoryApi';
/**
 * Actions related to the Inventory container
 */

// export const INVENTORY_SEARCH_METHOD_CHANGED = 'INVENTORY_SEARCH_METHOD_CHANGED';
// export const inventorySearchMethodChanged = (method: string): ReduxAction => ({
//     type: INVENTORY_SEARCH_METHOD_CHANGED,
//     payload: method
// });

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

    console.log('adding items, should include variant names');
    console.log(state.data.cardSearch.pendingCards)

    //Object.keys(state.cardSearch.pendingCards).forEach((key: string) => {
    Object.keys(state.data.cardSearch.pendingCards).forEach((key: string) => {

        //need to rethink this


        //see if it exists in the current inventory

        let itemToAdd: PendingCardsDto = state.data.cardSearch.pendingCards[key];
        
        

        itemToAdd.cards.forEach(card => {
            const newCard: InventoryCard = {
                id: 0,
                isFoil: card.isFoil,
                // multiverseId: card.multiverseId,
                statusId: card.statusId,
                // variantName: card.variantName,
                cardId: card.cardId,
                collectorNumber: card.collectorNumber,
                deckCards: [],
                name: card.name,
                set: card.set,
            }

            newCards.push(newCard);

        })
    });
    inventoryApi.addInventoryCardBatch(newCards).then(() => {
        dispatch(inventoryAddComplete());
        dispatch(requestInventoryOverviews());
    });

}

export const INVENTORY_ADD_COMPLETE = 'INVENTORY_ADD_COMPLETE';
export const inventoryAddComplete = (): ReduxAction => ({
    type: INVENTORY_ADD_COMPLETE
});

export const requestInventoryExport = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getInventoryExport(dispatch, getState());
    }
}

function getInventoryExport(dispatch: Dispatch, state: AppState): any {
    //TODO - add "isLoading" block
    inventoryApi.exportInventoryBackup().then((blob) => {
        // const exportFilename = "CarpentryBackup.zip"


    });

}


// /**
//  * 
//  * Inventory api stuff
//  * 
//  */

// //Thunks


// export const requestUpdateInventoryCard = (card: InventoryCard, statusId: number): any => {
//     return (dispatch: Dispatch, getState: any) => {
//         return updateInventoryCard(dispatch, getState(), card, statusId);
//     }   
// }

// function updateInventoryCard(dispatch: Dispatch, state: AppState, card: InventoryCard, statusId: number): any {
//     //TODO: Decide if this should be blocking (probably not?)
//     const updatedCard: InventoryCard = {
//         ...card,
//         statusId: statusId,
//     }
    
//     api_Inventory_Update(updatedCard).then(() => {
//         dispatch(requestInventoryItems());
//         //I HOPE this won't break, will prob require some null catches

//         //dispatch(requestInventoryDetail(updatedCard.name));
//         dispatch(requestInventoryDetail(state.app.inventory.selectedDetailItemName));


//         // if(state.inventory.selectedDetailItem){
//         //     dispatch(requestInventoryDetail(state.inventory.selectedDetailItem.cards[0].name));
//         // }
//     });
// }

// export const requestDeleteInventoryCard = (cardId: number): any => {
//     return (dispatch: Dispatch, getState: any) => {
//         return deleteInventoryCard(dispatch, getState(), cardId);
//     }   
// }

// function deleteInventoryCard(dispatch: Dispatch, state: AppState, cardId: number): any {
//     //TODO: Decide if this should be blocking (probably not?)
    
//     api_Inventory_Delete(cardId).then(() => {
//         dispatch(requestInventoryItems());
//         //I HOPE this won't break, will prob require some null catches

//         //I need "selectedInventoryDetailName" or "selectedInventoryDetailId" as an AppState property, not just "Selected card"

//         if(state.app.inventory.selectedDetailItemName){
//             dispatch(requestInventoryDetail(state.app.inventory.selectedDetailItemName));
//         }
//     });
// }
