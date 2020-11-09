import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions/index.actions';
import {
    // CardSet,
    GetSetFilters,
    GetRarityFilters,
    GetTypeFilters,
    GetColorFilters
} from '../util'
declare interface appFilterOptionsState {
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

    filterOptions: FilterOptionDto;
}

export const apiDataRequested = (state: appFilterOptionsState, action: ReduxAction): appFilterOptionsState => {
    const { scope } = action.payload;
    
    if(scope as ApiScopeOption !== "coreFilterOptions") return (state);
    
    const newState: appFilterOptionsState = {
        ...state,
        ...initialState,
    }

    return newState;
}

export const apiDataReceived = (state: appFilterOptionsState, action: ReduxAction): appFilterOptionsState => {
    
    const { scope, data } = action.payload;

    // console.log('card search data receive');
    // console.log(`scope: ${scope}`);

    if(scope as ApiScopeOption !== "coreFilterOptions") return (state);

    // const searchResultPayload: MagicCard[] = data || [];
    // let resultsById = {};
    // searchResultPayload.forEach(card => resultsById[card.multiverseId] = card);

    const searchResultPayload: FilterOptionDto = data || {};

    const newState: appFilterOptionsState = {
        ...state,
        filterOptions: searchResultPayload,
        // searchResultsById: resultsById,
        // allSearchResultIds: searchResultPayload.map(card => card.multiverseId),
        // isLoading: false,
        // searchResults: searchResultPayload,
    }
    return newState;
}

export const appFilterOptions = (state = initialState, action: ReduxAction): appFilterOptionsState => {
    switch(action.type){
        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);
            
        default:
            return(state)
    }
}

const initialState: appFilterOptionsState = {
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
}
