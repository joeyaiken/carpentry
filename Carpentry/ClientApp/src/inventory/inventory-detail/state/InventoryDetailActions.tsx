import { Dispatch } from 'redux';
import { inventoryApi } from '../../../api/inventoryApi';
import { AppState } from '../../../configureStore';
import { inventoryDetailRequested, inventoryDetailReceived } from '../../state/InventoryDataActions';

export const ensureInventoryDetailLoaded = (cardId: number): any => {
    return(dispatch: Dispatch, getState: any) => {
        tryLoadInventoryDetail(dispatch, getState(), cardId);
    }
}

function tryLoadInventoryDetail(dispatch: Dispatch, state: AppState, cardId: number){
    //Has a load been requested?
    //  if yes, return
    //So this could be two variables: queryIsLoaded, loadedId
    //  OR...wait still 2 variables: queryIsLoading, loadedId
    //  Okay, so I guess I won't change that approach

    // if(queryInProgress){
    //     return;
    // }
    const selectedCardId = state.inventory.data.detail.selectedCardId;
    const queryIsLoading = state.inventory.data.detail.isLoading;

    if(queryIsLoading || cardId === selectedCardId){
        return;
    }

    //So, should I dispatch this data action, or add a local action somewhere?
    dispatch(inventoryDetailRequested(cardId));

    inventoryApi.getInventoryDetail(cardId).then((result) => {
        dispatch(inventoryDetailReceived(result));
    }).catch((error) => {
        dispatch(inventoryDetailReceived(null));
    });

    


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