// import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../_actions';
// import { CARD_SEARCH_ADD_PENDING_CARD, CARD_SEARCH_REMOVE_PENDING_CARD, CARD_SEARCH_CLEAR_PENDING_CARDS } from '../_actions';
// import { INVENTORY_ADD_COMPLETE } from '../_actions/inventoryActions';

export interface State {
    searchResults: {
        isLoading: boolean;
        
        searchResultsById: {[multiverseId: number]: CardSearchResultDto};
        allSearchResultIds: number[];

        // selectedCard: MagicCard | null; //should probably be an AppState ID
        //what if this just used the other reducer?...
        // inventoryDetail: InventoryDetailDto | null;
    };
    inventoryDetail: {
        isLoading: boolean;
        //inventory cards
        inventoryCardsById: { [id: number]: InventoryCard };
        inventoryCardAllIds: number[];


        //--First step in showing an inventory detail is to itterate over MagicCard (or even card variant)
        //--Then would need to show each inventory card for a given magic card
        //      (all inventory cards where I.multiverseId === MC.multiverseId)

        //magic cards belonging to inventory cards
        cardsById: { [multiverseId: number]: MagicCard };
        allCardIds: number[];
    };
}














// const apiDataRequested = (state: CardSearchDataReducerState, action: ReduxAction): CardSearchDataReducerState => {
//     const { scope } = action.payload;

//     if(scope as ApiScopeOption === "cardSearchInventoryDetail"){
//         const newState: CardSearchDataReducerState = {
//             ...state,
//             inventoryDetail: {
//                 ...initialState.inventoryDetail,
//                 isLoading: true,
//             }
//         };

//         return newState;
//     } 
//     else if (scope as ApiScopeOption === "cardSearchResults"){
//         const newState: CardSearchDataReducerState = {
//             ...state,
//             searchResults: {
//                 ...initialState.searchResults,
//                 isLoading: true,
//             }
//         }

//         return newState;
//     }
//     else return (state);
// }

// const apiDataReceived = (state: CardSearchDataReducerState, action: ReduxAction): CardSearchDataReducerState => {
//     const { scope, data } = action.payload;

//     if(scope as ApiScopeOption === "cardSearchInventoryDetail"){
//         console.log('inventory detail received')
//         const detailResult: InventoryDetailDto = data;

//         let inventoryCardsById = {}
//         detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);

//         let cardsById = {}
//         detailResult.cards.forEach(card => cardsById[card.multiverseId] = card);

//         const newState: CardSearchDataReducerState = {
//             ...state,
//             inventoryDetail: {
//                 ...state.inventoryDetail,
//                 isLoading: false,
//                 inventoryCardsById: inventoryCardsById,
//                 inventoryCardAllIds: detailResult.inventoryCards.map(card => card.id),
//                 cardsById: cardsById,
//                 allCardIds: detailResult.cards.map(card => card.multiverseId),
//             }
            
//         };
//         console.log('new card search state');
//         console.log(newState);
//         return newState;
//     } 
//     else if (scope as ApiScopeOption === "cardSearchResults"){
//         console.log('card search results')
//         console.log(data);
//         // if(scope as ApiScopeOption !== "cardSearchResults") return (state);

//         const searchResultPayload: CardSearchResultDto[] = data || [];

//         let resultsById = {};

//         searchResultPayload.forEach(card => resultsById[card.cardId] = card);

//         const newState: CardSearchDataReducerState = {
//             ...state,
//             searchResults: {
//                 ...state.searchResults,
//                 isLoading: false,
//                 searchResultsById: resultsById,
//                 allSearchResultIds: searchResultPayload.map(card => card.cardId),
//             }
//         }

//         return newState;
//     }
//     else return (state);
// }

// const cardSearchAddPendingCard = (state = initialState, action: ReduxAction): CardSearchDataReducerState => {
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

//     const newState: CardSearchDataReducerState = {
//         ...state,
//         pendingCards: {
//             ...state.pendingCards,
//             [name]: cardToAdd
//         }
//     }
//     return newState;
// }

// const cardSearchRemovePendingCard = (state = initialState, action: ReduxAction): CardSearchDataReducerState => {
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
//         const newState: CardSearchDataReducerState = {
//             ...state,
//             pendingCards: pendingCardsAfterRemoval
//         }
//         return newState;

//     } else {
//         const newState: CardSearchDataReducerState = {
//             ...state,
//         }
//         return newState;
//     }
// }

export const cardSearchDataReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){

        // case API_DATA_REQUESTED:
        //     return apiDataRequested(state, action);

        // case API_DATA_RECEIVED:
        //     return apiDataReceived(state, action);

        // case CARD_SEARCH_ADD_PENDING_CARD:
        //     return cardSearchAddPendingCard(state, action);
            
        // case CARD_SEARCH_REMOVE_PENDING_CARD:
        //     return cardSearchRemovePendingCard(state, action);
    
        // case CARD_SEARCH_CLEAR_PENDING_CARDS:
        //     return {
        //         ...state,
                
        //         pendingCards: {},
        //     }// as cardSearchPendingCardsState;

        // case INVENTORY_ADD_COMPLETE:
        //     return {
        //         ...state,
        //         pendingCards: {},
        //         // pendingCardsSaving: false,
        //     } //as cardSearchPendingCardsState;

        default:
            return(state)
    }
}

const initialState: State = {
    searchResults: {
        isLoading: false,
        searchResultsById: {},
        allSearchResultIds: [],
    },
    inventoryDetail: {
        isLoading: false,
        inventoryCardsById: {},
        inventoryCardAllIds: [],
        cardsById: {},
        allCardIds: [],
    },
}