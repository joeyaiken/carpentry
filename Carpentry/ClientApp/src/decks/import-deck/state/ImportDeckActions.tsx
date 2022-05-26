import { push } from "react-router-redux";
import { Dispatch } from "redux";
import { decksApi } from "../../../api/decksApi";
import { RootState } from "../../../configureStore";

export const requestValidateImport = (dto: CardImportDto): any => {
    return(dispatch: Dispatch, getState: any) => {
        tryValidateImport(dispatch, getState(), dto);
    }
}

function tryValidateImport(dispatch: Dispatch, state: RootState, dto: CardImportDto): any {
    const { isValidating, isSaving } = state.decks.importDeck;
    if(isValidating || isSaving) return;

    dispatch(validateImportRequested());

    decksApi.validateDeckImport(dto).then((result) => {
        dispatch(validateImportReceived(result));
    });
}

export const VALIDATE_IMPORT_REQUESTED = 'VALIDATE_IMPORT_REQUESTED';
export const validateImportRequested = (): ReduxAction => ({
    type: VALIDATE_IMPORT_REQUESTED,
});

export const VALIDATE_IMPORT_RECEIVED = 'VALIDATE_IMPORT_RECEIVED';
export const validateImportReceived = (payload: ValidatedDeckImportDto): ReduxAction => ({
    type: VALIDATE_IMPORT_RECEIVED,
    payload: payload
});

// export const DECK_EXPORT_REQUESTED = 'DECK_EXPORT.DECK_EXPORT_REQUESTED';
// export const deckExportRequested = (): ReduxAction => ({
//     type: DECK_EXPORT_REQUESTED,
// });

// export const DECK_EXPORT_RECEIVED = 'DECK_EXPORT.DECK_EXPORT_RECEIVED';
// export const deckExportReceived = (resultPayload: string): ReduxAction => ({
//     type: DECK_EXPORT_RECEIVED,
//     payload: resultPayload,
// });

export const requestSaveImport = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        trySaveImport(dispatch, getState());
    }
}

function trySaveImport(dispatch: Dispatch, state: RootState): any {
    
    const { isValidating, isSaving } = state.decks.importDeck;
    if(isValidating || isSaving) return;

    dispatch(saveImportRequested());

    //const dto = state.decks.importDeck.

    // decksApi.addValidatedDeckImport().then((newId) => {
    //     dispatch(saveImportReceived());
    //     dispatch(push(`/decks/${newId}`));
    // });    
}

export const SAVE_IMPORT_REQUESTED = 'SAVE_IMPORT_REQUESTED';
export const saveImportRequested = (): ReduxAction => ({
    type: SAVE_IMPORT_REQUESTED,
});

export const SAVE_IMPORT_RECEIVED = 'SAVE_IMPORT_RECEIVED';
export const saveImportReceived = (): ReduxAction => ({
    type: SAVE_IMPORT_RECEIVED
});








//importDeckPropertyChanged



export const IMPORT_DECK_PROPERTY_CHANGED = 'IMPORT_DECK_PROPERTY_CHANGED';
export const importDeckPropertyChanged = (name: string, value: string): ReduxAction => ({
    type: IMPORT_DECK_PROPERTY_CHANGED,
    payload: {
        name: name,
        value: value
    }
});

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