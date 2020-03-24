import { 

} from '../actions/index.actions';
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

    filterOptions: {
        sets: FilterOption[];
        types: FilterOption[];
        colors: FilterOption[];
        rarities: FilterOption[];
    }
}

const exampleReducerFunction = (state: appFilterOptionsState, action: ReduxAction): appFilterOptionsState => {
    const newState: appFilterOptionsState = {
        ...state,
    }
    return newState;
}

export const appFilterOptions = (state = initialState, action: ReduxAction): appFilterOptionsState => {
    switch(action.type){

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
    filterOptions: {
        sets: GetSetFilters(),
        colors: GetColorFilters(),
        rarities: GetRarityFilters(),
        types: GetTypeFilters()
    },
}
