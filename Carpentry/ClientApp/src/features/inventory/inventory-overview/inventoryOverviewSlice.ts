import {createSlice,createAsyncThunk} from '@reduxjs/toolkit';
import {ApiStatus} from "../../../enums";
import {inventoryApi} from "../../../api/inventoryApi";

export interface State {
  data: {
    status: ApiStatus
    byId: { [id: number]: InventoryOverviewDto };
    allIds: number[];
  }
}

const initialState: State = {
  data: {
    status: ApiStatus.uninitialized,
    byId: {},
    allIds: [],
  }
}

export const getInventoryOverviews = createAsyncThunk<InventoryOverviewDto[], InventoryFilterProps>(
  'inventoryOverview/getInventoryOverviews',
  async (filterProps) => {
    const param: InventoryQueryParameter = {
      groupBy: filterProps.groupBy,
      text: filterProps.text,
      colors: filterProps.colorIdentity,
      skip: +filterProps.skip,
      take: +filterProps.take,
      sort: filterProps.sortBy,
      sortDescending: filterProps.sortDescending,
      set: filterProps.set,
      exclusiveColorFilters: filterProps.exclusiveColorFilters,
      multiColorOnly: filterProps.multiColorOnly,
      maxCount: +filterProps.maxCount,
      minCount: +filterProps.minCount,
      type: filterProps.type,
      rarity: filterProps.rarity,
    }
    return await inventoryApi.searchCards(param)
  }
)

export const inventoryOverviewSlice = createSlice({
  name: 'inventoryOverview',
  initialState: initialState,
  reducers: { },
  extraReducers: (builder) => {
    builder.addCase(getInventoryOverviews.pending, (state, action) => {
      state.data.allIds = [];
      state.data.byId = {};
      state.data.status = ApiStatus.loading;
    });
    builder.addCase(getInventoryOverviews.fulfilled, (state, action) => {
      let overviewsById = {}
      state.data.allIds = action.payload.map(item => {
        overviewsById[item.id] = item;
        return item.id;
      });
      state.data.byId = overviewsById;
      state.data.status = ApiStatus.initialized;
    });
    builder.addCase(getInventoryOverviews.rejected, (state, action) => {
      console.error('getInventoryOverviews thunk rejected: ', action);
      state.data.status = ApiStatus.errored;
    });
  }
});

export default inventoryOverviewSlice.reducer;