import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
import {decksApi} from "../../api/decksApi";

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

export const loadDeckOverviews = createAsyncThunk<
  DeckOverviewDto[],
  void//,
  // {
  //  
  // }
  >(
  'decksOverview/loadDeckOverviews',
  async (
   // forceReload //, thunkApi
  ) => {
    console.log('load deck overviews..............')
    return await decksApi.getDeckOverviews();
  }
);


export const deckOverviewsSlice = createSlice({
  name: 'decksData',
  initialState: initialState,
  reducers: {
    // deckOverviewsRequested: (state) => {
    //   // todo - decide if I need to clear current deck overviews
    //   state.isLoading = true;
    // },
    //
    //
    // deckOverviewsRequested: (state) => {
    //   state.overviews = initialState.overviews;
    //   state.overviews.isLoading = true;
    // },
    
    // deckOverviewsReceived: (state, action: PayloadAction<DeckOverviewDto[]>) => {
    //   const apiDecks = action.payload;
    //   let decksById: { [key:number]: DeckOverviewDto } = {};
    //   apiDecks.forEach(deck => {
    //     decksById[deck.id] = deck;
    //   });
    //  
    //   return {
    //     deckIds: apiDecks.map(deck => deck.id),
    //     decksById: decksById,
    //     isLoading: false,
    //     isInitialized: true,
    //   } as State;
    // },
    
  },
  extraReducers: (builder: any) => { //TODO - figure out why the builder type isn't being inferred
    builder.addCase(loadDeckOverviews.pending, (state: State) => {
      console.log('load asyncThunk pending')
      // todo - decide if I need to clear current deck overviews
      state.isLoading = true;
    });

    builder.addCase(loadDeckOverviews.fulfilled, (state: State, action: PayloadAction<DeckOverviewDto[]>) => {
      console.log('load fulfilled', action);
      const apiDecks = action.payload;
      let decksById: { [key:number]: DeckOverviewDto } = {};
      apiDecks.forEach(deck => {
        decksById[deck.id] = deck;
      });
      return {
        deckIds: apiDecks.map(deck => deck.id),
        decksById: decksById,
        isLoading: false,
        isInitialized: true,
      } as State;
    });
    builder.addCase(loadDeckOverviews.rejected, (state: State, more: PayloadAction) => {
      // TODO - Show a toast error or something
      console.log('load asyncThunk rejected: ', more)
    })
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