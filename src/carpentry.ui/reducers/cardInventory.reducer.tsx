import { combineReducers, Store } from 'redux';
import { Card } from 'mtgsdk-ts'
//import {} from './data/lumberyard'


// import { loadInitialCardSearchState } from '../../carpentry.data/lumberyard'
import { Lumberjack } from '../../carpentry.logic/lumberjack'
//reducer file ideas
//ui
//api
//data (read/write from cache)

import { ui } from './ui'
import { deckEditor } from './deckEditor'

import { carpentryDefaultStates } from '../../carpentry.logic/carpentryDefaultStates'

import { 
    // SEARCH_VALUE_CHANGE,
    // SEARCH_FILTER_CHANGE,
    // REQUEST_CARD_SEARCH,
    // RECEIVE_CARD_SEARCH,
    // REQUEST_CARD_DETAIL,
    // RECEIVE_CARD_DETAIL,
    // SEARCH_CARD_SELECTED,
    // ADD_CARD_TO_DECK,
} from '../actions'

///ciInitialized

import {
    // CS_ACTION_APPLIED,
    // CS_CARD_SELECTED,
    // CS_FILTER_CHANGED,
    // CS_INITIALIZED,
    // CS_SEARCH_APPLIED
    CI_INITIALIZED
} from '../actions/cardInventory.actions'

import {
    AP_SAVE_TO_INVENTORY
} from '../actions/addPack.actions'


//import todos from './todos'
//import visibilityFilter from './visibilityFilter'

export const cardInventory = (state: ICardInventoryState, action: ReduxAction): any => {
    switch(action.type){
        case CI_INITIALIZED:
            //no initial search filter for now, so lets just load everything?
            let newState: ICardInventoryState = {
                ...state,
                groupedCards: Lumberjack.getAllOwnedCardsBySet()
            }
            return newState;
        case AP_SAVE_TO_INVENTORY:
            const cards: IntDictionary = action.payload.cards;
            const setCode: string = action.payload.setCode;
            // console.log('adding cards to inventory');
            // console.log(cards);
            // console.log('existing inventory');
            // console.log(state.groupedCards);
            return {
                ...state,
                groupedCards: Lumberjack.addCardsToInventory(setCode,cards)
            }
        default:
            if(!state){
                state = carpentryDefaultStates.cardInventory();
            }
            return(state)
    }
}

