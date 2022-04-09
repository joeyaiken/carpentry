import { createSlice } from '@reduxjs/toolkit';
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';

export interface State {
  viewMethod: "grid" | "table";
  filters: InventoryFilterProps;
  cardImageMenuAnchor: HTMLButtonElement | null;
}

export const inventoryOverviewSlice = createSlice({
  name: 'inventoryOverview',
  initialState: {
    viewMethod: "grid",
    filters: initialCardSearchFilterProps(),
    cardImageMenuAnchor: null,
  },
  reducers: {
    inventoryOverviewFilterChanged: (state: State, action: PayloadAction<{filter: string, value: string | boolean}>) => {
      const { filter, value } = action.payload;
      state.filters = {
        ...state.filters,
        [filter]: value,
      }
    },
    cardMenuButtonClick: (state: State, action: PayloadAction<HTMLButtonElement>) => {
      state.cardImageMenuAnchor = action.payload;
    },
    quickFilterApplied: (state: State, action: PayloadAction<InventoryFilterProps>) => {
      state.filters = action.payload;
    }
  },
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