import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {ApiStatus} from "../../../enums";
import {decksApi} from "../../../api/decksApi";

export interface State {
  status: ApiStatus;
  isDialogOpen: boolean;
  deckExportPayload: string;
}

const initialState: State = {
  status: ApiStatus.initialized,
  isDialogOpen: false,
  deckExportPayload: '',
}

export const getDeckExport = createAsyncThunk<string,{deckId: number, exportType: string}>(
  'deckExport/getDeckExport',
  async (props) => 
    await decksApi.exportDeckList(props.deckId, props.exportType)
)

export const deckExportSlice = createSlice({
  name: 'deckExport',
  initialState: initialState,
  reducers: {
    openExportDialog: (state) => {
      state.isDialogOpen = true;
    },

    closeExportDialog: (state) => {
      state.isDialogOpen = false;
      state.deckExportPayload = '';
    }
  },
  extraReducers: (builder) => {
    builder.addCase(getDeckExport.pending, (state) => {
      state.deckExportPayload = '';
      state.status = ApiStatus.loading;
    });
    builder.addCase(getDeckExport.fulfilled, (state, action) => {
      state.deckExportPayload = action.payload;
      state.status = ApiStatus.initialized;
    });
    builder.addCase(getDeckExport.rejected, (state, action) => {
      console.error('getDeckExport rejected: ', action);
      state.status = ApiStatus.errored;
    })
  }
})

export const { openExportDialog, closeExportDialog } = deckExportSlice.actions;

export default deckExportSlice.reducer;