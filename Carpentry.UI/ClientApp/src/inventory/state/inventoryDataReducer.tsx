import { API_DATA_RECEIVED, API_DATA_REQUESTED } from "../../_actions";

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
        isLoading: boolean;
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

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);

        default:
            return(state)
    }
}

const initialState: InventoryDataReducerState = {
    overviews: {
        isLoading: false,
        byId: {},
        allIds: [],
    },
    detail: {
        isLoading: false,
        inventoryCardsById: {},
        inventoryCardAllIds: [],
        cardsById: {},
        allCardIds: [],
    }   
}

const apiDataRequested = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    const { scope } = action.payload;
    
    if(scope as ApiScopeOption === "inventoryOverview"){
        const newState: InventoryDataReducerState = {
            ...state,
            overviews: {
                ...initialState.overviews,
                isLoading: true,
            }
        };
    
        return newState;
    }
    else if (scope as ApiScopeOption === "inventoryDetail"){
        const newState: InventoryDataReducerState = {
            ...state,
            detail: {
                ...initialState.detail,
                isLoading: true,
            }
        };
        return newState;
    }
    else {
        return (state);
    }
}

const apiDataReceived = (state: InventoryDataReducerState, action: ReduxAction): InventoryDataReducerState => {
    const { scope, data } = action.payload;

    if(scope as ApiScopeOption === "inventoryOverview"){
        const apiOverviews: InventoryOverviewDto[] = data;

        let overviewsById = {}

        apiOverviews.forEach(item => {
            // console.log(`adding name ${item.name}`)
            overviewsById[item.id] = item;
        });

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
    else if (scope as ApiScopeOption === "inventoryDetail"){
        const detailResult: InventoryDetailDto | null = data;

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
                isLoading: false,

                inventoryCardsById: inventoryCardsById,
                inventoryCardAllIds: detailResult.inventoryCards.map(card => card.id),
                
                cardsById: cardsById,
                allCardIds: detailResult.cards.map(card => card.multiverseId),
            },
        };
        return newState;
    }
    else {
        return (state);
    }
}
