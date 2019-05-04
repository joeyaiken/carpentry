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
    AP_CARD_LOADED,
    AP_CLEAR_SELECTED_SET,
    AP_ADD_CARD,
    AP_REMOVE_CARD,
    AP_SAVE_TO_INVENTORY

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
const cleanState = (oldState: IAddPackState): IAddPackState => {
    return {
        ...oldState
    }
}

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
                somethingWrong: "blah",
                selectedSetCode: action.payload
            } as IAddPackState;
        case AP_CLEAR_SELECTED_SET:

            return {
                ...state,
                selectedSetCode: null,
                setSections: null,
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

            //New plan: call something like

            // const setName: string = state.selectedSetCode || "";
            //let groups = Lumberjack.mapSetToAddPackGroups(cardsFromAction);


            //I guess we needs the cards somehow


            if(state.selectedSetCode){
                if(!cardsFromAction) {
                    cardsFromAction = state.apiCache[state.selectedSetCode];
                } 
                //mappedCards = Lumberjack.mapCardDictionaryToGroupedNamedCardArray(cardsFromAction)
                mappedCards = Lumberjack.mapSetToAddPackGroups(cardsFromAction)
            }

            //Need to group / sort that payload, then apply to state

            return {
                ...state,
                isLoadingSet: false,
                // groupedCards: groups
                setSections: mappedCards
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

        case AP_ADD_CARD:
            let cardToAdd: string = action.payload.name;
            // let isFoil: boolean | undefined = action.payload.isFoil;
            return {
                ...state,
                pendingCards: Lumberjack.addCardToPending(cardToAdd, state.pendingCards, action.payload.isFoil)
            } as IAddPackState;
        case AP_REMOVE_CARD:
            let cardToRemove: string = action.payload.name;
            // let isFoil: boolean | undefined = action.payload.isFoil;
            return {
                ...state,
                pendingCards: Lumberjack.removeCardFromPending(cardToRemove, state.pendingCards, action.payload.isFoil)
            } as IAddPackState;
        case AP_SAVE_TO_INVENTORY:
            return {
                ...state,
                pendingCards: {},
                selectedSetCode: null
            } as IAddPackState;
        default:
            if(!state){
                state = {
                    apiCache: {},
                    isLoadingSet: false,
                    selectedSetCode: "WAR",
                    visibleSetFilters: null,
                    setSections: null,
                    pendingCards: {}
                } as IAddPackState;
            }
            return state;
    }
}

