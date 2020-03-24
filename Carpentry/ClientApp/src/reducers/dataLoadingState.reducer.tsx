import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions/index.actions';

declare interface DataLoadingAppState {
    coreFilterOptions: boolean;

    deckList: boolean;
    deckDetail: boolean;

    inventoryOverview: boolean;
    inventoryDetail: boolean;

    cardSearchResults: boolean;
    cardSearchInventoryDetail: boolean;
}

const appDataRequested = (state: DataLoadingAppState, action: ReduxAction): DataLoadingAppState => {
    const { scope } = action.payload; //scope: ApiScopeOption
    const newState: DataLoadingAppState = {
        ...state,
        [scope]: true,
    }
    return newState;
}

const appDataReceived = (state: DataLoadingAppState, action: ReduxAction): DataLoadingAppState => {
    const { scope, data } = action.payload; //scope: ApiScopeOption
    const newState: DataLoadingAppState = {
        ...state,
        [scope]: false,
    }
    return newState;
}

export const dataLoadingState = (state = initialState, action: ReduxAction): DataLoadingAppState => {
    switch(action.type){
        
        case API_DATA_REQUESTED:
            return appDataRequested(state, action);

        case API_DATA_RECEIVED:
            return appDataReceived(state, action);
        
        default:
            return(state)
    }
}

const initialState: DataLoadingAppState = {
    coreFilterOptions: false,

    deckList: false,
    deckDetail: false,

    inventoryOverview: false,
    inventoryDetail: false,

    cardSearchResults: false,
    cardSearchInventoryDetail: false,
}
