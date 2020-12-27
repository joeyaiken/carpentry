// import { 
//     CARD_DETAIL_RECEIVED,
//     CARD_DETAIL_REQUESTED,
//     DECK_CARD_MENU_BUTTON_CLICKED,
//     INVENTORY_CARD_MENU_BUTTON_CLICKED,
// } from "./CardDetailActions";

export interface State {
    // isLoading: boolean;
    // activeCardId: number;
    // activeCardName: string;

    // // deckCards: { //deck cards matching this selected name
    // // }

    // inventoryCards: {
    //     byId: { [id: number]: InventoryCard };
    //     allIds: number[];
    // }

    // cards:{ //card definitions
    //     byId: { [multiverseId: number]: MagicCard };
    //     allIds: number[];
    // }

    // cardGroups: { [cardId: number]: number[] }

    // deckCardMenuAnchor: HTMLButtonElement | null;
    // deckCardMenuAnchorId: number;
    // inventoryCardMenuAnchor: HTMLButtonElement | null;
    // inventoryCardMenuAnchorId: number;
}

export const cardDetailReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        // case CARD_DETAIL_REQUESTED: return cardDetailRequested(state, action);
        // case CARD_DETAIL_RECEIVED: return cardDetailReceived(state, action);
        // case DECK_CARD_MENU_BUTTON_CLICKED: return { ...state, deckCardMenuAnchor: action.payload, deckCardMenuAnchorId: parseInt(action.payload?.value) };
        // case INVENTORY_CARD_MENU_BUTTON_CLICKED: return { ...state, inventoryCardMenuAnchor: action.payload, inventoryCardMenuAnchorId: parseInt(action.payload?.value) };
        default: return(state);
    }
}

const initialState: State = {
    // isLoading: false,
    // activeCardId: 0,
    // activeCardName: '',
    // inventoryCards: {
    //     byId: {},
    //     allIds: [],
    // },
    // cards: {
    //     byId: {},
    //     allIds: [],
    // },
    // cardGroups: {},

    // deckCardMenuAnchor: null,
    // deckCardMenuAnchorId: 0,
    // inventoryCardMenuAnchor: null,
    // inventoryCardMenuAnchorId: 0,
}

// function cardDetailRequested(state: State, action: ReduxAction): State {
//     const newState: State = {
//         ...initialState,
//         activeCardId: state.activeCardId,
//         isLoading: true,
        
//     };
//     return newState;
// }

// function cardDetailReceived(state: State, action: ReduxAction): State {
//     const detailResult: InventoryDetailDto = action.payload;

//     let inventoryCardsById: { [id: number]: InventoryCard } = {};
//     detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);
//     const allInventoryCardIds = detailResult.inventoryCards.map(card => card.id);

//     let cardsById: { [cardId: number]: MagicCard } = {};
//     detailResult.cards.forEach(card => cardsById[card.cardId] = card);
//     const allCardIds = detailResult.cards.map(card => card.cardId);

//     let cardGroups: { [cardId: number]: number[] } = {};
    
//     allCardIds.forEach(cardId => {
//         const thisCard = cardsById[cardId];
        
//         const inventoryCardIds = allInventoryCardIds
//             .filter(inventoryCardId => inventoryCardsById[inventoryCardId].set === thisCard.set 
//                 && inventoryCardsById[inventoryCardId].collectorNumber === thisCard.collectionNumber);
//         cardGroups[cardId] = inventoryCardIds;
//     });

//     const newState: State = {
//         ...state,
//         isLoading: false,
//         activeCardId: detailResult.cardId,
//         activeCardName: detailResult.name,
//         inventoryCards: {
//             byId: inventoryCardsById,
//             allIds: allInventoryCardIds,
//         },
//         cards: {
//             byId: cardsById,
//             allIds: allCardIds,
//         },
//         cardGroups: cardGroups,
//     };
    
//     return newState;
// }