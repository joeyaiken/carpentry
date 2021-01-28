// import { 
//     SEARCH_REQUESTED,
//     SEARCH_RECEIVED,
//     ADD_PENDING_CARD,
//     REMOVE_PENDING_CARD,
//     FILTER_VALUE_CHANGED,
//     TOGGLE_CARD_SEARCH_VIEW_MODE,
//     CARD_SEARCH_SEARCH_METHOD_CHANGED,
//     CARD_SEARCH_CLEAR_PENDING_CARDS,
//     CARD_SEARCH_SELECT_CARD,
//     SAVE_REQUESTED,
//     SAVE_COMPLETED,
// } from "./InventoryAddCardsActions";

export interface State {
    searchProps: TrimmingToolSearchProps;



    // searchResults: {
    //     isLoading: boolean;
    //     searchResultsById: {[multiverseId: number]: CardSearchResultDto};
    //     allSearchResultIds: number[];
    // };

    // searchFilter: CardFilterProps;
    // viewMode: CardSearchViewMode;

    // //Consider grouping this in an obj like searchResults
    // pendingCardsSaving: boolean;
    // pendingCards: { [name: string]: PendingCardsDto } //key === name, should this also have a list to track all keys?
    // //

    // cardSearchMethod: "set" | "web" | "inventory";
    // selectedCard: CardSearchResultDto | null; //should probably be an AppState ID
    
}

export const inventoryAddCardsReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        // case SEARCH_REQUESTED: return searchRequested(state, action);
        // case SEARCH_RECEIVED: return searchReceived(state, action);
        // case FILTER_VALUE_CHANGED: return filterValueChanged(state, action);
        // case ADD_PENDING_CARD: return cardSearchAddPendingCard(state, action);
        // case REMOVE_PENDING_CARD: return cardSearchRemovePendingCard(state, action);
        // case TOGGLE_CARD_SEARCH_VIEW_MODE: return toggleSearchViewMode(state, action);
        // case CARD_SEARCH_SEARCH_METHOD_CHANGED: return resetCardSearchFilterProps(state, action);
        // case CARD_SEARCH_CLEAR_PENDING_CARDS: return {...state, pendingCards: {}}
        // case CARD_SEARCH_SELECT_CARD:
        //     const selectedCard: CardSearchResultDto = action.payload;
        //     return {
        //         ...state,
        //         selectedCard: selectedCard,
        //     };
        // case SAVE_REQUESTED: return { ...state, pendingCardsSaving: true };
        // case SAVE_COMPLETED: return { ...state, pendingCardsSaving: false, pendingCards: {} };
        default: return(state);
    }
}

const initialState: State = {
    searchProps: {
        setCode: '',
        group: '',
        minCount: 0,
        minBy: '',
    }
    // searchResults: {
    //     isLoading: false,
    //     searchResultsById: {},
    //     allSearchResultIds: [],
    // },

    // searchFilter: defaultSearchFilterProps(),
    // viewMode: "list",
    // pendingCardsSaving: false,
    // cardSearchMethod: "set",
    // selectedCard:  null, //should probably be an AppState ID
    // pendingCards: {},
};

// function defaultSearchFilterProps(): CardFilterProps {
//     return {
//         set: '',
//         colorIdentity: [],
//         //rarity: ['mythic','rare','uncommon','common'], //
//         rarity: [], //
//         type: '',//'Creature',
//         exclusiveColorFilters: false,
//         multiColorOnly: false,
//         cardName: '',
//         exclusiveName: false,
//         maxCount: 0,
//         minCount: 0,
//         format: '',
//         text: '',
//         group: '',
//     }
// }

// function searchRequested(state: State, action: ReduxAction): State {
//     const newState: State = {
//         ...state,
//         searchResults: {
//             ...initialState.searchResults,
//             isLoading: true,
//         }
//     }
//     return newState;
// }

