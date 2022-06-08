import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {ApiStatus} from "../../../enums";
import {decksApi} from "../../../api/decksApi";

export interface State {
  cardId: number;
  data: {
    status: ApiStatus,
    cardName: string;
    existingTags: CardTagDetailTag[];
    tagSuggestions: string[];
  }
}

const initialState: State = {
  cardId: 0,
  data: {
    status: ApiStatus.uninitialized,
    cardName: '',
    existingTags: [],
    tagSuggestions: [],
  }
}

export const loadTagDetail = createAsyncThunk<CardTagDetailDto,{deckId: number, cardId: number}>(
  'cardTags/loadTagDetail',
  async (props) =>
    decksApi.getCardTagDetails(props.deckId, props.cardId)
)

export const addCardTag = createAsyncThunk<void, CardTagDto>(
  'cardTags/addCardTag',
  async (dto) => decksApi.addCardTag(dto)
)

export const removeCardTag = createAsyncThunk<void, number>(
  'cardTags/removeCardTag',
  async (tagId) => decksApi.removeCardTag(tagId)
)

export const cardTagsSlice = createSlice({
  name: 'cardTags',
  initialState: initialState,
  reducers: { },
  extraReducers: (builder) => {
    // Get tags
    builder.addCase(loadTagDetail.pending, (state, action) => {
      state.cardId = action.meta.arg.cardId;
      state.data.status = ApiStatus.loading;
    });
    builder.addCase(loadTagDetail.fulfilled, (state, action) => {
      const detailResult: CardTagDetailDto = action.payload;

      state.data.cardName = detailResult.cardName;
      state.data.existingTags = detailResult.existingTags;
      state.data.tagSuggestions = detailResult.tagSuggestions;

      state.data.status = ApiStatus.initialized;
    });
    builder.addCase(loadTagDetail.rejected, (state, action) => {
      console.error('loadTagDetail thunk rejected: ', action);
      state.data.status = ApiStatus.errored;
    });

    // Add tag
    builder.addCase(addCardTag.pending, (state) => {
      state.data.status = ApiStatus.loading;
    });
    builder.addCase(addCardTag.fulfilled, (state) => {
      state.data.status = ApiStatus.uninitialized;
    });
    builder.addCase(addCardTag.rejected, (state, action) => {
      console.error('addCardTag thunk rejected: ', action);
      state.data.status = ApiStatus.errored;
    });

    // Remove tag
    builder.addCase(removeCardTag.pending, (state) => {
      state.data.status = ApiStatus.loading;
    });
    builder.addCase(removeCardTag.fulfilled, (state) => {
      state.data.status = ApiStatus.uninitialized;
    });
    builder.addCase(removeCardTag.rejected, (state, action) => {
      console.error('removeCardTag thunk rejected: ', action);
      state.data.status = ApiStatus.errored;
    });
  }
})

export default cardTagsSlice.reducer;