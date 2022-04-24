import {createSlice} from '@reduxjs/toolkit';
import {PayloadAction} from '@reduxjs/toolkit/dist/createAction';
import {Dispatch} from "redux";
import {coreApi} from "../api/coreApi";
// import {appCoreDataReceived, appCoreDataRequested} from "./state/coreDataActions";
import {RootState} from "../app/store";

export interface State {
  filterDataIsLoading: boolean;
  filterOptions: AppFiltersDto;
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
  filterDataIsLoading: false,
}

export const coreDataSlice = createSlice({
  name: 'coreData',
  initialState: initialState,
  reducers: {
    appFiltersRequested: (state: State) => {
      state.filterDataIsLoading = true;
    },
    appFiltersReceived: (state: State, action: PayloadAction<AppFiltersDto>) => {
      state.filterOptions = action.payload || {};
      state.filterDataIsLoading = false;
    }
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

export const { appFiltersReceived, appFiltersRequested } = coreDataSlice.actions;

export default coreDataSlice.reducer;