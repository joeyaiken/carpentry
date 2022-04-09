export const INVENTORY_OVERVIEW_FILTER_CHANGED = 'INVENTORY_OVERVIEW.FILTER_CHANGED';
export const inventoryOverviewFilterChanged = (filter: string, value: string | boolean): ReduxAction => ({
    type: INVENTORY_OVERVIEW_FILTER_CHANGED,
    payload: {
        filter: filter, 
        value: value
    }
});

export const CARD_MENU_BUTTON_CLICKED = 'INVENTORY_OVERVIEW.CARD_MENU_BUTTON_CLICKED';
export const cardMenuButtonClick = (menuAnchor: HTMLButtonElement | null): ReduxAction => ({
    type: CARD_MENU_BUTTON_CLICKED,
    payload: menuAnchor
});

export const QUICK_FILTER_APPLIED = 'QUICK_FILTER_APPLIED';
export const quickFilterApplied = (filter: InventoryFilterProps): ReduxAction => ({
    type: QUICK_FILTER_APPLIED,
    payload: filter,
});