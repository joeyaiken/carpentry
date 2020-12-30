import { Dispatch } from 'redux';
import { decksApi } from '../../../api/decksApi';
import { AppState } from '../../../configureStore';

export const ensureTagDetailLoaded = (cardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryLoadTagDetail(dispatch, getState(), cardId, false);
    }
}

export const reloadTagDetail = (cardId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryLoadTagDetail(dispatch, getState(), cardId, true);
    }
}

function tryLoadTagDetail(dispatch: Dispatch, state: AppState, cardId: number, forceReload: boolean): void {

    const isLoading = state.decks.cardTags.isLoading;
    const activeCardId = state.decks.cardTags.cardId;
    if(isLoading || (!forceReload && activeCardId === cardId)) return;

    //const deckId = state.decks.
    //TODO - ensure this is actually populated
    //Might be worth to have it passed to the container
    const deckId = state.decks.data.detail.deckId; 

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

//requestAddCardTag
export const requestAddCardTag = (dto: CardTagDto): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryAddCardTag(dispatch, getState(), dto);
    } 
}

//tryAddCardTag
function tryAddCardTag(dispatch: Dispatch, state: AppState, dto: CardTagDto): void {
    const isLoading = state.decks.cardTags.isLoading;
    if(isLoading) return;

    dispatch(addTagRequested());

    decksApi.addCardTag(dto).then(() => {
        dispatch(addTagReceived());
        dispatch(reloadTagDetail(state.decks.cardTags.cardId));
    });
}

export const ADD_TAG_REQUESTED = 'CARD_TAGS.ADD_TAG_REQUESTED';
export const addTagRequested = (): ReduxAction => ({
    type: ADD_TAG_REQUESTED,
});
export const ADD_TAG_RECEIVED = 'CARD_TAGS.ADD_TAG_RECEIVED';
export const addTagReceived = (): ReduxAction => ({
    type: ADD_TAG_RECEIVED,
});

//requestRemoveCardTag
export const requestRemoveCardTag = (cardTagId: number): any => {
    return (dispatch: Dispatch, getState: any) => {
        tryRemoveCardTag(dispatch, getState(), cardTagId);
    } 
}

//tryRemoveCardTag
function tryRemoveCardTag(dispatch: Dispatch, state: AppState, cardTagId: number): void {
    const isLoading = state.decks.cardTags.isLoading;
    if(isLoading) return;

    dispatch(removeTagRequested());

    decksApi.removeCardTag(cardTagId).then(() => {
        dispatch(removeTagReceived());
        dispatch(reloadTagDetail(state.decks.cardTags.cardId));
    });
}

export const REMOVE_TAG_REQUESTED = 'CARD_TAGS.REMOVE_TAG_REQUESTED';
export const removeTagRequested = (): ReduxAction => ({
    type: REMOVE_TAG_REQUESTED,
});
export const REMOVE_TAG_RECEIVED = 'CARD_TAGS.REMOVE_TAG_RECEIVED';
export const removeTagReceived = (): ReduxAction => ({
    type: REMOVE_TAG_RECEIVED,
});

//UI
export const NEW_TAG_CHANGE = 'CARD_TAGS.NEW_TAG_CHANGE';
export const newTagChange = (value: string): ReduxAction => ({
    type: NEW_TAG_CHANGE,
    payload: value,
});
