import {trackedSetsApi} from "../../api/trackedSetsApi";
import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import {ApiStatus} from "../../enums";
import {RootState} from "../../configureStore";
import {coreApi} from "../../api/coreApi";

export interface State {
  showUntrackedSets: boolean;
  trackedSets: {
    readDataStatus: ApiStatus,
    writeDataStatus: ApiStatus,
    byId: { [id: number]: SetDetailDto };
    allIds: number[];
  },
  
  collectionTotals: {
    status: ApiStatus,
    byId: { [id: number]: InventoryTotalsByStatusResult };
    allIds: number[];
  }
}

const initialState: State = {
  showUntrackedSets: false,
  trackedSets: {
    readDataStatus: ApiStatus.uninitialized,
    writeDataStatus: ApiStatus.initialized,
    byId: {},
    allIds: [],
  },
  collectionTotals: {
    status: ApiStatus.uninitialized,
    byId: {},
    allIds: [],
  }
}

export const loadTrackedSets = createAsyncThunk<NormalizedList<SetDetailDto>, {showUntracked: boolean, update: boolean}>(
  'trackedSets/loadTrackedSets',
  async (props) => trackedSetsApi.getTrackedSets(props.showUntracked, props.update)
);

export enum TrackedSetsApiAction {
  add,
  update,
  remove,
}

export const modifyTrackedSets = createAsyncThunk<void, {action: TrackedSetsApiAction, setId: number, showUntracked: boolean}>(
  'trackedSets/modifyTrackedSets',
  async(props, thunkApi) => {
    switch (props.action){
      case TrackedSetsApiAction.add:
        await trackedSetsApi.addTrackedSet(props.setId);
        break;
      case TrackedSetsApiAction.update:
        await trackedSetsApi.updateTrackedSet(props.setId);
        break;
      case TrackedSetsApiAction.remove:
        await trackedSetsApi.removeTrackedSet(props.setId);
        break;
    }
    thunkApi.dispatch(loadTrackedSets({showUntracked: props.showUntracked, update: false}))
  }
);

export const loadCollectionTotals = createAsyncThunk<NormalizedList<InventoryTotalsByStatusResult>>(
  'settings/loadCollectionTotals',
  async () => {
    return coreApi.GetCollectionTotals();
  }
)

export const settingsSlice = createSlice({
  name: 'settings',
  initialState: initialState,
  reducers: { },
  extraReducers: (builder) => {
    builder.addCase(loadTrackedSets.pending, (state, action) => {
      state.trackedSets.readDataStatus = ApiStatus.loading;
      state.showUntrackedSets = action.meta.arg.showUntracked;
    });
    builder.addCase(loadTrackedSets.fulfilled, (state, action) => {
      if(action.payload === null) return;
      
      state.trackedSets.byId = action.payload.byId;
      state.trackedSets.allIds = action.payload.allIds;
      state.trackedSets.readDataStatus = ApiStatus.initialized;
    });
    builder.addCase(loadTrackedSets.rejected, (state, action) => {
      console.error('loadTrackedSets thunk rejected: ', action);
      state.trackedSets.readDataStatus = ApiStatus.errored;
    });

    builder.addCase(modifyTrackedSets.pending, (state) => {
      state.trackedSets.writeDataStatus = ApiStatus.loading;
    });
    builder.addCase(modifyTrackedSets.fulfilled, (state) => {
      state.trackedSets.writeDataStatus = ApiStatus.initialized;
    });
    builder.addCase(modifyTrackedSets.rejected, (state, action) => {
      console.error('modifyTrackedSets thunk rejected: ', action);
      state.trackedSets.writeDataStatus = ApiStatus.errored;
    });


    builder.addCase(loadCollectionTotals.pending, (state, action) => {
      state.collectionTotals.status = ApiStatus.loading;
    });
    builder.addCase(loadCollectionTotals.fulfilled, (state, action) => {
      state.collectionTotals.byId = action.payload.byId;
      state.collectionTotals.allIds = action.payload.allIds;
      state.collectionTotals.status = ApiStatus.initialized;
    });
    builder.addCase(loadCollectionTotals.rejected, (state, action) => {
      console.error('loadCollectionTotals thunk rejected: ', action);
      state.collectionTotals.status = ApiStatus.errored;
    });
  }
});

export const selectSettingsApiStatus = (state: RootState): ApiStatus => {
  const readStatus = state.settings.trackedSets.readDataStatus;
  const writeStatus = state.settings.trackedSets.writeDataStatus;

  return (readStatus > writeStatus) ? readStatus : writeStatus;
}

export default settingsSlice.reducer;