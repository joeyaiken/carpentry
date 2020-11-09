import { DECK_OVERVIEWS_REQUESTED, DECK_OVERVIEWS_RECEIVED, DECK_DETAIL_REQUESTED, DECK_DETAIL_RECEIVED } from "./decksDataActions";

export interface State {
    overviews: {
        decksById: { [id: number]: DeckOverviewDto };
        deckIds: number[];
        isLoading: boolean;
        isInitialized: boolean;
    };
    detail: {
        isLoading: boolean;


        deckId: number;
    
        deckProps: DeckPropertiesDto | null;
    
        //This (InventoryOverviewDto) might need a GropId or something
        //THIS ISN'T GROUPED BY ID
        //It's grouped by NAME
        //cardOverviewsByName: { [name: string]: InventoryOverviewDto } //This contains a "group name" field
        cardOverviewsById: { [id: number]: DeckCardOverview } //This contains a "group name" field
        //allCardOverviewNames: string[];
        allCardOverviewIds: number[];
    
        cardDetailsById: { [id: number]: DeckCard }; 
        allCardDetailIds: number[];
    
        selectedInventoryCardIds: number[];
    
        deckStats: DeckStats | null;
    
        cardGroups: NamedCardGroup[];
    }
}

export const decksDataReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case DECK_OVERVIEWS_REQUESTED: return deckOverviewsRequested(state, action);
        case DECK_OVERVIEWS_RECEIVED: return deckOverviewsReceived(state, action);
        case DECK_DETAIL_REQUESTED: return deckDetailRequested(state, action);
        case DECK_DETAIL_RECEIVED: return deckDetailReceived(state, action);
        default: return(state)
    }
}

const initialState: State = {
    overviews: {
        decksById: {},
        deckIds: [],
        isLoading: false,
        isInitialized: false,
    },
    detail: {
        isLoading: false,

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
}

const deckOverviewsRequested = (state: State, action: ReduxAction): State => {
    const newState: State = {
        ...state,
        overviews: {
            ...initialState.overviews,
            isLoading: true,
        }
    };
    return newState;
}

const deckOverviewsReceived = (state: State, action: ReduxAction): State => {
    const apiDecks: DeckOverviewDto[] = action.payload;

    let decksById: { [key:number]: DeckOverviewDto } = {};

    apiDecks.forEach(deck => {
        decksById[deck.id] = deck;
    });

    const newState: State = {
        ...state,
        overviews: {
            deckIds: apiDecks.map(deck => deck.id),
            decksById: decksById,
            isLoading: false,
            isInitialized: true,
        }
    };

    return newState;
}

const deckDetailRequested = (state: State, action: ReduxAction): State => {
    const newState: State = {
        ...state,
        detail: {
            ...initialState.detail,
            isLoading: true,
        }
    };
    return newState;
}

const deckDetailReceived = (state: State, action: ReduxAction): State => {
    const dto: DeckDetailDto = action.payload;
    
    let overviewsById = {};
    let cardsById = {};
    
    dto.cardOverviews.forEach(card => overviewsById[card.id] = card);
    dto.cards.forEach(card => cardsById[card.id] = card);

    const allCardOverviewIds: number[] = dto.cardOverviews.map(card => card.id);

    const newState: State = {
        ...state,
        detail: {
            ...state.detail,

            isLoading: false,

            // selectedDeckDto: data,
            deckId: dto.props.id,
            deckProps: dto.props,
    
            //cardOverviewsById: overviewsByName
            cardOverviewsById: overviewsById,
            allCardOverviewIds: allCardOverviewIds,
    
            cardDetailsById: cardsById,
            allCardDetailIds: dto.cards.map(card => card.id),
    
            // cardGroups: [],
            cardGroups: selectGroupedDeckCards(overviewsById, allCardOverviewIds),
            // cardGroupNames: [],
            deckStats: dto.stats,
        },
    };
    return newState;
}

function selectGroupedDeckCards(overviewsById: { [id: number]: DeckCardOverview }, allOverviewIds: number[]): NamedCardGroup[] {
    var result: NamedCardGroup[] = [];
    // console.log('grouping deck editor cards')
    const cardGroups = ["Commander", "Creatures", "Spells", "Enchantments", "Artifacts", "Planeswalkers", "Lands", "Sideboard"];
    
    cardGroups.forEach(groupName => {

        const cardsInGroup = allOverviewIds.filter(id => overviewsById[id].category === groupName);

        if(cardsInGroup.length > 0){
            result.push({
                name: groupName,
                cardOverviewIds: cardsInGroup
            });
        }
    });

    return result;
}