//TODO - Delete once refactored away
import { CARD_SEARCH_SEARCH_METHOD_CHANGED } from '../_actions';
import { CARD_SEARCH_SAVE_PENDING_CARDS, INVENTORY_ADD_COMPLETE } from '../_actions/inventoryActions';
import { CARD_SEARCH_SELECT_CARD, TOGGLE_CARD_SEARCH_VIEW_MODE } from '../_actions/cardSearchActions';
// import { APP_BAR_ADD_CLICKED } from '../actions/core.actions';

//This is the reducer for the Card Search container / component / whatnot
//Should be cleaned up, have old commented references removed
//Goal is to use a data model that more matches the controller layer
//  Pending cards should be InventoryCard-like objects
//  Search results, even from scryfall, should have an updated DTO that handles variants / whatnot

//This component/container will use a modal to show possible card variants / foils
//Q: Should this modal belong specifically to this container, or be nested as part of the UI?
//      Going to try leaving it as part of this component for now, maybe even center it over the container instead of the whole app

export interface CardSearchAppReducerState {
    viewMode: CardSearchViewMode;
    pendingCardsSaving: boolean;
    cardSearchMethod: "set" | "web" | "inventory";
    selectedCard: CardSearchResultDto | null; //should probably be an AppState ID
}

const cardSearchSearchMethodChanged = (state = initialState, action: ReduxAction): CardSearchAppReducerState => {
    const newSearchMethod = action.payload;

    const newState: CardSearchAppReducerState = {
        ...state,
        cardSearchMethod: newSearchMethod,
    }
    return newState;
}

export const cardSearchAppReducer = (state = initialState, action: ReduxAction): CardSearchAppReducerState => {
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
            const selectedCard: CardSearchResultDto = action.payload;
            // console.log('card search - card selected')
            // console.log(selectedCard)
            return {
                ...state,
                selectedCard: selectedCard,
            };
        // case APP_BAR_ADD_CLICKED:
        //     alert('non-refactored code hit - CardSearchAppReducer - APP_BAR_ADD_CLICKED');
        //     return (state);
            // //under certain conditions, method should be set to Inventory
            // //FWIW something seems off with this approach, I can't reference state, I need to rely on a payload
            // const filters: FilterDescriptor[] | undefined = action.payload;

            // let searchMethod: "set" | "web" | "inventory" = "set";
            // // console.log('app bar add click - card search');
            // // console.log(action.payload);
            // if(filters && filters.length > 0){
            //     const searchMethodFilter = filters.find(f => f.name === "SearchMethod");
            //     if(searchMethodFilter){
            //         searchMethod = searchMethodFilter.value;
            //         console.log("set search method to "+searchMethod);
            //     }
            // }
            // return {
            //     ...state,
            //     cardSearchMethod: searchMethod,
            //     //need to populate Colors and Format, if data is provided
            // }
            
        case TOGGLE_CARD_SEARCH_VIEW_MODE:

            let newViewMode: CardSearchViewMode = "list";

            switch(state.viewMode){
                case "list":
                    newViewMode = "grid";
                    break;
                case "grid":
                    newViewMode = "list";
                    break;
            }
            
            return {
                ...state,
                viewMode: newViewMode,
            };

        default:
            return(state)
    }
}

const initialState: CardSearchAppReducerState = {
    viewMode: "list",
    pendingCardsSaving: false,
    cardSearchMethod: "set",
    selectedCard:  null, //should probably be an AppState ID
}