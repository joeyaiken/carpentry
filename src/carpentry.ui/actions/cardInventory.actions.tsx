import React from 'react';
import Magic, { CardFilter, Cards, Card } from 'mtgsdk-ts';
// import { Card, Set, CardFilter, SetFilter, PaginationFilter } from "./IMagic";

import Redux, { Store, Dispatch } from 'redux'
import { stat } from 'fs';
import { unmountComponentAtNode } from 'react-dom';
import { string } from 'prop-types';

// import { lumberyardSaveState } from '../data/lumberyard'






// export const SCOUT_SEARCH_FILTER_CHANGE = "SCOUT_SEARCH_FILTER_CHANGE";

// export const scoutSearchFilterChanged = (property: string, value: string): ReduxAction => ({
//     type: SCOUT_SEARCH_FILTER_CHANGE,
//     payload: {
//         property: property,
//         value: value
//     }
// });

// export const SCOUT_SEARCH_APPLIED = "SCOUT_SEARCH_APPLIED";

// export const scoutSearchApplied = (): ReduxAction => ({
//     type: SCOUT_SEARCH_APPLIED
// });


export const CI_INITIALIZED = 'CI_INITIALIZED';
export const ciInitialized = (): ReduxAction => ({
    type: CI_INITIALIZED
});

// export const CS_FILTER_CHANGED = 'CS_FILTER_CHANGED';
// export const csFilterChanged = (): ReduxAction => ({
//     type: CS_FILTER_CHANGED
//     //payload
// });

// export const CS_SEARCH_APPLIED = 'CS_SEARCH_APPLIED';
// export const csSearchApplied = (): ReduxAction => ({
//     type: CS_SEARCH_APPLIED
// });

// export const CS_CARD_SELECTED = 'CS_CARD_SELECTED';
// export const csCardSelected = (): ReduxAction => ({
//     type: CS_CARD_SELECTED
//     //payload
// });

// export const CS_ACTION_APPLIED = 'CS_ACTION_APPLIED';
// export const csActionApplied = (): ReduxAction => ({
//     type: CS_ACTION_APPLIED
//     //payload
// });