import {createSlice, createAsyncThunk} from '@reduxjs/toolkit';
import {ApiStatus} from "../../enums";
import {coreApi} from "../../api/coreApi";

export interface State {
  filterOptions: AppFiltersDto;
  filterDataStatus: ApiStatus;
}

const initialState: State = {
  filterOptions: {
    sets: [],
    colors: [],
    rarities: [],
    types: [],
    formats: [],
    statuses: [],
    searchGroups: [],
    groupBy: [],
    sortBy: [],
  },
  filterDataStatus: ApiStatus.uninitialized,
}

export const getCoreData = createAsyncThunk<AppFiltersDto>(
  'coreData/getCoreData',
  async () => coreApi.getCoreData()
);

export const coreDataSlice = createSlice({
  name: 'coreData',
  initialState: initialState,
  reducers: { },
  extraReducers: (builder) => {
    builder.addCase(getCoreData.pending, (state) => {
      state.filterDataStatus = ApiStatus.loading;
    });

    builder.addCase(getCoreData.fulfilled, (state, action) => {
      state.filterDataStatus = ApiStatus.initialized;
      state.filterOptions = action.payload || {};
    });

    builder.addCase(getCoreData.rejected, (state, action) => {
      console.error('getCoreData thunk rejected: ', action);
      state.filterDataStatus = ApiStatus.errored;
    });
  },
})

export default coreDataSlice.reducer;