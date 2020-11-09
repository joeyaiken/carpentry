import { Dispatch } from 'redux';
import { decksApi } from '../../api/decksApi';
import { AppState } from '../../configureStore';
// import { decksApi } from '../api/decksApi';
// import { AppState } from '../_reducers';
// // import { api } from './api';
// import { apiDataRequested, apiDataReceived } from './data.actions';

export const ensureDeckOverviewsLoaded = (): any => {
    console.log('ensureDeckOverviewsLoaded')
    return(dispatch: Dispatch, getState: any) => {
        tryLoadDeckOverviews(dispatch, getState());
    }
}

// export const requestDeckOverviews = (): any => {
//     console.log('requestDeckOverviews')
//     return (dispatch: Dispatch, getState: any) => {
//         getDeckOverviews(dispatch, getState());
//     }
// }

function tryLoadDeckOverviews(dispatch: Dispatch, state: AppState): void {
    console.log('tryLoadDeckOverviews')
    if(!state.decks.data.overviews.isInitialized){
        getDeckOverviews(dispatch, state);
    }
}

function getDeckOverviews(dispatch: Dispatch, state: AppState): any  {

    if(state.decks.data.overviews.isLoading){
        console.log('isLoading, returning')
        return;
    }

    console.log('requesting')
    dispatch(deckOverviewsRequested());
    console.log('now')
    decksApi.getDeckOverviews().then((results) => {
        console.log('loaded, returning')
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

//Deck Detail / Deck Editor data info will go here

export const DECK_DETAIL_REQUESTED = 'DECK_DETAIL_REQUESTED';
export const DECK_DETAIL_RECEIVED = 'DECK_DETAIL_RECEIVED';