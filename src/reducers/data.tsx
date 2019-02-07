// import { SELECT_DECK, ADD_DECK, LOG_STATE, SELECTED_DECK_CHANGE } from '../actions'
import Redux, { ReducersMapObject } from 'redux'
import { Stream } from 'stream';
// import React from 'react'

import { loadInitialDataStore } from '../data/lumberyard'

import { 
    CARD_BINDER_VIEW_CHANGE,
    CARD_BINDER_GROUP_CHANGE,
    CARD_BINDER_SORT_CHANGE
} from '../actions'



// interface UI {
//     isNavOpen: boolean;
//     isSideSheetOpen: boolean;
//     visibleSideSheet: string;

//     deckView: string;
//     deckGroup: string;
//     deckSort: string;
// }



//export const ui = (state: uiActionsProps, action: ReduxAction): ReducerMapObject<uiActionsProps> => {
export const data = (state: IDataStore, action: ReduxAction): any => {
    switch(action.type){
        // case CARD_BINDER_VIEW_CHANGE:
        //     return {
        //         ...state,
        //         deckView: action.payload
        //     }
        // case CARD_BINDER_GROUP_CHANGE:
        //     return {
        //         ...state,
        //         deckGroup: action.payload
        //     }
        // case CARD_BINDER_SORT_CHANGE:
        //     return {
        //         ...state,
        //         deckSort: action.payload
        //     }
        default:
            if(!state){
                state = loadInitialDataStore();
                // state = {
                //     deckView: 'card',
                //     deckGroup: 'none',
                //     deckSort: 'name',
                //     deckFilter: ''
                // }
            }
            return state;
    }
}
