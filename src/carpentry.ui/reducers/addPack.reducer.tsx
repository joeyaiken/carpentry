import { combineReducers, Store, ReducersMapObject, Reducer, Action, AnyAction } from 'redux';
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
    AP_INITIALIZED,
    AP_SET_SELECTED,
    AP_LOAD_SET_STARTED,
    AP_LOAD_SET_COMPLETE,
    AP_CARD_LOADED
} from '../actions/addPack.actions';

import {
    MTG_API_SEARCH_REQUESTED,
    MTG_API_SEARCH_COMPLETED
} from '../actions/mgtApi.actions';

//import todos from './todos'
//import visibilityFilter from './visibilityFilter'

//Reducer<ReducersMapObject<IAddPackState, Action<any>>, AnyAction>
//ReducersMapObject<IAddPackState>

//Reducer<ReducersMapObject<IAddPackState, Action<any>>, AnyAction>
export const addPack = (state: IAddPackState, action: ReduxAction): any => {
    switch(action.type){
        case AP_INITIALIZED:
            //no initial search filter for now, so lets just load everything?
            // let newState: ICardInventoryState = {
            //     ...state,
            //     groupedCards: Lumberjack.getGroupedCards()
            // }
            return state;
        case AP_SET_SELECTED:
            return {
                ...state,
                selectedSetCode: action.payload
            } as IAddPackState;
        case AP_LOAD_SET_STARTED:
            return {
                ...state,
                isLoadingSet: true
            } as IAddPackState;
        case AP_LOAD_SET_COMPLETE:
            //payload is going to be an array of cards
            let cardsFromAction: ICardDictionary | null = action.payload;
            let mappedCards: INamedCardArray[] = [];
            if(state.selectedSetCode){
                if(!cardsFromAction) {
                    cardsFromAction = state.apiCache[state.selectedSetCode];
                } 
                mappedCards = Lumberjack.mapCardDictionaryToGroupedNamedCardArray(cardsFromAction)
            }

            //Need to group / sort that payload, then apply to state

            return {
                ...state,
                isLoadingSet: false,
                groupedCards: mappedCards
                //need an object for the actual cards....
            } as IAddPackState;
        case AP_CARD_LOADED:
            const newCard: ICard = action.payload;
            
            let newCache = {
                [newCard.set]: {},
                ...state.apiCache
            } as ICardIndex;

            newCache[newCard.set][newCard.name] = newCard;

            return {
                ...state,
                apiCache: newCache
            } as IAddPackState;
        default:
            if(!state){
                state = {
                    apiCache: {},
                    isLoadingSet: false,
                    selectedSetCode: null,
                    visibleSetFilters: null,
                    groupedCards: null                     
                } as IAddPackState;
            }
            return state;
    }
}

