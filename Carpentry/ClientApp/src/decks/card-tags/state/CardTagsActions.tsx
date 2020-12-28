import { Dispatch } from 'redux';
import { decksApi } from '../../../api/decksApi';
import { AppState } from '../../../configureStore';

export const ensureTagDetailLoaded = (cardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryLoadTagDetail(dispatch, getState(), cardId, false);
    } 
}

function tryLoadTagDetail(dispatch: Dispatch, state: AppState, cardId: number, forceReload: boolean): void {

    const isLoading = state.decks.cardTags.isLoading;
    const activeCardId = state.decks.cardTags.cardId;
    if(isLoading || (!forceReload && activeCardId === cardId)) return;

    //const deckId = state.decks.
    const deckId = 0; //TODO - need to populate this properly

    dispatch(tagDetailRequested());

    decksApi.getCardTagDetails(deckId, cardId).then((result) => {
        dispatch(tagDetailReceived(result));
    });
}

export const TAG_DETAIL_REQUESTED = 'CARD_TAGS.TAG_DETAIL_REQUESTED';
export const tagDetailRequested = (): ReduxAction => ({
    type: TAG_DETAIL_REQUESTED,
});

export const TAG_DETAIL_RECEIVED = 'CARD_TAGS.TAG_DETAIL_RECEIVED';
export const tagDetailReceived = (payload: CardTagDetailDto): ReduxAction => ({
    type: TAG_DETAIL_RECEIVED,
    payload: payload,
});

export const NEW_TAG_CHANGE = 'CARD_TAGS.NEW_TAG_CHANGE';
export const newTagChange = (value: string): ReduxAction => ({
    type: NEW_TAG_CHANGE,
    payload: value,
});

//tryAddCardTag

export const ADD_TAG_REQUESTED = 'CARD_TAGS.ADD_TAG_REQUESTED';
export const addTagRequested = (): ReduxAction => ({
    type: ADD_TAG_REQUESTED,
});
export const ADD_TAG_RECEIVED = 'CARD_TAGS.ADD_TAG_RECEIVED';
export const addTagReceived = (): ReduxAction => ({
    type: ADD_TAG_RECEIVED,
});

//tryRemoveCardTag

export const REMOVE_TAG_REQUESTED = 'CARD_TAGS.REMOVE_TAG_REQUESTED';
export const removeTagRequested = (): ReduxAction => ({
    type: REMOVE_TAG_REQUESTED,
});
export const REMOVE_TAG_RECEIVED = 'CARD_TAGS.REMOVE_TAG_RECEIVED';
export const removeTagReceived = (): ReduxAction => ({
    type: REMOVE_TAG_RECEIVED,
});