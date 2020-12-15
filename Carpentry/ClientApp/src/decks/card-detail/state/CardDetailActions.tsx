import { FormatAlignCenterRounded } from '@material-ui/icons';
import { Dispatch } from 'redux';
import { inventoryApi } from '../../../api/inventoryApi';
import { AppState } from '../../../configureStore';

export const ensureCardDetailLoaded = (cardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryLoadCardDetail(dispatch, getState(), cardId, false);
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