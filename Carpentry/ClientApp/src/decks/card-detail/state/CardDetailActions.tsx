import { Dispatch } from 'redux';
import { inventoryApi } from '../../../api/inventoryApi';
import { AppState } from '../../../configureStore';

export const ensureCardDetailLoaded = (cardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryLoadCardDetail(dispatch, getState(), cardId, false);
    }
}

export const forceLoadCardDetail = (cardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryLoadCardDetail(dispatch, getState(), cardId, true);
    }
}

export const CARD_DETAIL_REQUESTED = 'DECK_CARD_DETAIL.CARD_DETAIL_REQUESTED';
export const cardDetailRequested = (): ReduxAction => ({
    type: CARD_DETAIL_REQUESTED,
});

export const CARD_DETAIL_RECEIVED = 'DECK_CARD_DETAIL.CARD_DETAIL_RECEIVED';
export const cardDetailReceived = (payload: InventoryDetailDto): ReduxAction => ({
    type: CARD_DETAIL_RECEIVED,
    payload: payload,
});

function tryLoadCardDetail(dispatch: Dispatch, state: AppState, cardId: number, forceReload: boolean): void {

    const isLoading = state.decks.cardDetail.isLoading;
    const activeCardId = state.decks.cardDetail.activeCardId;
    if(isLoading || (!forceReload && activeCardId === cardId)) return;

    dispatch(cardDetailRequested());

    inventoryApi.getInventoryDetail(cardId).then((result) => {
        dispatch(cardDetailReceived(result));
    });
}

//whatever, everything's unique, I can refactor & reduce later

export const DECK_CARD_MENU_BUTTON_CLICKED = 'DECK_CARD_DETAIL.DECK_CARD_MENU_BUTTON_CLICKED'
export const deckCardMenuButtonClicked = (cardMenuAnchor: HTMLElement | null): ReduxAction => ({
    type: DECK_CARD_MENU_BUTTON_CLICKED,
    payload: cardMenuAnchor
});

export const INVENTORY_CARD_MENU_BUTTON_CLICKED = 'DECK_CARD_DETAIL.INVENTORY_CARD_MENU_BUTTON_CLICKED';
export const inventoryCardMenuButtonClicked = (cardMenuAnchor: HTMLElement | null): ReduxAction => ({
    type: INVENTORY_CARD_MENU_BUTTON_CLICKED,
    payload: cardMenuAnchor
});



//handleDeckCardMenuClick
// handleInventoryCardMenuClick

/*
    cardMenuButtonClick(deck|inventory, Element)

    deckCardMenuButtonClicked(Element)
    inventoryCardMenuButtonClicked(Element)

    [deck|inventory]MenuButtonClicked

export const  = 'DECK_CARD_DETAIL.DECK_CARD_MENU_BUTTON_CLICKED'
export const deckCardMenuButtonClicked = (cardMenuAnchor: HTMLElement | null): ReduxAction => ({
    type: DECK_CARD_MENU_BUTTON_CLICKED,
    payload: cardMenuAnchor
});

export const  = 'DECK_CARD_DETAIL.INVENTORY_CARD_MENU_BUTTON_CLICKED';
export const inventoryCardMenuButtonClicked = (cardMenuAnchor: HTMLElement | null): ReduxAction => ({
    type: INVENTORY_CARD_MENU_BUTTON_CLICKED,
    payload: cardMenuAnchor
});
DECK_CARD_MENU_BUTTON_CLICKED
INVENTORY_CARD_MENU_BUTTON_CLICKED
*/

