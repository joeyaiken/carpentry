import { createSlice, createSelector } from '@reduxjs/toolkit';
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
import {RootState} from "../../../configureStore";

export interface DeckEditorState {
  viewMode: DeckEditorViewMode;
  selectedOverviewCardId: number | null;
  secondarySelectedCardId: number | null;
  groupBy: "type" | null;
  sortBy: "cost" | null;
  isSaving: boolean;
  isPropsModalOpen: boolean;
  deckModalProps: DeckPropertiesDto | null;
  // cardMenuAnchor: HTMLButtonElement | null;
  cardMenuAnchorId: number;
  
}

const initialState: DeckEditorState = {
  viewMode: "grouped",
  selectedOverviewCardId: null,
  secondarySelectedCardId: null,
  groupBy: null,
  sortBy: null,
  isSaving: false,
  isPropsModalOpen: false,
  deckModalProps: null,
  // cardMenuAnchor: null,
  cardMenuAnchorId: 0,
}

export const deckEditorSlice = createSlice({
  name: 'deckEditor',
  initialState: initialState,
  reducers: {
    
    deckEditorCardSelected: (state, action: PayloadAction<DeckCardOverview>) => {
      state.selectedOverviewCardId = action.payload.id;
    },
    
    toggleDeckViewMode: (state) => {
        switch(state.viewMode){
          case "list":
            state.viewMode = "grid";
            break;
          case "grid":
            state.viewMode = "grouped";
            break;
          case "grouped":
            state.viewMode = "list";
            break;
        }
    },
    
    // openDeckPropsModal: (state, action: PayloadAction<DeckPropertiesDto | null>) => {
    //   state.isPropsModalOpen = true;
    //   state.deckModalProps = action.payload;
    // },

    // closeDeckPropsModal: (state: DeckEditorState) => {
    //   state.isPropsModalOpen = false;
    //   state.deckModalProps = null;
    // },
    //
    // deckEditorSaveRequested: (state: DeckEditorState) => {
    //   state.isSaving = true;
    // },
    //
    // deckEditorSaveReceived: (state: DeckEditorState) => {
    //   state.isSaving = false;
    // },
    
    // case DECK_PROPS_MODAL_CHANGED: return deckPropsModalChanged(state, action);
    //     deckPropsModalChanged: (state: DeckEditorState, action: PayloadAction<string | number>) => {
    //     },
        
    //cardMenuButtonClicked: (state, action: PayloadAction<HTMLElement | null>) => {
    cardMenuButtonClicked: (state, action: PayloadAction<HTMLButtonElement | null>) => {
      // state.cardMenuAnchor = action.payload;
      
      // const test = action.payload?.value ?? "";
      //
      //
      // if(test) state.cardMenuAnchorId = parseInt(test);
      //
      // parseInt(test);
      // state.cardMenuAnchorId = parseInt(action.payload?.value);
    },
  },
});

export const {
  deckEditorCardSelected,
  toggleDeckViewMode,
  
  cardMenuButtonClicked,
} = deckEditorSlice.actions;

export default deckEditorSlice.reducer;


//So, this should be a 'memoized' selector in the slice?
// function getSelectedDeckDetails(state: AppState): DeckCardDetail[] {
//   const { cardOverviews, cardDetails } = state.decks.data.detail;
//   const { selectedOverviewCardId } = state.decks.deckEditor;
//   if(selectedOverviewCardId){
//     const match = cardOverviews.byId[selectedOverviewCardId];
//     if(match){
//       return match.detailIds.map(id => cardDetails.byId[id]);
//     }
//     // return cardOverviews.byId[selectedOverviewCardId].detailIds.map(id => cardDetails.byId[id]);
//   }
//   return [];
// }

// export const selectDeckProperties = createSelector(
//   [(state: RootState) => state.decks.detail.deckProps],
//   (props) => props 
// )

const selectedOverviewCardId = (state: RootState) => state.decks.deckEditor.selectedOverviewCardId;

export const getSelectedDeckDetails = createSelector(
  selectedOverviewCardId,
  (state: RootState) => state.decks.deckDetailData.cardOverviews.byId,
  (state: RootState) => state.decks.deckDetailData.cardDetails.byId,
  (selectedOverviewCardId, cardOverviews, cardDetails) => {
      if(selectedOverviewCardId){
        const match = cardOverviews[selectedOverviewCardId];
        if(match){
          return match.detailIds.map(id => cardDetails[id]);
        }
      }
      return [];
  }
);

export const selectDeckOverviews = createSelector(
  [
    (state: RootState) => state.decks.deckEditor.viewMode,
    (state: RootState) => state.decks.deckDetailData.cardOverviews,
    (state: RootState) => state.decks.deckDetailData.cardGroups,
  ],
  (
    viewMode,
    cardOverviews,
    cardGroups
  ) => {
    if(viewMode === "grouped"){
      return cardGroups.map(group => {
        const groupResult: CardOverviewGroup = {
          name: group.name,
          cardOverviews: group.cardOverviewIds.map(id => cardOverviews.byId[id]),
        }
        return groupResult;
      });
    } else {
      return [{
        name: "All",
        cardOverviews: cardOverviews.allIds.map(id => cardOverviews.byId[id]),
      }];
    }
  }
);
