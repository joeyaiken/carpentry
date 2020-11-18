import { TRACKED_SETS_DATA_REQUESTED, TRACKED_SETS_DATA_RECEIVED } from "./TrackedSetsActions";

export interface State {
    setsById: { [id: number]: SetDetailDto };
    setIds: number[];
    isLoading: boolean;
    showUntracked: boolean;
}

export const trackedSetsDataRequested = (state: State, action: ReduxAction): State => {
    const { showUntracked } = action.payload;

    const newState: State = {
        ...state,
        ...initialState,
        isLoading: true,
        showUntracked: showUntracked,
    }

    return newState;
}

export const trackedSetsDataReceived = (state: State, action: ReduxAction): State => {
    //I guess this is normally where Normalizr should be used?
    const apiSets: SetDetailDto[] = action.payload;

    //Create/Update/Delete actions will return null
    if(apiSets === null){
        return {
            ...state,
            isLoading: false,
        }
    }

    let setsById: { [key:number]: SetDetailDto } = {};

    apiSets.forEach(set => {
        setsById[set.setId] = set;
    });

    const newState: State = {
        isLoading: false,
        setIds: apiSets.map(set => set.setId),
        setsById: setsById,
        showUntracked: state.showUntracked,
    }

    return newState;
}

export const trackedSetsReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case TRACKED_SETS_DATA_REQUESTED:
            return trackedSetsDataRequested(state, action);

        case TRACKED_SETS_DATA_RECEIVED:
            return trackedSetsDataReceived(state, action);
            
        default:
            return(state)
    }
}

const initialState: State = {
    isLoading: false,
    showUntracked: false,
    setsById: {},
    setIds: [],
}

