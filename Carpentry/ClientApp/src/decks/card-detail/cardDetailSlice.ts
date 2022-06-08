import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {inventoryApi} from "../../api/inventoryApi";
import {ApiStatus} from "../../enums";

export interface State {
  // isLoading: boolean;
  status: ApiStatus;
  activeCardId: number;
  activeCardName: string;

  inventoryCards: {
    byId: { [id: number]: InventoryCard };
    allIds: number[];
  }

  cards:{ //card definitions
    byId: { [multiverseId: number]: MagicCard };
    allIds: number[];
  }

  cardGroups: { [cardId: number]: number[] }

  deckCardMenuAnchor: HTMLButtonElement | null;
  deckCardMenuAnchorId: number;
  inventoryCardMenuAnchor: HTMLButtonElement | null;
  inventoryCardMenuAnchorId: number;
}

const initialState: State = {
  // isLoading: false,
  status: ApiStatus.uninitialized,
  activeCardId: 0,
  activeCardName: '',
  inventoryCards: {
    byId: {},
    allIds: [],
  },
  cards: {
    byId: {},
    allIds: [],
  },
  cardGroups: {},

  deckCardMenuAnchor: null,
  deckCardMenuAnchorId: 0,
  inventoryCardMenuAnchor: null,
  inventoryCardMenuAnchorId: 0,
}

export const loadCardDetail = createAsyncThunk<InventoryDetailDto, number>(
  'cardDetail/loadCardDetail',
  async (cardId) => inventoryApi.getInventoryDetail(cardId)
)

const cardDetailSlice = createSlice({
  name: 'cardDetail',
  initialState: initialState,
  reducers: {
    // exampleAction: (state) => {
    //  
    // }
    
  },
  extraReducers: (builder) => {
    

    builder.addCase(loadCardDetail.pending, (state) => {
      state.status = ApiStatus.loading;
    });
    builder.addCase(loadCardDetail.fulfilled, (state) => {
      state.status = ApiStatus.initialized;
      
    });
    builder.addCase(loadCardDetail.rejected, (state, action) => {
      console.error('loadCardDetail thunk rejected: ', action);
      state.status = ApiStatus.errored;
    });
    
  }
})



export default cardDetailSlice.reducer;