// import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../_actions';
// import { CARD_SEARCH_ADD_PENDING_CARD, CARD_SEARCH_REMOVE_PENDING_CARD, CARD_SEARCH_CLEAR_PENDING_CARDS } from '../_actions';
// import { INVENTORY_ADD_COMPLETE } from '../_actions/inventoryActions';

import { CARD_SEARCH_RECEIVED, CARD_SEARCH_REQUESTED, CARD_SEARCH_INVENTORY_RECEIVED, CARD_SEARCH_INVENTORY_REQUESTED } from "./cardSearchDataActions";

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
        case CARD_SEARCH_REQUESTED: return cardSearchRequested(state, action);
        case CARD_SEARCH_RECEIVED: return cardSearchReceived(state, action);
        case CARD_SEARCH_INVENTORY_REQUESTED: return cardSearchInventoryRequested(state, action);
        case CARD_SEARCH_INVENTORY_RECEIVED: return cardSearchInventoryReceived(state, action);

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

        default: return(state);
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

function cardSearchRequested(state: State, action: ReduxAction): State {
    const newState: State = {
        ...state,
        searchResults: {
            ...initialState.searchResults,
            isLoading: true,
        }
    }
    return newState;
}

function cardSearchReceived(state: State, action: ReduxAction): State {
    const searchResultPayload: CardSearchResultDto[] = action.payload || [];
    let resultsById = {};
    searchResultPayload.forEach(card => resultsById[card.cardId] = card);
    const newState: State = {
        ...state,
        searchResults: {
            ...state.searchResults,
            isLoading: false,
            searchResultsById: resultsById,
            allSearchResultIds: searchResultPayload.map(card => card.cardId),
        }
    }
    return newState;
}

function cardSearchInventoryRequested(state: State, action: ReduxAction): State {
    const newState: State = {
        ...state,
        inventoryDetail: {
            ...initialState.inventoryDetail,
            isLoading: true,
        }
    };
    return newState;
}

function cardSearchInventoryReceived(state: State, action: ReduxAction): State {
    const detailResult: InventoryDetailDto = action.payload;

    let inventoryCardsById = {}
    detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);

    let cardsById = {}
    detailResult.cards.forEach(card => cardsById[card.multiverseId] = card);

    const newState: State = {
        ...state,
        inventoryDetail: {
            ...state.inventoryDetail,
            isLoading: false,
            inventoryCardsById: inventoryCardsById,
            inventoryCardAllIds: detailResult.inventoryCards.map(card => card.id),
            cardsById: cardsById,
            allCardIds: detailResult.cards.map(card => card.multiverseId),
        },
    };
    
    return newState;
}