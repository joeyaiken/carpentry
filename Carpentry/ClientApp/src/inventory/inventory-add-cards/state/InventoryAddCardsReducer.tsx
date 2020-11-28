import { CARD_SEARCH_RECEIVED, CARD_SEARCH_REQUESTED } from "./InventoryAddCardsActions";

export interface State {
    searchResults: {
        isLoading: boolean;
        searchResultsById: {[multiverseId: number]: CardSearchResultDto};
        allSearchResultIds: number[];
    };

    searchFilter: CardFilterProps;
    viewMode: CardSearchViewMode;
    pendingCardsSaving: boolean;
    cardSearchMethod: "set" | "web" | "inventory";
    selectedCard: CardSearchResultDto | null; //should probably be an AppState ID
    pendingCards: { [name: string]: PendingCardsDto } //key === name, should this also have a list to track all keys?
}

export const inventoryAddCardsReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case CARD_SEARCH_REQUESTED: return cardSearchRequested(state, action);
        case CARD_SEARCH_RECEIVED: return cardSearchReceived(state, action);

        // case CARD_SEARCH_ADD_PENDING_CARD:
        //     return cardSearchAddPendingCard(state, action);
            
        // case CARD_SEARCH_REMOVE_PENDING_CARD:
        //     return cardSearchRemovePendingCard(state, action);
    
        // case CARD_SEARCH_CLEAR_PENDING_CARDS:
        //     return {
        //         ...state,
                
        //         pendingCards: {},
        //     }// as cardSearchPendingCardsState;

        // case INVENTORY_ADD_COMPLETE:
        //     return {
        //         ...state,
        //         pendingCards: {},
        //         // pendingCardsSaving: false,
        //     } //as cardSearchPendingCardsState;

        default: return(state);
    }
}

const initialState: State = {
    searchResults: {
        isLoading: false,
        searchResultsById: {},
        allSearchResultIds: [],
    },

    searchFilter: defaultSearchFilterProps(),
    viewMode: "list",
    pendingCardsSaving: false,
    cardSearchMethod: "set",
    selectedCard:  null, //should probably be an AppState ID
    pendingCards: {},
};

//reducer helpers



//legacy trash --------------------------------------------------

// const cardSearchAddPendingCard = (state = initialState, action: ReduxAction): CardSearchDataReducerState => {
//     //'pending cards' is now a dictionary of 'pending card dto's
//     const {
//         // data, //0
//         name,
//         cardId,
//         isFoil, //false
//         // variant, //"normal"

//         //need
//         //cardId (for inv card)
//         //cardName (for display, and grouping?)
//     } = action.payload;
    
//     // const magicCardToAdd: MagicCard = data;
    
//     let cardToAdd: PendingCardsDto = state.pendingCards[name];
    
//     if(!cardToAdd){
//         cardToAdd = {
//             name: name,
//             cards: [],
//         };

//         //OK - So I need to either pass the name, or the whole card
        
        
//         // const magicCardToAdd = state.
//         // if we can't find a matching card, nothing gets added and this just continues silently (I guess)
//     //     if(magicCardToAdd){
//     //         cardToAdd = {
//     //             multiverseId: multiverseId,
//     //             cards: [],
//     //             // data: magicCardToAdd,
//     //         };
//     //     }                    
//     }

//     //These are the only 3 fields used by the api bulkAdd
//     cardToAdd.cards.push({
//         cardId: cardId,
//         isFoil: isFoil,
//         statusId: 1,
//     } as InventoryCard);

//     const newState: CardSearchDataReducerState = {
//         ...state,
//         pendingCards: {
//             ...state.pendingCards,
//             [name]: cardToAdd
//         }
//     }
//     return newState;
// }

// const cardSearchRemovePendingCard = (state = initialState, action: ReduxAction): CardSearchDataReducerState => {
//     const {
//         name,
//         cardId,
//         isFoil,
//     } = action.payload;

//     // const midToRemove = action.payload.multiverseId;
//     // const removeFoilCard = action.payload.isFoil;
//     // const variantToRemove: string = action.payload.variant;

//     let objToRemoveFrom = state.pendingCards[name];

//     if(objToRemoveFrom){

//         //let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.variantName === variantToRemove && x.isFoil === removeFoilCard);
//         let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.cardId === cardId && x.isFoil === isFoil);

