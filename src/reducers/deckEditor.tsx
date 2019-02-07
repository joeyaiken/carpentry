// import { SELECT_DECK, ADD_DECK, LOG_STATE, SELECTED_DECK_CHANGE } from '../actions'
import Redux, { ReducersMapObject } from 'redux'
import { Stream } from 'stream';
// import React from 'react'

import { 
    CARD_BINDER_VIEW_CHANGE,
    CARD_BINDER_GROUP_CHANGE,
    CARD_BINDER_SORT_CHANGE,
    CARD_BINDER_LAND_COUNT_CHANGE
} from '../actions'

import { loadInitialDeckEditorState } from '../data/lumberyard'

// interface UI {
//     isNavOpen: boolean;
//     isSideSheetOpen: boolean;
//     visibleSideSheet: string;

//     deckView: string;
//     deckGroup: string;
//     deckSort: string;
// }



//export const ui = (state: uiActionsProps, action: ReduxAction): ReducerMapObject<uiActionsProps> => {
export const deckEditor = (state: IDeckEditorState, action: ReduxAction): any => {
    switch(action.type){
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

        case CARD_BINDER_LAND_COUNT_CHANGE:            
            let newBinderState: IDeckEditorState = {
                ...state
            }
            // let activeDeck = newBinderState.deckList[state.selectedDeckId];
            // let manaType: string = action.payload.manaType;
            // activeDeck.basicLands = {
            //     ...activeDeck.basicLands,
            //     [manaType]: action.payload.newValue
            // }
            // newBinderState.deckList[state.selectedDeckId].basicLands = {

            // }
            
            // console.log(activeDeck);
            return newBinderState;
        default:
            if(!state){
                state = loadInitialDeckEditorState();
            }
            return state;
    }
}
