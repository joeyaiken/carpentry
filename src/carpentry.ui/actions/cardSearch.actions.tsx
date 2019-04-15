import React from 'react';
import Redux, { Store, Dispatch } from 'redux'

export const CS_INITIALIZED = 'CS_INITIALIZED';
export const csInitialized = (): ReduxAction => ({
    type: CS_INITIALIZED
});

export const CS_FILTER_CHANGED = 'CS_FILTER_CHANGED';
export const csFilterChanged = (): ReduxAction => ({
    type: CS_FILTER_CHANGED
    //payload
});

export const CS_SEARCH_APPLIED = 'CS_SEARCH_APPLIED';
export const csSearchApplied = (): ReduxAction => ({
    type: CS_SEARCH_APPLIED
});

export const CS_CARD_SELECTED = 'CS_CARD_SELECTED';
export const csCardSelected = (): ReduxAction => ({
    type: CS_CARD_SELECTED
    //payload
});

export const CS_ACTION_APPLIED = 'CS_ACTION_APPLIED';
export const csActionApplied = (): ReduxAction => ({
    type: CS_ACTION_APPLIED
    //payload
});