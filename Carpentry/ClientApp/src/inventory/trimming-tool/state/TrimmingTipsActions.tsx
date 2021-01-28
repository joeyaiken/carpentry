import { Dispatch } from 'redux'
import { inventoryApi } from '../../../api/inventoryApi';
import { AppState } from '../../../configureStore';


// import { api } from './api';

// export const ENSURE_TRIMMING_TIPS_LOADED = 'ENSURE_TRIMMING_TIPS_LOADED';
// export const ensureTrimmingTipsLoaded = (): ReduxAction => ({
//     type: ENSURE_TRIMMING_TIPS_LOADED
// });
export const ensureTrimmingTipsLoaded = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryLoadTrimmingTips(dispatch, getState());
    }
}


function tryLoadTrimmingTips(dispatch: Dispatch, state: AppState): any {
    // let isInitialized = true;

    // if(isInitialized){
    //     return;
    // }

    loadTrimmingTips(dispatch, state);

    //TODO - add "isLoading" block
    // api.inventory.exportInventoryBackup().then((blob) => {
    //     const exportFilename = "CarpentryBackup.zip"


    // });

}

function loadTrimmingTips(dispatch: Dispatch, state: AppState): any {
    let isLoading = false;

    if(isLoading){
        return;
    }

    inventoryApi.getTrimmingTips().then((result) => {

    });

    //dispatch isRequesting


    //api call =>

    //dispatch loaded

    

    //TODO - add "isLoading" block
    // api.inventory.exportInventoryBackup().then((blob) => {
    //     const exportFilename = "CarpentryBackup.zip"


    // });

}
