import { 
    API_DATA_REQUESTED, 
    API_DATA_RECEIVED 
} from '../actions/';

export interface InventoryDataReducerState {
    isLoading: boolean;
    byId: { [id: number]: InventoryOverviewDto };
    allIds: number[];
}

export const inventoryDataReducer = (state = initialState, action: ReduxAction): InventoryDataReducerState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);

        default:
            return(state)
    }
}

const initialState: InventoryDataReducerState = {
    isLoading: false,
    byId: {},
    allIds: [],
}

const apiDataRequested = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    const newState: InventoryDataReducerState = {
        ...state,
        isLoading: true,
    };

    return newState;
}

const apiDataReceived = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    const { data } = action.payload;

    const apiOverviews: InventoryOverviewDto[] = data;

    let overviewsById = {}

    apiOverviews.forEach(item => {
        overviewsById[item.id] = item;
    });

    const newState: InventoryDataReducerState = {
        ...state,
            isLoading: false,
            byId: overviewsById,
            allIds: apiOverviews.map(item => item.id),
    };

    return newState;
}
