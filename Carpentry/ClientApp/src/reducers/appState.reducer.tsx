//handles things like 
//  what's loading

//Goal: Have a single reducer that tracks what resources are currently loading

import { API_DATA_REQUESTED, INVENTORY_ADD_COMPLETE } from '../actions/index.actions';
import { NAVIGATE, NEW_DECK_SAVING, NEW_DECK_SAVE_COMPLETE, APP_BAR_ADD_CLICKED } from '../actions/core.actions';
import { DECK_CARD_REQUEST_ALTERNATE_VERSIONS } from '../actions/deckEditor.actions';

//export type ApiScopeOption = "deckList"  | "inventoryOverview" | "inventoryDetail";

declare interface AppCoreState {
    // newDeckIsSaving: boolean; //Should be removed
    // newDeckDto: DeckProperties; //Should be removed

    // visibleContainer: AppContainerEnum; //Should be removed
    selectedDeckId: number | null; //Can this be removed?
    // isCardSearchShowing: boolean; //Should be removed


    newDeckIsSaving: boolean;
    // newDeckDto: DeckProperties;
    visibleContainer: AppContainerEnum;
    isCardSearchShowing: boolean;
    
}

export const apiDataRequested = (state: AppCoreState, action: ReduxAction): AppCoreState => {
    const { scope } = action.payload;
    
    if(scope as ApiScopeOption !== "deckDetail") return (state);

    const newState: AppCoreState = {
        ...state,
        // loadingDeckDetail: true
        visibleContainer: 'deckEditor',
        selectedDeckId: action.payload,
    };
    
    return newState;
}

export const appState = (state = initialState, action: ReduxAction): AppCoreState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case NAVIGATE:
            //Right now, one of the things "Navigate" is used for is controlling the visibility of the AddDeck modal
            //I guess for now you can only add a deck from the main menu, but it's still done through modal
            //TODO fix this nonsense
            let destination: AppContainerEnum = action.payload;
            // console.log(destination);
            return {
                ...state,
                visibleContainer: destination
            }
        case NEW_DECK_SAVING:
            return {
                ...state,
                newDeckIsSaving: true,
            }
        case NEW_DECK_SAVE_COMPLETE:
            return {
                ...state,
                newDeckIsSaving: false,
            }

        case INVENTORY_ADD_COMPLETE:
            // console.log("confusion")
            return {
                ...state,
                isCardSearchShowing: false,
            }
        case APP_BAR_ADD_CLICKED: 
            return {
                ...state,
                isCardSearchShowing: !state.isCardSearchShowing
            }
        case DECK_CARD_REQUEST_ALTERNATE_VERSIONS:
            return {
                ...state,
                isCardSearchShowing: true,
            }
        // case API_DATA_RECEIVED:
        //     return appDataReceived(state, action);
        //         case CARD_SEARCH_ADDING_DECK_CARD:
        
//             return {
//                     ...state,
//                     inventoryDetailIsLoading: true,
//                 } as CardSearchState;
        default:
            return(state)
    }
}

const initialState: AppCoreState = {
    selectedDeckId: null,
    isCardSearchShowing: false,
    newDeckIsSaving: false,
    //visibleContainer: null,
    visibleContainer: "inventory",
}