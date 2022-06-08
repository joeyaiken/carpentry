// import { DECK_DETAIL_REQUESTED } from '../../state/decksDataActions';
import { 
    TOGGLE_DECK_VIEW_MODE, 
    DECK_EDITOR_CARD_SELECTED,
    CLOSE_DECK_PROPS_MODAL,
    // DECK_PROPS_MODAL_CHANGED,
    DECK_EDITOR_SAVE_RECEIVED,
    DECK_EDITOR_SAVE_REQUESTED,
    // OPEN_DECK_PROPS_MODAL,
    // CARD_MENU_BUTTON_CLICKED,
} from './DeckEditorActions';

//
// The deckEditorReducer is the App reducer for the DeckEditor container
//

export interface State {
    viewMode: DeckEditorViewMode;

    selectedOverviewCardId: number | null;
    secondarySelectedCardId: number | null;

    //added during refactor
    isSaving: boolean;
    isPropsModalOpen: boolean;

    //This was my attempt at creating a separate form state
    // with hooks, this should just be grabbed with a selector and mapped locally
    // deckModalProps: DeckPropertiesDto | null;   
}

export const deckEditorReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){

        case DECK_EDITOR_CARD_SELECTED:
            const selectedCardOverview: DeckCardOverview = action.payload;
            
            return {
                ...state,
                selectedOverviewCardId: selectedCardOverview.id,
                
            }
            
        case TOGGLE_DECK_VIEW_MODE:            

            let newViewMode: DeckEditorViewMode = "list";

            switch(state.viewMode){
                case "list":
                    newViewMode = "grid";
                    break;
                case "grid":
                    newViewMode = "grouped";
                    break;
                case "grouped":
                    newViewMode = "list";
                    break;
            }
            
            return {
                ...state,
                viewMode: newViewMode,
            } as State;

        // case OPEN_DECK_PROPS_MODAL: return { 
        //     ...state, 
        //     isPropsModalOpen: true, 
        //     // deckModalProps: action.payload 
        // };
        case CLOSE_DECK_PROPS_MODAL: return { 
            ...state, 
            isPropsModalOpen: false, 
            // deckModalProps: null 
        };
        case DECK_EDITOR_SAVE_REQUESTED: return { ...state, isSaving: true };
        case DECK_EDITOR_SAVE_RECEIVED: return { ...state, isSaving: false };
        // case DECK_PROPS_MODAL_CHANGED: return deckPropsModalChanged(state, action);
        default: return(state);
    }
}

const initialState: State = {
    viewMode: "grouped",
    selectedOverviewCardId: null,
    secondarySelectedCardId: null,
    isSaving: false,
    isPropsModalOpen: false,
    // deckModalProps: null,
}

// function deckPropsModalChanged(state: State, action: ReduxAction): State {
//     const {name, value} = action.payload;
//     // const appliedValue: string | number = action.payload.value;
//
//     // if(!state.deckModalProps) return (state);
//    
//     // const updatedState = {
//     //     ...state,
//     //     deckModalProps: {
//     //         ...state.deckModalProps,
//     //         [name]: appliedValue,
//     //     },
//     // }
//     // console.log("updatedState");
//     // console.log(updatedState.deckModalProps);
//     // return updatedState;
//
//     return {
//         ...state,
//         deckModalProps: (state.deckModalProps == null) ? null : {
//             ...state.deckModalProps,
//             [name]: value,
//         },
//     }
// }