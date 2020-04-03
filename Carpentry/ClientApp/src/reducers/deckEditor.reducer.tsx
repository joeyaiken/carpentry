import { 

} from '../actions/index.actions';
import { TOGGLE_DECK_VIEW_MODE, DECK_EDITOR_CARD_SELECTED } from '../actions/deckEditor.actions';

declare interface DeckEditorState {
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

}

export const deckEditor = (state = initialState, action: ReduxAction): DeckEditorState => {
    switch(action.type){

        case DECK_EDITOR_CARD_SELECTED:
            const selectedCardOverview: InventoryOverviewDto = action.payload;
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
            } as DeckEditorState;

        default:
            return(state)
    }
}

const initialState: DeckEditorState = {
    viewMode: "grouped",
    selectedOverviewCardId: null,
    secondarySelectedCardId: null,
    groupBy: null,
    sortBy: null, 
}
