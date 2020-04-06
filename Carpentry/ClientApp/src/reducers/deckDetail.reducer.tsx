import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions/index.actions';
import { DECK_EDITOR_CARD_SELECTED, DECK_PROPERTY_CHANGED } from '../actions/deckEditor.actions';

declare interface DeckDetailState {

    deckId: number;

    deckProps: DeckProperties | null;

    //This (InventoryOverviewDto) might need a GropId or something
    //THIS ISN'T GROUPED BY ID
    //It's grouped by NAME
    //cardOverviewsByName: { [name: string]: InventoryOverviewDto } //This contains a "group name" field
    cardOverviewsById: { [id: number]: InventoryOverviewDto } //This contains a "group name" field
    //allCardOverviewNames: string[];
    allCardOverviewIds: number[];

    cardDetailsById: { [id: number]: InventoryCard }; 
    allCardDetailIds: number[];

    selectedInventoryCardIds: number[];

    deckStats: DeckStats | null;

    cardGroups: NamedCardGroup[];
}



export const apiDataRequested = (state: DeckDetailState, action: ReduxAction): DeckDetailState => {
    const { scope } = action.payload;
    
    if(scope as ApiScopeOption !== "deckDetail") return (state);

    const newState: DeckDetailState = {
        ...state,
        // selectedDeckDto: null,
        ...initialState,
        // decks: data,
    };
    
    return newState;
}

function selectGroupedDeckCards(overviewsById: { [id: number]: InventoryOverviewDto }, allOverviewIds: number[]): NamedCardGroup[] {
    var result: NamedCardGroup[] = [];
    // console.log('grouping deck editor cards')
    const cardGroups = ["Commander", "Creatures", "Spells", "Enchantments", "Artifacts", "Planeswalkers", "Lands", "Sideboard"];
    
    cardGroups.forEach(groupName => {

        const cardsInGroup = allOverviewIds.filter(id => overviewsById[id].description === groupName);

        if(cardsInGroup.length > 0){
            result.push({
                name: groupName,
                cardOverviewIds: cardsInGroup
            });
        }
    });

    return result;
}

export const apiDataReceived = (state: DeckDetailState, action: ReduxAction): DeckDetailState => {
    const { scope, data } = action.payload;
    
    if(scope as ApiScopeOption !== "deckDetail") return (state);

    const dto: DeckDto = data;

    let overviewsById = {};
    let cardsById = {};
    
    dto.cardOverviews.forEach(card => overviewsById[card.id] = card);
    dto.cardDetails.forEach(card => cardsById[card.id] = card);

    const allCardOverviewIds: number[] = dto.cardOverviews.map(card => card.id);

    const newState: DeckDetailState = {
        ...state,
        // selectedDeckDto: data,
        deckId: dto.props.id,
        deckProps: dto.props,

        //cardOverviewsById: overviewsByName
        cardOverviewsById: overviewsById,
        allCardOverviewIds: allCardOverviewIds,

        cardDetailsById: cardsById,
        allCardDetailIds: dto.cardDetails.map(card => card.id),

        // cardGroups: [],
        cardGroups: selectGroupedDeckCards(overviewsById, allCardOverviewIds),
        // cardGroupNames: [],
        deckStats: dto.stats,

    };
    
    return newState;
}

export const deckDetail = (state = initialState, action: ReduxAction): DeckDetailState => {
    switch(action.type){

        case DECK_EDITOR_CARD_SELECTED:
            const selectedCardOverview: InventoryOverviewDto = action.payload;
            // let visibleCards: InventoryCard[] = [];
            // visibleCards = state.selectedDeckDto.cardDetails.filter((card) => (card.name === selectedCardOverview.name));

            const visibleCardIds = state.allCardDetailIds.filter(id => 
                state.cardDetailsById[id].name === selectedCardOverview.name
                && selectedCardOverview.description === state.cardDetailsById[id].deckCards[0].category
                //&& state.cardDetailsById[id].
                );
            // console.log('visible cards!');
            // const visibleCards = visibleCardIds.map(id => state.cardDetailsById[id]);
            // console.log(state)
            // console.log(visibleCards);
            return {
                ...state,
                // selectedCard: selectedCardOverview,
                selectedInventoryCardIds: visibleCardIds,
            }

        case DECK_PROPERTY_CHANGED:
            const propName: string = action.payload.name;
            const propValue: string = action.payload.value;

            if(state.deckProps){
                const updatedState: DeckDetailState = {
                    ...state,
                    deckProps: {
                        ...state.deckProps,
                        [propName]: propValue
                    }
                }
                return updatedState;
            }
            return {
                ...state,
            }
            
        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);

        default:
            // console.log('ui init')
            return(state)
    }
}

const initialState: DeckDetailState = {
    deckId: 0,
    deckProps: null,
          
    cardOverviewsById: {}, //This contains a "group name" field
    allCardOverviewIds: [],

    cardDetailsById: {},
    allCardDetailIds: [],
    
    deckStats: null,
    cardGroups: [],
    selectedInventoryCardIds: [],
}
