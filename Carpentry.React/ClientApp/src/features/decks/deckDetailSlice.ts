import { createSlice, createAsyncThunk, createSelector } from '@reduxjs/toolkit';
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
  isLoading: boolean;
  deckId: number;
  deckProps: DeckPropertiesDto;
  cardOverviews: {
    byId: { [id: number]: DeckCardOverview }
    allIds: number[];
  };
  cardDetails: {
    byId: { [deckCardId: number]: DeckCardDetail };
    allIds: number[];
  };
  deckStats: DeckStats | null;
  cardGroups: NamedCardGroup[];
}

const initialState: State = {
  isLoading: false,

  deckId: 0,
  deckProps: {
    id: 0,
    name: '',
    format: null,
    notes: '',
    basicW: 0,
    basicU: 0,
    basicB: 0,
    basicR: 0,
    basicG: 0,
  },

  cardDetails: {
    byId: {},
    allIds: [],
  },

  cardOverviews: {
    byId: {},
    allIds: [],
  },

  deckStats: null,
  cardGroups: [],
}

// export const reloadDeckDetail = (deckId: number): any => {
//   return (dispatch: Dispatch, getState: any) => {
//     tryLoadDeckDetail(dispatch, getState(), deckId, true);
//   }
// }
// export const ensureDeckDetailLoaded = (deckId: number): any => {
//   return (dispatch: Dispatch, getState: any) => {
//     tryLoadDeckDetail(dispatch, getState(), deckId, false);
//   }
// }
// function tryLoadDeckDetail(dispatch: Dispatch, state: AppState, deckId: number, forceReload: boolean): void {
//   if (state.decks.data.detail.isLoading || (!forceReload && state.decks.data.detail.deckId === deckId)) return;
//   dispatch(deckDetailRequested(deckId));
//   decksApi.getDeckDetail(deckId).then((result) => {
//     dispatch(deckDetailReceived(result));
//   });
// }

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

export const loadDeckOverviews = createAsyncThunk<DeckOverviewDto[], void,{
  // state: RootState,

}>(
  'decksData/loadDeckOverviews',
  async (forceReload, thunkApi) => {
    // console.log('loadDeckOverviews')
    const result = await decksApi.getDeckOverviews();
    // console.log('overviews',result)
    return result;
  }
);


export const decksDetailSlice = createSlice({
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

    deckDetailRequested: (state: State) => {
      state.isLoading = true
    },

    deckDetailReceived: (state: State, action: PayloadAction<DeckDetailDto>) => {
      const dto: DeckDetailDto = action.payload;

      let overviewsById: {[id: number]: DeckCardOverview} = {};
      let overviewIds: number[] = [];

      let detailsById: {[id: number]: DeckCardDetail} = {};
      let detailIds: number[] = [];

      dto.cards.forEach(cardOverview => {
        const overviewId = cardOverview.id;
        overviewIds.push(overviewId);
        overviewsById[overviewId] = {
          category: cardOverview.category,
          cmc: cardOverview.cmc,
          cost: cardOverview.cost,
          count: cardOverview.count,
          detailIds: cardOverview.details.map(detail => detail.id),
          id: cardOverview.id,
          img: cardOverview.img,
          name: cardOverview.name,
          type: cardOverview.type,
          cardId: cardOverview.cardId,
          tags: cardOverview.tags,
        };
        cardOverview.details.forEach(detail => {
          const detailId = detail.id;
          detailIds.push(detailId);
          //TODO - figure out why this mapping is happening.  Why isn't this just "detailsById[detailId] = detail"?
          detailsById[detailId] = {
            category: detail.category,
            id: detail.id,
            isFoil: detail.isFoil,
            name: detail.name,
            overviewId: cardOverview.id,
            set: detail.set,
            collectorNumber: detail.collectorNumber,
            inventoryCardId: detail.inventoryCardId,

            inventoryCardStatusId: 0,
            cardId: detail.cardId,
            deckId: detail.deckId,
            availabilityId: detail.availabilityId,

          };
        });
      });

      state.isLoading = false;
      state.deckId = dto.props.id;
      state.deckProps = dto.props;
      state.cardOverviews = {
        byId: overviewsById,
        allIds: overviewIds,
      };
      state.cardDetails = {
        byId: detailsById,
        allIds: detailIds,
      };
      state.cardGroups = selectGroupedDeckCards(overviewsById, overviewIds);
      state.deckStats = dto.stats;
    },
  },
  extraReducers: (builder: any) => { //TODO - figure out why the builder type isn't being inferred
    // builder.addCase(loadDeckOverviews.pending, (state: State) => {
    //   console.log('load pending');
    //   state.overviews = initialState.overviews;
    //   state.overviews.isLoading = true;
    // });
    //
    // builder.addCase(loadDeckOverviews.fulfilled, (state: State, action: PayloadAction<DeckOverviewDto[]>) => {
    //   console.log('load fulfilled', action);
    //   const apiDecks = action.payload;
    //   let decksById: { [key:number]: DeckOverviewDto } = {};
    //   apiDecks.forEach(deck => {
    //     decksById[deck.id] = deck;
    //   });
    //   state.overviews = {
    //     deckIds: apiDecks.map(deck => deck.id),
    //     decksById: decksById,
    //     isLoading: false,
    //     isInitialized: true,
    //   }
    // });

    // builder.addCase(loadDeckOverviews.rejected, (state, action) => {
    //
    // })
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
  deckDetailRequested,
  deckDetailReceived,
} = decksDetailSlice.actions;

// export const selectCount = (state: RootState) => state.counter.value

export default decksDetailSlice.reducer;

//const deckProperties: DeckPropertiesDto = useAppSelector(state => state.decks.data.detail.deckProps);
// export const selectDeckProperties = createSelector(
//   [(state: RootState) => state.decks.detail.deckProps],
//   (props) => props 
// )

export const selectDeckProperties = (state: RootState): DeckPropertiesDto => state.decks.detail.deckProps;

export const selectOverviewIds = (state: RootState): number[] => state.decks.detail.cardOverviews.allIds;

export const selectOverviewCard = (state: RootState, cardId: number): DeckCardOverview => state.decks.detail.cardOverviews.byId[cardId];
