import {
    TRIMMING_TOOL_FILTER_CHANGED,
    TRIMMING_TOOL_CARDS_REQUESTED,
    TRIMMING_TOOL_CARDS_RECEIVED,
    CARD_IMAGE_ANCHOR_SET,
    ADD_PENDING_CARD,
    REMOVE_PENDING_CARD,
    CLEAR_PENDING_CARDS,
} from './TrimmingToolActions';

export interface State {
    searchProps: TrimmingToolRequest;
    
    searchResults: {
        isLoading: boolean;
        byId: { [id: number]: InventoryOverviewDto };
        allIds: number[];
    };

    pendingCards: {
        isSaving: boolean;
        byId: { [cardId: number]: TrimmedCard };
        allIds: number[];
    }

    cardImageMenuAnchor: HTMLButtonElement | null;
}

export const trimmingToolReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case TRIMMING_TOOL_FILTER_CHANGED: return filterChanged(state, action);
        case TRIMMING_TOOL_CARDS_REQUESTED: return searchRequested(state, action);
        case TRIMMING_TOOL_CARDS_RECEIVED: return searchReceived(state, action);

        case CARD_IMAGE_ANCHOR_SET: return { ...state, cardImageMenuAnchor: action.payload }

        case ADD_PENDING_CARD: return addPendingCard(state, action);
        case REMOVE_PENDING_CARD: return removePendingCard(state, action);

        case CLEAR_PENDING_CARDS: return {...state, pendingCards: { byId: {}, allIds: [], isSaving: false }}

        // case SAVE_REQUESTED: return { ...state, pendingCardsSaving: true };
        // case SAVE_COMPLETED: return { ...state, pendingCardsSaving: false, pendingCards: {} };
        default: return(state);
    }
}

const initialState: State = {
    searchProps: {
        setCode: 'znr',
        searchGroup: 'Red',
        minCount: 11,
        // minBy: '',
    },

    searchResults: {
        isLoading: false,
        byId: {},
        allIds: [],
    },


    
    pendingCards: {
        isSaving: false,
        //If this is a dictionary built on CardId, this will have to store both foil & non-foil counts
        byId: {},
        allIds: [],
    },
    
    cardImageMenuAnchor: null,
};

function searchRequested(state: State, action: ReduxAction): State {
    const newState: State = {
        ...state,
        searchResults: {
            ...initialState.searchResults,
            isLoading: true,
        }
    }
    return newState;
}

function searchReceived(state: State, action: ReduxAction): State {
    const searchResultPayload: InventoryOverviewDto[] = action.payload || [];
    let resultsById = {};
    searchResultPayload.forEach(card => resultsById[card.id] = card);
    const newState: State = {
        ...state,
        searchResults: {
            ...state.searchResults,
            isLoading: false,
            byId: resultsById,
            allIds: searchResultPayload.map(card => card.id),
        }
    }
    return newState;
}

const addPendingCard = (state = initialState, action: ReduxAction): State => {
    const {
        name,
        cardId,
        isFoil,
    } = action.payload;

    let cardToAdd = state.pendingCards.byId[cardId];
    let cardIds = state.pendingCards.allIds;
    
    if(!cardToAdd){
        cardToAdd = {
            cardName: name,
            cardId: cardId,
            numberToTrim: 0,
            foilToTrim: 0,
        };
        cardIds.push(cardId);
    }

    isFoil ? cardToAdd.foilToTrim++ : cardToAdd.numberToTrim++;

    const newState: State = {
        ...state,
        pendingCards: {
            ...state.pendingCards,
            allIds: cardIds,
            byId: {
                ...state.pendingCards.byId,
                [cardId]: cardToAdd,
            }
        }
    }
    
    return newState;
}

const removePendingCard = (state = initialState, action: ReduxAction): State => {
    const {
        name,
        cardId,
        isFoil,
    } = action.payload;

    let objToRemoveFrom = state.pendingCards.byId[cardId];

    if(objToRemoveFrom){

        // let thisInvCardIndex = objToRemoveFrom.cards.findIndex(x => x.cardId === cardId && x.isFoil === isFoil);

        // if(thisInvCardIndex >= 0){
        //     objToRemoveFrom.cards.splice(thisInvCardIndex,1);
        // }

        isFoil ? objToRemoveFrom.foilToTrim-- : objToRemoveFrom.numberToTrim--;

        let pendingCardsAfterRemoval =  {
            ...state.pendingCards.byId,
            [cardId]: objToRemoveFrom
        }

        const pendingCardIds = state.pendingCards.allIds;

        //if this pending cards object has 0 items, it should be deleted from the dictionary
        if(objToRemoveFrom.numberToTrim === 0 && objToRemoveFrom.foilToTrim === 0){
            delete pendingCardsAfterRemoval[cardId];
            
            const idIndex = pendingCardIds.findIndex(x => x === cardId);
            pendingCardIds.splice(idIndex,1);
        }

        const newState: State = {
            ...state,
            pendingCards: {
                ...state.pendingCards,
                byId: pendingCardsAfterRemoval
            }
        }
        return newState;

    } else {
        const newState: State = {
            ...state,
        }
        return newState;
    }
}

const filterChanged = (state: State, action: ReduxAction): State => {
    const { filter, value } = action.payload;
    const newState: State = {
        ...state,
        searchProps: {
            ...state.searchProps,
            [filter]: value,
        }
    }
    return newState;
}