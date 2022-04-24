import { APP_CORE_DATA_REQUESTED, APP_CORE_DATA_RECEIVED } from "./coreDataActions";

// export interface State {
//     filterDataIsLoading: boolean;
//     filterOptions: AppFiltersDto;
// }
//
// export const coreDataReducer = (state = initialState, action: ReduxAction): State => {
//     switch(action.type){
//         case APP_CORE_DATA_REQUESTED:
//             return appFiltersRequested(state, action);
//
//         case APP_CORE_DATA_RECEIVED:
//             return appFiltersReceived(state, action);
//            
//         default:
//             return(state);
//     }
// }
//
// const initialState: State = {
//     filterOptions: {
//         sets: [],
//         colors: [],
//         rarities: [],
//         types: [],
//         formats: [],
//         statuses: [],
//         searchGroups: [],
//         groupBy: [],
//         sortBy: [],
//     },
//     filterDataIsLoading: false,
// }
//
// export const appFiltersRequested = (state: State, action: ReduxAction): State => {
//     const newState: State = {
//         ...state,
//         ...initialState,
//         filterDataIsLoading: true,
//     };
//     return newState;
// }
//
// export const appFiltersReceived = (state: State, action: ReduxAction): State => {
//     const searchResultPayload: AppFiltersDto = action.payload || {};
//     const newState: State = {
//         ...state,
//         filterOptions: searchResultPayload,
//         filterDataIsLoading: false,
//     };
//     return newState;
// }