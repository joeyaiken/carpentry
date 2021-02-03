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
    inventoryApi.getTrimmingToolCards(state.inventory.trimmingTool.searchProps).then((result) => {
        dispatch(trimmingToolCardsReceived(result));
    });
    
}

export const TRIMMING_TOOL_CARDS_REQUESTED = 'TRIMMING_TOOL.CARDS_REQUESTED';
export const trimmingToolCardsRequested = (): ReduxAction => ({
    type: TRIMMING_TOOL_CARDS_REQUESTED,
});

export const TRIMMING_TOOL_CARDS_RECEIVED = 'TRIMMING_TOOL.CARDS_RECEIVED';
export const trimmingToolCardsReceived = (cards: InventoryOverviewDto[]): ReduxAction => ({
    type: TRIMMING_TOOL_CARDS_RECEIVED,
    payload: cards,
});

export const CARD_IMAGE_ANCHOR_SET = 'TRIMMING_TOOL.CARD_IMAGE_ANCHOR_SET';
export const cardImageAnchorSet = (menuAnchor: HTMLButtonElement | null): ReduxAction => ({
    type: CARD_IMAGE_ANCHOR_SET,
    payload: menuAnchor
});

//TODO - Still need actions for submitting a payload of cards to trim
export const ADD_PENDING_CARD = 'TRIMMING_TOOL.ADD_PENDING_CARD';
export const addPendingCard = (name: string, cardId: number, isFoil: boolean) =>({
    type: ADD_PENDING_CARD,
    payload: {
        name: name,
        cardId: cardId,
        isFoil: isFoil,
    }
});

export const REMOVE_PENDING_CARD = 'TRIMMING_TOOL.REMOVE_PENDING_CARD';
export const removePendingCard = (name: string, cardId: number, isFoil: boolean) =>({
    type: REMOVE_PENDING_CARD,
    payload: {
        name: name,
        cardId: cardId,
        isFoil: isFoil,
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

    // dispatch(trimmingToolCardsRequested());
    
    // let searchProps = state.inventory.trimmingTool.searchProps;
    // searchProps.minCount = +searchProps.minCount;
    // inventoryApi.getTrimmingToolCards(state.inventory.trimmingTool.searchProps).then((result) => {
        // dispatch(trimmingToolCardsReceived(result));
    // });
    
}

// export const TRIM__CARDS_REQUESTED = 'TRIMMING_TOOL.TRIM_CARDS_REQUESTED';
// export const trimCardsRequested = (): ReduxAction => ({
//     type: TRIMMING_TOOL_CARDS_REQUESTED,
// });

// export const TRIM__CARDS_RECEIVED = 'TRIMMING_TOOL.TRIM_CARDS_RECEIVED';
// export const trimCardsReceived = (): ReduxAction => ({
//     type: TRIMMING_TOOL_CARDS_RECEIVED,
// });
