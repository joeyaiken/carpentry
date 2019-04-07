import React from 'react';
import Magic, { CardFilter, Cards, Card } from 'mtgsdk-ts';
// import { Card, Set, CardFilter, SetFilter, PaginationFilter } from "./IMagic";

import Redux, { Store, Dispatch } from 'redux'
import { stat } from 'fs';
import { unmountComponentAtNode } from 'react-dom';
import { string } from 'prop-types';

// import { lumberyardSaveState } from '../data/lumberyard'






export const SCOUT_SEARCH_FILTER_CHANGE = "SCOUT_SEARCH_FILTER_CHANGE";

export const scoutSearchFilterChanged = (property: string, value: string): ReduxAction => ({
    type: SCOUT_SEARCH_FILTER_CHANGE,
    payload: {
        property: property,
        value: value
    }
});
