//actions for the Deck List container & related files


import Redux, { Store, Dispatch, compose, combineReducers } from 'redux';
import { AppState } from '../reducers';
import { api_Decks_Search, api_Inventory_GetByName, api_Decks_Delete } from './api';
import { apiDataRequested, apiDataReceived } from './data.actions';

export const requestDeckList = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        getDeckList(dispatch, getState());
    }
}

function getDeckList(dispatch: Dispatch, state: AppState): any  {
    const _localApiScope: ApiScopeOption = "deckList";

    if(state.data.isLoading.deckList){
        return;
    }

    dispatch(apiDataRequested(_localApiScope));

    api_Decks_Search([]).then((results) => {
        dispatch(apiDataReceived(_localApiScope, results));
    });
}

export const requestDeleteDeck = (deckId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        deleteDeck(dispatch, getState(), deckId);
    }
}

function deleteDeck(dispatch: Dispatch, state: AppState, deckId: number): any {
    api_Decks_Delete(deckId).then(() => {
        dispatch(requestDeckList());
    });
}
