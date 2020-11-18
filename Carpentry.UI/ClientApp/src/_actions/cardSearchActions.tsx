import { Dispatch } from 'redux'
import { cardSearchApi } from '../api/cardSearchApi';
import { decksApi } from '../api/decksApi';
import { inventoryApi } from '../api/inventoryApi';
import { AppState } from '../_reducers';

//import { api_Cards_SearchSet, api_Cards_SearchWeb, api_Inventory_GetByName, api_Decks_AddCard, api_Decks_UpdateCard, api_Cards_SearchInventory } from './api';
// import { api } from './api';
// import { requestDeckDetail } from './core.actions';
import { apiDataRequested, apiDataReceived } from './data.actions';

//Request card search
export const requestCardSearch = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return searchCards(dispatch, getState());
    }   
}
//Select search result (to see variant / foil options)
export const CARD_SEARCH_SELECT_CARD = 'CARD_SEARCH_SELECT_CARD';
export const cardSearchSelectCard = (card: CardSearchResultDto): ReduxAction => ({
    type: CARD_SEARCH_SELECT_CARD,
    payload: card
});

//This should probably be a singular action (cardSearchSelectCard)
//cardSerchLoadInventory
export const requestCardSearchInventory = (card: CardSearchResultDto): any => {
    return (dispatch: Dispatch, getState: any) => {
        return searchCardSearchInventory(dispatch, getState(), card);
    }
}

//I don't really want to solve this tonight
function searchCardSearchInventory(dispatch: Dispatch, state: AppState, card: CardSearchResultDto): any{
    const _localApiScope: ApiScopeOption = "cardSearchInventoryDetail";
    //need another bool for isSearching
    const searchInProgress: boolean = state.data.cardSearch.inventoryDetail.isLoading;
    if(searchInProgress){
        console.log('DAMNIT THERE IS A SEARCH IN PROGRESS');
        return;
    }
    dispatch(apiDataRequested(_localApiScope));

    //need to figure out what API call I'm using, should re-use the existing inventory one
    inventoryApi.getInventoryDetail(card.cardId).then((results) =>{
        dispatch(apiDataReceived(_localApiScope, results));
    });

}


// //Card sear filter changed
// export const CARD_SEARCH_FILTER_CHANGED = 'CARD_SEARCH_FILTER_CHANGED';
// export const cardSearchFilterChanged = (filter: string, value: string | boolean): ReduxAction => ({
//     type: CARD_SEARCH_FILTER_CHANGED,
//     payload: {
//         filterName: filter, 
//         filterValue: value
//     }
// });

export const CARD_SEARCH_SEARCH_METHOD_CHANGED = 'CARD_SEARCH_SEARCH_METHOD_CHANGED';
export const cardSearchSearchMethodChanged = (method: string): ReduxAction => ({
    type: CARD_SEARCH_SEARCH_METHOD_CHANGED,
    payload: method
});

//clear pending cards
export const CARD_SEARCH_CLEAR_PENDING_CARDS = 'CARD_SEARCH_CLEAR_PENDING_CARDS'
export const cardSearchClearPendingCards = () =>({
    type: CARD_SEARCH_CLEAR_PENDING_CARDS
});
//save pending cards
export const CARD_SEARCH_ADD_PENDING_TO_INVENTORY = 'CARD_SEARCH_ADD_PENDING_TO_INVENTORY'
export const cardSearchAddPendingToInventory = (cards: InventoryCard[]): ReduxAction => ({
    type: CARD_SEARCH_ADD_PENDING_TO_INVENTORY,
    payload: cards
});

