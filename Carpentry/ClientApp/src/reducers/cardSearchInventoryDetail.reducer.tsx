import { 
    API_DATA_REQUESTED, 
    API_DATA_RECEIVED 
} from '../actions/index.actions';

declare interface CardSearchInventoryDetailState {
    //inventory cards
    inventoryCardsById: { [id: number]: InventoryCard };
    inventoryCardAllIds: number[];


    //--First step in showing an inventory detail is to itterate over MagicCard (or even card variant)
    //--Then would need to show each inventory card for a given magic card
    //      (all inventory cards where I.multiverseId === MC.multiverseId)

    //magic cards belonging to inventory cards
    cardsById: { [multiverseId: number]: MagicCard };
    allCardIds: number[];

}

const apiDataRequested = (state: CardSearchInventoryDetailState, action: ReduxAction): CardSearchInventoryDetailState => {
    const { scope } = action.payload;

    if(scope as ApiScopeOption !== "cardSearchInventoryDetail") return (state);

    const newState: CardSearchInventoryDetailState = {
        ...state,
        ...initialState,
    };

    return newState;
}

const apiDataReceived = (state: CardSearchInventoryDetailState, action: ReduxAction): CardSearchInventoryDetailState => {
    const { scope, data } = action.payload;
    if(scope as ApiScopeOption !== "cardSearchInventoryDetail") return (state);

    const detailResult: InventoryDetailDto = data;

    let inventoryCardsById = {}
    detailResult.inventoryCards.forEach(invCard => inventoryCardsById[invCard.id] = invCard);

    let cardsById = {}
    detailResult.cards.forEach(card => cardsById[card.multiverseId] = card);

    const newState: CardSearchInventoryDetailState = {
        ...state,

        inventoryCardsById: inventoryCardsById,
        inventoryCardAllIds: detailResult.inventoryCards.map(card => card.id),

        cardsById: cardsById,
        allCardIds: detailResult.cards.map(card => card.multiverseId),
    };
    return newState;
}

export const cardSearchInventoryDetail = (state = initialState, action: ReduxAction): CardSearchInventoryDetailState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);

        default:
            return(state)
    }
}

const initialState: CardSearchInventoryDetailState = {
    inventoryCardsById: {},
    inventoryCardAllIds: [],
    cardsById: {},
    allCardIds: [],
}
