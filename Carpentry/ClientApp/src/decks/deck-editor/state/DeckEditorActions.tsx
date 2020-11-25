// import { Dispatch } from 'redux';

import { type } from "os";
import { Dispatch } from "redux";
import { decksApi } from "../../../api/decksApi";
import { AppState } from "../../../configureStore";

export const TOGGLE_DECK_VIEW_MODE = 'TOGGLE_DECK_VIEW_MODE';
export const toggleDeckViewMode = (): ReduxAction => ({
    type: TOGGLE_DECK_VIEW_MODE,
});

export const DECK_EDITOR_CARD_SELECTED = 'DECK_EDITOR_CARD_SELECTED';
export const deckEditorCardSelected = (cardOverview: DeckCardOverview): ReduxAction => ({
    type: DECK_EDITOR_CARD_SELECTED,
    payload: cardOverview
});

export const OPEN_DECK_PROPS_MODAL = 'OPEN_DECK_PROPS_MODAL';
export const openDeckPropsModal = (deckProps: DeckPropertiesDto | null): ReduxAction => ({
    type: OPEN_DECK_PROPS_MODAL,
    payload: deckProps,
});

export const CLOSE_DECK_PROPS_MODAL = 'CLOSE_DECK_PROPS_MODAL';
export const closeDeckPropsModal = (): ReduxAction => ({
    type: CLOSE_DECK_PROPS_MODAL,
});

//export const requestSavePropsModal, 
export const requestSavePropsModal = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        trySaveDeckProps(dispatch, getState());
    }
}

export const DECK_PROPS_SAVE_REQUESTED = 'DECK_PROPS_SAVE_REQUESTED';
export const deckPropsSaveRequested = (): ReduxAction => ({
    type: DECK_PROPS_SAVE_REQUESTED,
});

export const DECK_PROPS_SAVE_RECEIVED = 'DECK_PROPS_SAVE_RECEIVED';
export const deckPropsSaveReceived = (): ReduxAction => ({
    type: DECK_PROPS_SAVE_RECEIVED,
});

function trySaveDeckProps(dispatch: Dispatch, state: AppState): void {
    //do I bother blocking this? Is a double-click that awful in this situation?
    //I guess I don't want to set a poor precident

    var isLoading = false;
    if(isLoading) return;

    //dispatch loading
    dispatch(deckPropsSaveRequested());

    //save
    //const deckPropsToUpdate = state.decks.deckEditor.

    // decksApi.updateDeck()

    //dispatch received


//     if(state.decks.data.detail.isLoading || state.decks.data.detail.deckId === deckId){
//         return;
//     }
//     dispatch(deckDetailRequested(deckId));
//     decksApi.getDeckDetail(deckId).then((result) => {
//         dispatch(deckDetailReceived(result));
//     });
}

export const DECK_PROPS_MODAL_CHANGED = 'DECK_PROPS_MODAL_CHANGED';
export const deckPropsModalChanged = (name: string, value: string): ReduxAction => ({
    type: DECK_PROPS_MODAL_CHANGED,
    payload: {
        name: name,
        value: value
    }
});


//requestDissasembleDeck

//requestDeleteDeck