import {createSlice,createAsyncThunk} from '@reduxjs/toolkit';
import {ApiStatus} from "../../../enums";
import {inventoryApi} from "../../../api/inventoryApi";

export interface State {
  viewMethod: "grid" | "table";
  filters: InventoryFilterProps;
  cardImageMenuAnchor: HTMLButtonElement | null;
  data: {
    status: ApiStatus
    byId: { [id: number]: InventoryOverviewDto };
    allIds: number[];
  }
}

const initialState: State = {
  viewMethod: "grid",
  filters: initialCardSearchFilterProps(),
  cardImageMenuAnchor: null,
  data: {
    status: ApiStatus.uninitialized,
    byId: {},
    allIds: [],
  }
}

//

export const getInventoryOverviews = createAsyncThunk<InventoryOverviewDto[], InventoryQueryParameter>(
  'inventoryOverview/getInventoryOverviews',
  async (props) => inventoryApi.searchCards(props)
)


export const inventoryOverviewSlice = createSlice({
  name: 'inventoryOverview',
  initialState: initialState,
  reducers: {
    // inventoryOverviewFilterChanged: (state: State, action: PayloadAction<{filter: string, value: string | boolean}>) => {
    //   const { filter, value } = action.payload;
    //   state.filters = {
    //     ...state.filters,
    //     [filter]: value,
    //   }
    // },
    // cardMenuButtonClick: (state: State, action: PayloadAction<HTMLButtonElement>) => {
    //   state.cardImageMenuAnchor = action.payload;
    // },
    // quickFilterApplied: (state: State, action: PayloadAction<InventoryFilterProps>) => {
    //   state.filters = action.payload;
    // }
  },
  extraReducers: (builder) => {
    
  }
});

function initialCardSearchFilterProps(): InventoryFilterProps {
  return {
    groupBy: "unique",
    sortBy: "price",
    set: "",
    text: "",
    exclusiveColorFilters: false,
    multiColorOnly: false,
    skip: 0,
    take: 25,
    type: "",
    colorIdentity: [],
    rarity: [],
    minCount: 0,
    maxCount: 0,
    sortDescending: true,
  } as InventoryFilterProps;
} 


export default inventoryOverviewSlice.reducer;