//does this technically return a ReduxAction ?
function searchCards(dispatch: Dispatch, state: AppState): any{
    const _localApiScope: ApiScopeOption = "cardSearchResults";
    const searchInProgress: boolean = state.data.cardSearch.searchResults.isLoading; //state.cardSearch.isLoading;

    //const cardSearchMethod = state.cardSearch.cardSearchMethod;
    const cardSearchMethod = state.app.cardSearch.cardSearchMethod;

    if(searchInProgress){
        return;
    }
    dispatch(apiDataRequested(_localApiScope));
    //why TF am I treating this like a bool?
    if(cardSearchMethod === "web"){
        cardSearchApi.searchWeb(state.ui.cardSearchFilterProps.cardName, state.ui.cardSearchFilterProps.exclusiveName).then((results) =>{
            dispatch(apiDataReceived(_localApiScope, results));
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
        const currentFilterProps = state.ui.cardSearchFilterProps;
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
            dispatch(apiDataReceived(_localApiScope, results));
        })

        // api.cardSearch.searchSet(param).then((results) => {
        //     dispatch(apiDataReceived(_localApiScope, results));
        // })
    }
}


export const requestAddDeckCard = (deckCardDto: DeckCardDto): any => {
    return (dispatch: Dispatch, getState: any) => {
        return addDeckCard(dispatch, getState(), deckCardDto);
    }
}

function addDeckCard(dispatch: Dispatch, state: AppState, deckCardDto: DeckCardDto): any{
    dispatch(cardSearchAddingDeckCard());

    // alert('broken code hit - cardSearchActions - addDeckCard')

    // const deckCardDto: DeckCardDto = {
    //     deckId: state.app.core.selectedDeckId || 0,
    //     //deckId: state.de,
    //     id: 0,
    //     inventoryCard: inventoryCard,
    //     categoryId: null,
    // }
    //console.log

    
    decksApi.addDeckCard(deckCardDto).then(() => {


        
        //After response, need to re-request inventory

        //!! This should be re-added eventually
        // if(state.app.cardSearch.selectedCard){
        //     dispatch(requestCardSearchInventory(state.app.cardSearch.selectedCard));
        // }

        // alert('broken code hit - CardSearchActions - addDeckCard (should be added, wont nav)');

        // if(state.app.core.visibleContainer === "deckEditor" && state.data.deckDetail.deckProps != null){
            
        //     //This should be a "navigate to deck detail"
        //     // dispatch(requestDeckDetail(state.data.deckDetail.deckProps.id));
        // }

    })
}

export const CARD_SEARCH_ADDING_DECK_CARD = 'CARD_SEARCH_ADDING_DECK_CARD';

export const cardSearchAddingDeckCard = (): ReduxAction => ({
    type: CARD_SEARCH_ADDING_DECK_CARD
});

//'request move' or 'request update' ?
//moving the selected card to the current deck
export const requestMoveDeckCard = (card: InventoryCard, deckCardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        return moveDeckCard(dispatch, getState(), card, deckCardId);
    }
}

function moveDeckCard(dispatch: Dispatch, state: AppState, card: InventoryCard, deckCardId: number): any {
    //I don't realistically know what this prop will look like yet
    
    alert('broken code hit - cardSearchActions - moveDeckCard')

    // const deckCardDto: DeckCardDto = {
    //     deckId: state.app.core.selectedDeckId || 0,
    //     // deckId: state.data.deckDetail.,
    //     id: deckCardId,
    //     inventoryCard: card,
    //     categoryId: null,
    // }


    // //maybe build a DTO
    // api.Decks.updateCard(deckCardDto).then(() => {

    //     if(state.app.cardSearch.selectedCard)
    //         dispatch(requestCardSearchInventory(state.app.cardSearch.selectedCard));
            
    //    //dispatch - update card search inventory 
    //     //What do I do when a deck card is added from here?

    //     //inv does
    //    //dispatch(requestInventoryDetail(state.inventory.selectedDetailItem.cards[0].name));
    // });
}

export const TOGGLE_CARD_SEARCH_VIEW_MODE = 'TOGGLE_CARD_SEARCH_VIEW_MODE';

export const toggleCardSearchViewMode = (): ReduxAction => ({
    type: TOGGLE_CARD_SEARCH_VIEW_MODE
});