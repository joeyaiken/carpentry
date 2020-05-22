import { API_DATA_REQUESTED, API_DATA_RECEIVED } from '../actions';

export interface DeckDataReducerState {
    overviews: {
        decksById: { [id: number]: DeckOverviewDto };
        deckIds: number[];
        isLoading: boolean;
        isInitialized: boolean;
    };
    detail: {
        isLoading: boolean;


        deckId: number;
    
        deckProps: DeckProperties | null;
    
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

export const deckDataReducer = (state = initialState, action: ReduxAction): DeckDataReducerState => {
    switch(action.type){

        case API_DATA_REQUESTED:
            return apiDataRequested(state, action);

        case API_DATA_RECEIVED:
            return apiDataReceived(state, action);

        default:
            return(state)
    }
}

const initialState: DeckDataReducerState = {
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

const apiDataRequested = (state: DeckDataReducerState, action: ReduxAction): DeckDataReducerState => {
    const { scope } = action.payload;
    
    if(scope as ApiScopeOption === "deckOverviews"){
        const newState: DeckDataReducerState = {
            ...state,
            overviews: {
                ...initialState.overviews,
                isLoading: true,
            }
        };
        return newState;
    }
    else if (scope as ApiScopeOption === "deckDetail"){
        const newState: DeckDataReducerState = {
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

const apiDataReceived = (state: DeckDataReducerState, action: ReduxAction): DeckDataReducerState => {
    const { scope, data } = action.payload;

        if(scope as ApiScopeOption === "deckOverviews"){
            //I guess this is normally where Normalizr should be used?
        const apiDecks: DeckOverviewDto[] = data;

        let decksById: { [key:number]: DeckOverviewDto } = {};

        apiDecks.forEach(deck => {
            decksById[deck.id] = deck;
        });

        const newState: DeckDataReducerState = {
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
    else if (scope as ApiScopeOption === "deckDetail"){
        const dto: DeckDetailDto = data;
    
        let overviewsById = {};
        let cardsById = {};
        
        dto.cardOverviews.forEach(card => overviewsById[card.id] = card);
        dto.cards.forEach(card => cardsById[card.id] = card);
    
        const allCardOverviewIds: number[] = dto.cardOverviews.map(card => card.id);
    
        const newState: DeckDataReducerState = {
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
    else {
        return (state);
    }
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