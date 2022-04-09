import { Dispatch } from 'redux'
import { inventoryApi } from '../../../api/inventoryApi';
import { AppState } from '../../../configureStore';

export const TRIMMING_TOOL_FILTER_CHANGED = 'TRIMMING_TOOL.FILTER_CHANGED';
export const trimmingToolFilterChanged = (filter: string, value: string | boolean): ReduxAction => ({
  type: TRIMMING_TOOL_FILTER_CHANGED,
  payload: {
    filter: filter,
    value: value
  }
});

export const requestTrimmingToolCards = (): any => {
  return (dispatch: Dispatch, getState: any) => {
    tryLoadTrimmingToolCards(dispatch, getState());
  }
}

function tryLoadTrimmingToolCards(dispatch: Dispatch, state: AppState): any {
  const isLoading = state.inventory.trimmingTool.searchResults.isLoading;

  if(isLoading) return;

  dispatch(trimmingToolCardsRequested());

  let searchProps = state.inventory.trimmingTool.searchProps;
  searchProps.minCount = +searchProps.minCount;
  searchProps.maxPrice = +searchProps.maxPrice;
  inventoryApi.getTrimmingToolCards(state.inventory.trimmingTool.searchProps).then((result) => {
    dispatch(trimmingToolCardsReceived(result));
  });
}

export const TRIMMING_TOOL_CARDS_REQUESTED = 'TRIMMING_TOOL.CARDS_REQUESTED';
export const trimmingToolCardsRequested = (): ReduxAction => ({
  type: TRIMMING_TOOL_CARDS_REQUESTED,
});

export const TRIMMING_TOOL_CARDS_RECEIVED = 'TRIMMING_TOOL.CARDS_RECEIVED';
export const trimmingToolCardsReceived = (cards: TrimmingToolResult[]): ReduxAction => ({
  type: TRIMMING_TOOL_CARDS_RECEIVED,
  payload: cards,
});

export const CARD_IMAGE_ANCHOR_SET = 'TRIMMING_TOOL.CARD_IMAGE_ANCHOR_SET';
export const cardImageAnchorSet = (menuAnchor: HTMLButtonElement | null): ReduxAction => ({
  type: CARD_IMAGE_ANCHOR_SET,
  payload: menuAnchor
});

export const ADD_PENDING_CARD = 'TRIMMING_TOOL.ADD_PENDING_CARD';
export const addPendingCard = (card: TrimmingToolResult, count: number) =>({
  type: ADD_PENDING_CARD,
  payload: {
    card: card,
    count: count,
  }
});

export const REMOVE_PENDING_CARD = 'TRIMMING_TOOL.REMOVE_PENDING_CARD';
export const removePendingCard = (card: TrimmingToolResult) =>({
  type: REMOVE_PENDING_CARD,
  payload: {
    card: card,
  }
});

export const CLEAR_PENDING_CARDS = 'TRIMMING_TOOL.CLEAR_PENDING_CARDS'
export const clearPendingCards = () =>({
  type: CLEAR_PENDING_CARDS
});

export const requestTrimCards = (): any => {
  return (dispatch: Dispatch, getState: any) => {
    tryTrimCards(dispatch, getState());
  }
}

function tryTrimCards(dispatch: Dispatch, state: AppState): any {
  const isSaving = state.inventory.trimmingTool.pendingCards.isSaving;
  if(isSaving) return;

  dispatch(trimCardsRequested());

  let cardsToTrim: TrimmedCardDto[] = [];
  const { byId } = state.inventory.trimmingTool.pendingCards;
  Object.keys(byId).forEach(id => {
    const card: TrimmedCard = byId[id];
    if(card.numberToTrim) {
      cardsToTrim.push({
        cardId: card.data.cardId,
        cardName: card.data.name,
        isFoil: card.data.isFoil ?? false,
        numberToTrim: card.numberToTrim,
      });
    }
  });

  inventoryApi.trimCards(cardsToTrim).then(() => {
    dispatch(trimCardsReceived());
    dispatch(requestTrimmingToolCards());
  });

}

export const TRIM_CARDS_REQUESTED = 'TRIMMING_TOOL.TRIM_CARDS_REQUESTED';
export const trimCardsRequested = (): ReduxAction => ({
  type: TRIM_CARDS_REQUESTED,
});

export const TRIM_CARDS_RECEIVED = 'TRIMMING_TOOL.TRIM_CARDS_RECEIVED';
export const trimCardsReceived = (): ReduxAction => ({
  type: TRIM_CARDS_RECEIVED,
});
