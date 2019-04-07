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

import { 
    SCOUT_SEARCH_FILTER_CHANGE,
    SCOUT_SEARCH_APPLIED
} from '../actions/cardScoutActions'



//import todos from './todos'
//import visibilityFilter from './visibilityFilter'

//const actions = (state = defaultStateData, action: ReduxAction): any => {
export const cardScoutCardSearch = (state: ICardScoutCardSearch, action: ReduxAction): any => {
    switch(action.type){

        case SCOUT_SEARCH_FILTER_CHANGE:
            return {
                ...state,
                filter:{
                    ...state.filter,
                    [action.payload.property]: action.payload.value
                }
                
            };
        case SCOUT_SEARCH_APPLIED:
            return {
                ...state,
            };

        default:
            if(!state){
                // state = Lumberjack.defaultStateInstance_cardSearch();
                state = {
                    filter: {
                        set: "",
                        name: "",
                        type:"",
                        colorIdentity: "",
                    },
                    searchIsInProgress: false
                }
            }
            return state;
    }
}
