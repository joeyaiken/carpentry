//TODO - Delete once refactored away
import { CARD_SEARCH_SEARCH_METHOD_CHANGED } from '../actions/index.actions';
import { CARD_SEARCH_SAVE_PENDING_CARDS, INVENTORY_ADD_COMPLETE } from '../actions/inventory.actions';
import { CARD_SEARCH_SELECT_CARD } from '../actions/cardSearch.actions';

//This is the reducer for the Card Search container / component / whatnot
//Should be cleaned up, have old commented references removed
//Goal is to use a data model that more matches the controller layer
//  Pending cards should be InventoryCard-like objects
//  Search results, even from scryfall, should have an updated DTO that handles variants / whatnot

//This component/container will use a modal to show possible card variants / foils
//Q: Should this modal belong specifically to this container, or be nested as part of the UI?
//      Going to try leaving it as part of this component for now, maybe even center it over the container instead of the whole app

declare interface CardSearchState {
    pendingCardsSaving: boolean;
    cardSearchMethod: "set" | "web" | "inventory";
    selectedCard: MagicCard | null; //should probably be an AppState ID
}

const cardSearchSearchMethodChanged = (state = initialState, action: ReduxAction): CardSearchState => {
    const newSearchMethod = action.payload;

    const newState: CardSearchState = {
        ...state,
        cardSearchMethod: newSearchMethod,
    }
    return newState;
}

export const cardSearch = (state = initialState, action: ReduxAction): CardSearchState => {
    switch(action.type){

        case CARD_SEARCH_SEARCH_METHOD_CHANGED:
            return cardSearchSearchMethodChanged(state, action);
          

        case CARD_SEARCH_SAVE_PENDING_CARDS:
            return{
                ...state,
                pendingCardsSaving: true,
            }
        
        case INVENTORY_ADD_COMPLETE:
            return {
                ...state,
                pendingCardsSaving: false,
            }

        case CARD_SEARCH_SELECT_CARD:
            const selectedCard: MagicCard = action.payload;
            return {
                ...state,
                selectedCard: selectedCard,
            } as CardSearchState;

        default:
            return(state)
    }
}

const initialState: CardSearchState = {
    pendingCardsSaving: false,
    cardSearchMethod: "set",
    selectedCard:  null, //should probably be an AppState ID
}