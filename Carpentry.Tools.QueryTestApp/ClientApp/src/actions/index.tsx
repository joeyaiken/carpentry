import { Dispatch } from 'redux'
import { AppState } from '../reducers';
import { api } from './api';

export const API_DATA_REQUESTED = 'API_DATA_REQUESTED';
export const apiDataRequested = (data?: any): ReduxAction => ({
    type: API_DATA_REQUESTED,
    payload: {
        data: data,
    },
});

export const API_DATA_RECEIVED = 'API_DATA_RECEIVED';
export const apiDataReceived = (data: any): ReduxAction => ({
   type: API_DATA_RECEIVED,
   payload: {
       data: data,
   },
});

//InventoryItems = Overview
export const requestInventoryOverviews = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getInventoryOverviews(dispatch, getState());
    }
}

//"search inventory"
function getInventoryOverviews(dispatch: Dispatch, state: AppState): any {
    const dataQueryInProgress = state.data.isLoading;
    if(dataQueryInProgress){
        return;
    }
    
    dispatch(apiDataRequested());
    
    const param: InventoryQueryParameter = {
        groupBy: 'unique', //customas
        text: '',
        colors: [],
        types: [],
        skip: 0,
        take: 100,
        format: null,
        sort: 'price',//state.inventory.searchFilter.sort,
        sortDescending: true,
        //sort: 'name',
        //set: 'eld'
        set: '',
        exclusiveColorFilters: false,
        multiColorOnly: false,
        maxCount: 0,
        minCount: 0,
        type: '',
        //rarity: state.cardSearch.cardSearchFilter.props.rarity,
        rarity: [],
        //rarity: ['c','u']
    }

    api.callTestQuery(param).then((result) => {
        dispatch(apiDataReceived(result));
    });

}