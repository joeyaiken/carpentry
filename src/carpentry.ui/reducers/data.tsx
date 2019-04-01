// import { SELECT_DECK, ADD_DECK, LOG_STATE, SELECTED_DECK_CHANGE } from '../actions'
import Redux, { ReducersMapObject } from 'redux'
import { Stream } from 'stream';
// import React from 'react'

//import { cacheDeckData, saveCardIndexCache, loadInitialDataStore } from '../../carpentry.data/lumberyard'

import { 
    INI_APP_DATA,

    DECK_EDITOR_DUPLICATE_SELECTED_CARD,
    DECK_EDITOR_REMOVE_ONE_SELECTED_CARD,
    DECK_EDITOR_REMOVE_ALL_SELECTED_CARD,
    DECK_EDITOR_CARD_SELECTED,
    CARD_BINDER_LAND_COUNT_CHANGE,
    SELECT_DECK,
    ADD_CARD_TO_DECK,
    ADD_CARD_TO_RARES,
    ADD_CARD_TO_INDEX,
    TOGGLE_DECK_EDITOR_STATUS
} from '../actions'

import { Lumberjack } from '../../carpentry.logic/lumberjack'

// interface UI {
//     isNavOpen: boolean;
//     isSideSheetOpen: boolean;
//     visibleSideSheet: string;

//     deckView: string;
//     deckGroup: string;
//     deckSort: string;
// }


//Data reducer should be the only one that has an instance of the Lumberyard



//export const ui = (state: uiActionsProps, action: ReduxAction): ReducerMapObject<uiActionsProps> => {
export const data = (state: IDataStore, action: ReduxAction): any => {
    // console.log('evaluating data state');
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
            if(state.selectedCard && state.selectedDeckId){
                // const cardToAdd = state.cardIndex[state.selectedCard.set][state.selectedCard.name];
                newDataStoreState.deckList[state.selectedDeckId].cards.push(state.selectedCard);
                Lumberjack.legacy_cacheDeckData(newDataStoreState.deckList);
            }
            return newDataStoreState;
        case DECK_EDITOR_REMOVE_ONE_SELECTED_CARD:
            if(state.selectedCard && state.selectedDeckId){
                //index of the first card?
                var firstSelectedCardIndex = newDataStoreState.deckList[state.selectedDeckId].cards.indexOf(state.selectedCard);

                newDataStoreState.deckList[state.selectedDeckId].cards.splice(firstSelectedCardIndex,1);
                // newDataStoreState.cardLists[state.selectedDeckId].cards.push(state.selectedCard);
            }
            Lumberjack.legacy_cacheDeckData(newDataStoreState.deckList);
            return newDataStoreState;
        case DECK_EDITOR_REMOVE_ALL_SELECTED_CARD:
            if(state.selectedCard && state.selectedDeckId){
                newDataStoreState.deckList[state.selectedDeckId].cards = newDataStoreState.deckList[state.selectedDeckId].cards.filter((card) => {
                    return (card != state.selectedCard);
                })
                // newDataStoreState.cardLists[state.selectedDeckId].cards.push(state.selectedCard);
            }
            Lumberjack.legacy_cacheDeckData(newDataStoreState.deckList);
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
            if(state.selectedDeckId){
                let activeDeckDetail = newDataStoreState.deckList[state.selectedDeckId];
                let manaType: string = action.payload.manaType;
                activeDeckDetail.basicLands = {
                    ...activeDeckDetail.basicLands,
                    [manaType]: action.payload.newValue
                }
            }
            // newBinderState.deckList[state.selectedDeckId].basicLands = {

            // }
            
            // console.log(activeDeck);
            // return newBinderState;

            //cache deck details
            Lumberjack.legacy_cacheDeckData(newDataStoreState.deckList);
            return newDataStoreState;
        case ADD_CARD_TO_DECK:
            // console.log('trying to add a card to this deck')
            // console.log(action.payload);
            if(state.selectedDeckId){
                let activeDeckCards = newDataStoreState.deckList[state.selectedDeckId];
                activeDeckCards.cards.push(action.payload);
            }
            Lumberjack.legacy_cacheDeckData(newDataStoreState.deckList);
            return newDataStoreState;
        case ADD_CARD_TO_INDEX:
            let cardToIndex: ICard = action.payload;
            if(!newDataStoreState.cardIndex[cardToIndex.set]){
                newDataStoreState.cardIndex[cardToIndex.set] = {}
            }
            //is there a problem with updating the index instead of not ovewriting things?
            newDataStoreState.cardIndex[cardToIndex.set][cardToIndex.name] = cardToIndex;
            Lumberjack.legacy_saveCardIndexCache(newDataStoreState.cardIndex);
            return newDataStoreState;
        case TOGGLE_DECK_EDITOR_STATUS:
            // console.log('status toggle?')
            if(state.selectedDeckId){
                let activedeck = newDataStoreState.deckList[state.selectedDeckId];
                activedeck.details.isUpToDate = !activedeck.details.isUpToDate;
            }
            Lumberjack.legacy_cacheDeckData(newDataStoreState.deckList);
            return newDataStoreState;

        case INI_APP_DATA:
            console.log('initializing stuff from data reducer')
            newDataStoreState = Lumberjack.legacy_loadInitialDataStore();
            return newDataStoreState;
        default:
            if(!state){
                state = Lumberjack.defaultStateInstance_dataStore();
            }
            return state;
    }
}