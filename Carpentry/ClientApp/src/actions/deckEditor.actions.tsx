import { Dispatch } from 'redux';
// import { api_Decks_RemoveCard, api_Decks_Update } from "./api";

import { AppState } from "../reducers";
import { api_Decks_Get, api_Decks_Update, api_Decks_RemoveCard, api_Decks_UpdateCard } from './api';
import { apiDataReceived, apiDataRequested } from './data.actions';

/*
    Thunks
*/
export const requestDeckDetail = (deckId: number): any => {
    console.log('requesting deck detail')
    return (dispatch: Dispatch, getState: any) => {
        return getDeckDetail(dispatch, getState(), deckId);
    }
}
function getDeckDetail(dispatch: Dispatch, state: AppState, deckId: number): any {    
    const _localApiScope: ApiScopeOption = "deckDetail";
    // dispatch(navigate(AppContainerEnum.DeckEditor));
    dispatch(apiDataRequested(_localApiScope ,deckId));

    api_Decks_Get(deckId).then((result) => {
        dispatch(apiDataReceived(_localApiScope, result));
    });
}

export const requestDeleteDeckCard = (deckCardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        return deleteDeckCard(dispatch, getState(), deckCardId);
    }
};
function deleteDeckCard(dispatch: Dispatch, state: AppState, deckCardId: number): any {
    api_Decks_RemoveCard(deckCardId).then(() => {
        //request an updated deck
        if(state.data.deckDetail.deckProps){
            dispatch(requestDeckDetail(state.data.deckDetail.deckProps.id));
        }
    });
}

export const requestSaveDeck = (props: DeckProperties): any => {
    return (dispatch: Dispatch, getState: any) => {
        return saveDeck(dispatch, getState(), props);
    }
}
function saveDeck(dispatch: Dispatch, state: AppState, props: DeckProperties): any {
    api_Decks_Update(props).then(() => {
        dispatch(requestDeckDetail(props.id));
    })
}

export const requestCancelDeckModalChanges = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return cancelDeckModalChanges(dispatch, getState());
    }
}
function cancelDeckModalChanges(dispatch: Dispatch, state: AppState){
    if(state.data.deckDetail.deckProps){
        console.log('canceling deck modal changes - actions')
        dispatch(requestDeckDetail(state.data.deckDetail.deckProps.id));
    }
}

export const requestUpdateDeckCardStatus = (deckCardId: number, status: string): any => {
    return (dispatch: Dispatch, getState: any) => {
        return updateDeckCardStatus(dispatch, getState(), deckCardId, status);
    }
}

//"status" should be "category" but w/e
function updateDeckCardStatus(dispatch: Dispatch, state: AppState, deckCardId: number, status: string): any {

    const something = state.data.deckDetail.cardDetailsById[deckCardId];

    // declare interface InventoryCard {
    //     id: number;
    //     multiverseId: number;
    //     name: string;
    //     set: string;
    //     isFoil: boolean;
    //     variantName: string;
    //     statusId: number; //normal == 1, buylist == 2, sellList == 3
    //     deckCards: InventoryDeckCardDto[];
    // }

    let categoryId: string | null = null;

    switch(status){
        case "mainboard":
        //     categoryId = null;
            break;
        case "sideboard":
            categoryId = 's';
            break;
        case "commander":
            categoryId = 'c';
            break;
    }


    let dto: DeckCardDto = {
        id: deckCardId,
        inventoryCard: something,
        deckId: state.data.deckDetail.deckId,
        categoryId: categoryId,
        //categoryId        
    }
    console.log('submitting DTO');
    console.log(dto);
    console.log(state);
    api_Decks_UpdateCard(dto).then(() => {
        dispatch(requestDeckDetail(dto.deckId));
    });



    // api_Decks_Update(props).then(() => {
    //     dispatch(requestDeckDetail(props.id));
    // })
}


/*
    Actions
*/

export const DECK_EDITOR_CARD_SELECTED = 'DECK_EDITOR_CARD_SELECTED';
export const deckEditorCardSelected = (cardOverview: InventoryOverviewDto): ReduxAction => ({
    type: DECK_EDITOR_CARD_SELECTED,
    payload: cardOverview
})

export const CARD_MENU_BUTTON_CLICK = 'CARD_MENU_BUTTON_CLICK'
export const cardMenuButtonClick = (element: HTMLElement | null): ReduxAction => ({
    type: CARD_MENU_BUTTON_CLICK,
    payload: element
});

export const DECK_CARD_REQUEST_ALTERNATE_VERSIONS = 'DECK_CARD_REQUEST_ALTERNATE_VERSIONS'
export const deckCardRequestAlternateVersions = (cardName: string): ReduxAction => ({
    type: DECK_CARD_REQUEST_ALTERNATE_VERSIONS,
    payload: cardName
});

export const DECK_PROPERTY_CHANGED = 'DECK_PROPERTY_CHANGED';
export const deckPropertyChanged = (name: string, value: string): ReduxAction => ({
    type: DECK_PROPERTY_CHANGED,
    payload: {
        name: name,
        value: value
    }
});

export const OPEN_DECK_PROPS_MODAL = 'OPEN_DECK_PROPS_MODAL';
export const openDeckPropsModal = (): ReduxAction => ({
    type: OPEN_DECK_PROPS_MODAL,
});

export const TOGGLE_DECK_VIEW_MODE = 'TOGGLE_DECK_VIEW_MODE';
export const toggleDeckViewMode = (): ReduxAction => ({
    type: TOGGLE_DECK_VIEW_MODE,
});
