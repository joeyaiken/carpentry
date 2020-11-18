export interface State {
    setsById: { [id: number]: SetDetailDto };
    setIds: number[];
    isLoading: boolean;
    showUntracked: boolean;
}

export const apiDataRequested = (state: State, action: ReduxAction): State => {
    const { scope, data } = action.payload;
    
    if(scope as ApiScopeOption !== "trackedSets") return (state);
    
    const { showUntracked } = data

    const newState: State = {
        ...state,
        ...initialState,
        isLoading: true,
        showUntracked: showUntracked,
    }

    return newState;
}

// export const apiDataReceived = (state: State, action: ReduxAction): State => {
//     const { scope, data } = action.payload;
    
//     if(scope as ApiScopeOption !== "trackedSets") return (state);

//     //I guess this is normally where Normalizr should be used?
//     const apiSets: SetDetailDto[] = data;

//     //Create/Update/Delete actions will return null
//     if(data === null){
//         return {
//             ...state,
//             isLoading: false,
//         }
//     }

//     let setsById: { [key:number]: SetDetailDto } = {};

//     apiSets.forEach(set => {
//         setsById[set.setId] = set;
//     });

//     const newState: State = {
//         isLoading: false,
//         setIds: apiSets.map(set => set.setId),
//         setsById: setsById,
//         showUntracked: state.showUntracked,
//     }



    
//     // const newState: State = {
//     //     ...state,
//     //     deckIds: apiDecks.map(deck => deck.id),
//     //     decksById: decksById,
//     //     isLoading: false,
//     //     isInitialized: true,
//     //     // decks: data,
//     // };


//     // console.log(searchResultPayload)
//     // const newState: State = {
//     //     ...state,
//     //     filterOptions: searchResultPayload,
//     //     isLoading: false,
//     //     // searchResultsById: resultsById,
//     //     // allSearchResultIds: searchResultPayload.map(card => card.multiverseId),
//     //     // isLoading: false,
//     //     // searchResults: searchResultPayload,
//     // }
//     // console.log('new filter state');
//     // console.log(newState)
//     return newState;
// }

// export const settingsDataReducer = (state = initialState, action: ReduxAction): State => {
//     switch(action.type){
//         case API_DATA_REQUESTED:
//             return apiDataRequested(state, action);

//         case API_DATA_RECEIVED:
//             return apiDataReceived(state, action);
            
//         default:
//             return(state)
//     }
// }

const initialState: State = {
    isLoading: false,
    showUntracked: false,
    setsById: {},
    setIds: [],
}

