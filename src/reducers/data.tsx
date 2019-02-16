// import { SELECT_DECK, ADD_DECK, LOG_STATE, SELECTED_DECK_CHANGE } from '../actions'
import Redux, { ReducersMapObject } from 'redux'
import { Stream } from 'stream';
// import React from 'react'

import { Lumberyard } from '../data/lumberyard'

import { 
    DECK_EDITOR_DUPLICATE_SELECTED_CARD,
    DECK_EDITOR_REMOVE_ONE_SELECTED_CARD,
    DECK_EDITOR_REMOVE_ALL_SELECTED_CARD,
    DECK_EDITOR_CARD_SELECTED,
    CARD_BINDER_LAND_COUNT_CHANGE,
    SELECT_DECK,
    ADD_CARD_TO_DECK,
    ADD_CARD_TO_RARES,
    ADD_CARD_TO_INDEX
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
   
    // let deckId = state.
    // let activeDeckCards = newDataStoreState.cardLists[state.selectedDeckId];
    // let activeDeckDetail = newDataStoreState.detailList[state.selectedDeckId];
    
    // if(state.selectedDeckId){
    //     let activeDeckCards = newDataStoreState.cardLists[state.selectedDeckId];
    //     let activeDeckDetail = newDataStoreState.detailList[state.selectedDeckId];
    // }

    //need to add the current card name to the deck of the current active deck
    //a lot of things need that deck so here:
    // const activeDeck = state.deckList[state.selectedDeckId];

    switch(action.type){
        case SELECT_DECK:
            // let newState: IUIState = {
            //     ...state,
            //     selectedDeckId: action.payload,
            //     isNavOpen: false
            // }
            newDataStoreState.selectedDeckId = action.payload

            // cacheUIState(newState);
            //also should cache the UI when the selected deck changes

            // return Object.assign({},state,{
            //     selectedDeckId: action.payload
            // })
            return newDataStoreState;
        case DECK_EDITOR_DUPLICATE_SELECTED_CARD:
            if(state.selectedCard)
                newDataStoreState.cardLists[state.selectedDeckId].cards.push(state.selectedCard);
                cacheDeckCards(newDataStoreState.cardLists);
            return newDataStoreState;
        case DECK_EDITOR_REMOVE_ONE_SELECTED_CARD:
            if(state.selectedCard){
                //index of the first card?
                var firstSelectedCardIndex = newDataStoreState.cardLists[state.selectedDeckId].cards.indexOf(state.selectedCard);

                newDataStoreState.cardLists[state.selectedDeckId].cards.splice(firstSelectedCardIndex,1);
                // newDataStoreState.cardLists[state.selectedDeckId].cards.push(state.selectedCard);
            }
            cacheDeckCards(newDataStoreState.cardLists);
            return newDataStoreState;
        case DECK_EDITOR_REMOVE_ALL_SELECTED_CARD:
            if(state.selectedCard){
                newDataStoreState.cardLists[state.selectedDeckId].cards = newDataStoreState.cardLists[state.selectedDeckId].cards.filter((card) => {
                    return (card != state.selectedCard);
                })
                // newDataStoreState.cardLists[state.selectedDeckId].cards.push(state.selectedCard);
            }
            cacheDeckCards(newDataStoreState.cardLists);
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
            // console.log('trying to update land counts')
            
            // console.log('active deck')
            // console.log(activeDeck)
            let activeDeckDetail = newDataStoreState.detailList[state.selectedDeckId];
            let manaType: string = action.payload.manaType;
            activeDeckDetail.basicLands = {
                ...activeDeckDetail.basicLands,
                [manaType]: action.payload.newValue
            }
            // newBinderState.deckList[state.selectedDeckId].basicLands = {

            // }
            
            // console.log(activeDeck);
            // return newBinderState;

            //cache deck details
            cacheDeckDetails(newDataStoreState.detailList);
            return newDataStoreState;
        case ADD_CARD_TO_DECK:
            // console.log('trying to add a card to this deck')
            // console.log(action.payload);

            let activeDeckCards = newDataStoreState.cardLists[state.selectedDeckId];
            activeDeckCards.cards.push(action.payload);
            cacheDeckCards(newDataStoreState.cardLists);
            return newDataStoreState;
        case ADD_CARD_TO_INDEX:
            let cardToIndex: ICard = action.payload;
            //is there a problem with updating the index instead of not ovewriting things?
            newDataStoreState.cardIndex[cardToIndex.name] = cardToIndex;
            saveCardIndexCache(newDataStoreState.cardIndex);
            return newDataStoreState
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
