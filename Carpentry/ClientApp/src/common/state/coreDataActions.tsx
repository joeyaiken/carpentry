import { Dispatch } from 'redux';
import { coreApi } from '../../api/coreApi';
import { AppState } from '../../configureStore';

export const requestCoreData = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getCoreData(dispatch, getState());
    }
}
function getCoreData(dispatch: Dispatch, state: AppState): any {
    const dataIsLoading = state.core.data.filterDataIsLoading;
    if(dataIsLoading) {
        return;
    }
    dispatch(appFilterOptionsRequested());
    coreApi.getFilterValues().then((results) => {
        dispatch(appFilterOptionsReceived(results));
    });
}

export const APP_FILTER_OPTIONS_REQUESTED = 'APP_FILTER_OPTIONS_REQUESTED';
export const appFilterOptionsRequested = (): ReduxAction => ({
    type: APP_FILTER_OPTIONS_REQUESTED
});

export const APP_FILTER_OPTIONS_RECEIVED = 'APP_FILTER_OPTIONS_RECEIVED';
export const appFilterOptionsReceived = (filters: AppFiltersDto): ReduxAction => ({
    type: APP_FILTER_OPTIONS_RECEIVED,
    payload: filters
});

