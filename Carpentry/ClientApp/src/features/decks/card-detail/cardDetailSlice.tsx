import { createSlice } from '@reduxjs/toolkit';
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
import {DECK_CARD_MENU_BUTTON_CLICKED, INVENTORY_CARD_MENU_BUTTON_CLICKED} from "./state/CardDetailActions";

export interface State {
  isLoading: boolean;
  activeCardId: number;
  activeCardName: string;

  inventoryCards: {
    byId: { [id: number]: InventoryCard };
    allIds: number[];
  }

  cards:{
    byId: { [multiverseId: number]: MagicCard };
    allIds: number[];
  }

  cardGroups: { [cardId: number]: number[] }

  deckCardMenuAnchor: HTMLButtonElement | null;
  deckCardMenuAnchorId: number;
  inventoryCardMenuAnchor: HTMLButtonElement | null;
  inventoryCardMenuAnchorId: number;
}

const initialState: State = {
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

  deckCardMenuAnchor: null,
  deckCardMenuAnchorId: 0,
  inventoryCardMenuAnchor: null,
  inventoryCardMenuAnchorId: 0,
}

export const cardDetailSlice = createSlice({
  name: 'cardDetail',
  initialState: {
    
  },
  reducers: {
    cardDetailRequested: (state: State, action: ReduxAction) => {
      //state = initialState;
      state.isLoading = true;
    },

//     function cardDetailReceived(state: State, action: ReduxAction): State {
//       const detailResult: InventoryDetailDto = action.payload;
//    
//       let inventoryCardsById: { [id: number]: InventoryCard } = {};
//       detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);
//       const allInventoryCardIds = detailResult.inventoryCards.map(card => card.id);
//    
//       let cardsById: { [cardId: number]: MagicCard } = {};
//       detailResult.cards.forEach(card => cardsById[card.cardId] = card);
//       const allCardIds = detailResult.cards.map(card => card.cardId);
//    
//       let cardGroups: { [cardId: number]: number[] } = {};
//    
//       allCardIds.forEach(cardId => {
//         const thisCard = cardsById[cardId];
//    
//         const inventoryCardIds = allInventoryCardIds
//           .filter(inventoryCardId => inventoryCardsById[inventoryCardId].set === thisCard.set
//             && inventoryCardsById[inventoryCardId].collectorNumber === thisCard.collectionNumber);
//         cardGroups[cardId] = inventoryCardIds;
//       });
//    
//       const newState: State = {
//         ...state,
//         isLoading: false,
//         activeCardId: detailResult.cardId,
//         activeCardName: detailResult.name,
//         inventoryCards: {
//           byId: inventoryCardsById,
//           allIds: allInventoryCardIds,
//         },
//         cards: {
//           byId: cardsById,
//           allIds: allCardIds,
//         },
//         cardGroups: cardGroups,
//       };
//    
//       return newState;
//     }

// case DECK_CARD_MENU_BUTTON_CLICKED: return { ...state, deckCardMenuAnchor: action.payload, deckCardMenuAnchorId: parseInt(action.payload?.value) };
    deckCardMenuButtonClicked: (state: State, action: PayloadAction<HTMLButtonElement>) => {
      state.deckCardMenuAnchor = action.payload;
      state.deckCardMenuAnchorId = parseInt(action.payload?.value);
    },

// case INVENTORY_CARD_MENU_BUTTON_CLICKED: return { ...state, inventoryCardMenuAnchor: action.payload, inventoryCardMenuAnchorId: parseInt(action.payload?.value) };
    
  },
});

export default cardDetailSlice.reducer;