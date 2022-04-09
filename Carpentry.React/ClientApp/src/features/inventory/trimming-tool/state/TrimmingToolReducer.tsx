import {
  TRIMMING_TOOL_FILTER_CHANGED,
  TRIMMING_TOOL_CARDS_REQUESTED,
  TRIMMING_TOOL_CARDS_RECEIVED,
  CARD_IMAGE_ANCHOR_SET,
  ADD_PENDING_CARD,
  REMOVE_PENDING_CARD,
  CLEAR_PENDING_CARDS,
  TRIM_CARDS_REQUESTED,
  TRIM_CARDS_RECEIVED,
} from './TrimmingToolActions';

export interface State {
  searchProps: TrimmingToolRequest;

  searchResults: {
    isLoading: boolean;
    byId: { [id: number]: TrimmingToolResult };
    allIds: number[];
  };

  pendingCards: {
    isSaving: boolean;
    byId: { [cardId: number]: TrimmedCard };
  }

  cardImageMenuAnchor: HTMLButtonElement | null;
}

export const trimmingToolReducer = (state = initialState, action: ReduxAction): State => {
  switch(action.type){
    case TRIMMING_TOOL_FILTER_CHANGED: return filterChanged(state, action);
    case TRIMMING_TOOL_CARDS_REQUESTED: return searchRequested(state, action);
    case TRIMMING_TOOL_CARDS_RECEIVED: return searchReceived(state, action);
    case CARD_IMAGE_ANCHOR_SET: return { ...state, cardImageMenuAnchor: action.payload }
    case ADD_PENDING_CARD: return addPendingCard(state, action);
    case REMOVE_PENDING_CARD: return removePendingCard(state, action);
    case TRIM_CARDS_REQUESTED: return trimRequested(state, action);
    case TRIM_CARDS_RECEIVED: return trimReceived(state, action);
    case CLEAR_PENDING_CARDS: return {...state, pendingCards: { byId: {}, isSaving: false }}
    default: return(state);
  }
}

const initialState: State = {
  searchProps: {
    setCode: 'znr',
    searchGroup: 'Red',
    minCount: 8,
    maxPrice: 0.1,
    filterBy: 'inventory',
  },

  searchResults: {
    isLoading: false,
    byId: {},
    allIds: [],
  },

  pendingCards: {
    isSaving: false,
    byId: {},
  },

  cardImageMenuAnchor: null,
};

function searchRequested(state: State, action: ReduxAction): State {
  const newState: State = {
    ...state,
    searchResults: {
      ...state.searchResults,
      ...initialState.searchResults,
      isLoading: true,
    }
  }
  return newState;
}

function searchReceived(state: State, action: ReduxAction): State {
  const searchResultPayload: InventoryOverviewDto[] = action.payload || [];
  let resultsById = {};
  searchResultPayload.forEach(card => resultsById[card.id] = card);
  const newState: State = {
    ...state,
    searchResults: {
      ...state.searchResults,
      isLoading: false,
      byId: resultsById,
      allIds: searchResultPayload.map(card => card.id),
    }
  }
  return newState;
}

function trimRequested(state: State, action: ReduxAction): State {
  const newState: State = {
    ...state,
    pendingCards: {
      ...state.pendingCards,
      isSaving: true,
    }
  }
  return newState;
}

function trimReceived(state: State, action: ReduxAction): State {
  const newState: State = {
    ...state,
    pendingCards: {
      ...initialState.pendingCards,
    }
  }
  return newState;
}

const addPendingCard = (state: State, action: ReduxAction): State => {
  const {
    card,
    count,
  } = action.payload;
  const cardResult: TrimmingToolResult = card;
  let pendingCard = state.pendingCards.byId[cardResult.cardId];
  
  if(!pendingCard){
    pendingCard = {
      data: card,
      numberToTrim: 0,
    };
  }
  
  pendingCard.numberToTrim += count;

  const newState: State = {
    ...state,
    pendingCards: {
      ...state.pendingCards,
      byId: {
        ...state.pendingCards.byId,
        [card.id]: pendingCard,
      }
    }
  }
  return newState;
}

const removePendingCard = (state: State, action: ReduxAction): State => {
  const {
    card,
    isFoil,
  } = action.payload;

  const cardResult: TrimmingToolResult = card;
  let pendingCard = state.pendingCards.byId[card.id];

  if(pendingCard){
    pendingCard.numberToTrim--;
    let pendingCardsAfterRemoval =  {
      ...state.pendingCards.byId,
      [card.id]: pendingCard
    }

    //if this pending cards object has 0 items, it should be deleted from the dictionary
    if(pendingCard.numberToTrim === 0){
      delete pendingCardsAfterRemoval[card.id];
    }

    const newState: State = {
      ...state,
      pendingCards: {
        ...state.pendingCards,
        byId: pendingCardsAfterRemoval
      }
    }
    return newState;

  } else {
    const newState: State = {
      ...state,
    }
    return newState;
  }
}

const filterChanged = (state: State, action: ReduxAction): State => {
  const { filter, value } = action.payload;
  const newState: State = {
    ...state,
    searchProps: {
      ...state.searchProps,
      [filter]: value,
    }
  }
  return newState;
}