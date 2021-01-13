import { 

} from "./ImportDeckActions";

export interface State{
    // deckProps: DeckPropertiesDto;
    // isSaving: boolean;
    importString: string;

    importIsValidated: boolean;
    importIsValid: boolean;
}

export const importDeckReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        // case NEW_DECK_PROPERTY_CHANGED: return deckPropertyChanged(state, action);
        // case NEW_DECK_MODAL_CLOSED: return initialState;
        // case NEW_DECK_SAVE_REQUESTED: return {...state, isSaving: true }
        // case NEW_DECK_SAVE_COMPLETE: return initialState;
        default: return(state);
    }
}

const initialState: State = {
    // deckProps: {
    //     id: 0,
    //     name: "",
    //     format: null,
    //     notes: "",
    //     basicW: 0,
    //     basicU: 0,
    //     basicB: 0,
    //     basicR: 0,
    //     basicG: 0,
    // },
    // isSaving: false,

    //isSaving | validating | loading | waiting | importing
    importString: "",

    importIsValidated: false,
    importIsValid: false,
}

// function deckPropertyChanged(state: State, action: ReduxAction): State {
//     const { name, value } = action.payload;
//     return {
//         ...state,
//         deckProps: {
//             ...state.deckProps,
//             [name]: value,
//         }
//     }
// }