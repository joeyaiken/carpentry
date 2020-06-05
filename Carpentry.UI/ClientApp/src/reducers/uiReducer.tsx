import { FILTER_VALUE_CHANGED
  //  , MENU_BUTTON_CLICKED
//    , MENU_OPTION_SELECTED 
} from '../actions/ui.actions';

// import { 
//     CARD_SEARCH_SEARCH_METHOD_CHANGED, API_DATA_RECEIVED,
// } from '../actions/index.actions';
// import { OPEN_NEW_DECK_MODAL, CANCLE_NEW_DECK, NEW_DECK_FIELD_CHANGE, APP_BAR_ADD_CLICKED } from '../actions/core.actions';
// import { CARD_MENU_BUTTON_CLICK, DECK_CARD_REQUEST_ALTERNATE_VERSIONS, OPEN_DECK_PROPS_MODAL } from '../actions/deckEditor.actions';
// import { inventoryDetail } from './inventoryDetail.reducer';

export interface UiReducerState {
    //filters
    inventoryFilterProps: CardFilterProps;
    cardSearchFilterProps: CardFilterProps;

    //menu anchors
    //Deck list (on main menu)
    //Deck Editor card
    //Inventory detail modal
    //(CS doesn't have one right?)

    //Could/should I use a single menu anchor?
    deckListMenuAnchor: HTMLButtonElement | null;   //in Deck List 
    deckEditorMenuAnchor: HTMLButtonElement | null;       //in Deck Editor (rename?)
    inventoryDetailMenuAnchor: HTMLButtonElement | null;


    //modal for showing inventory detail?

    //misc
    //Should controll this with a single bool
    deckPropsModalOpen: boolean;
    isNewDeckModalOpen: boolean; //Should be removed

    isInventoryDetailModalOpen: boolean;

    //newDeckDto: DeckProperties;
    deckModalProps: DeckProperties | null; //shown in the deckPropsModal

