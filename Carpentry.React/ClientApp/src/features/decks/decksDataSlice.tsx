import { createSlice } from '@reduxjs/toolkit';
// import {
//   DECK_DETAIL_RECEIVED,
//   DECK_DETAIL_REQUESTED,
//   DECK_OVERVIEWS_RECEIVED,
//   DECK_OVERVIEWS_REQUESTED
// } from "./state/decksDataActions";
import { DeckOverviewDto, DeckCardOverview, DeckCardDetail, DeckPropertiesDto, DeckStats, NamedCardGroup, DeckDetailDto } from '../../../../../Carpentry.Angular/ClientApp/src/app/decks/models';
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
import {Dispatch} from "redux";
import {decksApi} from "../../api/decksApi";

export interface State {
  overviews: {
    decksById: { [id: number]: DeckOverviewDto };
    deckIds: number[];
    isLoading: boolean;
    isInitialized: boolean;
  };
  detail: { //TODO - rename to 'deckDetail' or something
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
}

const initialState: State = {
  overviews: {
    decksById: {},
    deckIds: [],
    isLoading: false,
    isInitialized: false,
  },
  detail: {
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
}

export const reloadDeckDetail = (deckId: number): any => {
  return (dispatch: Dispatch, getState: any) => {
    tryLoadDeckDetail(dispatch, getState(), deckId, true);
  }
}

export const ensureDeckDetailLoaded = (deckId: number): any => {
  return (dispatch: Dispatch, getState: any) => {
    tryLoadDeckDetail(dispatch, getState(), deckId, false);
  }
}

function tryLoadDeckDetail(dispatch: Dispatch, state: AppState, deckId: number, forceReload: boolean): void {
  if (state.decks.data.detail.isLoading || (!forceReload && state.decks.data.detail.deckId === deckId)) return;
  dispatch(deckDetailRequested(deckId));
  decksApi.getDeckDetail(deckId).then((result) => {
    dispatch(deckDetailReceived(result));
  });
}

export const decksDataSlice = createSlice({
  name: 'decksData',
  initialState: initialState,
  reducers: {
    deckOverviewsRequested: (state: State) => {
      state.overviews = initialState.overviews;
      state.overviews.isLoading = true;
    },
    
    deckOverviewsReceived: (state: State, action: PayloadAction<DeckOverviewDto[]>) => {
      const apiDecks = action.payload;
      let decksById: { [key:number]: DeckOverviewDto } = {};
      apiDecks.forEach(deck => {
        decksById[deck.id] = deck;
      });
      state.overviews = {
        deckIds: apiDecks.map(deck => deck.id),
          decksById: decksById,
          isLoading: false,
          isInitialized: true,
      }
    },
    
    deckDetailRequested: (state: State) => {
      state.detail.isLoading = true
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
      
      state.detail.isLoading = false;
      state.detail.deckId = dto.props.id;
      state.detail.deckProps = dto.props;
      state.detail.cardOverviews = {
        byId: overviewsById,
        allIds: overviewIds,
      };
      state.detail.cardDetails = {
        byId: detailsById,
        allIds: detailIds,
      };
      state.detail.cardGroups = selectGroupedDeckCards(overviewsById, overviewIds);
      state.detail.deckStats = dto.stats;
    },
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
  deckOverviewsRequested,
  deckOverviewsReceived,
  deckDetailRequested,
  deckDetailReceived,
} = decksDataSlice.actions;

// export const selectCount = (state: RootState) => state.counter.value

export default decksDataSlice.reducer;