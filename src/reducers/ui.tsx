// import { SELECT_DECK, ADD_DECK, LOG_STATE, SELECTED_DECK_CHANGE } from '../actions'
import Redux, { ReducersMapObject } from 'redux'
import { Stream } from 'stream';
// import React from 'react'

import { UIStateManager } from '../data/lumberyard'

import { 
    APP_SHEET_TOGGLE, 
    APP_NAV_CLICK,
    // SELECTED_DECK_CHANGE,

    SELECT_DECK,
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
export const ui = (state: IUIState, action: ReduxAction): any => {
    switch(action.type){
        case APP_NAV_CLICK:
            return {
                ...state,
                isNavOpen: !state.isNavOpen
            }
        case APP_SHEET_TOGGLE: 
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
        case SELECT_DECK:
            let newState: IUIState = {
                ...state,
                selectedDeckId: action.payload,
                isNavOpen: false
            }
            
            UIStateManager.cacheUIState(newState);
            //also should cache the UI when the selected deck changes

            // return Object.assign({},state,{
            //     selectedDeckId: action.payload
            // })
            return newState;
        
        // case SELECTED_DECK_CHANGE:
        //     // console.log('deck changed');
        //     // console.log(action)
        //     //
        //     // return state;
        //     return {
        //         ...state,
        //         // deckList: state.deckList.map((deck) => { 
        //         //     return (deck.id == state.selectedDeckId) ? action.payload : deck
        //         // })
        //     };

        default:
            // console.log('ui state default reducer');
            // console.log(state);
            if(!state){
                state = UIStateManager.loadInitialUIState();
            }
            return state;
    }
}
