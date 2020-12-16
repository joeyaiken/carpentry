// import { DECK_DETAIL_REQUESTED, DECK_DETAIL_RECEIVED } from "../../state/decksDataActions";
import { 
    CARD_DETAIL_RECEIVED,
    CARD_DETAIL_REQUESTED,
} from "./CardDetailActions";



export interface State {

    //
    //new things to consider
    //  The goal is to not re-map too many groupings in containers
    //

    isLoading: boolean;
    activeCardId: number;
    activeCardName: string;

    // deckCards: { //deck cards matching this selected name

    // }

    inventoryCards: {
        byId: { [id: number]: InventoryCard };
        allIds: number[];
    }

    cards:{ //card definitions
        byId: { [multiverseId: number]: MagicCard };
        allIds: number[];
    }

    //yeah yeah this could also be its own obj
    cardGroups: { [cardId: number]: number[] }
    // cardGroupIds: number[]; //card group ids === cards.allIds




    //
    //existing things
    //

    // cardSearchMethod: "set" | "web" | "inventory";
    // selectedCard: CardSearchResultDto | null;
    // viewMode: CardSearchViewMode;
    // searchFilterProps: CardFilterProps;
    // searchResults: {
    //     isLoading: boolean;
        
    //     searchResultsById: {[multiverseId: number]: CardSearchResultDto};
    //     allSearchResultIds: number[];

    //     // selectedCard: MagicCard | null; //should probably be an AppState ID
    //     //what if this just used the other reducer?...
    //     // inventoryDetail: InventoryDetailDto | null;
    // };
    // inventoryDetail: {
    //     isLoading: boolean;
    //     activeCardId: number;

    //     //inventory cards
    //     inventoryCardsById: { [id: number]: InventoryCard };
    //     inventoryCardAllIds: number[];


    //     //--First step in showing an inventory detail is to itterate over MagicCard (or even card variant)
    //     //--Then would need to show each inventory card for a given magic card
    //     //      (all inventory cards where I.multiverseId === MC.multiverseId)

    //     //magic cards belonging to inventory cards
    //     cardsById: { [multiverseId: number]: MagicCard };
    //     allCardIds: number[];
    // };
}

export const cardDetailReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case CARD_DETAIL_REQUESTED: return cardDetailRequested(state, action);
        case CARD_DETAIL_RECEIVED: return cardDetailReceived(state, action);
        // case DECK_DETAIL_REQUESTED: return state;
        // case DECK_DETAIL_RECEIVED: return state;
        // case CARD_SEARCH_REQUESTED: return cardSearchRequested(state, action);
        // case CARD_SEARCH_RECEIVED: return cardSearchReceived(state, action);
        // case CARD_SEARCH_INVENTORY_REQUESTED: return cardSearchInventoryRequested(state, action);
        // case CARD_SEARCH_INVENTORY_RECEIVED: return cardSearchInventoryReceived(state, action);
        // case CARD_SEARCH_FILTER_VALUE_CHANGED: return filterValueChanged(state, action);
        // case TOGGLE_CARD_SEARCH_VIEW_MODE: return toggleSearchViewMode(state, action);

        // // case CARD_SEARCH_SEARCH_METHOD_CHANGED: return (state);
        
        // // case CARD_SEARCH_SELECT_CARD: return (state);
        // case CARD_SEARCH_SELECT_CARD:
        //     const selectedCard: CardSearchResultDto = action.payload;
        //     return {
        //         ...state,
        //         selectedCard: selectedCard,
        //     };
        
        // case CARD_SEARCH_ADDING_DECK_CARD: return (state);
        
        default: return(state);
    }
}

const initialState: State = {
    // cardSearchMethod: "set",
    // selectedCard: null,
    // viewMode: "list",
    // searchFilterProps: defaultSearchFilterProps(),
    // searchResults: {
    //     isLoading: false,
    //     searchResultsById: {},
    //     allSearchResultIds: [],
    // },

    isLoading: false,
    activeCardId: 0,
    activeCardName: '',
    inventoryCards: {
        byId: {},
        allIds: [],
    },
    cards: {
        byId: {},
        allIds: [],
    },
    cardGroups: {},
    // cardGroupIds: []
    // inventoryDetail: {
    //     isLoading: false,
    //     activeCardId: 0,
    //     inventoryCardsById: {},
    //     inventoryCardAllIds: [],
    //     cardsById: {},
    //     allCardIds: [],
    // },
}

// function defaultSearchFilterProps(): CardFilterProps {
//     return {
//         // setId: null,
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


// function cardSearchRequested(state: State, action: ReduxAction): State {
//     const newState: State = {
//         ...state,
//         searchResults: {
//             ...initialState.searchResults,
//             isLoading: true,
//         }
//     }
//     return newState;
// }

// function cardSearchReceived(state: State, action: ReduxAction): State {
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

function cardDetailRequested(state: State, action: ReduxAction): State {
    const newState: State = {
        // ...state, //should I even reapply this?...
        ...initialState,
        activeCardId: state.activeCardId,
        isLoading: true,
        
    };
    return newState;
}



// inventoryCards: {
//     byId: { [id: number]: InventoryCard };
//     allIds: number[];
// }

// cards:{
//     byId: { [multiverseId: number]: MagicCard };
//     allIds: number[];
// }

// //yeah yeah this could also be its own obj
// cardGroups: { [cardId: number]: number[] }
// cardGroupIds: number[];





function cardDetailReceived(state: State, action: ReduxAction): State {
    const detailResult: InventoryDetailDto = action.payload;

    let inventoryCardsById: { [id: number]: InventoryCard } = {};
    detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);
    const allInventoryCardIds = detailResult.inventoryCards.map(card => card.id);

    let cardsById: { [cardId: number]: MagicCard } = {};
    detailResult.cards.forEach(card => cardsById[card.cardId] = card);
    const allCardIds = detailResult.cards.map(card => card.cardId);

    let cardGroups: { [cardId: number]: number[] } = {};
    // let cardGroupIds: number[];
    allCardIds.forEach(cardId => {
        const thisCard = cardsById[cardId];
        // cardGroupIds.push()
        const inventoryCardIds = allInventoryCardIds
            .filter(inventoryCardId => inventoryCardsById[inventoryCardId].set === thisCard.set 
                && inventoryCardsById[inventoryCardId].collectorNumber === thisCard.collectionNumber);
        cardGroups[cardId] = inventoryCardIds;
    });


    const newState: State = {
        ...state,
        isLoading: false,
        activeCardId: detailResult.cardId,
        activeCardName: detailResult.name,
        inventoryCards: {
            byId: inventoryCardsById,
            allIds: allInventoryCardIds,
        },
        cards: {
            byId: cardsById,
            allIds: allCardIds,
        },

        cardGroups: cardGroups,
        // cardGroupIds: [],

    };
    
    return newState;
}

// const filterValueChanged = (state: State, action: ReduxAction): State => {
//     //const { type, filter, value } = action.payload;
//     const { filter, value } = action.payload;
//     // console.log('CS filter changed');
//     // console.log(action.payload);

//     const existingFilter = state.searchFilterProps;
//     const newState: State = {
//         ...state,
//         searchFilterProps: {
//             ...existingFilter, 
//             [filter]: value,
//         }
//     }
//     return newState;
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
