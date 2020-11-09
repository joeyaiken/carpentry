import { 
    INVENTORY_SEARCH_METHOD_CHANGED,
} from '../actions/index.actions';

declare interface InventoryState {
    searchMethod: InventorySearchMethod;
    viewMode: InventoryViewMode;

    //selectedDetailItem

    //selectedCardName
    //selectedDetailCardName
    
    selectedDetailItemName: string | null;


}

const inventorySearchMethodChanged = (state: InventoryState, action: ReduxAction): InventoryState => {
    const newSearchMethod = action.payload;
    const newState: InventoryState = {
        ...state,
        searchMethod: newSearchMethod,
    };
    return newState;
}

export const inventory = (state = initialState, action: ReduxAction): InventoryState => {
    switch(action.type){

        case INVENTORY_SEARCH_METHOD_CHANGED:
            return inventorySearchMethodChanged(state, action);

        default:
            return(state)
    }
}

const initialState: InventoryState = {
    searchMethod: "quantity",
    viewMode: "grid",
    selectedDetailItemName: null,
}
