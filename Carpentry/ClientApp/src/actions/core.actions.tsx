// import { AppContainerEnum } from '../reducers/core.reducer';
import Redux, { Store, Dispatch, compose, combineReducers } from 'redux';
import { AppState } from '../reducers';
import { api_Decks_Search, api_Decks_Get, api_Decks_Add
       , api_Core_GetFilterOptions 
    ,api//, _api

} from './api';
import { apiDataRequested, apiDataReceived } from './data.actions';

/**
 * UI actions for this app
 */

export const APP_BAR_SECTION_TOGGLE = 'APP_BAR_SECTION_TOGGLE';
export const appBarSectionToggle = (): ReduxAction => ({
    type: APP_BAR_SECTION_TOGGLE
});

export const APP_BAR_ADD_CLICKED = 'APP_BAR_ADD_CLICKED';
export const appBarAddClicked = (filters?: FilterDescriptor[]): ReduxAction => ({
    type: APP_BAR_ADD_CLICKED,
    payload: filters,
});

export const NAVIGATE = 'NAVIGATE';
export const navigate = (destination: 'inventory' | 'buyList' | 'cardSearch' | 'newDeck' | null): ReduxAction => ({
    type: NAVIGATE,
    payload: destination
});

export const OPEN_NEW_DECK_MODAL = 'OPEN_NEW_DECK_MODAL';
export const openNewDeckModal = (): ReduxAction => ({
    type: OPEN_NEW_DECK_MODAL
});

export const CANCLE_NEW_DECK = 'CANCLE_NEW_DECK';
export const cancleNewDeck = (): ReduxAction => ({
    type: CANCLE_NEW_DECK
});

export const NEW_DECK_FIELD_CHANGE = 'NEW_DECK_FIELD_CHANGE';
export const newDeckFieldChange = (name: string, value: string): ReduxAction  => ({
    type: NEW_DECK_FIELD_CHANGE,
    payload: {
        name: name,
        value: value
    }
});

//new deck save in progress
export const NEW_DECK_SAVING = 'NEW_DECK_SAVING';
export const newDeckSaving = (): ReduxAction => ({
    type: NEW_DECK_SAVING
});

//new deck save complete
export const NEW_DECK_SAVE_COMPLETE = 'NEW_DECK_SAVE_COMPLETE';
export const newDeckSaveComplete = (): ReduxAction => ({
    type: NEW_DECK_SAVE_COMPLETE
});

export const requestSaveNewDeck = (newDeck: DeckProperties): any => {
    return (dispatch: Dispatch, getState: any) => {
        return saveNewDeck(dispatch, getState(), newDeck);
    }
}
function saveNewDeck(dispatch: Dispatch, state: AppState, newDeck: DeckProperties): any {
    if(state.app.core.newDeckIsSaving){
        return;
    }
    dispatch(newDeckSaving());

    api_Decks_Add(newDeck).then((results) => {
        dispatch(newDeckSaveComplete());
    });
}

export const requestDeckDetail = (deckId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getDeckDetail(dispatch, deckId);
    }
}

function getDeckDetail(dispatch: Dispatch, deckId: number): any {
    const _localApiScope: ApiScopeOption = "deckDetail";
    dispatch(apiDataRequested(_localApiScope, deckId));
    api_Decks_Get(deckId).then((result) => {
        dispatch(apiDataReceived(_localApiScope, result));
    });
}



export const requestCoreData = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getCoreData(dispatch, getState());
    }
}
function getCoreData(dispatch: Dispatch, state: AppState): any {
    const dataIsLoading = state.data.isLoading.coreFilterOptions;
    console.log('get core data ----------');
    if(dataIsLoading) {
        return;
    }
    dispatch(apiDataRequested('coreFilterOptions'));
    // dispatch(newDeckSaving());
    //api.core_GetFilterOptions().then((results) => {
    api.core.getFilterOptions().then((results) => {
        // api_Core_GetFilterOptions().then((results) => {
        dispatch(apiDataReceived('coreFilterOptions', results));
    });
    // api_Decks_Add(newDeck).then((results) => {
    //     dispatch(newDeckSaveComplete());
    // });
}

// export const CORE_DATA_REQUESTED = 'CORE_DATA_REQUESTED';
// export const coreDataRequested = (): ReduxAction  => ({
//     type: CORE_DATA_REQUESTED
// });


// export const coreDataRequested = (name: string, value: string): ReduxAction  => ({
//     type: NEW_DECK_FIELD_CHANGE,
//     payload: {
//         name: name,
//         value: value
//     }
// });
