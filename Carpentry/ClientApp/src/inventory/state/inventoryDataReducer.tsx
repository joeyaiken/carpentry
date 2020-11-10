import { 
    INVENTORY_OVERVIEWS_RECEIVED, 
    INVENTORY_OVERVIEWS_REQUESTED,
    INVENTORY_DETAIL_REQUESTED,
    INVENTORY_DETAIL_RECEIVED,
    TRIMMING_TIPS_REQUESTED,
    TRIMMING_TIPS_RECEIVED,
} from "./InventoryDataActions";

export interface InventoryDataReducerState {
    //Overviews
    overviews: {
        isLoading: boolean;
        //This needs to be able to handle any of the 3 groupings
        //When grouped by name, I should apply an index on the C# layer

        byId: { [id: number]: InventoryOverviewDto };
        allIds: number[];
    }
    detail: {
        selectedCardId: number | null;
        isLoading: boolean;
        selectedCardName: string;
        //inventory cards
        inventoryCardsById: { [id: number]: InventoryCard };
        inventoryCardAllIds: number[];
    
        //magic cards belonging to inventory cards
        cardsById: { [multiverseId: number]: MagicCard };
        allCardIds: number[];
    }
}

export const inventoryDataReducer = (state = initialState, action: ReduxAction): InventoryDataReducerState => {
    switch(action.type){
        case INVENTORY_OVERVIEWS_REQUESTED: return inventoryOverviewsRequested(state, action);
        case INVENTORY_OVERVIEWS_RECEIVED: return inventoryOverviewsReceived(state, action);

        case INVENTORY_DETAIL_REQUESTED: return inventoryDetailRequested(state, action);
        case INVENTORY_DETAIL_RECEIVED: return inventoryDetailReceived(state, action);
        
        case TRIMMING_TIPS_REQUESTED: return trimmingTipsRequested(state, action);
        case TRIMMING_TIPS_RECEIVED: return trimmingTipsReceived(state, action);
        
        default: return(state)
    }
}

const initialState: InventoryDataReducerState = {
    overviews: {
        isLoading: false,
        byId: {},
        allIds: [],
    },
    detail: {
        selectedCardId: null,
        selectedCardName: "",
        isLoading: false,
        inventoryCardsById: {},
        inventoryCardAllIds: [],
        cardsById: {},
        allCardIds: [],
    }   
}

const inventoryOverviewsRequested = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    const newState: InventoryDataReducerState = {
        ...state,
        overviews: {
            ...initialState.overviews,
            isLoading: true,
        }
    };
    return newState;
}

const inventoryOverviewsReceived = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    const apiOverviews: InventoryOverviewDto[] = action.payload;

    let overviewsById = {}

    apiOverviews.forEach(item => {
        // console.log(`adding name ${item.name}`)
        overviewsById[item.id] = item;
    });

    // console.log('INVENTORY OVERVIEWS RECEIVED')

    const newState: InventoryDataReducerState = {
        ...state,
        overviews: {
            isLoading: false,
            byId: overviewsById,
            allIds: apiOverviews.map(item => item.id),
        }
    };

    return newState;
}

const inventoryDetailRequested = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    const selectedCardId = action.payload;
    const newState: InventoryDataReducerState = {
        ...state,
        detail: {
            ...initialState.detail,
            isLoading: true,
            selectedCardId: selectedCardId, //will be number
        }
    };
    return newState;
}

const inventoryDetailReceived = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    const detailResult: InventoryDetailDto | null = action.payload;

    if(detailResult === null){
        return {
            ...initialState,
        }
    }

    let inventoryCardsById = {}
    detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);

    let cardsById = {}
    detailResult.cards.forEach(card => cardsById[card.multiverseId] = card);

    const newState: InventoryDataReducerState = {
        ...state,
        detail: {
            selectedCardId: state.detail.selectedCardId,
            selectedCardName: detailResult.name,
            isLoading: false,

            inventoryCardsById: inventoryCardsById,
            inventoryCardAllIds: detailResult.inventoryCards.map(card => card.id),
            
            cardsById: cardsById,
            allCardIds: detailResult.cards.map(card => card.multiverseId),
        },
    };
    return newState;
}

const trimmingTipsRequested = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    return state;
}

const trimmingTipsReceived = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    return state;
}