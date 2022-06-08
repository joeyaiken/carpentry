import { 
    // DECK_OVERVIEWS_REQUESTED, 
    // DECK_OVERVIEWS_RECEIVED, 
    DECK_DETAIL_REQUESTED, DECK_DETAIL_RECEIVED } from "./decksDataActions";

export interface State {
    // overviews: {
    //     decksById: { [id: number]: DeckOverviewDto };
    //     deckIds: number[];
    //     isLoading: boolean;
    //     isInitialized: boolean;
    // };
    detail: { //TODO - rename to 'deckDetail' or something
        isLoading: boolean;


        deckId: number;
    
        //deckProps: DeckPropertiesDto | null;
        deckProps: DeckPropertiesDto;

        cardOverviews: {
        //    ById[#:{
        //        name; count; cmc;
        //        details[#,#,#]
        //    }]
        //    AllIds
            byId: { [id: number]: DeckCardOverview }
            allIds: number[];
        };

        cardDetails: {
            byId: { [deckCardId: number]: DeckCardDetail };
            allIds: number[];
        };


        //This (InventoryOverviewDto) might need a GropId or something
        //THIS ISN'T GROUPED BY ID
        //It's grouped by NAME
        //cardOverviewsByName: { [name: string]: InventoryOverviewDto } //This contains a "group name" field
        // cardOverviewsById: { [id: number]: DeckCardOverview } //This contains a "group name" field

        //allCardOverviewNames: string[];
        // allCardOverviewIds: number[];
    
        // cardDetailsById: { [id: number]: DeckCard }; 
        // allCardDetailIds: number[];
    
        // selectedInventoryCardIds: number[];
    
        deckStats: DeckStats | null;
    
        cardGroups: NamedCardGroup[];
    }

    //searchResults: { overviews }
}

export const decksDataReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        // case DECK_OVERVIEWS_REQUESTED: return deckOverviewsRequested(state, action);
        // case DECK_OVERVIEWS_RECEIVED: return deckOverviewsReceived(state, action);
        case DECK_DETAIL_REQUESTED: return deckDetailRequested(state, action);
        case DECK_DETAIL_RECEIVED: return deckDetailReceived(state, action);
        default: return(state)
    }
}

const initialState: State = {
    // overviews: {
    //     decksById: {},
    //     deckIds: [],
    //     isLoading: false,
    //     isInitialized: false,
    // },
    detail: {
        isLoading: false,

        deckId: 0,
        deckProps: {
            id: 0,
            name: '',
            format: null,
            notes: '',
            basicW: 0,
            basicU: 0,
            basicB: 0,
            basicR: 0,
            basicG: 0,
        },

        cardDetails: {
            byId: {},
            allIds: [],
        },

        cardOverviews: {
            byId: {},
            allIds: [],
        },

        deckStats: null,
        cardGroups: [],
        // selectedInventoryCardIds: [],
    }   
}

// const deckOverviewsRequested = (state: State, action: ReduxAction): State => {
//     const newState: State = {
//         ...state,
//         overviews: {
//             ...initialState.overviews,
//             isLoading: true,
//         }
//     };
//     return newState;
// }

// const deckOverviewsReceived = (state: State, action: ReduxAction): State => {
//     const apiDecks: DeckOverviewDto[] = action.payload;
//
//     let decksById: { [key:number]: DeckOverviewDto } = {};
//
//     apiDecks?.forEach(deck => {
//         decksById[deck.id] = deck;
//     });
//
//     const newState: State = {
//         ...state,
//         overviews: {
//             deckIds: apiDecks?.map(deck => deck.id),
//             decksById: decksById,
//             isLoading: false,
//             isInitialized: true,
//         }
//     };
//
//     return newState;
// }

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
    
    let overviewsById: {[id: number]: DeckCardOverview} = {};
    let overviewIds: number[] = [];

    let detailsById: {[id: number]: DeckCardDetail} = {};
    let detailIds: number[] = [];

    dto.cards.forEach(cardOverview => {
        const overviewId = cardOverview.id;
        overviewIds.push(overviewId);
        overviewsById[overviewId] = {
            category: cardOverview.category,
            cmc: cardOverview.cmc,
            cost: cardOverview.cost,
            count: cardOverview.count,
            detailIds: cardOverview.details.map(detail => detail.id),
            id: cardOverview.id,
            img: cardOverview.img,
            name: cardOverview.name,
            type: cardOverview.type,
            cardId: cardOverview.cardId,
            tags: cardOverview.tags,
        };
        cardOverview.details.forEach(detail => {
            const detailId = detail.id;
            detailIds.push(detailId);
            detailsById[detailId] = {
                category: detail.category,
                id: detail.id,
                isFoil: detail.isFoil,
                name: detail.name,
                overviewId: cardOverview.id,
                set: detail.set,
                collectorNumber: detail.collectorNumber,
                inventoryCardId: detail.inventoryCardId,

                inventoryCardStatusId: 0,
                cardId: detail.cardId,
                deckId: detail.deckId,
                availabilityId: detail.availabilityId,

            };
        });
    });




    // dto.cardOverviews.forEach(card => overviewsById[card.id] = card);
    // dto.cards.forEach(card => cardsById[card.id] = card);

    // dto.cards.forEach(card => {
    //     overviewIds.push(card.id);
    //     overviewsById[card.id] = {
    //         category: card.category,
    //         cmc: card.cmc,
    //     }

    // });

    // const allCardOverviewIds: number[] = dto.cardOverviews.map(card => card.id);
    /* 
    cardOverviews: {
        byId: { [id: number]: DeckCardOverview }
        allIds: number[];
    };

    cardDetails: {
        byId: { [deckCardId: number]: DeckCardDetail };
        allIds: number[];
    };
    */
    const newState: State = {
        ...state,
        detail: {
            ...state.detail,

            isLoading: false,

            deckId: dto.props.id,
            deckProps: dto.props,
    
            cardOverviews: {
                byId: overviewsById,
                allIds: overviewIds,   
            },
            cardDetails: {
                byId: detailsById,
                allIds: detailIds,
            },

            // cardGroups: [],
            cardGroups: selectGroupedDeckCards(overviewsById, overviewIds),
            // cardGroupNames: [],
            deckStats: dto.stats,
        },
    };
    return newState;
}

function selectGroupedDeckCards(overviewsById: { [id: number]: DeckCardOverview }, allOverviewIds: number[]): NamedCardGroup[] {
    var result: NamedCardGroup[] = [];
    
    const cardGroups = ["Commander", "Creatures", "Spells", "Enchantments", "Artifacts", "Planeswalkers", "Lands", "Sideboard"];

    //Am I worried about the fact that cards might get excluded if I mess up the groups?....
    
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