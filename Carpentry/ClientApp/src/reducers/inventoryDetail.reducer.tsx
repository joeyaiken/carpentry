import { 
    API_DATA_REQUESTED, 
    API_DATA_RECEIVED 
} from '../actions/index.actions';

declare interface InventoryDetailState {
    //inventory cards
    inventoryCardsById: { [id: number]: InventoryCard };
    inventoryCardAllIds: number[];


    //--First step in showing an inventory detail is to itterate over MagicCard (or even card variant)
    //--Then would need to show each inventory card for a given magic card
    //      (all inventory cards where I.multiverseId == MC.multiverseId)

    //magic cards belonging to inventory cards
    cardsById: { [multiverseId: number]: MagicCard };
    allCardIds: number[];

}

const apiDataRequested = (state: InventoryDetailState, action: ReduxAction): InventoryDetailState => {
    const { scope } = action.payload;

    if(scope as ApiScopeOption !== "inventoryDetail") return (state);

    const newState: InventoryDetailState = {
        ...state,
        ...initialState,
    };

    return newState;
}

const apiDataReceived = (state: InventoryDetailState, action: ReduxAction): InventoryDetailState => {
    const { scope, data } = action.payload;
    if(scope as ApiScopeOption !== "inventoryDetail") return (state);

    const detailResult: InventoryDetailDto | null = data;

    if(detailResult == null){
        return {
            ...initialState,
        }
    }

    let inventoryCardsById = {}
    detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);

    let cardsById = {}
    detailResult.cards.forEach(card => cardsById[card.multiverseId] = card);

    const newState: InventoryDetailState = {
        ...state,

        inventoryCardsById: inventoryCardsById,
        inventoryCardAllIds: detailResult.inventoryCards.map(card => card.id),

        cardsById: cardsById,
        allCardIds: detailResult.cards.map(card => card.multiverseId),
        
        // selectedDetailItem: data,
    };
    return newState;


    // if(scope as ApiScopeOption === "inventoryOverview") {
    //     const newState: InventoryState = {
    //         ...state,
    //         inventoryItems: data,
    //     };
    //     return newState;
    // } else if(scope as ApiScopeOption === "inventoryDetail") {
        
    // }
    // else return (state);
}

// const inventorySearchMethodChanged = (state: InventoryState, action: ReduxAction): InventoryState => {
//     const newSearchMethod = action.payload;
//     const newState: InventoryState = {
//         ...state,
//         searchMethod: newSearchMethod,
//     };
//     return newState;
// }

// export const inventory = (state = initialState, action: ReduxAction): InventoryState => {
//     switch (action.type) {

//         case API_DATA_REQUESTED:
//             return apiDataRequested(state, action);

//         case API_DATA_RECEIVED:
//             return apiDataReceived(state, action);

//         // case INVENTORY_SEARCH_METHOD_CHANGED:
//         //     return inventorySearchMethodChanged(state, action);

//         default:
//             return (state)
//     }
// }


// const exampleReducerFunction = (state: InventoryDetailState, action: ReduxAction): InventoryDetailState => {
//     const newState: InventoryDetailState = {
//         ...state,
//     }
//     return newState;
// }

export const inventoryDetail = (state = initialState, action: ReduxAction): InventoryDetailState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);

        default:
            return(state)
    }
}

const initialState: InventoryDetailState = {
    inventoryCardsById: {},
    inventoryCardAllIds: [],
    cardsById: {},
    allCardIds: [],
}
