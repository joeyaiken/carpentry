import {
    CARD_SEARCH_FILTER_VALUE_CHANGED,
    TOGGLE_CARD_SEARCH_VIEW_MODE,
    // CARD_SEARCH_SEARCH_METHOD_CHANGED,
    CARD_SEARCH_SELECT_CARD,
    CARD_SEARCH_ADDING_DECK_CARD,
    CARD_SEARCH_REQUESTED,
    CARD_SEARCH_RECEIVED,
    CARD_SEARCH_INVENTORY_REQUESTED,
    CARD_SEARCH_INVENTORY_RECEIVED, CARD_SEARCH_DECK_CARD_ADDED,
} from "./DeckAddCardsActions";

export interface State {
    // cardSearchMethod: "set" | "web" | "inventory";
    selectedCard: CardSearchResultDto | null;
    viewMode: CardSearchViewMode;
    searchFilterProps: CardFilterProps;
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
        inventoryCardsAllIds: number[];


        //--First step in showing an inventory detail is to itterate over MagicCard (or even card variant)
        //--Then would need to show each inventory card for a given magic card
        //      (all inventory cards where I.multiverseId === MC.multiverseId)

        //magic cards belonging to inventory cards
        cardsById: { [multiverseId: number]: MagicCard };
        allCardIds: number[];
    };
    addCardIsSaving: boolean;
}

export const deckAddCardsReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case CARD_SEARCH_REQUESTED: return cardSearchRequested(state, action);
        case CARD_SEARCH_RECEIVED: return cardSearchReceived(state, action);
        case CARD_SEARCH_INVENTORY_REQUESTED: return cardSearchInventoryRequested(state, action);
        case CARD_SEARCH_INVENTORY_RECEIVED: return cardSearchInventoryReceived(state, action);
        case CARD_SEARCH_FILTER_VALUE_CHANGED: return filterValueChanged(state, action);
        case TOGGLE_CARD_SEARCH_VIEW_MODE: return toggleSearchViewMode(state, action);

        // case CARD_SEARCH_SEARCH_METHOD_CHANGED: return (state);
        
        // case CARD_SEARCH_SELECT_CARD: return (state);
        case CARD_SEARCH_SELECT_CARD:
            const selectedCard: CardSearchResultDto = action.payload;
            return {
                ...state,
                selectedCard: selectedCard,
            };
        
        case CARD_SEARCH_ADDING_DECK_CARD:
            return {
                ...state,
                addCardIsSaving: true,
            };
        
        case CARD_SEARCH_DECK_CARD_ADDED: 
            return {
                ...state,
                addCardIsSaving: false,
            };
            
        
        default: return(state);
    }
}

const initialState: State = {
    // cardSearchMethod: "set",
    selectedCard: null,
    viewMode: "list",
    searchFilterProps: defaultSearchFilterProps(),
    searchResults: {
        isLoading: false,
        searchResultsById: {},
        allSearchResultIds: [],
    },
    inventoryDetail: {
        isLoading: false,
        inventoryCardsById: {},
        inventoryCardsAllIds: [],
        cardsById: {},
        allCardIds: [],
    },
    addCardIsSaving: false,
}

function defaultSearchFilterProps(): CardFilterProps {
    return {
        // setId: null,
        set: '',
        colorIdentity: [],
        //rarity: ['mythic','rare','uncommon','common'], //
        rarity: [], //
        type: '',//'Creature',
        exclusiveColorFilters: false,
        multiColorOnly: false,
        cardName: '',
        exclusiveName: false,
        maxCount: 0,
        minCount: 0,
        format: '',
        text: '',
        group: '',
    }
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
            inventoryCardsAllIds: detailResult.inventoryCards.map(card => card.id),
            cardsById: cardsById,
            allCardIds: detailResult.cards.map(card => card.multiverseId),
        },
    };
    
    return newState;
}

const filterValueChanged = (state: State, action: ReduxAction): State => {
    //const { type, filter, value } = action.payload;
    const { filter, value } = action.payload;
    // console.log('CS filter changed');
    // console.log(action.payload);

    const existingFilter = state.searchFilterProps;
    const newState: State = {
        ...state,
        searchFilterProps: {
            ...existingFilter, 
            [filter]: value,
        }
    }
    return newState;
}

function toggleSearchViewMode(state: State, action: ReduxAction): State {
    let newViewMode: CardSearchViewMode = "list";

    switch(state.viewMode){
        case "list":
            newViewMode = "grid";
            break;
        case "grid":
            newViewMode = "list";
            break;
    }
    
    return {
        ...state,
        viewMode: newViewMode,
    };
}
