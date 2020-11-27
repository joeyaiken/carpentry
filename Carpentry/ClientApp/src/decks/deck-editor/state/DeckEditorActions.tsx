// import { Dispatch } from 'redux';
import { push } from "connected-react-router";
import { Dispatch } from "redux";
import { decksApi } from "../../../api/decksApi";
import { AppState } from "../../../configureStore";
import { reloadDeckDetail, requestDeckOverviews } from "../../state/decksDataActions";

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

export const requestSavePropsModal = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        trySaveDeckProps(dispatch, getState());
    }
}

export const DECK_EDITOR_SAVE_REQUESTED = 'DECK_EDITOR_SAVE_REQUESTED';
export const deckEditorSaveRequested = (): ReduxAction => ({
    type: DECK_EDITOR_SAVE_REQUESTED,
});

export const DECK_EDITOR_SAVE_RECEIVED = 'DECK_EDITOR_SAVE_RECEIVED';
export const deckEditorSaveReceived = (): ReduxAction => ({
    type: DECK_EDITOR_SAVE_RECEIVED,
});

// export const DECK_PROPS_SAVE_REQUESTED = 'DECK_PROPS_SAVE_REQUESTED';
// export const deckPropsSaveRequested = (): ReduxAction => ({
//     type: DECK_PROPS_SAVE_REQUESTED,
// });

// export const DECK_PROPS_SAVE_RECEIVED = 'DECK_PROPS_SAVE_RECEIVED';
// export const deckPropsSaveReceived = (): ReduxAction => ({
//     type: DECK_PROPS_SAVE_RECEIVED,
// });

function trySaveDeckProps(dispatch: Dispatch, state: AppState): void {
    //do I bother blocking this? Is a double-click that awful in this situation?
    //I guess I don't want to set a poor precident

    var isSaving = state.decks.deckEditor.isSaving;
    const deckPropsToUpdate = state.decks.deckEditor.deckModalProps;
    if(isSaving || deckPropsToUpdate === null){
        return
    }

    //dispatch loading
    //dispatch(deckPropsSaveRequested());
    dispatch(deckEditorSaveRequested());

    //save
    
    // console.log('saving deck props');
    // console.log(deckPropsToUpdate);
    deckPropsToUpdate.basicW = +deckPropsToUpdate.basicW;
    deckPropsToUpdate.basicU = +deckPropsToUpdate.basicU;
    deckPropsToUpdate.basicB = +deckPropsToUpdate.basicB;
    deckPropsToUpdate.basicR = +deckPropsToUpdate.basicR;
    deckPropsToUpdate.basicG = +deckPropsToUpdate.basicG;

    decksApi.updateDeck(deckPropsToUpdate).then(() => {
        dispatch(deckEditorSaveReceived());
        //now what?
        dispatch(closeDeckPropsModal());
        dispatch(reloadDeckDetail(deckPropsToUpdate.id));
        //do i somehow need to refresh?
        //this.props.dispatch(push(`/inventory/${cardId}`));
        // console.log('pushing!');
        // dispatch(push(`/decks/${deckPropsToUpdate.id}`));
    });

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
export const deckPropsModalChanged = (name: string, value: string | number): ReduxAction => ({
    type: DECK_PROPS_MODAL_CHANGED,
    payload: {
        name: name,
        value: value
    }
});

export const requestDisassembleDeck = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        tryDisassembleDeck(dispatch, getState());
    }
}
//Not going to implement this until I'm done removing the .ui and .legacy project
function tryDisassembleDeck(dispatch: Dispatch, state: AppState): void {
    //Not going to implement this until I'm done removing the .ui and .legacy project
    console.log('Prentending to disassemble deck');
    dispatch(closeDeckPropsModal());
    //is[Saving|Loading]Check?
    //dispatch
    //api
    //dispatch
}

export const requestDeleteDeck = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        tryDeleteDeck(dispatch, getState());
    }
}
function tryDeleteDeck(dispatch: Dispatch, state: AppState): void {
    //is[Saving|Loading]Check?
    const isSaving = state.decks.deckEditor.isSaving;
    if(isSaving) return;
    //dispatch
    dispatch(deckEditorSaveRequested());
    //api
    const idToDelete = state.decks.data.detail.deckId;

    decksApi.deleteDeck(idToDelete).then(() => {
        dispatch(deckEditorSaveReceived());
        dispatch(push('/'));
        dispatch(requestDeckOverviews());
    });
}


