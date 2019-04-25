import React from 'react';
import Magic, { CardFilter, Cards, Card } from 'mtgsdk-ts';
// import { Card, Set, CardFilter, SetFilter, PaginationFilter } from "./IMagic";

import Redux, { Store, Dispatch } from 'redux'
import { stat } from 'fs';
import { unmountComponentAtNode } from 'react-dom';
import { string } from 'prop-types';

import { mapCardToICard } from '../../carpentry.data/lumberyard';

//export const searchApplied = (filter: string): any => {
export const mtgApiRequestAddPackSearch = (): any => {
    console.log('search requested')

    return (dispatch: Dispatch, getState: any) => {
        return tryFetchCardsForAddPack(dispatch, getState());
    }
};

// export const mtgApiRequestAddPackSearch = (): any => {
//     ((dispatch: Dispatch, getState: any) => tryFetchCards(dispatch, getState()))
// };

function tryFetchCardsForAddPack(dispatch: Dispatch, state: State) {
    // console.log('state');
    // console.log(state)
    // console.log(state.mtgApiSearch)
    if(!state.mtgApiSearch.searchInProgress && state.addPack.selectedSetCode){
        //fetchCards(dispatch, state.mgtApiSearch.searchFilter);
        fetchCardsForSet(dispatch, state.addPack.selectedSetCode);
    }
}

export const MTG_API_SEARCH_REQUESTED = 'MTG_API_SEARCH_REQUESTED';
export const mtgApiSearechRequested = (): ReduxAction => ({
    type: MTG_API_SEARCH_REQUESTED,
});

export const MTG_API_SEARCH_COMPLETED = 'MTG_API_SEARCH_COMPLETED';
export const mtgApiSearechCompleted = (): ReduxAction => ({
    type: MTG_API_SEARCH_COMPLETED,
});

export const MTG_API_SEARCH_ITEM_RETURNED = 'MTG_API_SEARCH_ITEM_RETURNED';
export const mtgApiSearchItemReturned = (card: ICard): ReduxAction => ({
    type: MTG_API_SEARCH_ITEM_RETURNED,
    payload: card
});

//This is supposed to return a function that takes in whatever TF Dispatch is
function fetchCardsForSet(dispatch: Dispatch, setCode: string): any {
    
    dispatch(mtgApiSearechRequested());

    //dispatch(requestCardSearch());
    let cardFilter: CardFilter = {};
    cardFilter.set = setCode;
    cardFilter.pageSize = 100;
    
    return Cards.all(cardFilter)
    .on("data", (card: Card) => {
        // console.log(card.name);
        return dispatch(mtgApiSearchItemReturned(mapCardToICard(card)));
    })
    .on("cancel", () => {
        console.log("cancel");
    })
    .on("error", (err: Error) => {
        console.log("error: "+err);
    })
    .on("end", () => {
        console.log("done");
        return dispatch(mtgApiSearechCompleted());
    });


    // // cardFilte
    // return Cards.where(cardFilter).then(results => {
    //     console.log('grand result dump');
    //     console.log(results);
    //     // return dispatch(receiveCardSearch(results));
    // });

    

}



// import { lumberyardSaveState } from '../data/lumberyard'


//initializations
export const INI_APP_DATA = 'INI_APP_DATA'


//Actions

//Navigation actions
export const APP_NAV_CLICK = 'APP_NAV_CLICK';
export const APP_NAV_DECK_SELECT = 'APP_NAV_DECK_SELECT'
export const APP_NAV_DECK_ADD = 'APP_NAV_DECK_ADD'
export const APP_SHEET_TOGGLE = 'APP_SHEET_TOGGLE'

export const SAVE_APP_STATE = 'SAVE_APP_STATE'

export const ADD_DECK = 'ADD_DECK'
export const SELECT_DECK = 'SELECT_DECK'
export const LOG_STATE = 'LOG_STATE'
// export const SELECTED_DECK_CHANGE = 'SELECTED_DECK_CHANGE'
export const SELECTED_DECK_SAVED = 'SELECTED_DECK_SAVED'


