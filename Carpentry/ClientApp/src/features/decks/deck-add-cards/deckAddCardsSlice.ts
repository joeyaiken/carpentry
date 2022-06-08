import { createSlice } from '@reduxjs/toolkit';
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
import {Dispatch} from "redux";
import {RootState} from "../../../app/store";
import {decksApi} from "../../../api/decksApi";
import {loadDeckDetails} from "../deckDetailSlice";

export interface State {
  // cardSearchMethod: "set" | "web" | "inventory";
  selectedCard: CardSearchResultDto | null;
  viewMode: CardSearchViewMode;
  searchFilterProps: CardFilterProps;
  searchResults: {
    isLoading: boolean;

    searchResultsById: {[multiverseId: number]: CardSearchResultDto};
    allSearchResultIds: number[];

    // selectedCard: MagicCard | null; //should probably be an AppState ID
    //what if this just used the other reducer?...
    // inventoryDetail: InventoryDetailDto | null;
  };
  inventoryDetail: {
    isLoading: boolean;
    //inventory cards
    inventoryCardsById: { [id: number]: InventoryCard };
    inventoryCardsAllIds: number[];


    //--First step in showing an inventory detail is to itterate over MagicCard (or even card variant)
    //--Then would need to show each inventory card for a given magic card
    //      (all inventory cards where I.multiverseId === MC.multiverseId)

    //magic cards belonging to inventory cards
    cardsById: { [multiverseId: number]: MagicCard };
    allCardIds: number[];
  };
  addCardIsSaving: boolean;
}

const initialState: State = {
  // cardSearchMethod: "set",
  selectedCard: null,
  viewMode: "list",
  searchFilterProps: {
    // setId: null,
    set: '',
    colorIdentity: [],
    //rarity: ['mythic','rare','uncommon','common'], //
    rarity: [], //
    type: '',//'Creature',
    exclusiveColorFilters: false,
    multiColorOnly: false,
    cardName: '',
    exclusiveName: false,
    maxCount: 0,
    minCount: 0,
    format: '',
    text: '',
    group: '',
  },
  searchResults: {
    isLoading: false,
    searchResultsById: {},
    allSearchResultIds: [],
  },
  inventoryDetail: {
    isLoading: false,
    inventoryCardsById: {},
    inventoryCardsAllIds: [],
    cardsById: {},
    allCardIds: [],
  },
  addCardIsSaving: false,
}

export const deckAddCardsSlice = createSlice({
  name: 'deckAddCards',
  initialState: initialState,
  reducers: {
    cardSearchAddingDeckCard: (state: State) => {
      state.addCardIsSaving = true;
    },

    cardSearchDeckCardAdded: (state: State) => {
      state.addCardIsSaving = false;
    }
  },
});

export const {
  cardSearchAddingDeckCard,
  cardSearchDeckCardAdded,
} = deckAddCardsSlice.actions;

export default deckAddCardsSlice.reducer;

export const selectSearchFilterProps = (state: RootState): CardFilterProps => state.decks.deckAddCards.searchFilterProps;

//TODO - replace with 'createAsyncThunk'
export const requestAddDeckCard = (deckCardDto: DeckCardDto): any => {
// export const requestAddDeckCard = (inventoryCard: InventoryCard): any => {
  return (dispatch: Dispatch, getState: any) => {
    return addDeckCard(dispatch, getState(), deckCardDto);
  }
}

//TODO - should this belong in data actions?
function addDeckCard(dispatch: Dispatch, state: RootState, deckCardDto: DeckCardDto): any{
  dispatch(cardSearchAddingDeckCard());
  decksApi.addDeckCard(deckCardDto).then(() => {
    // //need to re-query deck data
    //
    // // dispatch(reloadDeckDetail(deckCardDto.deckId));
    // dispatch(loadDeckDetails(deckCardDto.deckId));
    //
    // //also need to reload inventory detail
    // const selectedCard = state.decks.deckAddCards.selectedCard;
    //
    // if(selectedCard){
    //   dispatch(requestCardSearchInventory(selectedCard));
    // }
    //
    // dispatch(cardSearchDeckCardAdded());
  })
}