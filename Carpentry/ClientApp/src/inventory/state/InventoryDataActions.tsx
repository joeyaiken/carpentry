import { Dispatch } from "redux";
import { inventoryApi } from "../../api/inventoryApi";
import { RootState } from "../../configureStore";



export const requestInventoryOverviews = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getInventoryOverviews(dispatch, getState());
    }
}

function getInventoryOverviews(dispatch: Dispatch, state: RootState): any {
    // console.log('actions - getInventoryItems START');
    //const dataQueryInProgress = state.inventory.overviewIsLoading;
    const dataQueryInProgress = state.inventory.data.overviews.isLoading;  //.isLoading.inventoryOverview;
    if(dataQueryInProgress){
        return;
    }
    
    //dispatch(inventoryItemsRequested());
    dispatch(inventoryOverviewsRequested());
    
    //TODO -- This whole chunk shouyld come from the inventory overview app reducer
    //  It should also probably be passed as a param and not purely read from app state

    const existingFilters = state.inventory.overviews.filters;
    const param: InventoryQueryParameter = { 
        groupBy: existingFilters.groupBy,
        text: existingFilters.text,
        colors: existingFilters.colorIdentity,
        skip: +existingFilters.skip,
        take: +existingFilters.take,
        sort: existingFilters.sortBy,
        sortDescending: existingFilters.sortDescending,
        set: existingFilters.set,
        exclusiveColorFilters: existingFilters.exclusiveColorFilters,
        multiColorOnly: existingFilters.multiColorOnly,
        maxCount: +existingFilters.maxCount,
        minCount: +existingFilters.minCount,
        type: existingFilters.type,
        rarity: existingFilters.rarity,
    }

    inventoryApi.searchCards(param).then((result) => {
        dispatch(inventoryOverviewsReceived(result));
    });

}

export const INVENTORY_OVERVIEWS_REQUESTED = 'INVENTORY_OVERVIEWS_REQUESTED';
export const inventoryOverviewsRequested = (): ReduxAction => ({
    type: INVENTORY_OVERVIEWS_REQUESTED,
});

export const INVENTORY_OVERVIEWS_RECEIVED = 'INVENTORY_OVERVIEWS_RECEIVED';
export const inventoryOverviewsReceived = (data: InventoryOverviewDto[]): ReduxAction => ({
    type: INVENTORY_OVERVIEWS_RECEIVED,
    payload: data,
});

// export const DECK_OVERVIEWS_RECEIVED = 'DECK_OVERVIEWS_RECEIVED';
// export const deckOverviewsReceived = (data: DeckOverviewDto[]): ReduxAction => ({
//     type: DECK_OVERVIEWS_RECEIVED,
//     payload: data
// });

export const INVENTORY_DETAIL_REQUESTED = 'INVENTORY_DETAIL_REQUESTED';
export const inventoryDetailRequested = (cardId: number): ReduxAction => ({
    type: INVENTORY_DETAIL_REQUESTED,
    payload: cardId,
});

export const INVENTORY_DETAIL_RECEIVED = 'INVENTORY_DETAIL_RECEIVED';
export const inventoryDetailReceived = (detail: InventoryDetailDto | null): ReduxAction => ({
    type: INVENTORY_DETAIL_RECEIVED,
    payload: detail,
});


export const TRIMMING_TIPS_REQUESTED = 'TRIMMING_TIPS_REQUESTED';
export const trimmingTipsRequested = (): ReduxAction => ({
    type: TRIMMING_TIPS_REQUESTED
});

export const TRIMMING_TIPS_RECEIVED = 'TRIMMING_TIPS_RECEIVED';
export const trimmingTipsReceived = (): ReduxAction => ({
    type: TRIMMING_TIPS_RECEIVED
});

