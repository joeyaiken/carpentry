import { 
    // INVENTORY_SEARCH_METHOD_CHANGED,
} from '../_actions';

export interface InventoryAppReducerState {
    // searchMethod: InventorySearchMethod;

    // groupBy: InventoryGroupMethod;

    viewMethod: "grid" | "table";

    filters: InventoryFilterProps;


    //groupBy
    // ? search filters??



    // viewMode: InventoryViewMode;

    // //selectedDetailItem

    // //selectedCardName
    // //selectedDetailCardName
    
    // selectedDetailItemName: string | null;


}

// const inventorySearchMethodChanged = (state: InventoryAppReducerState, action: ReduxAction): InventoryAppReducerState => {
//     const newSearchMethod = action.payload;
//     const newState: InventoryAppReducerState = {
//         ...state,
//         searchMethod: newSearchMethod,
//     };
//     return newState;
// }

export const inventoryAppReducer = (state = initialState, action: ReduxAction): InventoryAppReducerState => {
    switch(action.type){

        // case INVENTORY_SEARCH_METHOD_CHANGED:
        //     return inventorySearchMethodChanged(state, action);

        default:
            return(state)
    }
}


function initialCardSearchFilterProps(): InventoryFilterProps {
    return {
        groupBy: "unique",
        sortBy: "price",
        // setId: null,
        // set: '',
        // colorIdentity: [],
        // rarity: [], //['mythic','rare','uncommon','common'], //
        // type: '',
        // exclusiveColorFilters: false,
        // multiColorOnly: false,
        // cardName: '',
        // exclusiveName: false,
        // maxCount: 0,
        // minCount: 0,
        // format: '',
        // text: '',
        // group: '',

    } as InventoryFilterProps;
} 

const initialState: InventoryAppReducerState = {
    viewMethod: "grid",
    filters: initialCardSearchFilterProps(),
    // searchMethod: "mid",
    // groupBy: "unique",
    // viewMode: "grid",
    // selectedDetailItemName: null,
}
