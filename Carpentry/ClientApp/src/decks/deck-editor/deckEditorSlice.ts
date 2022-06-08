// This slice represents the Deck Editor app logic

// It will not hold the data result, as that's going to be stored in its own slice
//    Or that's the plan, we'll see if it's actually an option

//for now this can literally just hold selectors
import {RootState} from "../../configureStore";
import {createSlice} from "@reduxjs/toolkit";

// export const getSelectedDeckId = (state: RootState) => state.decks.data.detail.deckId; 





export interface State {
  viewMode: DeckEditorViewMode;
  
  //
  //selectedCard: InventoryOverviewDto | null; // should this be an ID in AppState?

  //Do I want to be able to distinguish between a selected card (highlighted) and hover card (mouse-over)
  //when hover -> show on right
  //When select but no hover -> show select
  //else show stats

  //selectedOverviewCardName: string | null;
  selectedOverviewCardId: number | null;
  secondarySelectedCardId: number | null;

  //this is the filtered inventory cards that should show for a deck
  // selectedInventoryCardIds: number[];
  //selectedInventoryCards: InventoryCard[]; //key = id, but maybe should be a list of IDs, or removed completely

  //Next question, how exactly do I handle grouped cards?!?
  groupBy: "type" | null;
  sortBy: "cost" | null; //| "name", 

  //added during refactor
  isSaving: boolean;
  isPropsModalOpen: boolean;
  deckModalProps: DeckPropertiesDto | null;

  cardMenuAnchor: HTMLButtonElement | null;
  cardMenuAnchorId: number;
}

export const initialState: State = {
  viewMode: "grouped",
  selectedOverviewCardId: null,
  secondarySelectedCardId: null,
  groupBy: null,
  sortBy: null,
  isSaving: false,
  isPropsModalOpen: false,
  deckModalProps: null,
  cardMenuAnchor: null,
  cardMenuAnchorId: 0,
}

export const deckEditorSlice = createSlice({
  name: 'deckEditor',
  initialState: initialState,
  reducers: { 
    // cardMenuButtonClicked: (state)
    
    
  }
})

export default deckEditorSlice.reducer;