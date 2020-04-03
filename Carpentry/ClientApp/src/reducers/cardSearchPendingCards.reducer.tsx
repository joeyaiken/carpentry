import { CARD_SEARCH_ADD_PENDING_CARD, CARD_SEARCH_REMOVE_PENDING_CARD, CARD_SEARCH_CLEAR_PENDING_CARDS } from '../actions/index.actions';
import { INVENTORY_ADD_COMPLETE } from '../actions/inventory.actions';

declare interface cardSearchPendingCardsState {

    pendingCards: { [key:number]: PendingCardsDto } //key == id, should this also have a list to track all keys? 
    //wait ID or MID ?
}

// const exampleReducerFunction = (state: cardSearchPendingCardsState, action: ReduxAction): cardSearchPendingCardsState => {
//     const newState: cardSearchPendingCardsState = {
//         ...state,
//     }
//     return newState;
// }

const cardSearchAddPendingCard = (state = initialState, action: ReduxAction): cardSearchPendingCardsState => {
    //'pending cards' is now a dictionary of 'pending card dto's
    const {
        data, //0
        isFoil, //false
        variant, //"normal"
    } = action.payload;
    
    const magicCardToAdd: MagicCard = data;
    
    let cardToAdd: PendingCardsDto = state.pendingCards[data.multiverseId];
    
    if(!cardToAdd){
        cardToAdd = {
            multiverseId: magicCardToAdd.multiverseId,
            name: magicCardToAdd.name,
            cards: [],
        };

        //OK - So I need to either pass the name, or the whole card
        
        
        // const magicCardToAdd = state.
        // if we can't find a matching card, nothing gets added and this just continues silently (I guess)
    //     if(magicCardToAdd){
    //         cardToAdd = {
    //             multiverseId: multiverseId,
    //             cards: [],
    //             // data: magicCardToAdd,
    //         };
    //     }                    
    }

    cardToAdd.cards.push({
        multiverseId: magicCardToAdd.multiverseId, 
        isFoil: isFoil,
        variantName: variant,
        statusId: 1,
    } as InventoryCard);

    const newState: cardSearchPendingCardsState = {
        ...state,
        pendingCards: {
            ...state.pendingCards,
            [magicCardToAdd.multiverseId]: cardToAdd
        }
    }
    return newState;
}

const cardSearchRemovePendingCard = (state = initialState, action: ReduxAction): cardSearchPendingCardsState => {
    // const {
                // } = action.payload;

    const midToRemove = action.payload.multiverseId;
    const removeFoilCard = action.payload.isFoil;
    const variantToRemove: string = action.payload.variant;

    let objToRemoveFrom = state.pendingCards[midToRemove];

    if(objToRemoveFrom){

        let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.variantName == variantToRemove && x.isFoil == removeFoilCard);

        if(thisInvCard >= 0){
            objToRemoveFrom.cards.splice(thisInvCard,1);
        }

        let pendingCardsAfterRemoval =  {
            ...state.pendingCards,
            [midToRemove]: objToRemoveFrom
        }
        //if this pending cards object has 0 items, it should be deleted from the dictionary
        if(objToRemoveFrom.cards.length == 0){
            delete pendingCardsAfterRemoval[midToRemove];
        }
        const newState: cardSearchPendingCardsState = {
            ...state,
            pendingCards: pendingCardsAfterRemoval
        }
        return newState;

    } else {
        const newState: cardSearchPendingCardsState = {
            ...state,
        }
        return newState;
    }
}



export const cardSearchPendingCards = (state = initialState, action: ReduxAction): cardSearchPendingCardsState => {
    switch(action.type){

        case CARD_SEARCH_ADD_PENDING_CARD:
            return cardSearchAddPendingCard(state, action);
            
        case CARD_SEARCH_REMOVE_PENDING_CARD:
            return cardSearchRemovePendingCard(state, action);
  
        case CARD_SEARCH_CLEAR_PENDING_CARDS:
            return {
                ...state,
                pendingCards: {},
            }// as cardSearchPendingCardsState;

        case INVENTORY_ADD_COMPLETE:
            return {
                ...state,
                pendingCards: {},
                // pendingCardsSaving: false,
            } //as cardSearchPendingCardsState;


        default:
            return(state)
    }
}

const initialState: cardSearchPendingCardsState = {
    pendingCards: {}
}
