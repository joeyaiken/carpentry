import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
// import {
//   DECK_DETAIL_RECEIVED,
//   DECK_DETAIL_REQUESTED,
//   DECK_OVERVIEWS_RECEIVED,
//   DECK_OVERVIEWS_REQUESTED
// } from "./state/decksDataActions";
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
import {Dispatch} from "redux";
import {decksApi} from "../../api/decksApi";
import {AppDispatch, RootState} from "../../app/store";
// import {Root} from "react-dom";

export interface State {
  decksById: { [id: number]: DeckOverviewDto };
  deckIds: number[];
  isLoading: boolean;
  isInitialized: boolean;
}

const initialState: State = {
  decksById: {},
  deckIds: [],
  isLoading: false,
  isInitialized: false,
}

// export const ensureDeckOverviewsLoaded = (): any => {
//   return(dispatch: Dispatch, getState: any) => {
//     getDeckOverviews(dispatch, getState(), false);
//   }
// }

// function getDeckOverviews(dispatch: Dispatch, state: RootState, forceReload: boolean): any  {
//   if(state.decks.data.overviews.isLoading || (!forceReload && state.decks.data.overviews.isInitialized)) return;
//   dispatch(deckOverviewsRequested());
//   decksApi.getDeckOverviews().then((results: DeckOverviewDto[]) => {
//     dispatch(deckOverviewsReceived(results));
//   });
// }

export const loadDeckOverviews = createAsyncThunk<DeckOverviewDto[], void>(
  'decksData/loadDeckOverviews',
  async (forceReload, thunkApi) => {
    return await decksApi.getDeckOverviews();
  }
);


export const deckOverviewsSlice = createSlice({
  name: 'decksData',
  initialState: initialState,
  reducers: {
    // deckOverviewsRequested: (state) => {
    //   state.overviews = initialState.overviews;
    //   state.overviews.isLoading = true;
    // },
    //
    // deckOverviewsReceived: (state, action: PayloadAction<DeckOverviewDto[]>) => {
    //   const apiDecks = action.payload;
    //   let decksById: { [key:number]: DeckOverviewDto } = {};
    //   apiDecks.forEach(deck => {
    //     decksById[deck.id] = deck;
    //   });
    //   state.overviews = {
    //     deckIds: apiDecks.map(deck => deck.id),
    //       decksById: decksById,
    //       isLoading: false,
    //       isInitialized: true,
    //   }
    // },
    
  },
  extraReducers: (builder: any) => { //TODO - figure out why the builder type isn't being inferred
    builder.addCase(loadDeckOverviews.pending, (state: State) => {
      state = initialState;
      state.isLoading = true;
    });

    builder.addCase(loadDeckOverviews.fulfilled, (state: State, action: PayloadAction<DeckOverviewDto[]>) => {
      console.log('load fulfilled', action);
      const apiDecks = action.payload;
      let decksById: { [key:number]: DeckOverviewDto } = {};
      apiDecks.forEach(deck => {
        decksById[deck.id] = deck;
      });
      state = {
        deckIds: apiDecks.map(deck => deck.id),
        decksById: decksById,
        isLoading: false,
        isInitialized: true,
      }
    });
    // builder.addCase(loadDeckOverviews.rejected, (state, action) => { })
  }
})

function selectGroupedDeckCards(overviewsById: { [id: number]: DeckCardOverview }, allOverviewIds: number[]): NamedCardGroup[] {
  var result: NamedCardGroup[] = [];

  const cardGroups = ["Commander", "Creatures", "Spells", "Enchantments", "Artifacts", "Planeswalkers", "Lands", "Sideboard"];

  //Am I worried about the fact that cards might get excluded if I mess up the groups?....

  cardGroups.forEach(groupName => {

    const cardsInGroup = allOverviewIds.filter(id => overviewsById[id].category === groupName);

    if(cardsInGroup.length > 0){
      result.push({
        name: groupName,
        cardOverviewIds: cardsInGroup
      });
    }
  });

  return result;
}

export const {
  // deckOverviewsRequested,
  // deckOverviewsReceived,
  // deckDetailRequested,
  // deckDetailReceived,
} = deckOverviewsSlice.actions;

// export const selectCount = (state: RootState) => state.counter.value

export default deckOverviewsSlice.reducer;