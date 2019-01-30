// import { SELECT_DECK, ADD_DECK, LOG_STATE, SELECTED_DECK_CHANGE } from '../actions'
import Redux from 'redux'
import { Stream } from 'stream';
// import React from 'react'

import { 
    APP_NAV_CLICK,
    CARD_BINDER_SHEET_TOGGLE,
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
export const ui = (state: UIState, action: ReduxAction): any => {
    switch(action.type){
        case APP_NAV_CLICK:
            return {
                ...state,
                isNavOpen: !state.isNavOpen
            }
        case CARD_BINDER_SHEET_TOGGLE: 
            ///isSearchOpen: boolean;
            // isRareBinderOpen: boolean;
            // isDetailOpen: boolean;
            let sheetisOpen = true
            let visibleSideSheet = action.payload
            if(state.visibleSideSheet == action.payload){
                sheetisOpen = false;
                visibleSideSheet = '';
            }
            return {
                ...state,
                isSideSheetOpen: sheetisOpen,
                visibleSideSheet: visibleSideSheet
                // isSearchOpen: false,
                // isRareBinderOpen: false,
                // isDetailOpen: false,
                // [action.payload]: true
            }
        case CARD_BINDER_VIEW_CHANGE:
            return {
                ...state,
                deckView: action.payload
            }
        case CARD_BINDER_GROUP_CHANGE:
            return {
                ...state,
                deckGroup: action.payload
            }
        case CARD_BINDER_SORT_CHANGE:
            return {
                ...state,
                deckSort: action.payload
            }
        // case OPEN_FIND_MODAL:
        //     return {
        //         isFindModalVisible: true
        //     } as uiActionsProps;
        // case CLOSE_FIND_MODAL:
        //     return {
        //         isFindModalVisible: false
        //     } as uiActionsProps;
        default:
            console.log('ui state default reducer');
            console.log(state);
            if(!state){
                state = {
                    // isFindModalVisible: true
                    isNavOpen: false,
                    isSideSheetOpen: false,
                    visibleSideSheet: '',

                    deckView: 'card',
                    deckGroup: 'none',
                    deckSort: 'name',
                    deckFilter: ''
                }// a
            }
            return state;
            // return {
            //     // isFindModalVisible: true
            //     something: true
            // }// as uiActionsProps;
    }
}
