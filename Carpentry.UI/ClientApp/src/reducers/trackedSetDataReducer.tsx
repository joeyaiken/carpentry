import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions/';

export interface TrackedSetDataReducerState {
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

export const trackedSetDataRequested = (state: TrackedSetDataReducerState, action: ReduxAction): TrackedSetDataReducerState => {
    const { scope } = action.payload;
    
    if(scope as ApiScopeOption !== "coreFilterOptions") return (state);
    
    const newState: TrackedSetDataReducerState = {
        ...state,
        ...initialState,
        isLoading: true,
    }

    return newState;
}

export const trackedSetDataReceived = (state: TrackedSetDataReducerState, action: ReduxAction): TrackedSetDataReducerState => {
    
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
    const newState: TrackedSetDataReducerState = {
        ...state,
        filterOptions: searchResultPayload,
        isLoading: false,
        // searchResultsById: resultsById,
        // allSearchResultIds: searchResultPayload.map(card => card.multiverseId),
        // isLoading: false,
        // searchResults: searchResultPayload,
    }
    console.log('new filter state');
    console.log(newState)
    return newState;
}

export const trackedSetDataReducer = (state = initialState, action: ReduxAction): TrackedSetDataReducerState => {
    switch(action.type){
        case API_DATA_REQUESTED:
            return trackedSetDataRequested(state, action);

        case API_DATA_RECEIVED:
            return trackedSetDataReceived(state, action);
            
        default:
            return(state)
    }
}

const initialState: TrackedSetDataReducerState = {
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
