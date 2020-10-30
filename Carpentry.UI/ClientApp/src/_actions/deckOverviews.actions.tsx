import { Dispatch } from 'redux';
import { decksApi } from '../api/decksApi';
import { AppState } from '../_reducers';
// import { api } from './api';
import { apiDataRequested, apiDataReceived } from './data.actions';

export const ensureDeckOverviewsLoaded = (): any => {
    console.log('ensureDeckOverviewsLoaded')
    return(dispatch: Dispatch, getState: any) => {
        tryLoadDeckOverviews(dispatch, getState());
    }
}

export const requestDeckOverviews = (): any => {
    console.log('requestDeckOverviews')
    return (dispatch: Dispatch, getState: any) => {
        getDeckOverviews(dispatch, getState());
    }
}

function tryLoadDeckOverviews(dispatch: Dispatch, state: AppState): void {
    console.log('tryLoadDeckOverviews')
    if(!state.data.deckOverviews.isInitialized){
        getDeckOverviews(dispatch, state);
    }
}

function getDeckOverviews(dispatch: Dispatch, state: AppState): any  {
    console.log('getDeckOverviews')
    const _localApiScope: ApiScopeOption = "deckOverviews";

    if(state.data.deckOverviews.isLoading){
        console.log('isLoading, returning')
        return;
    }

    console.log('requesting')
    dispatch(apiDataRequested(_localApiScope));
    console.log('now')
    decksApi.getDeckOverviews().then((results) => {
        console.log('loaded, returning')
        dispatch(apiDataReceived(_localApiScope, results));
    });
}