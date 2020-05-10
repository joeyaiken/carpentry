// import { AppContainerEnum } from '../reducers/core.reducer';
import Redux, { Store, Dispatch, compose, combineReducers } from 'redux';
// import { AppState } from '../reducers';
// import { api_Decks_Search, api_Decks_Get, api_Decks_Add } from './api';

/**
 * New actions from refactor
 */
export const API_DATA_REQUESTED = 'API_DATA_REQUESTED';
export const apiDataRequested = (scope: ApiScopeOption, data?: any): ReduxAction => ({
    type: API_DATA_REQUESTED,
    payload: {
        scope: scope,
        data: data,
    },
});

export const API_DATA_RECEIVED = 'API_DATA_RECEIVED';
export const apiDataReceived = (scope: ApiScopeOption, data: any): ReduxAction => ({
   type: API_DATA_RECEIVED,
   payload: {
       scope: scope,
       data: data,
   },
});