    newDeckDto: DeckProperties;

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

const apiDataReceived = (state: UiReducerState, action: ReduxAction): UiReducerState => {
    const { scope, data } = action.payload;

    // switch(scope as ApiScopeOption){
    //     case "inventoryDetail":

    // }
    if (scope as ApiScopeOption === "inventoryDetail"){
        const newState: UiReducerState = {
            ...state,
            isInventoryDetailModalOpen: Boolean(data),
        };
        return newState;
    }
    else if (scope as ApiScopeOption === "deckDetail"){
        const newState: UiReducerState = {
            ...state,
            deckPropsModalOpen: false,
        };
        return newState;
    } else return (state);

    // if(scope as ApiScopeOption !== "deckDetail") return (state);
    // // if(!data){
    // //     return {
    // //         ...state,
    // //         isInventoryDetailModalOpen: false,
    // //     }
    // // }
    // console.log('closing deck props modal')
    // const newState: UiReducerState = {
    //     ...state,
    //     isInventoryDetailModalOpen: Boolean(data),
    //     deckPropsModalOpen: false,
    // };
    // return newState;
}

const filterValueChanged = (state: UiReducerState, action: ReduxAction): UiReducerState => {
    const { type, filter, value } = action.payload;
    const existingFilter = state[type];
    const newState: UiReducerState = {
        ...state,
        [type]: {
            ...existingFilter, 
            [filter]: value,
        }
    }
    return newState;
}

const menuButtonClicked = (state: UiReducerState, action: ReduxAction): UiReducerState => {
    const { type, anchor } = action.payload;

    // console.log(`menuButtonClicked :${type}`);
    const newState: UiReducerState = {
        ...state,
        //deckListMenuAnchor: action.payload
        [type]: anchor
    }
    return newState;
}

const menuOptionSelected = (state: UiReducerState, action: ReduxAction): UiReducerState => {
    const anchorType = action.payload;
    const newState: UiReducerState = {
        ...state,
        //deckListMenuAnchor: null
        [anchorType]: null
    }
    return newState;
}

const resetCardSearchFilterProps = (state: UiReducerState, action: ReduxAction): UiReducerState => {
    const newState: UiReducerState = {
        ...state,
        cardSearchFilterProps: initialCardSearchFilterProps(),
    }
    return newState;
}

export const uiReducer = (state = initialState, action: ReduxAction): UiReducerState => {
    switch(action.type){
        // case API_DATA_RECEIVED:
        //     return apiDataReceived(state, action);

        case FILTER_VALUE_CHANGED: 
            return filterValueChanged(state, action);
            
        // case MENU_BUTTON_CLICKED:
        //     return menuButtonClicked(state, action);

        // case MENU_OPTION_SELECTED:
        //     return menuOptionSelected(state, action);

        // case CARD_SEARCH_SEARCH_METHOD_CHANGED:
        //     return resetCardSearchFilterProps(state, action);

        // case OPEN_NEW_DECK_MODAL:
        //     return {
        //         ...state,
        //         isNewDeckModalOpen: true,
        //         newDeckDto: emptyDeckDto(),
        //     }

        // case CANCLE_NEW_DECK: 
        //     return {
        //         ...state,
        //         isNewDeckModalOpen: false,
        //     }

        // case NEW_DECK_FIELD_CHANGE:
        //     const modalPropName: string = action.payload.name;
        //     const modalPropValue: string = action.payload.value;
        //     return {
        //         ...state,
        //         newDeckDto: {
        //             ...state.newDeckDto,
        //             [modalPropName]: modalPropValue
        //         }
        //     }
        // case CARD_MENU_BUTTON_CLICK:
        //     return {
        //         ...state,
        //         deckEditorMenuAnchor: action.payload
        //     }

        // case DECK_CARD_REQUEST_ALTERNATE_VERSIONS:
        //     return {
        //         ...state,
        //         deckEditorMenuAnchor: null,
        //     }

        // case OPEN_DECK_PROPS_MODAL:
        //     console.log('opening deck props modal')
        //     return {
        //         ...state,
        //         deckPropsModalOpen: true,
        //     }

        // case APP_BAR_ADD_CLICKED:
        //     //under certain conditions, method should be set to Inventory
        //     //FWIW something seems off with this approach, I can't reference state, I need to rely on a payload

        //     const filters: FilterDescriptor[] | undefined = action.payload;
        //     // let searchMethod: "set" | "web" | "inventory" = "set";
        //     // console.log('app bar add click - card search');
        //     // console.log(action.payload);
        //     if(filters && filters.length > 0){
        //         const colorFilters = filters.find(f => f.name === "Colors");
        //         const formatFilter = filters.find(f => f.name === "Format");
        //         return {
        //             ...state,
        //             cardSearchFilterProps: {
        //                 ...state.cardSearchFilterProps,
        //                 colorIdentity: colorFilters ? colorFilters.value : [],
        //                 format: formatFilter ? formatFilter.value : '',
        //             }
        //         }

        //     }
        //     else return (state);
            

        default:
            return(state)
    }
}

const emptyDeckDto = (): DeckProperties   =>  ({
    basicW: 0,
    basicU: 0,
    basicB: 0,
    basicR: 0,
    basicG: 0,
    id: 0,
    format: "Modern",
    name: "",
    notes: ""
});

function initialCardSearchFilterProps(): CardFilterProps {
    return {
        // setId: null,
        set: '',
        colorIdentity: [],
        rarity: [], //['mythic','rare','uncommon','common'], //
        type: '',
        exclusiveColorFilters: false,
        multiColorOnly: false,
        cardName: '',
        exclusiveName: false,
        maxCount: 0,
        minCount: 0,
        format: '',
        text: '',
    } as CardFilterProps;
} 

function mockFilterProps(): CardFilterProps {
    return{
        // setId: null,
        set: '',
        colorIdentity: ['R'],
        rarity: [],//['uncommon','common'], //
        type: '',//'Creature',
        exclusiveColorFilters: false,
        multiColorOnly: false,
        cardName: '',
        exclusiveName: false,
        maxCount: 0,
        minCount: 0,
        format: '',
        text: '',
    } as CardFilterProps;
}

const initialState: UiReducerState = {
    inventoryFilterProps: defaultSearchFilterProps(),
    cardSearchFilterProps: mockFilterProps(),
    deckListMenuAnchor: null,
    deckEditorMenuAnchor: null,
    inventoryDetailMenuAnchor: null,
    
    newDeckDto: emptyDeckDto(),

    deckPropsModalOpen: false,
    isNewDeckModalOpen: false,
    isInventoryDetailModalOpen: false,

    deckModalProps: null,



    // inventoryFilterProps: defaultSearchFilterProps(),
    // cardSearchFilterProps: mockFilterProps(),

    // deckListMenuAnchor: null,
    // cardMenuAnchor: null,

    // deckPropsModalOpen: false,
    // isNewDeckModalOpen: false,
}

function defaultSearchFilterProps(): CardFilterProps {
    return {
        // setId: null,
        set: '',
        colorIdentity: [],
        //rarity: ['mythic','rare','uncommon','common'], //
        rarity: [], //
        type: '',
        exclusiveColorFilters: false,
        multiColorOnly: false,
        cardName: '',
        exclusiveName: false,
        maxCount: 0,
        minCount: 0,
        format: '',
        text: '',
    }
}