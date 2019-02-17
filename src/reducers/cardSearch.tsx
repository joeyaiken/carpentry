import { combineReducers, Store } from 'redux';
import { Card } from 'mtgsdk-ts'
//import {} from './data/lumberyard'


import { loadInitialCardSearchState } from '../data/lumberyard'

//reducer file ideas
//ui
//api
//data (read/write from cache)

import { ui } from './ui'
import { deckEditor } from './deckEditor'

import { 
    // SELECT_DECK, 
    // ADD_DECK, 
    // LOG_STATE, 
    // SELECTED_DECK_CHANGE, 
    // FIND_MODAL_FILTER_CHANGE,
    SEARCH_VALUE_CHANGE,
    
    REQUEST_CARD_SEARCH,
    RECEIVE_CARD_SEARCH,

    REQUEST_CARD_DETAIL,
    RECEIVE_CARD_DETAIL,

    // SELECTED_DECK_SAVED,
    SEARCH_CARD_SELECTED,
    ADD_CARD_TO_DECK,
    // ADD_CARD_TO_RARES,

    // CARD_BINDER_LAND_COUNT_CHANGE,

    // ON_SECTION_TOGGLE
} from '../actions'



//import todos from './todos'
//import visibilityFilter from './visibilityFilter'

//const actions = (state = defaultStateData, action: ReduxAction): any => {
export const cardSearch = (state: ICardSearch, action: ReduxAction): any => {
    switch(action.type){

        
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
        case SEARCH_VALUE_CHANGE:
            return {
                ...state,
                searchFilter: {
                    ...state.searchFilter,
                    name: action.payload
                }
            }
        case SEARCH_CARD_SELECTED:
            return {
                ...state,
                searchFilter: {
                    ...state.searchFilter,
                    selectedCardId: action.payload.cardId,
                    selectedCardName: action.payload.cardName
                }
            }
        // case SEARCH_APPLIED:
        case REQUEST_CARD_SEARCH:
            return {
                ...state,
                searchIsFetching: true
                // searchFilter: {
                //     ...state.searchFilter,
                //     isFetching: true
                // }
            }

        case RECEIVE_CARD_SEARCH:
            // console.log('recieve card search thingy');
            const payloadCards: Card[] = action.payload;
            // let newIndex = {...state.cardIndex}

            console.log('search recieve index')
            // console.log(newIndex)

            // payloadCards.forEach((card) => {
            //     if(!newIndex[card.id]){
            //         newIndex[card.id] = {
            //             cardId: card.id,
            //             data: card
            //             // card: card
            //         };
            //     }
            // });
            // console.log('updated card index');
            // console.log(newIndex);
            // localStorage.setItem('card-index-cache', JSON.stringify(newIndex));

            return {
                ...state,
                searchFilter: {
                    ...state.searchFilter,
                    // isFetching: false,
                    results: action.payload
                },
                // cardIndex: newIndex,
                searchIsFetching: false

                    
                    // ...state.cardIndex,
                    //...selectedDeck, [event.target.name]: event.target.value
            }
        case REQUEST_CARD_DETAIL:
            // let newDetailIndex = {...state.cardIndex}
            // let requestedId: string = action.payload;
            // newDetailIndex[requestedId] = {
            //     ...newDetailIndex[requestedId],
            //     cardId: requestedId
            // }

            return {
                ...state,
                searchIsFetching: true
                // cardIndex: newDetailIndex
            }

        case RECEIVE_CARD_DETAIL:
            // let revieveDetailIndex = {...state.cardIndex}
            let payload: Card = action.payload
            // revieveDetailIndex[payload.id] = {
            //     ...revieveDetailIndex[payload.id],
            //     data: payload
            // }
            console.log('fetch card recieved')

            const { requestedCards } = state


            
            return {
                ...state,
                searchIsFetching: false
                // cardIndex: revieveDetailIndex
            }

        case ADD_CARD_TO_DECK:
            
            // console.log('attempting to add card to active deck')
            // let activeDeck = state.deckList[state.selectedDeckId];

            let newCardAddedState: ICardSearch = {
                ...state
            }
            // newCardAddedState.deckList[state.selectedDeckId].cards.push(action.payload)
            return newCardAddedState;

        default:
            if(!state){
                state = loadInitialCardSearchState();
            }
            return state;
    }
}