// function searchReceived(state: State, action: ReduxAction): State {
//     const searchResultPayload: CardSearchResultDto[] = action.payload || [];
//     let resultsById = {};
//     searchResultPayload.forEach(card => resultsById[card.cardId] = card);
//     const newState: State = {
//         ...state,
//         searchResults: {
//             ...state.searchResults,
//             isLoading: false,
//             searchResultsById: resultsById,
//             allSearchResultIds: searchResultPayload.map(card => card.cardId),
//         }
//     }
//     return newState;
// }

// const cardSearchAddPendingCard = (state = initialState, action: ReduxAction): State => {
//     //'pending cards' is now a dictionary of 'pending card dto's
//     const {
//         // data, //0
//         name,
//         cardId,
//         isFoil, //false
//         // variant, //"normal"

//         //need
//         //cardId (for inv card)
//         //cardName (for display, and grouping?)
//     } = action.payload;
    
//     // const magicCardToAdd: MagicCard = data;
    
//     let cardToAdd: PendingCardsDto = state.pendingCards[name];
    
//     if(!cardToAdd){
//         cardToAdd = {
//             name: name,
//             cards: [],
//         };

//         //OK - So I need to either pass the name, or the whole card
        
        
//         // const magicCardToAdd = state.
//         // if we can't find a matching card, nothing gets added and this just continues silently (I guess)
//     //     if(magicCardToAdd){
//     //         cardToAdd = {
//     //             multiverseId: multiverseId,
//     //             cards: [],
//     //             // data: magicCardToAdd,
//     //         };
//     //     }                    
//     }

//     //These are the only 3 fields used by the api bulkAdd
//     cardToAdd.cards.push({
//         cardId: cardId,
//         isFoil: isFoil,
//         statusId: 1,
//     } as InventoryCard);

//     const newState: State = {
//         ...state,
//         pendingCards: {
//             ...state.pendingCards,
//             [name]: cardToAdd
//         }
//     }
//     return newState;
// }

// const cardSearchRemovePendingCard = (state = initialState, action: ReduxAction): State => {
//     const {
//         name,
//         cardId,
//         isFoil,
//     } = action.payload;

//     // const midToRemove = action.payload.multiverseId;
//     // const removeFoilCard = action.payload.isFoil;
//     // const variantToRemove: string = action.payload.variant;

//     let objToRemoveFrom = state.pendingCards[name];

//     if(objToRemoveFrom){

//         //let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.variantName === variantToRemove && x.isFoil === removeFoilCard);
//         let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.cardId === cardId && x.isFoil === isFoil);

//         if(thisInvCard >= 0){
//             objToRemoveFrom.cards.splice(thisInvCard,1);
//         }

//         let pendingCardsAfterRemoval =  {
//             ...state.pendingCards,
//             [name]: objToRemoveFrom
//         }
//         //if this pending cards object has 0 items, it should be deleted from the dictionary
//         if(objToRemoveFrom.cards.length === 0){
//             delete pendingCardsAfterRemoval[name];
//         }
//         const newState: State = {
//             ...state,
//             pendingCards: pendingCardsAfterRemoval
//         }
//         return newState;

//     } else {
//         const newState: State = {
//             ...state,
//         }
//         return newState;
//     }
// }

// function toggleSearchViewMode(state: State, action: ReduxAction): State {
//     let newViewMode: CardSearchViewMode = "list";

//     switch(state.viewMode){
//         case "list":
//             newViewMode = "grid";
//             break;
//         case "grid":
//             newViewMode = "list";
//             break;
//     }
    
//     return {
//         ...state,
//         viewMode: newViewMode,
//     };
// }

// const filterValueChanged = (state: State, action: ReduxAction): State => {
//     //const { type, filter, value } = action.payload;
//     const { type, filter, value } = action.payload;
//     console.log('CS filter changed');
//     console.log(action.payload);

//     const existingFilter = state.searchFilter;
//     const newState: State = {
//         ...state,
//         searchFilter: {
//             ...existingFilter, 
//             [filter]: value,
//         }
//     }
//     return newState;
// }

// const resetCardSearchFilterProps = (state: State, action: ReduxAction): State => {
//     const newState: State = {
//         ...state,
//         searchFilter: defaultSearchFilterProps(),
//     }
//     return newState;
// }