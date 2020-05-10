/// <reference types="react-scripts" />
/// <reference types="redux" />
/// <reference types="react-redux" />
/// <reference types="redux-thunk" />

declare type ApiScopeOption = 
    'coreFilterOptions' | 
    "deckOverviews" | 
    "deckDetail" | 
    "inventoryOverview" | 
    "inventoryDetail" | 
    "cardSearchResults" | 
    "cardSearchInventoryDetail" ;


declare interface ReduxAction extends AnyAction {
    type: any, //Should be combined type of all fuckin action types?
    error?: any,
    payload?: any,
    meta?: any
}