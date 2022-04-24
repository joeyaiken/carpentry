import { createSlice } from '@reduxjs/toolkit';
// import {State} from "./state/InventoryAddCardsReducer";
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';

export interface State {
  
}


export const inventoryAddCardsSlice = createSlice({
  name: 'inventoryAddCards',
  initialState: {
    
  },
  reducers: {
    searchRequested: (state: State) => {
      state.searchResults.isLoading = true;
    },

    searchReceived: (state: State, action: PayloadAction<CardSearchResultDto[]>) => {
      const searchResultPayload: CardSearchResultDto[] = action.payload || [];
      let resultsById: { [cardId: number]: CardSearchResultDto} = {};
      searchResultPayload.forEach(card => resultsById[card.cardId] = card);
      
      state.searchResults = {
        isLoading: false,
        searchResultsById: resultsById,
        allSearchResultIds: searchResultPayload.map(card => card.cardId),
      }
    },
    
    //TODO - replace redux action with an actual type
    cardSearchAddPendingCard: (state: State, action: ReduxAction) => {
      //'pending cards' is now a dictionary of 'pending card dto's
      const {
        name,
        cardId,
        isFoil,
      } = action.payload;
    
      let cardToAdd: PendingCardsDto = state.pendingCards[name];
    
      if(!cardToAdd){
        cardToAdd = {
          name: name,
          cards: [],
        };
      }

      //These are the only 3 fields used by the api bulkAdd
      cardToAdd.cards.push({
        cardId: cardId,
        isFoil: isFoil,
        statusId: 1,
      } as InventoryCard);
    
      const newState: State = {
        ...state,
        pendingCards: {
          ...state.pendingCards,
          [name]: cardToAdd
        }
      }
      return newState;
    },

    //TODO - replace redux action with an actual type
    cardSearchRemovePendingCard: (state: State, action: ReduxAction) => {
      const {
        name,
        cardId,
        isFoil,
      } = action.payload;
    
      let objToRemoveFrom = state.pendingCards[name];
    
      if(objToRemoveFrom){
        let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.cardId === cardId && x.isFoil === isFoil);
        if(thisInvCard >= 0){
          objToRemoveFrom.cards.splice(thisInvCard,1);
        }
    
        let pendingCardsAfterRemoval =  {
          ...state.pendingCards,
          [name]: objToRemoveFrom
        }
        //if this pending cards object has 0 items, it should be deleted from the dictionary
        if(objToRemoveFrom.cards.length === 0){
          delete pendingCardsAfterRemoval[name];
        }
        const newState: State = {
          ...state,
          pendingCards: pendingCardsAfterRemoval
        }
        return newState;
    
      } else {
        const newState: State = {
          ...state,
        }
        return newState;
      }
    },

    toggleSearchViewMode: (state: State, action: ReduxAction) => {
      let newViewMode: CardSearchViewMode = "list";
    
      switch(state.viewMode){
        case "list":
          newViewMode = "grid";
          break;
        case "grid":
          newViewMode = "list";
          break;
      }
      
      state.viewMode = newViewMode;
    },

    // TODO - this should be local component state
    //
    // const filterValueChanged = (state: State, action: ReduxAction): State => {
    //   const { type, filter, value } = action.payload;
    //   const existingFilter = state.searchFilter;
    //   const newState: State = {
    //     ...state,
    //     searchFilter: {
    //       ...existingFilter,
    //       [filter]: value,
    //     }
    //   }
    //   return newState;
    // },
  },
});



//actions

//selectors

export default inventoryAddCardsSlice.reducer;