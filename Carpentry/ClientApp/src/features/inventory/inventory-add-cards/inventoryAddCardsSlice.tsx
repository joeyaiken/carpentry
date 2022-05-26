import { createSlice } from '@reduxjs/toolkit';
// import {State} from "./state/InventoryAddCardsReducer";
import { PayloadAction } from '@reduxjs/toolkit/dist/createAction';
import {RootState} from "../../../app/store";

export interface State {
  searchResults: {
    //TODO - should be a 'status' enum instead of an isLoading boolean
    isLoading: boolean;
    searchResultsById: {[multiverseId: number]: CardSearchResultDto};
    allSearchResultIds: number[];
  };

  searchFilter: CardFilterProps;
  viewMode: CardSearchViewMode;

  //Consider grouping this in an obj like searchResults
  pendingCardsSaving: boolean;
  pendingCards: { [name: string]: PendingCardsDto } //key === name, should this also have a list to track all keys?
  //

  selectedCard: CardSearchResultDto | null; //should probably be an AppState ID
}

const initialState: State = {
  searchResults: {
    isLoading: false,
    searchResultsById: {},
    allSearchResultIds: [],
  },
  searchFilter: {
    set: '',
    colorIdentity: [],
    rarity: [],
    type: '',
    exclusiveColorFilters: false,
    multiColorOnly: false,
    cardName: '',
    exclusiveName: false,
    maxCount: 0,
    minCount: 0,
    format: '',
    text: '',
    group: '',
  },
  viewMode: "list",
  pendingCardsSaving: false,
  selectedCard:  null, //should probably be an AppState ID
  pendingCards: {},
};

export const inventoryAddCardsSlice = createSlice({
  name: 'inventoryAddCards',
  initialState: initialState,
  reducers: {
    // searchRequested: (state: State) => {
    //   state.searchResults.isLoading = true;
    // },
    //
    // searchReceived: (state: State, action: PayloadAction<CardSearchResultDto[]>) => {
    //   const searchResultPayload: CardSearchResultDto[] = action.payload || [];
    //   let resultsById: { [cardId: number]: CardSearchResultDto} = {};
    //   searchResultPayload.forEach(card => resultsById[card.cardId] = card);
    //  
    //   state.searchResults = {
    //     isLoading: false,
    //     searchResultsById: resultsById,
    //     allSearchResultIds: searchResultPayload.map(card => card.cardId),
    //   }
    // },
    //

    cardSearchSelectCard: (state, action: PayloadAction<CardSearchResultDto>) => {
      state.selectedCard = action.payload;
    },
      

    
    
    //TODO - replace redux action with an actual type
    addPendingCard: (state, action: PayloadAction<{
      name: string,
      cardId: number,
      isFoil: boolean,
    }>) => {
      //'pending cards' is now a dictionary of 'pending card dto's
      const {
        name,
        cardId,
        isFoil,
      } = action.payload;
    
      let pendingCard: PendingCardsDto = state.pendingCards[name];
    
      if(!pendingCard){
        pendingCard = {
          name: name,
          cards: [],
        };
      }

      //These are the only 3 fields used by the api bulkAdd
      pendingCard.cards.push({
        cardId: cardId,
        isFoil: isFoil,
        statusId: 1,
      } as InventoryCard);
      
      state.pendingCards[name] = pendingCard;
      
      
      //
      // const newState: State = {
      //   ...state,
      //   pendingCards: {
      //     ...state.pendingCards,
      //     [name]: pendingCard
      //   }
      // }
      // return newState;
    },

    //TODO - replace redux action with an actual type
    removePendingCard: (state, action: PayloadAction<{
      name: string,
      cardId: number,
      isFoil: boolean,
    }>) => {
      const {
        name,
        cardId,
        isFoil,
      } = action.payload;

      let pendingCardToRemoveFrom = state.pendingCards[name];

      if(pendingCardToRemoveFrom){
        let thisInvCard = pendingCardToRemoveFrom.cards.findIndex(x => x.cardId === cardId && x.isFoil === isFoil);
        if(thisInvCard >= 0){
          pendingCardToRemoveFrom.cards.splice(thisInvCard,1);
        }

        
        
        
        //This might just happen through connections...
        state.pendingCards[name] = pendingCardToRemoveFrom;
        
        // let pendingCardsAfterRemoval =  {
        //   ...state.pendingCards,
        //   [name]: pendingCardToRemoveFrom
        // }
        
        
        
        
        //if this pending cards object has 0 items, it should be deleted from the dictionary
        if(pendingCardToRemoveFrom.cards.length === 0){
          // delete pendingCardsAfterRemoval[name];
          delete state.pendingCards[name];
        }
        // state.pendingCards = pendingCardsAfterRemoval;
        
        
        // const newState: State = {
        //   ...state,
        //   pendingCards: pendingCardsAfterRemoval
        // }
        // return newState;

        // } else {
        // const newState: State = {
        //   ...state,
        // }
        // return newState;
      }
    },

    // toggleSearchViewMode: (state: State, action: ReduxAction) => {
    //   let newViewMode: CardSearchViewMode = "list";
    //
    //   switch(state.viewMode){
    //     case "list":
    //       newViewMode = "grid";
    //       break;
    //     case "grid":
    //       newViewMode = "list";
    //       break;
    //   }
    //
    //   state.viewMode = newViewMode;
    // },

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

// This is probably a bad selector
export const selectSearchResultItem = (state: RootState, cardId: number): CardListItem => {
  const card = state.inventory.inventoryAddCards.searchResults.searchResultsById[cardId];
  const pendingCard = state.inventory.inventoryAddCards.pendingCards[card.name]
  return {
    data: card,
    count: pendingCard && pendingCard.cards.length,
  } as CardListItem;
}



const selectSearchResults = (state: RootState): CardListItem[] => {
  const { allSearchResultIds, searchResultsById } = state.inventory.inventoryAddCards.searchResults;
  const containerState = state.inventory.inventoryAddCards;
  
  return allSearchResultIds.map(cardId => {
    
    const card = searchResultsById[cardId];
    
    return {
      data: card,
      
      //on the search result list
      count: containerState.pendingCards[card.name] && containerState.pendingCards[card.name].cards.length,
    } as CardListItem;
    
  });
  
  
  //
  // const searchResults: CardSearchResultDto[] = allSearchResultIds.map(cid => searchResultsById[cid])
  //
  // const mappedSearchResults: CardListItem[] = searchResults.map(card => ({
  //   data: card,
  //   count: containerState.pendingCards[card.name] && containerState.pendingCards[card.name].cards.length,
  // }) as CardListItem);
  //
  // return mappedSearchResults;
  
}



//actions
export const {
  cardSearchSelectCard,
  addPendingCard,
  removePendingCard,
} = inventoryAddCardsSlice.actions;

//selectors

export default inventoryAddCardsSlice.reducer;