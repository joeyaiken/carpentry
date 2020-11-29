import { 
    TOGGLE_DECK_VIEW_MODE, 
    DECK_EDITOR_CARD_SELECTED,
    CLOSE_DECK_PROPS_MODAL,
    DECK_PROPS_MODAL_CHANGED,
    DECK_EDITOR_SAVE_RECEIVED,
    DECK_EDITOR_SAVE_REQUESTED,
    OPEN_DECK_PROPS_MODAL,
} from './DeckEditorActions';

//
// The deckEditorReducer is the App reducer for the DeckEditor container
//

export interface State {
    viewMode: DeckEditorViewMode;
    //
    //selectedCard: InventoryOverviewDto | null; // should this be an ID in AppState?
    
    //Do I want to be able to distinguish between a selected card (highlighted) and hover card (mouse-over)
    //when hover -> show on right
    //When select but no hover -> show select
    //else show stats

    //selectedOverviewCardName: string | null;
    selectedOverviewCardId: number | null;
    secondarySelectedCardId: number | null;

    //this is the filtered inventory cards that should show for a deck
    // selectedInventoryCardIds: number[];
    //selectedInventoryCards: InventoryCard[]; //key = id, but maybe should be a list of IDs, or removed completely

    //Next question, how exactly do I handle grouped cards?!?
    groupBy: "type" | null;
    sortBy: "cost" | null; //| "name", 

    //added during refactor
    isSaving: boolean;
    isPropsModalOpen: boolean;
    deckModalProps: DeckPropertiesDto | null;

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

        case OPEN_DECK_PROPS_MODAL: return { ...state, isPropsModalOpen: true, deckModalProps: action.payload };
        case CLOSE_DECK_PROPS_MODAL: return { ...state, isPropsModalOpen: false, deckModalProps: null };
        case DECK_EDITOR_SAVE_REQUESTED: return { ...state, isSaving: true };
        case DECK_EDITOR_SAVE_RECEIVED: return { ...state, isSaving: false };
        case DECK_PROPS_MODAL_CHANGED: return deckPropsModalChanged(state, action);
        default: return(state);
    }
}

const initialState: State = {
    viewMode: "grouped",
    selectedOverviewCardId: null,
    secondarySelectedCardId: null,
    groupBy: null,
    sortBy: null, 
    isSaving: false,
    isPropsModalOpen: false,
    deckModalProps: null,
}

function deckPropsModalChanged(state: State, action: ReduxAction): State {
    const {name, value} = action.payload;
    // const appliedValue: string | number = action.payload.value;

    // if(!state.deckModalProps) return (state);
    
    // const updatedState = {
    //     ...state,
    //     deckModalProps: {
    //         ...state.deckModalProps,
    //         [name]: appliedValue,
    //     },
    // }
    // console.log("updatedState");
    // console.log(updatedState.deckModalProps);
    // return updatedState;

    return {
        ...state,
        deckModalProps: (state.deckModalProps == null) ? null : {
            ...state.deckModalProps,
            [name]: value,
        },
    }
}