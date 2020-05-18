// import Redux, { Store, Dispatch, compose, combineReducers } from 'redux';
// import { AppState } from '../reducers';
// import { api_Decks_Search, api_Inventory_GetByName, api_Decks_Delete } from './api';

export const FILTER_VALUE_CHANGED = 'FILTER_VALUE_CHANGED';
export const filterValueChanged = (type: 'inventoryFilterProps' | 'cardSearchFilterProps', filter: string, value: string | boolean): ReduxAction => ({
    type: FILTER_VALUE_CHANGED,
    payload: {
        type: type,
        filter: filter,
        value: value,
    }
});

export const MENU_BUTTON_CLICKED = 'MENU_BUTTON_CLICKED';
//MenuAnchorOptions
//export const menuButtonClicked = (anchorType: 'deckListMenuAnchor' | 'cardMenuAnchor', deckListMenuAnchor: HTMLElement | null): ReduxAction => ({
    export const menuButtonClicked = (anchorType: MenuAnchorOptions, deckListMenuAnchor: HTMLElement | null): ReduxAction => ({
    type: MENU_BUTTON_CLICKED,
    payload: {
        type: anchorType, 
        anchor: deckListMenuAnchor
    }
});

export const MENU_OPTION_SELECTED = 'MENU_OPTION_SELECTED';
//export const menuOptionSelected = (anchorType: 'deckListMenuAnchor' | 'cardMenuAnchor'): ReduxAction => ({
export const menuOptionSelected = (anchorType: MenuAnchorOptions): ReduxAction => ({
    type: MENU_OPTION_SELECTED,
    payload: anchorType
});