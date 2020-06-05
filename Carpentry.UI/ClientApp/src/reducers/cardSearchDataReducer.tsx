import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions';
import { CARD_SEARCH_ADD_PENDING_CARD, CARD_SEARCH_REMOVE_PENDING_CARD, CARD_SEARCH_CLEAR_PENDING_CARDS } from '../actions/';
import { INVENTORY_ADD_COMPLETE } from '../actions/inventoryActions';

export interface CardSearchDataReducerState {
    searchResults: {
        isLoading: boolean;
        searchResultsById: {[multiverseId: number]: MagicCard};
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
    
    //wait ID or MID ?
    pendingCards: { [key:number]: PendingCardsDto } //key === id, should this also have a list to track all keys? 
}

const apiDataRequested = (state: CardSearchDataReducerState, action: ReduxAction): CardSearchDataReducerState => {
    const { scope } = action.payload;

    if(scope as ApiScopeOption === "cardSearchInventoryDetail"){
        const newState: CardSearchDataReducerState = {
            ...state,
            inventoryDetail: {
                ...initialState.inventoryDetail,
                isLoading: true,
            }
        };

        return newState;
    } 
    else if (scope as ApiScopeOption === "cardSearchResults"){
        const newState: CardSearchDataReducerState = {
            ...state,
            searchResults: {
                ...initialState.searchResults,
                isLoading: true,
            }
        }

        return newState;
    }
    else return (state);
}

const apiDataReceived = (state: CardSearchDataReducerState, action: ReduxAction): CardSearchDataReducerState => {
    const { scope, data } = action.payload;

    if(scope as ApiScopeOption === "cardSearchInventoryDetail"){

        const detailResult: InventoryDetailDto = data;

        let inventoryCardsById = {}
        detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);

        let cardsById = {}
        detailResult.cards.forEach(card => cardsById[card.multiverseId] = card);

        const newState: CardSearchDataReducerState = {
            ...state,
            inventoryDetail: {
                ...state.inventoryDetail,
                isLoading: false,
                inventoryCardsById: inventoryCardsById,
                inventoryCardAllIds: detailResult.inventoryCards.map(card => card.id),
                cardsById: cardsById,
                allCardIds: detailResult.cards.map(card => card.multiverseId),
            }
            
        };

        return newState;
    } 
    else if (scope as ApiScopeOption === "cardSearchResults"){
        console.log('card search results')
        console.log(data);
        // if(scope as ApiScopeOption !== "cardSearchResults") return (state);

        const searchResultPayload: MagicCard[] = data || [];

        let resultsById = {};

        searchResultPayload.forEach(card => resultsById[card.multiverseId] = card);

        const newState: CardSearchDataReducerState = {
            ...state,
            searchResults: {
                ...state.searchResults,
                isLoading: false,
                searchResultsById: resultsById,
                allSearchResultIds: searchResultPayload.map(card => card.multiverseId),
            }
        }

        return newState;
    }
    else return (state);
}

const cardSearchAddPendingCard = (state = initialState, action: ReduxAction): CardSearchDataReducerState => {
    //'pending cards' is now a dictionary of 'pending card dto's
    const {
        data, //0
        isFoil, //false
        variant, //"normal"
    } = action.payload;
    
    const magicCardToAdd: MagicCard = data;
    
    let cardToAdd: PendingCardsDto = state.pendingCards[data.multiverseId];
    
    if(!cardToAdd){
        cardToAdd = {
            multiverseId: magicCardToAdd.multiverseId,
            name: magicCardToAdd.name,
            cards: [],
        };

        //OK - So I need to either pass the name, or the whole card
        
        
        // const magicCardToAdd = state.
        // if we can't find a matching card, nothing gets added and this just continues silently (I guess)
    //     if(magicCardToAdd){
    //         cardToAdd = {
    //             multiverseId: multiverseId,
    //             cards: [],
    //             // data: magicCardToAdd,
    //         };
    //     }                    
    }

    cardToAdd.cards.push({
        multiverseId: magicCardToAdd.multiverseId, 
        isFoil: isFoil,
        variantName: variant,
        statusId: 1,
    } as InventoryCard);

    const newState: CardSearchDataReducerState = {
        ...state,
        pendingCards: {
            ...state.pendingCards,
            [magicCardToAdd.multiverseId]: cardToAdd
        }
    }
    return newState;
}

const cardSearchRemovePendingCard = (state = initialState, action: ReduxAction): CardSearchDataReducerState => {
    // const {
                // } = action.payload;

    const midToRemove = action.payload.multiverseId;
    const removeFoilCard = action.payload.isFoil;
    const variantToRemove: string = action.payload.variant;

    let objToRemoveFrom = state.pendingCards[midToRemove];

    if(objToRemoveFrom){

        let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.variantName === variantToRemove && x.isFoil === removeFoilCard);

        if(thisInvCard >= 0){
            objToRemoveFrom.cards.splice(thisInvCard,1);
        }

        let pendingCardsAfterRemoval =  {
            ...state.pendingCards,
            [midToRemove]: objToRemoveFrom
        }
        //if this pending cards object has 0 items, it should be deleted from the dictionary
        if(objToRemoveFrom.cards.length === 0){
            delete pendingCardsAfterRemoval[midToRemove];
        }
        const newState: CardSearchDataReducerState = {
            ...state,
            pendingCards: pendingCardsAfterRemoval
        }
        return newState;

    } else {
        const newState: CardSearchDataReducerState = {
            ...state,
        }
        return newState;
    }
}

export const cardSearchDataReducer = (state = initialState, action: ReduxAction): CardSearchDataReducerState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);

        case CARD_SEARCH_ADD_PENDING_CARD:
            return cardSearchAddPendingCard(state, action);
            
        case CARD_SEARCH_REMOVE_PENDING_CARD:
            return cardSearchRemovePendingCard(state, action);
    
        case CARD_SEARCH_CLEAR_PENDING_CARDS:
            return {
                ...state,
                
                pendingCards: {},
            }// as cardSearchPendingCardsState;

        case INVENTORY_ADD_COMPLETE:
            return {
                ...state,
                pendingCards: {},
                // pendingCardsSaving: false,
            } //as cardSearchPendingCardsState;

        default:
            return(state)
    }
}

const initialState: CardSearchDataReducerState = {
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
    pendingCards: {},
}