// import { SELECT_DECK, ADD_DECK, LOG_STATE, SELECTED_DECK_CHANGE } from '../actions'
import Redux, { ReducersMapObject } from 'redux'
import { Stream } from 'stream';
// import React from 'react'

import { loadInitialDataStore } from '../data/lumberyard'

import { 
    DECK_EDITOR_DUPLICATE_SELECTED_CARD,
    DECK_EDITOR_REMOVE_ONE_SELECTED_CARD,
    DECK_EDITOR_REMOVE_ALL_SELECTED_CARD,
    DECK_EDITOR_CARD_SELECTED,
    CARD_BINDER_LAND_COUNT_CHANGE
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
    let newDataStoreState: IDataStore = {
        ...state
    }
    //need to add the current card name to the deck of the current active deck
    //a lot of things need that deck so here:
    // const activeDeck = state.deckList[state.selectedDeckId];

    switch(action.type){    
        case DECK_EDITOR_DUPLICATE_SELECTED_CARD:
            if(state.selectedCard)
                newDataStoreState.cardLists[state.selectedDeckId].cards.push(state.selectedCard);
            return newDataStoreState;
        case DECK_EDITOR_REMOVE_ONE_SELECTED_CARD:
            if(state.selectedCard){
                //index of the first card?
                var firstSelectedCardIndex = newDataStoreState.cardLists[state.selectedDeckId].cards.indexOf(state.selectedCard);

                newDataStoreState.cardLists[state.selectedDeckId].cards.splice(firstSelectedCardIndex,1);
                // newDataStoreState.cardLists[state.selectedDeckId].cards.push(state.selectedCard);
            }
            return newDataStoreState;
        case DECK_EDITOR_REMOVE_ALL_SELECTED_CARD:
            if(state.selectedCard){
                newDataStoreState.cardLists[state.selectedDeckId].cards = newDataStoreState.cardLists[state.selectedDeckId].cards.filter((card) => {
                    return (card != state.selectedCard);
                })
                // newDataStoreState.cardLists[state.selectedDeckId].cards.push(state.selectedCard);
            }
            return newDataStoreState;
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
        case DECK_EDITOR_CARD_SELECTED:
            newDataStoreState.selectedCard = action.payload;
            return newDataStoreState;


        case CARD_BINDER_LAND_COUNT_CHANGE:
            // let newBinderState: IDeckEditorState = {
            //     ...state
            // }
            // let activeDeck = newDataStoreState.deckList[state.selectedDeckId];
            let activeDeck = newDataStoreState.detailList[state.selectedDeckId];
            let manaType: string = action.payload.manaType;
            activeDeck.basicLands = {
                ...activeDeck.basicLands,
                [manaType]: action.payload.newValue
            }
            // newBinderState.deckList[state.selectedDeckId].basicLands = {

            // }
            
            // console.log(activeDeck);
            // return newBinderState;
        
            return newDataStoreState;
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
