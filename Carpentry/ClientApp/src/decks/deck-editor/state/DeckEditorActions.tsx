// import { Dispatch } from 'redux';




export const TOGGLE_DECK_VIEW_MODE = 'TOGGLE_DECK_VIEW_MODE';
export const toggleDeckViewMode = (): ReduxAction => ({
    type: TOGGLE_DECK_VIEW_MODE,
});

export const DECK_EDITOR_CARD_SELECTED = 'DECK_EDITOR_CARD_SELECTED';
export const deckEditorCardSelected = (cardOverview: DeckCardOverview): ReduxAction => ({
    type: DECK_EDITOR_CARD_SELECTED,
    payload: cardOverview
})