// export const FIND_MODAL_OPEN = 'FIND_MODAL_OPEN';
// export const FIND_MODAL_CLOSE = 'FIND_MODAL_CLOSE';
// export const FIND_MODAL_FILTER_CHANGE = 'FIND_MODAL_FILTER_CHANGE';
// export const FIND_MODAL_FILTER_APPLY = 'FIND_MODAL_FILTER_APPLY';


export const SEARCH_VALUE_CHANGE = 'SEARCH_VALUE_CHANGE'
// export const SEARCH_APPLIED = 'SEARCH_APPLIED'
export const SEARCH_CARD_SELECTED = 'SEARCH_CARD_SELECTED'

export const SEARCH_FILTER_CHANGE = 'SEARCH_FILTER_CHANGE';

export const ADD_CARD_TO_DECK = 'ADD_CARD_TO_DECK';
export const ADD_CARD_TO_INDEX = 'ADD_CARD_TO_INDEX';
export const ADD_CARD_TO_RARES = 'ADD_CARD_TO_RARES';

export const REQUEST_CARD_SEARCH = 'REQUEST_CARD_SEARCH';
export const RECEIVE_CARD_SEARCH = 'RECEIVE_CARD_SEARCH';

export const REQUEST_CARD_DETAIL = 'REQUEST_CARD_DETAIL';
export const RECEIVE_CARD_DETAIL = 'RECEIVE_CARD_DETAIL';

//deck editor actions
export const TOGGLE_DECK_EDITOR_STATUS = 'TOGGLE_DECK_EDITOR_STATUS';

export const DECK_EDITOR_CARD_SELECTED = 'DECK_EDITOR_CARD_SELECTED';

export const DECK_EDITOR_DUPLICATE_SELECTED_CARD = 'DECK_EDITOR_DUPLICATE_SELECTED_CARD';
export const DECK_EDITOR_REMOVE_ONE_SELECTED_CARD = 'DECK_EDITOR_REMOVE_ONE_SELECTED_CARD';
export const DECK_EDITOR_REMOVE_ALL_SELECTED_CARD = 'DECK_EDITOR_REMOVE_ALL_SELECTED_CARD';

export const CARD_BINDER_LAND_COUNT_CHANGE = 'CARD_BINDER_LAND_COUNT_CHANGE';
export const CARD_BINDER_VIEW_CHANGE = 'CARD_BINDER_VIEW_CHANGE';
export const CARD_BINDER_GROUP_CHANGE = 'CARD_BINDER_GROUP_CHANGE';
export const CARD_BINDER_SORT_CHANGE = 'CARD_BINDER_SORT_CHANGE';
export const CARD_BINDER_FILTER_CHANGE = 'CARD_BINDER_FILTER_CHANGE';
export const CARD_BINDER_SELECTION_CHANGE = 'CARD_BINDER_SELECTION_CHANGE';
export const CARD_BINDER_CARD_ADD = 'CARD_BINDER_CARD_ADD';
export const CARD_BINDER_CARD_REMOVE = 'CARD_BINDER_CARD_REMOVE';
export const DECK_EDITOR_SECTION_TOGGLE = 'DECK_EDITOR_SECTION_TOGGLE'; // selection toggle ??

// export const APP_DATA_STORE_STRING_CHANGE = 'APP_DATA_STORE_STRING_CHANGE';

// export const ON_SECTION_TOGGLE = 'ON_SECTION_TOGGLE'

//Action Creators
// export function addDeck(deck: CardDeck): ReduxAction {
//     return {
//         type: ADD_DECK,
//         payload: {
//             deck: deck
//         }
//     }
// }


export const initAppData = (): ReduxAction => ({
    type: INI_APP_DATA
});


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

// export const deckChanged = (updatedDeck: CardDeck): ReduxAction => ({
//     type: SELECTED_DECK_CHANGE,
//     payload: updatedDeck
// });

export const saveDeck = (): ReduxAction => ({
    type: SELECTED_DECK_SAVED
});

export const logState = (): ReduxAction => ({
    type: LOG_STATE
});

// export const openFindCardModal = (): ReduxAction => ({
//     type: FIND_MODAL_OPEN
// });

