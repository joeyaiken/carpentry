import React from 'react';
import Magic, { CardFilter, Cards, Card } from 'mtgsdk-ts';
// import { Card, Set, CardFilter, SetFilter, PaginationFilter } from "./IMagic";

import Redux, { Store, Dispatch } from 'redux'
import { stat } from 'fs';
import { unmountComponentAtNode } from 'react-dom';


//Actions

//Navigation actions
export const APP_NAV_CLICK = 'APP_NAV_CLICK';
export const APP_NAV_DECK_SELECT = 'APP_NAV_DECK_SELECT'
export const APP_NAV_DECK_ADD = 'APP_NAV_DECK_ADD'


export const ADD_DECK = 'ADD_DECK'
export const SELECT_DECK = 'SELECT_DECK'
export const LOG_STATE = 'LOG_STATE'
export const SELECTED_DECK_CHANGE = 'SELECTED_DECK_CHANGE'
export const SELECTED_DECK_SAVED = 'SELECTED_DECK_SAVED'


export const FIND_MODAL_OPEN = 'FIND_MODAL_OPEN';
export const FIND_MODAL_CLOSE = 'FIND_MODAL_CLOSE';
export const FIND_MODAL_FILTER_CHANGE = 'FIND_MODAL_FILTER_CHANGE';
export const FIND_MODAL_FILTER_APPLY = 'FIND_MODAL_FILTER_APPLY';


export const SEARCH_VALUE_CHANGE = 'SEARCH_VALUE_CHANGE'
export const SEARCH_APPLIED = 'SEARCH_APPLIED'
export const SEARCH_CARD_SELECTED = 'SEARCH_CARD_SELECTED'

export const ADD_CARD_TO_DECK = 'ADD_CARD_TO_DECK';
export const ADD_CARD_TO_RARES = 'ADD_CARD_TO_RARES';

export const REQUEST_CARD_SEARCH = 'REQUEST_CARD_SEARCH';
export const RECEIVE_CARD_SEARCH = 'RECEIVE_CARD_SEARCH';

export const REQUEST_CARD_DETAIL = 'REQUEST_CARD_DETAIL'
export const RECEIVE_CARD_DETAIL = 'RECEIVE_CARD_DETAIL'

//card binder actions
export const CARD_BINDER_LAND_COUNT_CHANGE = 'CARD_BINDER_LAND_COUNT_CHANGE';
export const CARD_BINDER_VIEW_CHANGE = 'CARD_BINDER_VIEW_CHANGE'
export const CARD_BINDER_GROUP_CHANGE = 'CARD_BINDER_GROUP_CHANGE'
export const CARD_BINDER_SORT_CHANGE = 'CARD_BINDER_SORT_CHANGE'
export const CARD_BINDER_FILTER_CHANGE = 'CARD_BINDER_FILTER_CHANGE'
export const CARD_BINDER_SELECTION_CHANGE = 'CARD_BINDER_SELECTION_CHANGE'
export const CARD_BINDER_CARD_ADD = 'CARD_BINDER_CARD_ADD'
export const CARD_BINDER_CARD_REMOVE = 'CARD_BINDER_CARD_REMOVE'
export const CARD_BINDER_SECTION_TOGGLE = 'CARD_BINDER_SECTION_TOGGLE' // selection toggle ??
export const CARD_BINDER_SHEET_TOGGLE = 'CARD_BINDER_SHEET_TOGGLE'


export const ON_SECTION_TOGGLE = 'ON_SECTION_TOGGLE'

//Action Creators
// export function addDeck(deck: CardDeck): ReduxAction {
//     return {
//         type: ADD_DECK,
//         payload: {
//             deck: deck
//         }
//     }
// }



export const appNavClick = (): ReduxAction => ({
    type: APP_NAV_CLICK
})


//deck: CardDeck
export const addDeck = (): ReduxAction => ({
    type: ADD_DECK,
    // payload: deck
});

export const selectDeck = (id: number): ReduxAction => ({
    type: SELECT_DECK,
    payload: id
    // payload: {
    //     id: id
    // }
});

export const deckChanged = (updatedDeck: CardDeck): ReduxAction => ({
    type: SELECTED_DECK_CHANGE,
    payload: updatedDeck
});

