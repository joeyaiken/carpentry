import { createSlice } from '@reduxjs/toolkit';
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';

export interface DeckEditorState {
  viewMode: DeckEditorViewMode;
  selectedOverviewCardId: number | null;
  secondarySelectedCardId: number | null;
  groupBy: "type" | null;
  sortBy: "cost" | null;
  isSaving: boolean;
  isPropsModalOpen: boolean;
  deckModalProps: DeckPropertiesDto | null;
  cardMenuAnchor: HTMLButtonElement | null;
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
  cardMenuAnchor: null,
  cardMenuAnchorId: 0,
}

export const deckEditorSlice = createSlice({
  name: 'deckEditor',
  initialState: initialState,
  reducers: {
    deckEditorCardSelected: (state: DeckEditorState, action: PayloadAction<DeckCardOverview>) => {
      state.selectedOverviewCardId = action.payload.id;
    },
    
    toggleDeckViewMode: (state: DeckEditorState) => {
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
    
    openDeckPropsModal: (state: DeckEditorState, action: PayloadAction<DeckPropertiesDto | null>) => {
      state.isPropsModalOpen = true;
      state.deckModalProps = action.payload;
    },

    closeDeckPropsModal: (state: DeckEditorState) => {
      state.isPropsModalOpen = false;
      state.deckModalProps = null;
    },
    
    deckEditorSaveRequested: (state: DeckEditorState) => {
      state.isSaving = true;
    },
    
    deckEditorSaveReceived: (state: DeckEditorState) => {
      state.isSaving = false;
    },
    
// case DECK_PROPS_MODAL_CHANGED: return deckPropsModalChanged(state, action);
//     deckPropsModalChanged: (state: DeckEditorState, action: PayloadAction<string | number>) => {
//     },
    
    cardMenuButtonClicked: (state: DeckEditorState, action: PayloadAction<HTMLElement | null>) => {
      state.cardMenuAnchor = action.payload;
      state.cardMenuAnchorId = parseInt(action.payload?.value);
    },
  },
});

export const {
  deckEditorCardSelected
} = deckEditorSlice.actions;

export default deckEditorSlice.reducer;