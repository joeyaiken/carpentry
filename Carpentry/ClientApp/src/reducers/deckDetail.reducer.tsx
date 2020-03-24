import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions/index.actions';
import { DECK_EDITOR_CARD_SELECTED, DECK_PROPERTY_CHANGED } from '../actions/deckEditor.actions';

declare interface DeckDetailState {
    deckProps: DeckProperties | null;

    //This (InventoryOverviewDto) might need a GropId or something
    //THIS ISN'T GROUPED BY ID
    //It's grouped by NAME
    cardOverviewsByName: { [name: string]: InventoryOverviewDto } //This contains a "group name" field
    allCardOverviewNames: string[];

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

function selectGroupedDeckCards(overviewsByName: { [name: string]: InventoryOverviewDto }, allOverviewNames: string[]): NamedCardGroup[] {
    var result: NamedCardGroup[] = [];
    
    const cardGroups = ["Creatures", "Spells", "Enchantments", "Artifacts", "Planeswalkers", "Lands"];
    
    cardGroups.forEach(groupName => {

        const cardsInGroup = allOverviewNames.filter(name => overviewsByName[name].description == groupName);

        if(cardsInGroup.length > 0){
            result.push({
                name: groupName,
                cardNames: cardsInGroup
            });
        }
    });

    return result;
}

export const apiDataReceived = (state: DeckDetailState, action: ReduxAction): DeckDetailState => {
    const { scope, data } = action.payload;
    
    if(scope as ApiScopeOption !== "deckDetail") return (state);

    const dto: DeckDto = data;

    let overviewsByName = {};
    let cardsById = {};
    
    dto.cardOverviews.forEach(card => overviewsByName[card.name] = card);
    dto.cardDetails.forEach(card => cardsById[card.id] = card);

    const allCardOverviewNames: string[] = dto.cardOverviews.map(card => card.name);

    const newState: DeckDetailState = {
        ...state,
        // selectedDeckDto: data,
        deckProps: dto.props,

        //cardOverviewsById: overviewsByName
        cardOverviewsByName: overviewsByName,
        allCardOverviewNames: allCardOverviewNames,

        cardDetailsById: cardsById,
        allCardDetailIds: dto.cardDetails.map(card => card.id),

        // cardGroups: [],
        cardGroups: selectGroupedDeckCards(overviewsByName, allCardOverviewNames),
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
            // visibleCards = state.selectedDeckDto.cardDetails.filter((card) => (card.name == selectedCardOverview.name));

            const visibleCardIds = state.allCardDetailIds.filter(id => state.cardDetailsById[id].name == selectedCardOverview.name);

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
    deckProps: null,
          
    cardOverviewsByName: {}, //This contains a "group name" field
    allCardOverviewNames: [],

    cardDetailsById: {},
    allCardDetailIds: [],
    
    deckStats: null,
    cardGroups: [],
    selectedInventoryCardIds: [],
}
