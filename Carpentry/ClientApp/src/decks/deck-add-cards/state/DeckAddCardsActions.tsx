import { Dispatch } from "redux";
import { cardSearchApi } from "../../../api/cardSearchApi";
import { decksApi } from "../../../api/decksApi";
import { inventoryApi } from "../../../api/inventoryApi";
import { AppState } from "../../../configureStore";

export const CARD_SEARCH_FILTER_VALUE_CHANGED = 'DECK_ADD_CARDS.CARD_SEARCH_FILTER_VALUE_CHANGED';
export const cardSearchFilterValueChanged = (type: 'inventoryFilterProps' | 'cardSearchFilterProps', filter: string, value: string | boolean): ReduxAction => ({
    type: CARD_SEARCH_FILTER_VALUE_CHANGED,
    payload: {
        type: type,
        filter: filter,
        value: value,
    }
});

export const TOGGLE_CARD_SEARCH_VIEW_MODE = 'DECK_ADD_CARDS.TOGGLE_CARD_SEARCH_VIEW_MODE';
export const toggleCardSearchViewMode = (): ReduxAction => ({
    type: TOGGLE_CARD_SEARCH_VIEW_MODE
});

//Select search result (to see variant / foil options)
export const CARD_SEARCH_SELECT_CARD = 'DECK_ADD_CARDS.CARD_SEARCH_SELECT_CARD';
export const cardSearchSelectCard = (card: CardSearchResultDto): ReduxAction => ({
    type: CARD_SEARCH_SELECT_CARD,
    payload: card
});


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

export const CARD_SEARCH_ADDING_DECK_CARD = 'DECK_ADD_CARDS.CARD_SEARCH_ADDING_DECK_CARD';
export const cardSearchAddingDeckCard = (): ReduxAction => ({
    type: CARD_SEARCH_ADDING_DECK_CARD
});

export const requestCardSearch = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return searchCards(dispatch, getState());
    }   
}

export const CARD_SEARCH_REQUESTED = 'DECK_ADD_CARDS.CARD_SEARCH_REQUESTED';
export const cardSearchRequested = (): ReduxAction => ({
    type: CARD_SEARCH_REQUESTED,
});

export const CARD_SEARCH_RECEIVED = 'DECK_ADD_CARDS.CARD_SEARCH_RECEIVED'
export const cardSearchReceived = (results: CardSearchResultDto[]): ReduxAction => ({
    type: CARD_SEARCH_RECEIVED,
    payload: results,

});

function searchCards(dispatch: Dispatch, state: AppState): any{

    const containerState = state.decks.deckAddCards;

    const searchInProgress: boolean = containerState.searchResults.isLoading;

    if(searchInProgress){
        return;
    }
    dispatch(cardSearchRequested());

    const currentFilterProps = state.decks.deckAddCards.searchFilterProps;

    const param: CardSearchQueryParameter = {
        text: currentFilterProps.text,
        colorIdentity: currentFilterProps.colorIdentity,
        exclusiveColorFilters: currentFilterProps.exclusiveColorFilters,
        multiColorOnly: currentFilterProps.multiColorOnly,
        rarity: currentFilterProps.rarity,
        set: currentFilterProps.set,
        type: currentFilterProps.type,
        searchGroup: currentFilterProps.group,
        excludeUnowned: false,
    }

    cardSearchApi.searchInventory(param).then((results) => {
        dispatch(cardSearchReceived(results));
    });
}


//This should probably be a singular action (cardSearchSelectCard)
//cardSerchLoadInventory
export const requestCardSearchInventory = (card: CardSearchResultDto): any => {
    return (dispatch: Dispatch, getState: any) => {
        return searchCardSearchInventory(dispatch, getState(), card);
    }
}

export const CARD_SEARCH_INVENTORY_REQUESTED = 'DECK_ADD_CARDS.CARD_SEARCH_INVENTORY_REQUESTED';
export const cardSearchInventoryRequested = (): ReduxAction => ({
    type: CARD_SEARCH_INVENTORY_REQUESTED,
});

export const CARD_SEARCH_INVENTORY_RECEIVED = 'DECK_ADD_CARDS.CARD_SEARCH_INVENTORY_RECEIVED';
export const cardSearchInventoryReceived = (payload: InventoryDetailDto): ReduxAction => ({
    type: CARD_SEARCH_INVENTORY_RECEIVED,
    payload: payload,
});


//I don't really want to solve this tonight
function searchCardSearchInventory(dispatch: Dispatch, state: AppState, card: CardSearchResultDto): any{
    //need another bool for isSearching
    const searchInProgress: boolean = state.decks.deckAddCards.inventoryDetail.isLoading;
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

