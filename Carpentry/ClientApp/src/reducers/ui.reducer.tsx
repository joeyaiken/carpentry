import { FILTER_VALUE_CHANGED, MENU_BUTTON_CLICKED, MENU_OPTION_SELECTED } from '../actions/ui.actions';

import { 
    CARD_SEARCH_SEARCH_METHOD_CHANGED,
} from '../actions/index.actions';
import { OPEN_NEW_DECK_MODAL, CANCLE_NEW_DECK, NEW_DECK_FIELD_CHANGE } from '../actions/core.actions';
import { CARD_MENU_BUTTON_CLICK, DECK_CARD_REQUEST_ALTERNATE_VERSIONS, OPEN_DECK_PROPS_MODAL } from '../actions/deckEditor.actions';

declare interface uiState {
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

const filterValueChanged = (state: uiState, action: ReduxAction): uiState => {
    const { type, filter, value } = action.payload;
    const existingFilter = state[type];
    const newState: uiState = {
        ...state,
        [type]: {
            ...existingFilter, 
            [filter]: value,
        }
    }
    return newState;
}

const menuButtonClicked = (state: uiState, action: ReduxAction): uiState => {
    const { type, anchor } = action.payload;

    // console.log(`menuButtonClicked :${type}`);
    const newState: uiState = {
        ...state,
        //deckListMenuAnchor: action.payload
        [type]: anchor
    }
    return newState;
}

const menuOptionSelected = (state: uiState, action: ReduxAction): uiState => {
    const anchorType = action.payload;
    const newState: uiState = {
        ...state,
        //deckListMenuAnchor: null
        [anchorType]: null
    }
    return newState;
}

const resetCardSearchFilterProps = (state: uiState, action: ReduxAction): uiState => {
    const newState: uiState = {
        ...state,
        cardSearchFilterProps: initialCardSearchFilterProps(),
    }
    return newState;
}

export const ui = (state = initialState, action: ReduxAction): uiState => {
    switch(action.type){
        case FILTER_VALUE_CHANGED: 
            return filterValueChanged(state, action);
            
        case MENU_BUTTON_CLICKED:
            return menuButtonClicked(state, action);

        case MENU_OPTION_SELECTED:
            return menuOptionSelected(state, action);

        case CARD_SEARCH_SEARCH_METHOD_CHANGED:
            return resetCardSearchFilterProps(state, action);

        case OPEN_NEW_DECK_MODAL:
            return {
                ...state,
                isNewDeckModalOpen: true,
                newDeckDto: emptyDeckDto(),
            }

        case CANCLE_NEW_DECK: 
            return {
                ...state,
                isNewDeckModalOpen: false,
            }

        case NEW_DECK_FIELD_CHANGE:
            const modalPropName: string = action.payload.name;
            const modalPropValue: string = action.payload.value;
            return {
                ...state,
                newDeckDto: {
                    ...state.newDeckDto,
                    [modalPropName]: modalPropValue
                }
            }
        case CARD_MENU_BUTTON_CLICK:
            return {
                ...state,
                deckEditorMenuAnchor: action.payload
            }

        case DECK_CARD_REQUEST_ALTERNATE_VERSIONS:
            return {
                ...state,
                deckEditorMenuAnchor: null,
            }

        case OPEN_DECK_PROPS_MODAL:
            return {
                ...state,
                deckPropsModalOpen: true,
            }

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
        set: '',
        colorIdentity: [],
        rarity: ['mythic','rare','uncommon','common'], //
        type: '',
        exclusiveColorFilters: true,
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
        set: 'eld',
        colorIdentity: ['R'],
        rarity: ['uncommon','common'], //
        type: 'Creature',
        exclusiveColorFilters: true,
        multiColorOnly: false,
        cardName: '',
        exclusiveName: false,
        maxCount: 0,
        minCount: 0,
        format: '',
        text: '',
    } as CardFilterProps;
}

const initialState: uiState = {
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
        set: 'thb',
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