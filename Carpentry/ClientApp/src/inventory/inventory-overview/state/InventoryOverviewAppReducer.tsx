//app reducer for the inventory overview container

////

// import { 
//     // INVENTORY_SEARCH_METHOD_CHANGED,
// } from '../_actions';

export interface State {
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

// const inventorySearchMethodChanged = (state: State, action: ReduxAction): State => {
//     const newSearchMethod = action.payload;
//     const newState: State = {
//         ...state,
//         searchMethod: newSearchMethod,
//     };
//     return newState;
// }

export const inventoryOverviewAppReducer = (state = initialState, action: ReduxAction): State => {
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

const initialState: State = {
    viewMethod: "grid",
    filters: initialCardSearchFilterProps(),
    // searchMethod: "mid",
    // groupBy: "unique",
    // viewMode: "grid",
    // selectedDetailItemName: null,
}