export const saveDeck = (): ReduxAction => ({
    type: SELECTED_DECK_SAVED
});

export const logState = (): ReduxAction => ({
    type: LOG_STATE
});

export const openFindCardModal = (): ReduxAction => ({
    type: FIND_MODAL_OPEN
});

export const closeFindCardModal = (): ReduxAction => ({
    type: FIND_MODAL_CLOSE
});

export const findModalFilterChange = (updatedFilter: SearchFilterProps): ReduxAction => ({
    type: FIND_MODAL_FILTER_CHANGE,
    payload: updatedFilter
});

export const findModalFilterApply = () => ({
    type: FIND_MODAL_FILTER_APPLY
});

export const searchValueChange = (newValue: string): ReduxAction => ({
    type: SEARCH_VALUE_CHANGE,
    payload: newValue
});

export const searchCardSelected = (cardId: string): ReduxAction => ({
    type: SEARCH_CARD_SELECTED,
    payload: cardId
});

export const addCardToDeck = (cardId: string): ReduxAction => ({
    type: ADD_CARD_TO_DECK,
    payload: cardId
});

export const addCardToRares = (cardId: string): ReduxAction => ({
    type: ADD_CARD_TO_RARES,
    payload: cardId
});


//START: Card binder action action creators
export const cardBinderLandCountChange = (newValue: number, manaType: string): ReduxAction => ({
    type: CARD_BINDER_LAND_COUNT_CHANGE,
    payload: {
        newValue: newValue,
        manaType: manaType
    }
})

export const cardBinderViewChange = (value: string): ReduxAction => ({
    type: CARD_BINDER_VIEW_CHANGE,
    payload: value
})

export const cardBinderGroupChange = (value: string): ReduxAction => ({
    type: CARD_BINDER_GROUP_CHANGE,
    payload: value
})

export const cardBinderSortChange = (value: string): ReduxAction => ({
    type: CARD_BINDER_SORT_CHANGE,
    payload: value
})

export const cardBinderFilterChange = (value: string): ReduxAction => ({
    type: CARD_BINDER_FILTER_CHANGE,
    payload: value
})

export const cardBinderSelectionChange = (): ReduxAction => ({
    type: CARD_BINDER_SELECTION_CHANGE,
    payload: undefined
})

export const cardBinderCardAdd = (): ReduxAction => ({
    type: CARD_BINDER_CARD_ADD,
    payload: undefined
})

export const cardBinderCardRemove = (): ReduxAction => ({
    type: CARD_BINDER_CARD_REMOVE,
    payload: undefined
})

export const cardbinderSectionToggle = (): ReduxAction => ({
    type: CARD_BINDER_SECTION_TOGGLE,
    payload: undefined
})

//END: Card binder action action creators


export const onSectionToggle = (sectionIndex: number): ReduxAction => ({
    type: ON_SECTION_TOGGLE,
    payload: sectionIndex
})

export const cardBinderSheetToggle = (section: string): ReduxAction => ({
    type: CARD_BINDER_SHEET_TOGGLE,
    payload: section
})
//CARD_BINDER_SHEET_TOGGLE


// function canFetchCards(state: AppState){
//     return (!state.searchFilter.isFetching);
// }

// export const badSearch = () => {
//     let filter: CardFilter = {
//         name: "Nicol"
//     }
//     Cards.where(filter).then(results => {
//         // console.log('grand result dump');
//         // console.log(results);
//         // return dispatch(receiveCardSearch(results));
//         // return results;
//         // for (const card of results) console.log(card.name);
//     });
// }

export const fetchCardsIfNeeded = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        if(shouldFetchCards(getState())){
            console.log('WE SHOULD FETCH A CARD');
            return tryFetchCardDetail(dispatch, getState());
        }
        
    }
}

function shouldFetchCards(state: State): boolean {
    //return true if there are any requested cards
    if(!state.actions.searchIsFetching && state.actions.requestedCards.length > 0){
        return true;
    } else {
        return false
    }
}


