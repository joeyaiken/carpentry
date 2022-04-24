import { Dispatch } from "redux";
import { coreApi } from "../../../api/coreApi";
import { AppState } from "../../../configureStore";

// export const TRACKED_SETS_DATA_REQUESTED = 'TRACKED_SETS_DATA_REQUESTED';
// export const trackedSetsDataRequested = (showUntracked: boolean, update: boolean): ReduxAction => ({
//     type: TRACKED_SETS_DATA_REQUESTED,
//     payload:{
//         showUntracked: showUntracked, 
//         update: update
//     }
// });

// export const TRACKED_SETS_DATA_RECEIVED = 'TRACKED_SETS_DATA_RECEIVED';
// export const trackedSetsDataReceived = (dto: SetDetailDto | null): ReduxAction => ({
//     type: TRACKED_SETS_DATA_RECEIVED,
//     payload: dto,
// });

export const requestTrackedSets = (showUntracked: boolean, update: boolean): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getTrackedSets(dispatch, getState(), showUntracked, update);
    }
}
function getTrackedSets(dispatch: Dispatch, state: AppState, showUntracked: boolean, update: boolean): any {
    const dataIsLoading = state.settings.trackedSets.isLoading;//state.data.trackedSets.isLoading;
    if(dataIsLoading){
        return;
    }
    dispatch(trackedSetsDataRequested(showUntracked, update));
    coreApi.getTrackedSets(showUntracked, update).then((results) => {
        dispatch(trackedSetsDataReceived(results));
    });
}

export const requestAddTrackedSet = (setId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        return addTrackedSet(dispatch, getState(), setId);
    }
}
function addTrackedSet(dispatch: Dispatch, state: AppState, setId: number): any {
    const dataIsLoading = state.settings.trackedSets.isLoading;//state.data.trackedSets.isLoading;
    if(dataIsLoading){
        return;
    }

    const showUntrackedVal = state.settings.trackedSets.showUntracked;
    dispatch(trackedSetsDataRequested(showUntrackedVal,  false));
    coreApi.addTrackedSet(setId).then(() => {
        dispatch(trackedSetsDataReceived(null));
        dispatch(requestTrackedSets(showUntrackedVal, false));
    });
}

export const requestUpdateTrackedSet = (setId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        return updateTrackedSet(dispatch, getState(), setId);
    }
}
function updateTrackedSet(dispatch: Dispatch, state: AppState, setId: number): any {
    const dataIsLoading = state.settings.trackedSets.isLoading;//state.data.trackedSets.isLoading;
    if(dataIsLoading){
        return;
    }
    // console.log('updating tracked sets ping 1');
    
    const showUntrackedVal = state.settings.trackedSets.showUntracked//state.data.trackedSets.showUntracked;
    dispatch(trackedSetsDataRequested(showUntrackedVal, false));
    coreApi.updateTrackedSet(setId).then(() => {
        // console.log('updating tracked sets ping 2');
        dispatch(trackedSetsDataReceived(null));
        // console.log('updating tracked sets ping 3');
        dispatch(requestTrackedSets(showUntrackedVal, false));
    });
}

export const requestRemoveTrackedSet = (setId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        return removeTrackedSet(dispatch, getState(), setId);
    }
}
function removeTrackedSet(dispatch: Dispatch, state: AppState, setId: number): any {
    const dataIsLoading = state.settings.trackedSets.isLoading;
    if(dataIsLoading){
        return;
    }
    
    const showUntrackedVal = state.settings.trackedSets.showUntracked;
    dispatch(trackedSetsDataRequested(showUntrackedVal, false));
    coreApi.removeTrackedSet(setId).then(() => {
        dispatch(trackedSetsDataReceived(null));
        dispatch(requestTrackedSets(showUntrackedVal, false));
    });
}