import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import {PayloadAction} from '@reduxjs/toolkit/dist/createAction';
import {RootState} from "../../../configureStore";
import {ApiStatus} from "../../../enums";
import {cardSearchApi} from "../../../api/cardSearchApi";
import {inventoryApi} from "../../../api/inventoryApi";

export interface State {
  searchResults: {
    status: ApiStatus;
    searchResultsById: {[multiverseId: number]: CardSearchResultDto};
    allSearchResultIds: number[];
  };

  viewMode: CardSearchViewMode;
  
  pendingCards: {
    status: ApiStatus,
    byName: { [name: string]: PendingCardsDto }
  }
  
  selectedCard: CardSearchResultDto | null; //should probably be an AppState ID, not a full model
}

const initialState: State = {
  searchResults: {
    status: ApiStatus.uninitialized,
    searchResultsById: {},
    allSearchResultIds: [],
  },
  viewMode: "list",
  selectedCard:  null, //should probably be an AppState ID
  pendingCards: {
    status: ApiStatus.initialized,
    // should an AllNames array exist here to control sorting?...
    byName: {},
  },
};

export const searchCards = createAsyncThunk<CardSearchResultDto[], CardFilterProps>(
  'inventoryAddCards/searchCards',
  async (currentFilterProps) => {
    const param: CardSearchQueryParameter = {
      text: currentFilterProps.text,
      colorIdentity: currentFilterProps.colorIdentity,
      exclusiveColorFilters: currentFilterProps.exclusiveColorFilters,
      multiColorOnly: currentFilterProps.multiColorOnly,
      rarity: currentFilterProps.rarity,
      set: currentFilterProps.set,
      type: currentFilterProps.type,
      searchGroup: currentFilterProps.group,
      excludeUnowned: false,
    }
    return cardSearchApi.searchInventory(param);
  }
)

export const savePendingCards = createAsyncThunk<void>(
  'inventoryAddCards/savePendingCards',
  async (_, thunkApi) => { 
    
    const state = thunkApi.getState() as RootState;
    
    let newCards: InventoryCard[] = [];
    
    const pendingCards = state.inventory.inventoryAddCards.pendingCards;
    Object.keys(pendingCards).forEach(key => {
      let itemToAdd: PendingCardsDto = pendingCards[key];
      itemToAdd.cards.forEach(card => {
        const newCard: InventoryCard = {
          id: 0,
          isFoil: card.isFoil,
          statusId: card.statusId,
          cardId: card.cardId,
          collectorNumber: card.collectorNumber,
          name: card.name,
          set: card.set,
          deckCardCategory: null,
          deckCardId: null,
          deckId: null,
          deckName: null,
        }
        newCards.push(newCard);
      });
    });
    
    await inventoryApi.addInventoryCardBatch(newCards);
  }
)

export const inventoryAddCardsSlice = createSlice({
  name: 'inventoryAddCards',
  initialState: initialState,
  reducers: {
    cardSearchSelectCard: (state, action: PayloadAction<CardSearchResultDto>) => {
      state.selectedCard = action.payload;
    },

    // TODO - replace untyped payload with an actual type
    addPendingCard: (state, action: PayloadAction<{
      name: string,
      cardId: number,
      isFoil: boolean,
    }>) => {
      const {
        name,
        cardId,
        isFoil,
      } = action.payload;
    
      console.log('add pending card, initial state', state);
      
      let pendingCard: PendingCardsDto = state.pendingCards[name];
    
      if(!pendingCard){
        pendingCard = {
          name: name,
          cards: [],
        };
      }

      // These are the only 3 fields used by the api bulkAdd
      pendingCard.cards.push({
        cardId: cardId,
        isFoil: isFoil,
        statusId: 1,
      } as InventoryCard);
      
      state.pendingCards.byName[name] = pendingCard;
      
    },

    // TODO - replace untyped payload with an actual type
    removePendingCard: (state, action: PayloadAction<{
      name: string,
      cardId: number,
      isFoil: boolean,
    }>) => {
      const {
        name,
        cardId,
        isFoil,
      } = action.payload;

      let pendingCardToRemoveFrom = state.pendingCards[name];

      if(pendingCardToRemoveFrom){
        let thisInvCard = pendingCardToRemoveFrom.cards.findIndex(x => x.cardId === cardId && x.isFoil === isFoil);
        if(thisInvCard >= 0){
          pendingCardToRemoveFrom.cards.splice(thisInvCard,1);
        }

        // This might just happen through connections...
        state.pendingCards.byName[name] = pendingCardToRemoveFrom;

        // if this pending cards object has 0 items, it should be deleted from the dictionary
        if(pendingCardToRemoveFrom.cards.length === 0){
          delete state.pendingCards.byName[name];
        }
      }
    },
    
    clearPendingCards: (state) => {
      state.pendingCards.byName = {};
    },

    toggleSearchViewMode: (state) => {
      state.viewMode = (state.viewMode === "list") ? "grid" : "list";
    },

  },
  extraReducers: (builder) => {
    // Search for cards
    builder.addCase(searchCards.pending, (state) => {
      state.searchResults.searchResultsById = {};
      state.searchResults.allSearchResultIds = [];
      state.searchResults.status = ApiStatus.loading;
    });
    builder.addCase(searchCards.fulfilled, (state, action) => {
      let resultsById = {};
      state.searchResults.allSearchResultIds = action.payload.map(card => {
        resultsById[card.cardId] = card;
        return card.cardId;
      });
      state.searchResults.searchResultsById = resultsById;
      state.searchResults.status = ApiStatus.initialized;
    });
    builder.addCase(searchCards.rejected, (state, action) => {
      console.error('searchCards thunk rejected: ', action);
      state.searchResults.status = ApiStatus.errored;
    });

    // Save pending cards
    builder.addCase(savePendingCards.pending, (state) => {
      state.pendingCards.status = ApiStatus.loading;
    });
    builder.addCase(savePendingCards.fulfilled, (state) => {
      state.pendingCards.status = ApiStatus.initialized;
      state.pendingCards.byName = {};
    });
    builder.addCase(savePendingCards.rejected, (state, action) => {
      console.error('savePendingCards thunk rejected: ', action);
      state.pendingCards.status = ApiStatus.errored;
    });
  }
});

// This is probably a bad selector
export const selectSearchResultItem = (state: RootState, cardId: number): CardListItem => {
  const card = state.inventory.inventoryAddCards.searchResults.searchResultsById[cardId];
  const pendingCard = state.inventory.inventoryAddCards.pendingCards[card.name]
  return {
    data: card,
    count: pendingCard && pendingCard.cards.length,
  } as CardListItem;
}

// const selectSearchResults = (state: RootState): CardListItem[] => {
//   const { allSearchResultIds, searchResultsById } = state.inventory.inventoryAddCards.searchResults;
//   const containerState = state.inventory.inventoryAddCards;
//  
//   return allSearchResultIds.map(cardId => {
//     const card = searchResultsById[cardId];
//     return {
//       data: card,
//       count: containerState.pendingCards[card.name] && containerState.pendingCards[card.name].cards.length,
//     } as CardListItem;
//    
//   });
// }

export const {
  cardSearchSelectCard,
  addPendingCard,
  removePendingCard,
  clearPendingCards,
  toggleSearchViewMode,
} = inventoryAddCardsSlice.actions;

export default inventoryAddCardsSlice.reducer;