// export const cardDetailRequested = (cardId: string): any => {
//     return (dispatch: Dispatch, getState: any) => {
//         return tryFetchCardDetail(cardId, dispatch, getState());
//     }
// }
function tryFetchCardDetail(dispatch: Dispatch, state: State) {



    // const requestedCard = state.actions.cardIndex[cardId];
    // console.log('requested card');
    // console.log(requestedCard);
    // if(!requestedCard){
    //     console.log('no card found, requesting ' + cardId);
    //     fetchCardDetail(cardId, dispatch);
    // }
    // else if (!requestedCard.updateRequested){
    //     console.log('card found and no request, requesting ' + cardId);
    //     fetchCardDetail(cardId, dispatch);
    // } else {
    //     console.log('---card found and REQUEST PENDING ' + cardId);
    // }

    const nextIdToFetch = state.actions.requestedCards[0];
    if(!state.actions.searchIsFetching && nextIdToFetch){
        console.log('requestING card');
        fetchCardDetail(nextIdToFetch, dispatch);
    }
}

// function formatIdString(id: string){
//     id.slice(0, 8)
// }

function fetchCardDetail(cardId: string, dispatch: Dispatch){
    dispatch(requestCardDetail(cardId));

    console.log('about to request id:');
    console.log(cardId)

    let parsedId = [
        cardId.slice(0, 8),
        cardId.slice(8, 12),
        cardId.slice(12, 16),
        cardId.slice(16, 20),
        cardId.slice(20)
    ].join("-")
    console.log(parsedId)
    
    let cardFilter: CardFilter = {
        //name: "Nicol"
        // name: filter
        
    }

    return Cards.find(parsedId).then((result) => {
        console.log('card request recieve');
        console.log(result)
        return dispatch(receiveCardDetail(result));
    }, (error) => {
        console.log('error ?')
        console.log(error)
        return dispatch(receiveCardDetail(undefined));
    });
}

//This is what's called from the QuickSearchAddWhatever
export const searchApplied = (filter: string): any => {
    return (dispatch: Dispatch, getState: any) => {
        return tryFetchCards(dispatch, getState());
    }
};
function tryFetchCards(dispatch: Dispatch, state: State) {
    //if(!state.actions.searchFilter.isFetching){
    if(!state.actions.searchIsFetching){
        fetchCards(dispatch, state.actions.searchFilter.name);
    }
}

//export const fetchCards = (): any => {
//This is supposed to return a function that takes in whatever TF Dispatch is
function fetchCards(dispatch: Dispatch, filter: string): any {
    
    dispatch(requestCardSearch());

    let cardFilter: CardFilter = {
        //name: "Nicol"
        name: filter
    }   
    return Cards.where(cardFilter).then(results => {
        console.log('grand result dump');
        console.log(results);
        return dispatch(receiveCardSearch(results));
    });
}

export const requestCardSearch = (): ReduxAction => ({
    type: REQUEST_CARD_SEARCH,
    // payload: filter
});

export const receiveCardSearch = (cards: Card[]): ReduxAction => ({
    type: RECEIVE_CARD_SEARCH,
    payload: cards
});

export const requestCardDetail = (cardId: string): ReduxAction => ({
    type: REQUEST_CARD_DETAIL,
    payload: cardId
});
export const receiveCardDetail = (card: Card | undefined): ReduxAction => ({
    type: RECEIVE_CARD_DETAIL,
    payload: card
});

// export const searchForCards = (filter: CardFilter): any => {
//     console.log('search for cards called');
//     //state = getState();
//     return (dispatch: any, getState: any) => {
//         let state: AppState = getState();
//         if(!state.searchFilter.isFetching){
//             return dispatch(fetchCards(filter));
//         }
//     }
// }


//MTG api calls



// //This is the function that will be called by the UI
// function tryFetchCards(filter: CardFilter){
//     return (dispatch: any, getState: any) => {
        
//         // return dispatch()
//     }



//     // if(true){
//     //     return dispatchEvent(fetchCards(filter));
//     // }
//     //if not already fetching cards...

// }


    // let filter: CardFilter = {
    //     name: "Nicol"
    // }
    // console.log('searching for cards');
    // console.log(React);
    // console.log(Cards);

    // Cards.where(filter).then(results => {
    //         console.log('grand result dump');
    //         console.log(results);
    //         for (const card of results) console.log(card.name);
    //     });

    
    // return Magic.Cards.
// }