import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions';

export interface DeckOverviewsState {
    decksById: { [id: number]: DeckOverviewDto };
    deckIds: number[];
    isLoading: boolean;
    isInitialized: boolean;
}

export const apiDataRequested = (state: DeckOverviewsState, action: ReduxAction): DeckOverviewsState => {
    const { scope, data } = action.payload;
    
    if(scope as ApiScopeOption !== "deckOverviews") return (state);

    const newState: DeckOverviewsState = {
        ...state,
        deckIds: [],
        decksById: {},
        isLoading: true,
    };

    return newState;
}

export const apiDataReceived = (state: DeckOverviewsState, action: ReduxAction): DeckOverviewsState => {
    const { scope, data } = action.payload;
    
    if(scope as ApiScopeOption !== "deckOverviews") return (state);

    //I guess this is normally where Normalizr should be used?
    const apiDecks: DeckOverviewDto[] = data;

    let decksById: { [key:number]: DeckOverviewDto } = {};

    apiDecks.forEach(deck => {
        decksById[deck.id] = deck;
    });
    //

    const newState: DeckOverviewsState = {
        ...state,
        deckIds: apiDecks.map(deck => deck.id),
        decksById: decksById,
        isLoading: false,
        isInitialized: true,
        // decks: data,
    };

    return newState;
}

export const deckOverviews = (state = initialState, action: ReduxAction): DeckOverviewsState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);
        
        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);
        
        default: 
            return(state);
    }
}
  
const initialState: DeckOverviewsState = {
    decksById: {},
    deckIds: [],
    isLoading: false,
    isInitialized: false,
}
  
