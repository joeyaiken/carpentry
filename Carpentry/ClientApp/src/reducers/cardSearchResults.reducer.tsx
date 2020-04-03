import { API_DATA_RECEIVED, API_DATA_REQUESTED } from '../actions/index.actions';

declare interface cardSearchResultsState {
    searchResultsById: {[multiverseId: number]: MagicCard};
    allSearchResultIds: number[];

    // selectedCard: MagicCard | null; //should probably be an AppState ID
    //what if this just used the other reducer?...
    // inventoryDetail: InventoryDetailDto | null;
}

// const exampleReducerFunction = (state: cardSearchResultsState, action: ReduxAction): cardSearchResultsState => {
//     const newState: cardSearchResultsState = {
//         ...state,
//     }
//     return newState;
// }



//////////////////////////

//////////////////////////

// export const _apiDataReceived = (state: DeckListState, action: ReduxAction): DeckListState => {
//     const { scope, data } = action.payload;
    
//     if(scope as ApiScopeOption !== "deckList") return (state);

//     //I guess this is normally where Normalizr should be used?
//     const apiDecks: DeckProperties[] = data;

//     let decksById = {};

//     apiDecks.forEach(deck => {
//         decksById[deck.id] = deck;
//     });
//     //

//     const newState: DeckListState = {
//         ...state,
//         deckIds: apiDecks.map(deck => deck.id),
//         decksById: decksById,
//         // decks: data,
//     };

//     return newState;
// }

export const apiDataRequested = (state: cardSearchResultsState, action: ReduxAction): cardSearchResultsState => {
    const { scope } = action.payload;
    
    if(scope as ApiScopeOption !== "cardSearchResults") return (state);

    const newState: cardSearchResultsState = {
        ...state,
        ...initialState,
    }

    return newState;
}

export const apiDataReceived = (state: cardSearchResultsState, action: ReduxAction): cardSearchResultsState => {
    
    const { scope, data } = action.payload;

    // console.log('card search data receive');
    // console.log(`scope: ${scope}`);

    if(scope as ApiScopeOption !== "cardSearchResults") return (state);

    const searchResultPayload: MagicCard[] = data || [];

    let resultsById = {};

    searchResultPayload.forEach(card => resultsById[card.multiverseId] = card);



    const newState: cardSearchResultsState = {
        ...state,
        searchResultsById: resultsById,
        allSearchResultIds: searchResultPayload.map(card => card.multiverseId),
        // isLoading: false,
        // searchResults: searchResultPayload,
    }
    return newState;
}


export const cardSearchResults = (state = initialState, action: ReduxAction): cardSearchResultsState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);

        default:
            return(state)
    }
}

const initialState: cardSearchResultsState = {
    searchResultsById: {},
    allSearchResultIds: [],
}
