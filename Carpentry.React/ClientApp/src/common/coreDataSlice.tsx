import {createSlice, createAsyncThunk} from '@reduxjs/toolkit';
import {PayloadAction} from '@reduxjs/toolkit/dist/createAction';
// import {appCoreDataReceived, appCoreDataRequested} from "./state/coreDataActions";
import {ApiStatus} from "../enums";
import {coreApi} from "../api/coreApi";
import {Add, AddIcCall, Build} from "@material-ui/icons";

export interface State {
  // filterDataIsLoading: boolean;
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

// export const requestCoreData = (): any => {
//   return (dispatch: Dispatch, getState: any) => {
//     return getCoreData(dispatch, getState());
//   }
// }
//
// function getCoreData(dispatch: Dispatch, state: RootState): any {
//   const dataIsLoading = state.core.data.filterDataIsLoading;
//   if(dataIsLoading) {
//     return;
//   }
//
//   dispatch(appCoreDataRequested()); //todo - rename
//   coreApi.getCoreData().then((results) => {//todo - rename
//     dispatch(appCoreDataReceived(results));//todo - rename
//   });
// }

// export const { appFiltersReceived, appFiltersRequested } = coreDataSlice.actions;

export default coreDataSlice.reducer;