import { push } from "react-router-redux";
import { Dispatch } from "redux";
// import { decksApi } from "../../../api/decksApi";
import { AppState } from "../../../configureStore";
// import { requestDeckOverviews } from "../../state/decksDataActions";


// export const NEW_DECK_PROPERTY_CHANGED = 'NEW_DECK_PROPERTY_CHANGED';
// export const newDeckPropertyChanged = (name: string, value: string): ReduxAction => ({
//     type: NEW_DECK_PROPERTY_CHANGED,
//     payload: {
//         name: name,
//         value: value
//     }
// });

// export const NEW_DECK_MODAL_CLOSED = 'NEW_DECK_MODAL_CLOSED';
// export const newDeckModalClosed = (): ReduxAction => ({
//     type: NEW_DECK_MODAL_CLOSED
// });

// export const requestSaveNewDeck = (): any => {
//     return(dispatch: Dispatch, getState: any) => {
//         trySaveNewDeck(dispatch, getState());
//     }
// }

// export const NEW_DECK_SAVE_REQUESTED = 'NEW_DECK_SAVE_REQUESTED';
// export const newDeckSaveRequested = (): ReduxAction => ({
//     type: NEW_DECK_SAVE_REQUESTED
// });

// export const NEW_DECK_SAVE_COMPLETE = 'NEW_DECK_SAVE_COMPLETE';
// export const newDeckSaveComplete = (): ReduxAction => ({
//     type: NEW_DECK_SAVE_COMPLETE
// });

// function trySaveNewDeck(dispatch: Dispatch, state: AppState): any {
    
//     const isSaving = false;
//     if(isSaving){
//         return;
//     }
    
//     dispatch(newDeckSaveRequested());
    
//     const deckToSave = state.decks.newDeck.deckProps;

//     decksApi.addDeck(deckToSave).then(newId => {
//         dispatch(newDeckSaveComplete());
//         dispatch(requestDeckOverviews());
//         dispatch(push(`/decks/${newId}`));
//     });
// }