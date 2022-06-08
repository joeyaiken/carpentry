import {ApiStatus} from "../../../enums";
import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {decksApi} from "../../../api/decksApi";

export interface State {
  data: {
    status: ApiStatus,
    decksById: { [id: number]: DeckOverviewDto };
    deckIds: number[];
  }
}

const initialState: State = {
  data: {
    status: ApiStatus.uninitialized,
    decksById: {},
    deckIds: [],
  }
}

export const loadDeckOverviews = createAsyncThunk<DeckOverviewDto[]>(
  'decksOverview/loadDeckOverviews',
  async () => await decksApi.getDeckOverviews()
);

export const deckListSlice = createSlice({
  name: 'deckList',
  initialState: initialState,
  reducers: { },
  extraReducers: (builder) => {
    builder.addCase(loadDeckOverviews.pending, (state) => {
      state.data.decksById = {};
      state.data.deckIds = [];
      state.data.status = ApiStatus.loading;
    });

    builder.addCase(loadDeckOverviews.fulfilled, (state, action) => {
      const apiDecks = action.payload;
      
      let decksById: { [key:number]: DeckOverviewDto } = {};
      const deckIds = apiDecks.map(deck => {
        decksById[deck.id] = deck;
        return deck.id;
      });
      
      state.data.decksById = decksById;
      state.data.deckIds = deckIds;
      state.data.status = ApiStatus.initialized;
    });
    builder.addCase(loadDeckOverviews.rejected, (state, action) => {
      console.error('loadDeckOverviews rejected: ', action);
      state.data.status = ApiStatus.errored;
    })
  }
})

export default deckListSlice.reducer;