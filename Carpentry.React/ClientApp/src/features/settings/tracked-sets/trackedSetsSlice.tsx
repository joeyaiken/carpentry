import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import {coreApi} from "../../../api/coreApi";
// import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
// import {Dispatch} from "redux";
// import {AppState} from "../../../store";
// import {coreApi} from "../../../api/coreApi";
//

export interface  State {
  //   setsById: { [id: number]: SetDetailDto };
  //   setIds: number[];
    trackedSetsIsLoading: boolean;
    
  //   showUntracked: boolean;
}

const initialState: State = {
  trackedSetsIsLoading = false,
}

class PayloadAction {
}

export const trackedSetsSlice = createSlice({
  name: 'trackedSets',
  initialState: initialState,
  reducers: { 
    
  },
  extraReducers: (builder: any) => { //TODO - figure out why the builder type isn't being inferred
    builder.addCase(loadTrackedSets.pending, (state: State) => {
    //     const { showUntracked } = action.payload;
    //
    //     const newState: State = {
    //         ...state,
    //         ...initialState,
    //         isLoading: true,
    //         showUntracked: showUntracked,
    //     }
    //
    //     return newState;
    });
    builder.addCase(loadTrackedSets.fulfilled, (state: State) => {
    //     //I guess this is normally where Normalizr should be used?
    //     const apiSets: SetDetailDto[] = action.payload;
    //
    //     //Create/Update/Delete actions will return null
    //     if(apiSets === null){
    //         return {
    //             ...state,
    //             isLoading: false,
    //         }
    //     }
    //
    //     let setsById: { [key:number]: SetDetailDto } = {};
    //
    //     apiSets.forEach(set => {
    //         setsById[set.setId] = set;
    //     });
    //
    //     const newState: State = {
    //         isLoading: false,
    //         setIds: apiSets.map(set => set.setId),
    //         setsById: setsById,
    //         showUntracked: state.showUntracked,
    //     }
    //
    //     return newState;
    });
    builder.addCase(loadTrackedSets.rejected, (state: State, action: PayloadAction) => {
      // TODO - Show a toast error or something
      console.log('loadTrackedSets thunk rejected: ', action);
      state.trackedSetsIsLoading = false;
    });
  }
})

//export const loadTrackedSets = createAsyncThunk<SetDetailDto, void>(
export const loadTrackedSets = createAsyncThunk<SetDetailDto, { showUntracked: boolean, update: boolean}>(
  'trackedSets/loadTrackedSets',
  //showUntracked: boolean, update: boolean
  async (props) => coreApi.getTrackedSets(props.showUntracked, props.update)
);








// export interface State {
//   setsById: { [id: number]: SetDetailDto };
//   setIds: number[];
//   isLoading: boolean;
//   showUntracked: boolean;
// }
//
// export const trackedSetsSlice = createSlice({
//   name: 'trackedSets',
//   initialState: {
//     isLoading: false,
//     showUntracked: false,
//     setsById: {},
//     setIds: [],
//   },
//   reducers: {
//     trackedSetsDataRequested: (state: State, action: PayloadAction<{showUntracked: boolean}>) => {
//       state = {
//         ...state,
//         isLoading: true,
//         showUntracked: action.payload.showUntracked,
//       }
//     },
//     trackedSetsDataReceived: (state: State, action: PayloadAction<SetDetailDto[]>)=> {
//       //I guess this is normally where Normalizr should be used?
//       const apiSets: SetDetailDto[] = action.payload;
//
//       //Create/Update/Delete actions will return null
//       if(apiSets === null){
//         state.isLoading = false;
//       } else {
//         let setsById: { [key:number]: SetDetailDto } = {};
//
//         apiSets.forEach(set => {
//           setsById[set.setId] = set;
//         });
//
//         state = {
//           isLoading: false,
//           setIds: apiSets.map(set => set.setId),
//           setsById: setsById,
//           showUntracked: state.showUntracked,
//         }
//       }
//     }
//   },
// });
//
// //TODO - Make a selector 'thinkIsLoading()' that, as put, checks if the think is loading 
//
// export const requestTrackedSets = (showUntracked: boolean, update: boolean): any => {
//   return (dispatch: Dispatch, getState: any) => {
//     return getTrackedSets(dispatch, getState(), showUntracked, update);
//   }
// }
// function getTrackedSets(dispatch: Dispatch, state: AppState, showUntracked: boolean, update: boolean): any {
//   const dataIsLoading = state.settings.isLoading;//state.data.trackedSets.isLoading;
//   if(dataIsLoading){
//     return;
//   }
//   dispatch(trackedSetsDataRequested(showUntracked, update));
//   coreApi.getTrackedSets(showUntracked, update).then((results) => {
//     dispatch(trackedSetsDataReceived(results));
//   });
// }
//
// export const requestAddTrackedSet = (setId: number): any => {
//   return (dispatch: Dispatch, getState: any) => {
//     return addTrackedSet(dispatch, getState(), setId);
//   }
// }
// function addTrackedSet(dispatch: Dispatch, state: AppState, setId: number): any {
//   const dataIsLoading = state.settings.isLoading;
//   if(dataIsLoading){
//     return;
//   }
//
//   const showUntrackedVal = state.settings.showUntracked;
//   dispatch(trackedSetsDataRequested(showUntrackedVal,  false));
//   coreApi.addTrackedSet(setId).then(() => {
//     dispatch(trackedSetsDataReceived(null));
//     dispatch(requestTrackedSets(showUntrackedVal, false));
//   });
// }
//
// export const requestUpdateTrackedSet = (setId: number): any => {
//   return (dispatch: Dispatch, getState: any) => {
//     return updateTrackedSet(dispatch, getState(), setId);
//   }
// }
// function updateTrackedSet(dispatch: Dispatch, state: AppState, setId: number): any {
//   const dataIsLoading = state.settings.isLoading;//state.data.trackedSets.isLoading;
//   if(dataIsLoading){
//     return;
//   }
//   // console.log('updating tracked sets ping 1');
//
//   const showUntrackedVal = state.settings.showUntracked//state.data.trackedSets.showUntracked;
//   dispatch(trackedSetsDataRequested(showUntrackedVal, false));
//   coreApi.updateTrackedSet(setId).then(() => {
//     // console.log('updating tracked sets ping 2');
//     dispatch(trackedSetsDataReceived(null));
//     // console.log('updating tracked sets ping 3');
//     dispatch(requestTrackedSets(showUntrackedVal, false));
//   });
// }
//
// export const requestRemoveTrackedSet = (setId: number): any => {
//   return (dispatch: Dispatch, getState: any) => {
//     return removeTrackedSet(dispatch, getState(), setId);
//   }
// }
// function removeTrackedSet(dispatch: Dispatch, state: AppState, setId: number): any {
//   const dataIsLoading = state.settings.isLoading;
//   if(dataIsLoading){
//     return;
//   }
//
//   const showUntrackedVal = state.settings.showUntracked;
//   dispatch(trackedSetsDataRequested(showUntrackedVal, false));
//   coreApi.removeTrackedSet(setId).then(() => {
//     dispatch(trackedSetsDataReceived(null));
//     dispatch(requestTrackedSets(showUntrackedVal, false));
//   });
// }
//
// export const { trackedSetsDataRequested, trackedSetsDataReceived } = trackedSetsSlice.actions;
//
// export default trackedSetsSlice.reducer;