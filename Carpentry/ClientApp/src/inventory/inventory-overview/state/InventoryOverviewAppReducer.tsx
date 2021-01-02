//app reducer for the inventory overview container

import { CARD_MENU_BUTTON_CLICKED, INVENTORY_OVERVIEW_FILTER_CHANGED, QUICK_FILTER_APPLIED } from "./InventoryOverviewActions";

// import { INVENTORY_OVERVIEW_FILTER_CHANGED } from "./inventoryOverviewActions";

export interface State {
    // searchMethod: InventorySearchMethod;

    // groupBy: InventoryGroupMethod;

    viewMethod: "grid" | "table";

    filters: InventoryFilterProps;

    cardImageMenuAnchor: HTMLButtonElement | null;

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

/*
export const QUICK_FILTER_APPLIED = 'QUICK_FILTER_APPLIED';
export const quickFilterApplied = (filter: InventoryFilterProps): ReduxAction => ({
    type: QUICK_FILTER_APPLIED,
    payload: filter,
});
*/

export const inventoryOverviewAppReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case INVENTORY_OVERVIEW_FILTER_CHANGED: return inventoryOverviewFilterChanged(state, action);
        case CARD_MENU_BUTTON_CLICKED: return { ...state, cardImageMenuAnchor: action.payload };
        case QUICK_FILTER_APPLIED: return { ...state, filters: action.payload }
        default: return(state);
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
    cardImageMenuAnchor: null,
    // searchMethod: "mid",
    // groupBy: "unique",
    // viewMode: "grid",
    // selectedDetailItemName: null,
}
