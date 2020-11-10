//app reducer for the inventory overview container

import { INVENTORY_OVERVIEW_FILTER_CHANGED } from "./inventoryOverviewActions";

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
        case INVENTORY_OVERVIEW_FILTER_CHANGED: return inventoryOverviewFilterChanged(state, action);
        default: return(state)
    }
}

const inventoryOverviewFilterChanged = (state: State, action: ReduxAction): State => {
    const { filter, value } = action.payload;
    const newState: State = {
        ...state,
        filters: {
            ...state.filters,
            [filter]: value,
        }
    }
    return newState;
}

function initialCardSearchFilterProps(): InventoryFilterProps {
    return {
        groupBy: "unique",
        sortBy: "price",
        set: "",
        text: "",
        exclusiveColorFilters: false,
        multiColorOnly: false,
        skip: 0,
        take: 100,
        type: "",
        colorIdentity: [],
        rarity: [],
        minCount: 0,
        maxCount: 0,
        sortDescending: true,
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
