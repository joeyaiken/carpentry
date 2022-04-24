import { createSlice } from '@reduxjs/toolkit';
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
import {Dispatch} from "redux";

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

export const deckAddCardsSlice = createSlice({
  name: 'deckAddCards',
  initialState: {
    
  },
  reducers: {
    
  },
});



export default deckAddCardsSlice.reducer;