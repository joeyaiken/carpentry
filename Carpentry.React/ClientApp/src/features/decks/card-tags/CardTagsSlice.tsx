import { createSlice } from '@reduxjs/toolkit';
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
import {Dispatch} from "redux";

// //TODO - Actually have this model locally...
// import {CardTagDetailDto} from "../../../../../../Carpentry.Angular/ClientApp/src/app/decks/models";

export interface State {
  isLoading: boolean;

  cardId: number;
  cardName: string;
  existingTags: CardTagDetailTag[];
  tagSuggestions: string[];

  newTagName: string;
}

const initialState: State = {
  isLoading: false,

  cardId: 0,
  cardName: '',
  existingTags: [],
  tagSuggestions: [],

  newTagName: '',
}

export const cardTagsSlice = createSlice({
  name: 'cardTags',
  initialState: initialState,
  reducers: {
    tagDetailRequested: (state: State) => {
      state.isLoading = true;
    },
    tagDetailReceived: (state: State, action: PayloadAction<CardTagDetailDto>) => {
      const detailResult: CardTagDetailDto = action.payload;
      state.isLoading = false;
      state.cardId = detailResult.cardId;
      state.cardName = detailResult.cardName;
      state.existingTags = detailResult.existingTags;
      state.tagSuggestions = detailResult.tagSuggestions;
      state.newTagName = '';
    },
  }
})