import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions/';

export interface AppFilterOptionsReducerState {
    // sets: {
    //     byName: {[name:string]: FilterOption}
    //     allKeys: string[]
    // }
    // types: {
    //     byName: {[name:string]: FilterOption}
    //     allKeys: string[]
    // }
    // colors: {
    //     byName: {[name:string]: FilterOption}
    //     allKeys: string[]
    // }
    // rarities: {
    //     byName: {[name:string]: FilterOption}
    //     allKeys: string[]
    // }
    isLoading: boolean;
    filterOptions: AppFiltersDto;
}

export const apiDataRequested = (state: AppFilterOptionsReducerState, action: ReduxAction): AppFilterOptionsReducerState => {
    const { scope } = action.payload;
    
    if(scope as ApiScopeOption !== "coreFilterOptions") return (state);
    
    const newState: AppFilterOptionsReducerState = {
        ...state,
        ...initialState,
        isLoading: true,
    }

    return newState;
}

export const apiDataReceived = (state: AppFilterOptionsReducerState, action: ReduxAction): AppFilterOptionsReducerState => {
    
    const { scope, data } = action.payload;

    // console.log('card search data receive');
    // console.log(`scope: ${scope}`);

    if(scope as ApiScopeOption !== "coreFilterOptions") return (state);
    console.log('filters recieved')
    // const searchResultPayload: MagicCard[] = data || [];
    // let resultsById = {};
    // searchResultPayload.forEach(card => resultsById[card.multiverseId] = card);

    const searchResultPayload: AppFiltersDto = data || {};

    console.log(searchResultPayload)
    const newState: AppFilterOptionsReducerState = {
        ...state,
        filterOptions: searchResultPayload,
        isLoading: false,
        // searchResultsById: resultsById,
        // allSearchResultIds: searchResultPayload.map(card => card.multiverseId),
        // isLoading: false,
        // searchResults: searchResultPayload,
    }
    return newState;
}

export const appFilterOptionsReducer = (state = initialState, action: ReduxAction): AppFilterOptionsReducerState => {
    switch(action.type){
        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);
            
        default:
            return(state)
    }
}

const initialState: AppFilterOptionsReducerState = {
    // sets: {
    //     byName:{},
    //     allKeys: [],
    // },
    // types: {
    //     byName:{},
    //     allKeys: [],
    // },
    // colors: {
    //     byName:{},
    //     allKeys: [],
    // },
    // rarities: {
    //     byName:{},
    //     allKeys: [],
    // },
    // filterOptions: {
    //     sets: GetSetFilters(),
    //     colors: GetColorFilters(),
    //     rarities: GetRarityFilters(),
    //     types: GetTypeFilters()
    // },
    filterOptions: {
        sets: [],
        colors: [],
        rarities: [],
        types: [],
        formats: [],
        statuses: [],
    },
    isLoading: false,
}