// export const closeFindCardModal = (): ReduxAction => ({
//     type: FIND_MODAL_CLOSE
// });

// export const findModalFilterChange = (updatedFilter: SearchFilterProps): ReduxAction => ({
//     type: FIND_MODAL_FILTER_CHANGE,
//     payload: updatedFilter
// });

// export const findModalFilterApply = () => ({
//     type: FIND_MODAL_FILTER_APPLY
// });

export const searchValueChange = (newValue: string): ReduxAction => ({
    type: SEARCH_VALUE_CHANGE,
    payload: newValue
});

export const searchCardSelected = (cardId: string, cardName: string): ReduxAction => ({
    type: SEARCH_CARD_SELECTED,
    // payload: cardId
    payload: {
        cardId: cardId,
        cardName: cardName
    }
});

export const searchFilterChanged = (property: string, value: string|boolean): ReduxAction => ({
    type: SEARCH_FILTER_CHANGE,
    payload: {
        property: property,
        value: value
    }
})

export const addCardToDeck = (card: IDeckCard): ReduxAction => ({
    type: ADD_CARD_TO_DECK,
    payload: card
});

export const addCardToIndex = (card: ICard): ReduxAction => ({
    type: ADD_CARD_TO_INDEX,
    payload: card
})

export const addCardToRares = (cardId: string): ReduxAction => ({
    type: ADD_CARD_TO_RARES,
    payload: cardId
});


//START: Deck Editor action creators

export const toggleDeckEditorStatus = (): ReduxAction => ({
    type: TOGGLE_DECK_EDITOR_STATUS
});


// export const DECK_EDITOR_CARD_SELECTED = 'DECK_EDITOR_CARD_SELECTED'
//deckEditorCardSelected
export const deckEditorCardSelected = (cardName: string): ReduxAction => ({
    type: DECK_EDITOR_CARD_SELECTED,
    payload: cardName
})

export const deckEditorLandCountChange = (newValue: number, manaType: string): ReduxAction => ({
    type: CARD_BINDER_LAND_COUNT_CHANGE,
    payload: {
        newValue: newValue,
        manaType: manaType
    }
})

export const deckEditorViewChange = (value: string): ReduxAction => ({
    type: CARD_BINDER_VIEW_CHANGE,
    payload: value
})

export const deckEditorGroupChange = (value: string): ReduxAction => ({
    type: CARD_BINDER_GROUP_CHANGE,
    payload: value
})

export const deckEditorSortChange = (value: string): ReduxAction => ({
    type: CARD_BINDER_SORT_CHANGE,
    payload: value
})

export const deckEditorDuplicateSelectedCard = (): ReduxAction => ({
    type: DECK_EDITOR_DUPLICATE_SELECTED_CARD
})

export const deckEditorRemoveOneSelectedCard = (): ReduxAction => ({
    type: DECK_EDITOR_REMOVE_ONE_SELECTED_CARD
})

export const deckEditorRemoveAllSelectedCards = (): ReduxAction => ({
    type: DECK_EDITOR_REMOVE_ALL_SELECTED_CARD
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

export const deckEditorSectionToggle = (sectionIndex: string): ReduxAction => ({
    type: DECK_EDITOR_SECTION_TOGGLE,
    payload: undefined
})



//END: Card binder action action creators
 

// export const onSectionToggle = (sectionIndex: string): ReduxAction => ({
//     type: ON_SECTION_TOGGLE,
//     payload: sectionIndex
// })

export const appSheetToggle = (section: string): ReduxAction => ({
    type: APP_SHEET_TOGGLE,
    payload: section
})
//CARD_BINDER_SHEET_TOGGLE






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

////////////////////////////////////////////////////////////////////////////////
//SAVE_APP_STATE
export const appNavSave = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return trySaveState(dispatch, getState());
    }

}

function trySaveState(dispatch: Dispatch, state: State) {
    // lumberyardSaveState(state);
    dispatch(saveAppState())
}

export const saveAppState = (): ReduxAction => ({
    type: SAVE_APP_STATE
})

////////////////////////////////////////////////////////////////////////////////