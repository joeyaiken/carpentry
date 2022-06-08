import {ApiStatus} from "../../enums";
import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {decksApi} from "../../api/decksApi";
import {PayloadAction} from "@reduxjs/toolkit/dist/createAction";
import {RootState} from "../../configureStore";


export interface State {
  //isLoading: boolean;
  status: ApiStatus;
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
  // isLoading: false,
  status: ApiStatus.uninitialized,
  
  deckId: 0,
  deckProps: {
    id: 0,
    name: '',
    // format: null,
    format: '',
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

export const loadDeckDetails = createAsyncThunk<DeckDetailDto, number>(
  'decksData/loadDeckDetails',
  async (deckId) => await decksApi.getDeckDetail(deckId)
);

export const decksDetailSlice = createSlice({
  name: 'decksData',
  initialState: initialState,
  reducers: { },
  extraReducers: (builder: any) => { //TODO - figure out why the builder type isn't being inferred

    builder.addCase(loadDeckDetails.pending, (state: State) => {
      state.status = ApiStatus.loading;
      // Should existing fields be cleared here?...
    });

    builder.addCase(loadDeckDetails.fulfilled, (state: State, action: PayloadAction<DeckDetailDto>) => {
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

      // state.isLoading = false;
      state.status = ApiStatus.initialized;
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
    });

    builder.addCase(loadDeckDetails.rejected, (state: State, action: PayloadAction) => {
      state.status = ApiStatus.errored;
    });
  }
})

// TODO - This grouping should really be done in the controller/service layer, not here 
function selectGroupedDeckCards(overviewsById: { [id: number]: DeckCardOverview }, allOverviewIds: number[]): NamedCardGroup[] {
  let result: NamedCardGroup[] = [];

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

export const getSelectedDeckId = (state: RootState) => state.decks.deckDetailData.deckId;

export const selectDeckProperties = (state: RootState): DeckPropertiesDto => state.decks.deckDetailData.deckProps;

export const selectOverviewIds = (state: RootState): number[] => state.decks.deckDetailData.cardOverviews.allIds;

export const selectOverviewCard = (state: RootState, cardId: number): DeckCardOverview => state.decks.deckDetailData.cardOverviews.byId[cardId];

export default decksDetailSlice.reducer;