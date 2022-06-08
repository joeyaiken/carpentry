import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {ApiStatus} from "../../../enums";
import {inventoryApi} from "../../../api/inventoryApi";

export interface State {

  selectedCardId: number | null;
  // selectedCardName: string;
  //
  // cardGroups: { [cardId: number]: number[] }
  
  data: {
    status: ApiStatus,

    selectedCardName: string,
    
    // inventory cards
    inventoryCardsById: { [id: number]: InventoryCard };
    inventoryCardAllIds: number[];

    // magic cards belonging to inventory cards
    cardsById: { [multiverseId: number]: MagicCard };
    allCardIds: number[];

    // populated after data is received, won't change between queries 
    cardGroups: { [cardId: number]: number[] }
  },
}

const initialState: State = {
  
  selectedCardId: null,
  
  data: {
    status: ApiStatus.uninitialized,
    
    selectedCardName: "unknown",
    
    inventoryCardsById: {},
    inventoryCardAllIds: [],
    
    cardsById: {},
    allCardIds: [],
    
    cardGroups: {},
  }
}

export const loadInventoryDetail = createAsyncThunk<InventoryDetailDto, number>(
  'inventoryDetail/loadInventoryDetail',
  async (cardId) => 
    inventoryApi.getInventoryDetail(cardId)
)

export const inventoryDetailSlice = createSlice({
  name: 'inventoryDetail',
  initialState: initialState,
  reducers: { },
  extraReducers: (builder) => {
    builder.addCase(loadInventoryDetail.pending, (state, action) => {
      state.data.status = ApiStatus.loading;
      state.selectedCardId = action.meta.arg;
      
      state.data.inventoryCardsById = {};
      state.data.inventoryCardAllIds = [];
      
      state.data.cardsById = {};
      state.data.allCardIds = [];
    });
    builder.addCase(loadInventoryDetail.fulfilled, (state, action) => {

      const detailResult: InventoryDetailDto | null = action.payload;
      
      if(detailResult !== null){
        
        let inventoryCardsById = {}
        const allInventoryCardIds = detailResult.inventoryCards.map(inventoryCard => {
          inventoryCardsById[inventoryCard.id] = inventoryCard;
          return inventoryCard.id;
        });
        
        let cardsById = {}
        const allCardIds = detailResult.cards.map(card => {
          cardsById[card.cardId] = card;
          return card.cardId;
        });
        
        // fill card groups
        let cardGroups: { [cardId: number]: number[] } = {};
        allCardIds.forEach(cardId => {
          const thisCard = cardsById[cardId];
          cardGroups[cardId] = allInventoryCardIds
            .filter(inventoryCardId => inventoryCardsById[inventoryCardId].set === thisCard.set
              && inventoryCardsById[inventoryCardId].collectorNumber === thisCard.collectionNumber);
        });

        state.data.selectedCardName = detailResult.name;
        
        state.data.inventoryCardsById = inventoryCardsById;
        state.data.inventoryCardAllIds = allInventoryCardIds;
        
        state.data.cardsById = cardsById;
        state.data.allCardIds = allCardIds;
        
        state.data.cardGroups = cardGroups;
      }
      
      state.data.status = ApiStatus.initialized;
    });
    builder.addCase(loadInventoryDetail.rejected, (state, action) => {
      console.error('trimCards thunk rejected: ', action);
      state.data.status = ApiStatus.errored;
    });
  }
})

export default inventoryDetailSlice.reducer;