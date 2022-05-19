import {coreApi} from "../../api/coreApi";
import {createSlice, createAsyncThunk} from '@reduxjs/toolkit';
import {RootState} from "../../app/store";
import {ApiStatus} from "../../enums";

export interface State {
  showUntrackedSets: boolean;
  trackedSets: {
    readDataStatus: ApiStatus,
    writeDataStatus: ApiStatus,
    byId: { [id: number]: SetDetailDto };
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
  }
}

export const loadTrackedSets = createAsyncThunk<SetDetailDto[], {showUntracked: boolean, update: boolean}>(
  'trackedSets/loadTrackedSets',
  async (props, thunkApi) => 
    await coreApi.getTrackedSets(props.showUntracked, props.update)
);

export enum TrackedSetsApiAction {
  add,
  update,
  remove,
}
export const modifyTrackedSets = createAsyncThunk<void, {action: TrackedSetsApiAction, setId: number, showUntracked: boolean}>(
  'trackedSets/',
  async(props, thunkApi) => {
    switch (props.action){
      case TrackedSetsApiAction.add: 
        console.log('add set', props.setId);
        await coreApi.addTrackedSet(props.setId);
        break;
      case TrackedSetsApiAction.update:
        await coreApi.updateTrackedSet(props.setId);
        break;
      case TrackedSetsApiAction.remove:
        await coreApi.removeTrackedSet(props.setId);
        break;
    }
    thunkApi.dispatch(loadTrackedSets({showUntracked: props.showUntracked, update: false}))
  }
);

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
      const apiSets: SetDetailDto[] = action.payload;
      if(apiSets===null) return;
      
      let setsById: { [key:number]: SetDetailDto } = {};
      apiSets.forEach(set => {
          setsById[set.setId] = set;
      });
      state.trackedSets.byId = setsById;
      state.trackedSets.allIds = apiSets.map(set => set.setId);

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
  }
});

export const selectSettingsApiStatus = (state: RootState): ApiStatus => {
  const readStatus = state.settings.trackedSets.readDataStatus;
  const writeStatus = state.settings.trackedSets.writeDataStatus;
  
  return (readStatus > writeStatus) ? readStatus : writeStatus;
}

export default settingsSlice.reducer;