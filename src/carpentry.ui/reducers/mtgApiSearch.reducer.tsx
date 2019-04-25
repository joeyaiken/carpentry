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
    SEARCH_VALUE_CHANGE,
    SEARCH_FILTER_CHANGE,
    REQUEST_CARD_SEARCH,
    RECEIVE_CARD_SEARCH,
    REQUEST_CARD_DETAIL,
    RECEIVE_CARD_DETAIL,
    SEARCH_CARD_SELECTED,
    ADD_CARD_TO_DECK,
} from '../actions'

import {
    CS_ACTION_APPLIED,
    CS_CARD_SELECTED,
    CS_FILTER_CHANGED,
    CS_INITIALIZED,
    CS_SEARCH_APPLIED
} from '../actions/cardSearch.actions'

import {
    MTG_API_SEARCH_REQUESTED,
    MTG_API_SEARCH_ITEM_RETURNED,
    MTG_API_SEARCH_COMPLETED,
    cardBinderSelectionChange
} from '../actions/mgtApi.actions';




//import todos from './todos'
//import visibilityFilter from './visibilityFilter'


//const actions = (state = defaultStateData, action: ReduxAction): any => {
//export const cardSearch = (state: ICardSearch, action: ReduxAction): any => {
export const mtgApiSearch = (state: IMtgApiSearchState, action: ReduxAction): any => {
    switch(action.type){
        case MTG_API_SEARCH_REQUESTED:
            return {
                ...state,
                searchInProgress: true,
                //search results 
                searchResults: []
            } as IMtgApiSearchState;
        case MTG_API_SEARCH_ITEM_RETURNED:
            // console.log('processing search result');
            // console.log(action.payload)
            const newState: IMtgApiSearchState = {
                ...state,
                searchResults: [...state.searchResults, action.payload]
            }

            // console.log(newState);

            return newState;
        case MTG_API_SEARCH_COMPLETED:
            return {
                ...state,
                searchInProgress: false
            } as IMtgApiSearchState;
        // case SELECTED_DECK_CHANGE:
        //     // console.log('deck changed');
        //     // console.log(action)
        //     //
        //     // return state;
        //     return {
        //         ...state,
        //         deckList: state.deckList.map((deck) => { 
        //             return (deck.id == state.selectedDeckId) ? action.payload : deck
        //         })
        //     };

        // case FIND_MODAL_FILTER_CHANGE:
        //     // console.log('FILTER CHANGED action')
        //     // console.log(action)
        //     return {
        //         ...state,
        //         searchFilter: action.payload
        //     } as AppState;
        // case SEARCH_VALUE_CHANGE:
        //     return {
        //         ...state,
        //         searchFilter: {
        //             ...state.searchFilter,
        //             name: action.payload
        //         }
        //     }

        // case SEARCH_FILTER_CHANGE:
        //     let newFilterState = {
        //         ...state
        //     }
        //     const filterProperty: string = action.payload.property;
        //     const filterValue: string = action.payload.value;
        //     console.log('filter change - '+filterProperty + ' - ' + filterValue)
        //     switch(filterProperty) {
        //         case 'name':
        //             newFilterState.searchFilter.name = filterValue;
        //         break;
        //         case 'set':
        //             newFilterState.searchFilter.setFilterString = filterValue.toUpperCase();
        //         break;
        //         case 'colorIdentity':
        //             newFilterState.searchFilter.colorIdentity = filterValue
        //         break;
        //         case 'lands':
        //             switch(filterValue){
        //                 case 'R':
        //                     newFilterState.searchFilter.includeRed = !newFilterState.searchFilter.includeRed;
        //                     break;
        //                 case 'U':
        //                     newFilterState.searchFilter.includeBlue = !newFilterState.searchFilter.includeBlue;
        //                     break;
        //                 case 'G':
        //                     newFilterState.searchFilter.includeGreen = !newFilterState.searchFilter.includeGreen;
        //                     break;
        //                 case 'W':
        //                     newFilterState.searchFilter.includeWhite = !newFilterState.searchFilter.includeWhite;
        //                     break;
        //                 case 'B':
        //                     newFilterState.searchFilter.includeBlack = !newFilterState.searchFilter.includeBlack
        //                     break;
                        
        //             }
        //         break;
        //     }
        //     return newFilterState;
        // case SEARCH_CARD_SELECTED:
        //     return {
        //         ...state,
        //         searchFilter: {
        //             ...state.searchFilter,
        //             selectedCardId: action.payload.cardId,
        //             selectedCardName: action.payload.cardName
        //         }
        //     }
        // case SEARCH_APPLIED:
        // case REQUEST_CARD_SEARCH:
        //     return {
        //         ...state,
        //         searchIsFetching: true
        //         // searchFilter: {
        //         //     ...state.searchFilter,
        //         //     isFetching: true
        //         // }
        //     }

        // case RECEIVE_CARD_SEARCH:
        //     // console.log('recieve card search thingy');
        //     const payloadCards: Card[] = action.payload;
        //     // let newIndex = {...state.cardIndex}

        //     console.log('search recieve index')
        //     // console.log(newIndex)

        //     // payloadCards.forEach((card) => {
        //     //     if(!newIndex[card.id]){
        //     //         newIndex[card.id] = {
        //     //             cardId: card.id,
        //     //             data: card
        //     //             // card: card
        //     //         };
        //     //     }
        //     // });
        //     // console.log('updated card index');
        //     // console.log(newIndex);
        //     // localStorage.setItem('card-index-cache', JSON.stringify(newIndex));

        //     return {
        //         ...state,
        //         searchFilter: {
        //             ...state.searchFilter,
        //             // isFetching: false,
        //             results: action.payload
        //         },
        //         // cardIndex: newIndex,
        //         searchIsFetching: false

                    
        //             // ...state.cardIndex,
        //             //...selectedDeck, [event.target.name]: event.target.value
        //     }
        // case REQUEST_CARD_DETAIL:
        //     // let newDetailIndex = {...state.cardIndex}
        //     // let requestedId: string = action.payload;
        //     // newDetailIndex[requestedId] = {
        //     //     ...newDetailIndex[requestedId],
        //     //     cardId: requestedId
        //     // }

        //     return {
        //         ...state,
        //         searchIsFetching: true
        //         // cardIndex: newDetailIndex
        //     }

        // case RECEIVE_CARD_DETAIL:
        //     // let revieveDetailIndex = {...state.cardIndex}
        //     let payload: Card = action.payload
        //     // revieveDetailIndex[payload.id] = {
        //     //     ...revieveDetailIndex[payload.id],
        //     //     data: payload
        //     // }
        //     console.log('fetch card recieved')

        //     const { requestedCards } = state


            
        //     return {
        //         ...state,
        //         searchIsFetching: false
        //         // cardIndex: revieveDetailIndex
        //     }

        // case ADD_CARD_TO_DECK:
            
        //     // console.log('attempting to add card to active deck')
        //     // let activeDeck = state.deckList[state.selectedDeckId];

        //     let newCardAddedState: ICardSearch = {
        //         ...state
        //     }
        //     // newCardAddedState.deckList[state.selectedDeckId].cards.push(action.payload)
        //     return newCardAddedState;

        default:
            if(!state){
                // state = Lumberjack.defaultStateInstance_cardSearch();
                state = {
                    searchInProgress: false,
                    searchResults: []
                } as IMtgApiSearchState
            }
            return state;
    }
}