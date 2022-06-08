import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import {ApiStatus} from "../../../enums";
import {PayloadAction} from "@reduxjs/toolkit/dist/createAction";
import {inventoryApi} from "../../../api/inventoryApi";

export interface State {
  searchResults: {
    status: ApiStatus;
    byId: { [id: number]: TrimmingToolResult };
    allIds: number[];
  };
  pendingCards: {
    status: ApiStatus;
    byId: { [cardId: number]: TrimmedCard };
  }
}

const initialState: State = {
  searchResults: {
    status: ApiStatus.uninitialized,
    byId: {},
    allIds: [],
  },
  pendingCards: {
    status: ApiStatus.uninitialized,
    byId: {},
  },
};

export const loadTrimmingToolCards = createAsyncThunk<TrimmingToolResult[], TrimmingToolRequest>(
  'trimmingTool/loadTrimmingToolCards',
  async (searchProps) => 
    inventoryApi.getTrimmingToolCards(searchProps)
);

export const trimCards = createAsyncThunk<void, TrimmedCardDto[]>(
  'trimmingTool/trimCards',
  async (cardsToTrim, thunkApi) => {
    await inventoryApi.trimCards(cardsToTrim);
    // After the save has completed, we want to clear pending cards & re-query for trimming tool cards
    thunkApi.dispatch(clearPendingCards());
  }
);

export const trimmingToolSlice = createSlice({
  name: 'trimmingTool',
  initialState: initialState,
  reducers: {
    addPendingCard: (state, action: PayloadAction<{
      card: TrimmingToolResult,
      count: number,
    }>) => {
      const cardResult = action.payload.card;
      let pendingCard = state.pendingCards.byId[cardResult.cardId];
      if(!pendingCard?.data){
        pendingCard = {
          data: cardResult,
          numberToTrim: 0,
        };
      }
      pendingCard.numberToTrim += action.payload.count;

      // TODO - figure out if this line is even necessary
      state.pendingCards.byId[cardResult.cardId] = pendingCard
    },
    
    removePendingCard: (state, action: PayloadAction<TrimmingToolResult>) => {
      const cardResult = action.payload;
      let pendingCard = state.pendingCards.byId[cardResult.cardId];
      if(pendingCard) {
        pendingCard.numberToTrim--;
        if(pendingCard.numberToTrim === 0) {
          delete state.pendingCards[cardResult.cardId];
        }
      }
    },
    
    clearPendingCards: (state) => {
      state.pendingCards.byId = {};
      // Would this break anything? The idea is to re-search after cards are trimmed
      state.searchResults.status = ApiStatus.uninitialized;
    }
  },
  extraReducers: (builder) => {
    builder.addCase(loadTrimmingToolCards.pending, (state, action) => {
      state.searchResults.status = ApiStatus.loading;
    });
    builder.addCase(loadTrimmingToolCards.fulfilled, (state, action) => {
      let resultsById = {};
      const allIds = action.payload.map(card => {
        resultsById[card.id] = card;
        return card.id;
      });
      state.searchResults.byId = resultsById;
      state.searchResults.allIds = allIds;
      state.searchResults.status = ApiStatus.initialized;
    });
    builder.addCase(loadTrimmingToolCards.rejected, (state, action) => {
      console.error('loadTrimmingToolCards thunk rejected: ', action);
      state.searchResults.status = ApiStatus.errored;
    });
    
    builder.addCase(trimCards.pending, (state, action) => {
      state.pendingCards.status = ApiStatus.loading;
    });
    builder.addCase(trimCards.fulfilled, (state, action) => {
      state.pendingCards.byId = {};
      state.pendingCards.status = ApiStatus.initialized;
    });
    builder.addCase(trimCards.rejected, (state, action) => {
      console.error('trimCards thunk rejected: ', action);
      state.pendingCards.status = ApiStatus.errored;
    });
  },
});

export const {addPendingCard, removePendingCard, clearPendingCards} = trimmingToolSlice.actions;

export default trimmingToolSlice.reducer;