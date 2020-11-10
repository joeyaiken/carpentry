//IDK why this is erroring
//wat hmm
export const INVENTORY_OVERVIEW_FILTER_CHANGED = 'INVENTORY_OVERVIEW_FILTER_CHANGED';
export const inventoryOverviewFilterChanged = (filter: string, value: string | boolean): ReduxAction => ({
    type: INVENTORY_OVERVIEW_FILTER_CHANGED,
    payload: {
        filter: filter, 
        value: value
    }
});