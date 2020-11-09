import { Dispatch } from "redux";
import { inventoryApi } from "../../api/inventoryApi";
import { AppState } from "../../configureStore";



export const requestInventoryOverviews = (): any => {
    return (dispatch: Dispatch, getState: any) => {
        return getInventoryOverviews(dispatch, getState());
    }
}

function getInventoryOverviews(dispatch: Dispatch, state: AppState): any {
    // console.log('actions - getInventoryItems START');
    //const dataQueryInProgress = state.inventory.overviewIsLoading;
    const dataQueryInProgress = state.inventory.data.overviews.isLoading;  //.isLoading.inventoryOverview;
    if(dataQueryInProgress){
        return;
    }
    // console.log('actions - getInventoryItems - calling inventoryItemsRequested');

    //dispatch(inventoryItemsRequested());
    dispatch(inventoryOverviewsRequested());
    
    // console.log('actions - getInventoryItems - calling api_getAllInventoryItems');


    //TODO -- This whole chunk shouyld come from the inventory overview app reducer
    //  It should also probably be passed as a param and not purely read from app state
    const param: InventoryQueryParameter = { 
        //groupBy: state.inventory.searchFilter.searchMethod,
        //groupBy: state.inventory.searchMethod,
        groupBy: 'unique',//state.app.inventory.searchMethod,
        text: state.inventory.overviews.filters.text,
        //sets: [],
        colors: state.inventory.overviews.filters.colorIdentity,
        types: [],
        skip: 0,
        take: 100,
        // format: state.inventory.overviews.filters.format,
        sort:   'price',//state.inventory.searchFilter.sort,
        sortDescending: true,
        //sort: 'name',
        //set: 'eld'
        set: state.inventory.overviews.filters.set,
        exclusiveColorFilters: state.inventory.overviews.filters.exclusiveColorFilters,
        multiColorOnly: state.inventory.overviews.filters.multiColorOnly,
        // maxCount: state.inventory.overviews.filters.maxCount || 0,
        // minCount: state.inventory.overviews.filters.minCount || 0,
        type: state.inventory.overviews.filters.type,
        //rarity: state.cardSearch.cardSearchFilter.props.rarity,
        rarity: state.inventory.overviews.filters.rarity,
        //rarity: ['c','u']
        format: null,
        maxCount: 0,
        minCount: 0,
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
