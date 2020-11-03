import { Dispatch } from 'redux';
import { AppState } from '../../../_reducers';

export const ensureInventoryDetailLoaded = (cardId: number): any => {
    return(dispatch: Dispatch, getState: any) => {
        tryLoadInventoryDetail(dispatch, getState(), cardId);
    }
}

function tryLoadInventoryDetail(dispatch: Dispatch, state: AppState, cardId: number){
    const _localApiScope: ApiScopeOption = "inventoryDetail";

    //Has a load been requested?
    //  if yes, return
    //So this could be two variables: queryIsLoaded, loadedId
    //  OR...wait still 2 variables: queryIsLoading, loadedId
    //  Okay, so I guess I won't change that approach

    // if(queryInProgress){
    //     return;
    // }


    // //Dispatch load requested
    // dispatch(apiDataRequested(_localApiScope));

    // //attempt load
    // //  dispatch load recieved
    // api_Inventory_GetByName(name).then((result) => {
    //     dispatch(apiDataReceived(_localApiScope, result));

    //      if error, set empty/null, but don't set loaded id
    //      if success, but no match, set null AND loaded ID
    
    // });



    //Too tired for this for now...




    //This will try to load the provided ID (not null)
    //On failed load, will just set things to null/unknown/whatever

    //To think on...do I want the old data-reducers now?

    //Set some flag / state variable to indicate that this ID has, in fact, been loaded
    //  maybe an app-level one, not data-level?

}