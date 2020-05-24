import { 
    // INVENTORY_SEARCH_METHOD_CHANGED,
} from '../actions/';

export interface InventoryAppReducerState {
    searchMethod: InventorySearchMethod;
    // viewMode: InventoryViewMode;

    // //selectedDetailItem

    // //selectedCardName
    // //selectedDetailCardName
    
    // selectedDetailItemName: string | null;


}

const inventorySearchMethodChanged = (state: InventoryAppReducerState, action: ReduxAction): InventoryAppReducerState => {
    const newSearchMethod = action.payload;
    const newState: InventoryAppReducerState = {
        ...state,
        searchMethod: newSearchMethod,
    };
    return newState;
}

export const inventoryAppReducer = (state = initialState, action: ReduxAction): InventoryAppReducerState => {
    switch(action.type){

        // case INVENTORY_SEARCH_METHOD_CHANGED:
        //     return inventorySearchMethodChanged(state, action);

        default:
            return(state)
    }
}

const initialState: InventoryAppReducerState = {
    searchMethod: "mid",
    // viewMode: "grid",
    // selectedDetailItemName: null,
}
