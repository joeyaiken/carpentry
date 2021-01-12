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

    dispatch(appCoreDataRequested()); //todo - rename
    coreApi.getCoreData().then((results) => {//todo - rename
        dispatch(appCoreDataReceived(results));//todo - rename
    });
}

//todo - rename
export const APP_CORE_DATA_REQUESTED = 'CORE_DATA.APP_CORE_DATA_REQUESTED';
export const appCoreDataRequested = (): ReduxAction => ({
    type: APP_CORE_DATA_REQUESTED
});

//todo - rename
export const APP_CORE_DATA_RECEIVED = 'CORE_DATA.APP_CORE_DATA_RECEIVED';
export const appCoreDataReceived = (filters: AppFiltersDto): ReduxAction => ({
    type: APP_CORE_DATA_RECEIVED,
    payload: filters
});

