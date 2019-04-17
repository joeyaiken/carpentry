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


///ciInitialized

import {
    AP_INITIALIZED
} from '../actions/addPack.actions';

//import todos from './todos'
//import visibilityFilter from './visibilityFilter'

export const addPack = (state: any, action: ReduxAction): any => {
    switch(action.type){
        case AP_INITIALIZED:
            //no initial search filter for now, so lets just load everything?
            // let newState: ICardInventoryState = {
            //     ...state,
            //     groupedCards: Lumberjack.getGroupedCards()
            // }
            return state;
            
        default:
            // if(!state){
            //     // state = carpentryDefaultStates.cardInventory();
            // }
            return({
                
            })
    }
}