//         if(thisInvCard >= 0){
//             objToRemoveFrom.cards.splice(thisInvCard,1);
//         }

//         let pendingCardsAfterRemoval =  {
//             ...state.pendingCards,
//             [name]: objToRemoveFrom
//         }
//         //if this pending cards object has 0 items, it should be deleted from the dictionary
//         if(objToRemoveFrom.cards.length === 0){
//             delete pendingCardsAfterRemoval[name];
//         }
//         const newState: CardSearchDataReducerState = {
//             ...state,
//             pendingCards: pendingCardsAfterRemoval
//         }
//         return newState;

//     } else {
//         const newState: CardSearchDataReducerState = {
//             ...state,
//         }
//         return newState;
//     }
// }

function cardSearchRequested(state: State, action: ReduxAction): State {
    const newState: State = {
        ...state,
        searchResults: {
            ...initialState.searchResults,
            isLoading: true,
        }
    }
    return newState;
}

function cardSearchReceived(state: State, action: ReduxAction): State {
    const searchResultPayload: CardSearchResultDto[] = action.payload || [];
    let resultsById = {};
    searchResultPayload.forEach(card => resultsById[card.cardId] = card);
    const newState: State = {
        ...state,
        searchResults: {
            ...state.searchResults,
            isLoading: false,
            searchResultsById: resultsById,
            allSearchResultIds: searchResultPayload.map(card => card.cardId),
        }
    }
    return newState;
}


import { CARD_SEARCH_ADD_PENDING_CARD, CARD_SEARCH_CLEAR_PENDING_CARDS, CARD_SEARCH_FILTER_VALUE_CHANGED, CARD_SEARCH_REMOVE_PENDING_CARD, CARD_SEARCH_SELECT_CARD } from "./InventoryAddCardsActions";

export interface State {
    searchFilter: CardFilterProps;

    viewMode: CardSearchViewMode;
    pendingCardsSaving: boolean;
    cardSearchMethod: "set" | "web" | "inventory";
    selectedCard: CardSearchResultDto | null; //should probably be an AppState ID

    pendingCards: { [name: string]: PendingCardsDto } //key === name, should this also have a list to track all keys?
}

  export interface UiReducerState {
      //filters
      cardSearchFilterProps: CardFilterProps;
  
    //   //Could/should I use a single menu anchor?
    //   deckListMenuAnchor: HTMLButtonElement | null;   //in Deck List 
    //   deckEditorMenuAnchor: HTMLButtonElement | null;       //in Deck Editor (rename?)
    //   inventoryDetailMenuAnchor: HTMLButtonElement | null;
  
  
      //modal for showing inventory detail?
  
      //misc
      //Should controll this with a single bool
      deckPropsModalOpen: boolean;
      isNewDeckModalOpen: boolean; //Should be removed
  
      isInventoryDetailModalOpen: boolean;
  
      //newDeckDto: DeckProperties;
      deckModalProps: DeckPropertiesDto | null; //shown in the deckPropsModal
  
      newDeckDto: DeckPropertiesDto;
  
      // //legacy
      // //ui - Data that represents how the UI is currently displayed
      
      // //filters
      // inventoryFilterProps: CardFilterProps;
      // cardSearchFilterProps: CardFilterProps;
  
      // //menu anchors
      // deckListMenuAnchor: HTMLButtonElement | null; //in Deck List 
      // cardMenuAnchor: HTMLButtonElement | null; //in Deck Editor (rename?)
  
      // //misc
      // deckPropsModalOpen: boolean;
      // isNewDeckModalOpen: boolean; //Should be removed
  }
  
  // const apiDataReceived = (state: UiReducerState, action: ReduxAction): UiReducerState => {
  //     const { scope, data } = action.payload;
  
  //     // switch(scope as ApiScopeOption){
  //     //     case "inventoryDetail":
  
  //     // }
  //     if (scope as ApiScopeOption === "inventoryDetail"){
  //         const newState: UiReducerState = {
  //             ...state,
  //             isInventoryDetailModalOpen: Boolean(data),
  //         };
  //         return newState;
  //     }
  //     else if (scope as ApiScopeOption === "deckDetail"){
  //         const newState: UiReducerState = {
  //             ...state,
  //             deckPropsModalOpen: false,
  //         };
  //         return newState;
  //     } else return (state);
  
  //     // if(scope as ApiScopeOption !== "deckDetail") return (state);
  //     // // if(!data){
  //     // //     return {
  //     // //         ...state,
  //     // //         isInventoryDetailModalOpen: false,
  //     // //     }
  //     // // }
  //     // console.log('closing deck props modal')
  //     // const newState: UiReducerState = {
  //     //     ...state,
  //     //     isInventoryDetailModalOpen: Boolean(data),
  //     //     deckPropsModalOpen: false,
  //     // };
  //     // return newState;
  // }
  

  
  // const menuButtonClicked = (state: UiReducerState, action: ReduxAction): UiReducerState => {
  //     const { type, anchor } = action.payload;
  
  //     // console.log(`menuButtonClicked :${type}`);
  //     const newState: UiReducerState = {
  //         ...state,
  //         //deckListMenuAnchor: action.payload
  //         [type]: anchor
  //     }
  //     return newState;
  // }
  
  // const menuOptionSelected = (state: UiReducerState, action: ReduxAction): UiReducerState => {
  //     const anchorType = action.payload;
  //     const newState: UiReducerState = {
  //         ...state,
  //         //deckListMenuAnchor: null
  //         [anchorType]: null
  //     }
  //     return newState;
  // }
  
  // const resetCardSearchFilterProps = (state: UiReducerState, action: ReduxAction): UiReducerState => {
  //     const newState: UiReducerState = {
  //         ...state,
  //         cardSearchFilterProps: initialCardSearchFilterProps(),
  //     }
  //     return newState;
  // }
  
