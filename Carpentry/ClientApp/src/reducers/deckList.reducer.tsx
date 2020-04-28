import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions/index.actions';

//Not worried about specific "x_REQUESTED" and "X_RECEIVED" actions, too much overhead/multiple actions
//Instead, using a reusable "API_DATA_REQUESTED" and "API_DATA_RECEIVED"
//THIS SPECIFIC REDUCER doesn't care when data is requested I guess, unless I want to clear the deck list

//Okay I know I'm "supposed" to refactor out domain data but it makes sense to keep this grouped together

//I'm not sure if I should just start by moving objects around, or actually create that updated state?
//Why half-ass it now??
declare interface DeckListState {

    //OLD
    //decks: DeckProperties[]; //key = id

    //NEW
    decksById: { [id: number]: DeckOverviewDto }
    deckIds: number[]

}

export const apiDataRequested = (state: DeckListState, action: ReduxAction): DeckListState => {
    const { scope, data } = action.payload;
    
    if(scope as ApiScopeOption !== "deckList") return (state);

    const newState: DeckListState = {
        ...state,
        deckIds: [],
        decksById: {},
    };

    return newState;
}

export const apiDataReceived = (state: DeckListState, action: ReduxAction): DeckListState => {
    const { scope, data } = action.payload;
    
    if(scope as ApiScopeOption !== "deckList") return (state);

    //I guess this is normally where Normalizr should be used?
    const apiDecks: DeckOverviewDto[] = data;

    let decksById: { [key:number]: DeckOverviewDto } = {};

    apiDecks.forEach(deck => {
        decksById[deck.id] = deck;
    });
    //

    const newState: DeckListState = {
        ...state,
        deckIds: apiDecks.map(deck => deck.id),
        decksById: decksById,
        // decks: data,
    };

    return newState;
}

export const deckList = (state = initialState, action: ReduxAction): DeckListState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);
        
        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);
        
        default: 
            return(state);
    }
}
  
const initialState: DeckListState = {
    decksById: {},
    deckIds: [],
}
  
