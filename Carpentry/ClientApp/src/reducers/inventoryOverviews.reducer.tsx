import { 
    API_DATA_REQUESTED, 
    API_DATA_RECEIVED 
} from '../actions/index.actions';

declare interface InventoryOverviewsState {
    //Actually IDK if this should be ID or name
    ////THIS SHOULD BE SWAPPED TO EVENTUALLY!!!
    //But right now I don't know if they Natural Key will be MultiverseId or Name...
    //byId: { [multiverseId: number]: InventoryOverviewDto }; //is this grouped by name or unique MID ?
    byName: { [name: string]: InventoryOverviewDto }; //is this grouped by name or unique MID ?
    allNames: string[];
}

const apiDataRequested = (state: InventoryOverviewsState, action: ReduxAction): InventoryOverviewsState => {
    const { scope } = action.payload;
    
    if(scope as ApiScopeOption !== "inventoryOverview") return (state);

    const newState: InventoryOverviewsState = {
        ...state,
        ...initialState,
    };

    return newState;
}

const apiDataReceived = (state: InventoryOverviewsState, action: ReduxAction): InventoryOverviewsState => {
    const { scope, data } = action.payload;
    
    if(scope as ApiScopeOption !== "inventoryOverview") return (state);

    const apiOverviews: InventoryOverviewDto[] = data;

    let overviewsByName = {}

    apiOverviews.forEach(item => {
        // console.log(`adding name ${item.name}`)
        overviewsByName[item.name] = item;
    });

    const newState: InventoryOverviewsState = {
        ...state,
        byName: overviewsByName,
        allNames: apiOverviews.map(item => item.name),
    };

    return newState;
}

export const inventoryOverviews = (state = initialState, action: ReduxAction): InventoryOverviewsState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);

        default:
            return(state)
    }
}

const initialState: InventoryOverviewsState = {
    byName: {},
    allNames: [],
}