export const cardSearchReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case CARD_SEARCH_FILTER_VALUE_CHANGED:
            return filterValueChanged(state, action);
        case CARD_SEARCH_ADD_PENDING_CARD:
            return cardSearchAddPendingCard(state, action);
            
        case CARD_SEARCH_REMOVE_PENDING_CARD:
            return cardSearchRemovePendingCard(state, action);
    
        case CARD_SEARCH_CLEAR_PENDING_CARDS:
            return {
                ...state,
                
                pendingCards: {},
            }// as cardSearchPendingCardsState;

        case CARD_SEARCH_SELECT_CARD:
            const selectedCard: CardSearchResultDto = action.payload;
            // console.log('card search - card selected')
            // console.log(selectedCard)
            return {
                ...state,
                selectedCard: selectedCard,
            };

        default:
            return(state);
            // deckOverviewsReceived
    }
}

const cardSearchAddPendingCard = (state = initialState, action: ReduxAction): State => {
    //'pending cards' is now a dictionary of 'pending card dto's
    const {
        // data, //0
        name,
        cardId,
        isFoil, //false
        // variant, //"normal"

        //need
        //cardId (for inv card)
        //cardName (for display, and grouping?)
    } = action.payload;
    
    // const magicCardToAdd: MagicCard = data;
    
    let cardToAdd: PendingCardsDto = state.pendingCards[name];
    
    if(!cardToAdd){
        cardToAdd = {
            name: name,
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

    //These are the only 3 fields used by the api bulkAdd
    cardToAdd.cards.push({
        cardId: cardId,
        isFoil: isFoil,
        statusId: 1,
    } as InventoryCard);

    const newState: State = {
        ...state,
        pendingCards: {
            ...state.pendingCards,
            [name]: cardToAdd
        }
    }
    return newState;
}

const cardSearchRemovePendingCard = (state = initialState, action: ReduxAction): State => {
    const {
        name,
        cardId,
        isFoil,
    } = action.payload;

    // const midToRemove = action.payload.multiverseId;
    // const removeFoilCard = action.payload.isFoil;
    // const variantToRemove: string = action.payload.variant;

    let objToRemoveFrom = state.pendingCards[name];

    if(objToRemoveFrom){

        //let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.variantName === variantToRemove && x.isFoil === removeFoilCard);
        let thisInvCard = objToRemoveFrom.cards.findIndex(x => x.cardId === cardId && x.isFoil === isFoil);

        if(thisInvCard >= 0){
            objToRemoveFrom.cards.splice(thisInvCard,1);
        }

        let pendingCardsAfterRemoval =  {
            ...state.pendingCards,
            [name]: objToRemoveFrom
        }
        //if this pending cards object has 0 items, it should be deleted from the dictionary
        if(objToRemoveFrom.cards.length === 0){
            delete pendingCardsAfterRemoval[name];
        }
        const newState: State = {
            ...state,
            pendingCards: pendingCardsAfterRemoval
        }
        return newState;

    } else {
        const newState: State = {
            ...state,
        }
        return newState;
    }
}

const filterValueChanged = (state: State, action: ReduxAction): State => {
    //const { type, filter, value } = action.payload;
    const { type, filter, value } = action.payload;
    console.log('CS filter changed');
    console.log(action.payload);

    const existingFilter = state.searchFilter;
    const newState: State = {
        ...state,
        searchFilter: {
            ...existingFilter, 
            [filter]: value,
        }
    }
    return newState;
}

//   export const uiReducer = (state = initialState, action: ReduxAction): UiReducerState => {
//       switch(action.type){
//           // case API_DATA_RECEIVED:
//           //     return apiDataReceived(state, action);
  
//         //   case FILTER_VALUE_CHANGED: 
//         //       return filterValueChanged(state, action);
              
//           // case MENU_BUTTON_CLICKED:
//           //     return menuButtonClicked(state, action);
  
//           // case MENU_OPTION_SELECTED:
//           //     return menuOptionSelected(state, action);
  
//           // case CARD_SEARCH_SEARCH_METHOD_CHANGED:
//           //     return resetCardSearchFilterProps(state, action);
  
//           // case OPEN_NEW_DECK_MODAL:
//           //     return {
//           //         ...state,
//           //         isNewDeckModalOpen: true,
//           //         newDeckDto: emptyDeckDto(),
//           //     }
  
//           // case CANCLE_NEW_DECK: 
//           //     return {
//           //         ...state,
//           //         isNewDeckModalOpen: false,
//           //     }
  
//           // case NEW_DECK_FIELD_CHANGE:
//           //     const modalPropName: string = action.payload.name;
//           //     const modalPropValue: string = action.payload.value;
//           //     return {
//           //         ...state,
//           //         newDeckDto: {
//           //             ...state.newDeckDto,
//           //             [modalPropName]: modalPropValue
//           //         }
//           //     }
//           // case CARD_MENU_BUTTON_CLICK:
//           //     return {
//           //         ...state,
//           //         deckEditorMenuAnchor: action.payload
//           //     }
  
//           // case DECK_CARD_REQUEST_ALTERNATE_VERSIONS:
//           //     return {
//           //         ...state,
//           //         deckEditorMenuAnchor: null,
//           //     }
  
//           // case OPEN_DECK_PROPS_MODAL:
//           //     console.log('opening deck props modal')
//           //     return {
//           //         ...state,
//           //         deckPropsModalOpen: true,
//           //     }
  
//           // case APP_BAR_ADD_CLICKED:
//           //     //under certain conditions, method should be set to Inventory
//           //     //FWIW something seems off with this approach, I can't reference state, I need to rely on a payload
  
//           //     const filters: FilterDescriptor[] | undefined = action.payload;
//           //     // let searchMethod: "set" | "web" | "inventory" = "set";
//           //     // console.log('app bar add click - card search');
//           //     // console.log(action.payload);
//           //     if(filters && filters.length > 0){
//           //         const colorFilters = filters.find(f => f.name === "Colors");
//           //         const formatFilter = filters.find(f => f.name === "Format");
//           //         return {
//           //             ...state,
//           //             cardSearchFilterProps: {
//           //                 ...state.cardSearchFilterProps,
//           //                 colorIdentity: colorFilters ? colorFilters.value : [],
//           //                 format: formatFilter ? formatFilter.value : '',
//           //             }
//           //         }
  
//           //     }
//           //     else return (state);
              
  
//         //   default:
//         //       return(state)
//       }
//   }

  
function defaultSearchFilterProps(): CardFilterProps {
    return {
        // setId: null,
        set: '',
        colorIdentity: [],
        //rarity: ['mythic','rare','uncommon','common'], //
        rarity: [], //
        type: '',//'Creature',
        exclusiveColorFilters: false,
        multiColorOnly: false,
        cardName: '',
        exclusiveName: false,
        maxCount: 0,
        minCount: 0,
        format: '',
        text: '',
        group: '',
    }
}
