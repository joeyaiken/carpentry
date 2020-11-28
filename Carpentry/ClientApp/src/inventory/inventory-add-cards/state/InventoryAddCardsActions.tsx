import { push } from "react-router-redux";
import { Dispatch } from "redux";
import { cardSearchApi } from "../../../api/cardSearchApi";
import { inventoryApi } from "../../../api/inventoryApi";
import { AppState } from "../../../configureStore";

export const requestSearch = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return trySearchCards(dispatch, getState());
    }   
}

export const SEARCH_REQUESTED = 'INVENTORY_ADD_CARDS.SEARCH_REQUESTED';
export const searchRequested = (): ReduxAction => ({
    type: SEARCH_REQUESTED,
});

export const SEARCH_RECEIVED = 'INVENTORY_ADD_CARDS.SEARCH_RECEIVED'
export const searchReceived = (results: CardSearchResultDto[]): ReduxAction => ({
    type: SEARCH_RECEIVED,
    payload: results,

});

function trySearchCards(dispatch: Dispatch, state: AppState): any{
    const searchInProgress: boolean = state.inventory.inventoryAddCards.searchResults.isLoading;
    if(searchInProgress){
        return;
    }
    dispatch(searchRequested());
    const containerState = state.inventory.inventoryAddCards;
    if(containerState.cardSearchMethod === "web"){
        const { cardName, exclusiveName } = containerState.searchFilter;
        cardSearchApi.searchWeb(cardName, exclusiveName).then((results) =>{
            dispatch(searchReceived(results));
        });
    } else {
        const currentFilterProps = containerState.searchFilter;
        const param: CardSearchQueryParameter = {
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
            dispatch(searchReceived(results));
        });
    }
}

export const ADD_PENDING_CARD = 'INVENTORY_ADD_CARDS.ADD_PENDING_CARD';
export const addPendingCard = (name: string, cardId: number, isFoil: boolean) =>({
    type: ADD_PENDING_CARD,
    payload: {
        name: name,
        cardId: cardId,
        isFoil: isFoil,
    }
});

export const REMOVE_PENDING_CARD = 'INVENTORY_ADD_CARDS.REMOVE_PENDING_CARD';
export const removePendingCard = (name: string, cardId: number, isFoil: boolean) =>({
    type: REMOVE_PENDING_CARD,
    payload: {
        name: name,
        cardId: cardId,
        isFoil: isFoil,
    }
});

export const FILTER_VALUE_CHANGED = 'INVENTORY_ADD_CARDS.FILTER_VALUE_CHANGED';
export const cardSearchFilterValueChanged = (type: 'inventoryFilterProps' | 'cardSearchFilterProps', filter: string, value: string | boolean): ReduxAction => ({
    type: FILTER_VALUE_CHANGED,
    payload: {
        type: type,
        filter: filter,
        value: value,
    }
});

export const TOGGLE_CARD_SEARCH_VIEW_MODE = 'INVENTORY_ADD_CARDS.TOGGLE_CARD_SEARCH_VIEW_MODE';
export const toggleCardSearchViewMode = (): ReduxAction => ({
    type: TOGGLE_CARD_SEARCH_VIEW_MODE
});


export const CARD_SEARCH_SEARCH_METHOD_CHANGED = 'INVENTORY_ADD_CARDS.CARD_SEARCH_SEARCH_METHOD_CHANGED';
export const cardSearchSearchMethodChanged = (method: string): ReduxAction => ({
    type: CARD_SEARCH_SEARCH_METHOD_CHANGED,
    payload: method
});

export const CARD_SEARCH_CLEAR_PENDING_CARDS = 'INVENTORY_ADD_CARDS.CARD_SEARCH_CLEAR_PENDING_CARDS'
export const cardSearchClearPendingCards = () =>({
    type: CARD_SEARCH_CLEAR_PENDING_CARDS
});

export const CARD_SEARCH_SELECT_CARD = 'INVENTORY_ADD_CARDS.CARD_SEARCH_SELECT_CARD'; //Select search result (to see variant / foil options)
export const cardSearchSelectCard = (card: CardSearchResultDto): ReduxAction => ({
    type: CARD_SEARCH_SELECT_CARD,
    payload: card
});

export const requestSavePendingCards = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        trySavePendingCards(dispatch, getState());
    }
}

export const SAVE_REQUESTED = 'INVENTORY_ADD_CARDS.SAVE_REQUESTED';
export const saveRequested = (): ReduxAction => ({
    type: SAVE_REQUESTED,
});

export const SAVE_COMPLETED = 'INVENTORY_ADD_CARDS.SAVE_COMPLETED';
export const saveCompleted = (): ReduxAction => ({
    type: SAVE_COMPLETED,
});

function trySavePendingCards(dispatch: Dispatch, state: AppState){
    const isSaving = state.inventory.inventoryAddCards.pendingCardsSaving;
    if(isSaving){
        return;
    }

    dispatch(saveRequested())

    let newCards: InventoryCard[] = [];

    Object.keys(state.inventory.inventoryAddCards.pendingCards).forEach((key: string) => {
        let itemToAdd: PendingCardsDto = state.inventory.inventoryAddCards.pendingCards[key];
        itemToAdd.cards.forEach(card => {
            const newCard: InventoryCard = {
                id: 0,
                isFoil: card.isFoil,
                statusId: card.statusId,
                cardId: card.cardId,
                collectorNumber: card.collectorNumber,
                deckCards: [],
                name: card.name,
                set: card.set,
            }
            newCards.push(newCard);
        });
    });

    inventoryApi.addInventoryCardBatch(newCards).then(() => {
        dispatch(saveCompleted());
        dispatch(push('/inventory'));
    });
}
