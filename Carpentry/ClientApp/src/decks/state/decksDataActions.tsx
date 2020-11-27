import { Dispatch } from 'redux';
import { decksApi } from '../../api/decksApi';
import { AppState } from '../../configureStore';
// import { decksApi } from '../api/decksApi';
// import { AppState } from '../_reducers';
// // import { api } from './api';
// import { apiDataRequested, apiDataReceived } from './data.actions';

export const requestDeckOverviews = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        getDeckOverviews(dispatch, getState(), true);
    }
}

export const ensureDeckOverviewsLoaded = (): any => {
    return(dispatch: Dispatch, getState: any) => {
        getDeckOverviews(dispatch, getState(), false);
    }
}

function getDeckOverviews(dispatch: Dispatch, state: AppState, forceReload: boolean): any  {
    if(state.decks.data.overviews.isLoading || (!forceReload && state.decks.data.overviews.isInitialized)) return;
    dispatch(deckOverviewsRequested());
    decksApi.getDeckOverviews().then((results) => {
        dispatch(deckOverviewsReceived(results));
    });
}

export const DECK_OVERVIEWS_REQUESTED = 'DECK_OVERVIEWS_REQUESTED';
export const deckOverviewsRequested = (): ReduxAction => ({
    type: DECK_OVERVIEWS_REQUESTED
});

export const DECK_OVERVIEWS_RECEIVED = 'DECK_OVERVIEWS_RECEIVED';
export const deckOverviewsReceived = (data: DeckOverviewDto[]): ReduxAction => ({
    type: DECK_OVERVIEWS_RECEIVED,
    payload: data
});

export const DECK_DETAIL_REQUESTED = 'DECK_DETAIL_REQUESTED';
export const deckDetailRequested = (deckId: number): ReduxAction => ({
    type: DECK_DETAIL_REQUESTED,
    payload: deckId,
});

export const DECK_DETAIL_RECEIVED = 'DECK_DETAIL_RECEIVED';
export const deckDetailReceived = (dto: DeckDetailDto): ReduxAction => ({
    type: DECK_DETAIL_RECEIVED,
    payload: dto,
});

export const reloadDeckDetail = (deckId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryLoadDeckDetail(dispatch, getState(), deckId, true);
    }
}

export const ensureDeckDetailLoaded = (deckId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryLoadDeckDetail(dispatch, getState(), deckId, false);
    }
}

function tryLoadDeckDetail(dispatch: Dispatch, state: AppState, deckId: number, forceReload: boolean): void {
    if (state.decks.data.detail.isLoading || (!forceReload && state.decks.data.detail.deckId === deckId)) return;
    dispatch(deckDetailRequested(deckId));
    decksApi.getDeckDetail(deckId).then((result) => {
        dispatch(deckDetailReceived(result));
    });
}