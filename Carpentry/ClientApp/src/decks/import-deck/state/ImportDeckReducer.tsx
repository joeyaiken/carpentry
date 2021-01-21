import { IMPORT_DECK_PROPERTY_CHANGED, VALIDATE_IMPORT_REQUESTED, VALIDATE_IMPORT_RECEIVED, SAVE_IMPORT_REQUESTED, SAVE_IMPORT_RECEIVED } from "./ImportDeckActions";

export interface State{
    // deckProps: DeckPropertiesDto;
    
    // ui: {
    //     name: string;
    //     format: null | DeckFormatOption;
    //     notes: string;
    //     importMethod: string;
    //     importString: string;
    // };

    ui: DeckImportUiProps;
    //validation results

    // isSaving: boolean;

    //at some point I might want a single variable, but I have no idea what to name it
    //Different vars == more detailed loading screens I guess



    isValidating: boolean;
    isSaving: boolean;

    importString: string;

    importIsValidated: boolean;
    // importIsValid: boolean;


    validatedImport: ValidatedDeckImportDto;

    //to add - the validated import dto
}

export const importDeckReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case IMPORT_DECK_PROPERTY_CHANGED: return importPropertyChanged(state, action);
        case VALIDATE_IMPORT_REQUESTED: return {...state, isValidating: true }
        case VALIDATE_IMPORT_RECEIVED: 
        return {
            ...state,
            isValidating: false,
            validatedImport: action.payload,
            importIsValidated: true,
        }
        case SAVE_IMPORT_REQUESTED: return {...state, isSaving: true }
        case SAVE_IMPORT_RECEIVED: return {...state, isSaving: false }
        // case NEW_DECK_MODAL_CLOSED: return initialState; //should mimic to ensure modal clears on successful save
        default: return(state);
    }
}

const initialState: State = {
    ui: {
        // id: 0,
        name: "",
        format: null,
        notes: "",
        importMethod: "",
        importString: "",
        // basicW: 0,
        // basicU: 0,
        // basicB: 0,
        // basicR: 0,
        // basicG: 0,
    },

    validatedImport: mockImportPayload(),
    // isSaving: false,

    //isSaving | validating | loading | waiting | importing

    isValidating: false,
    isSaving: false,

    importString: "",

    importIsValidated: false,
    // importIsValid: false,
}

function importPropertyChanged(state: State, action: ReduxAction): State {
    const { name, value } = action.payload;
    return {
        ...state,
        ui: {
            ...state.ui,
            [name]: value,
        }
    }
}

function defaultImportPayload(): ValidatedDeckImportDto {
    return {
        deckProps: {
            id: 0,
            name: "",
            format: null,
            notes: "",
            basicW: 0,
            basicU: 0,
            basicB: 0,
            basicR: 0,
            basicG: 0,
        },
        isValid: false,
        untrackedSets: [],
        validatedCards: [],
    };
}
function mockImportPayload(): ValidatedDeckImportDto {
    return {
        deckProps: {
            id: 0,
            name: "",
            format: null,
            notes: "",
            basicW: 0,
            basicU: 0,
            basicB: 0,
            basicR: 0,
            basicG: 0,
        },
        isValid: false,
        untrackedSets: [
            {setId: 19, setCode: "eld"},
            {setId: 25, setCode: "rna"},
            {setId: 21, setCode: "m20"},
            {setId: 29, setCode: "grn"},
            {setId: 40, setCode: "rix"},
            {setId: 24, setCode: "war"},
            {setId: 32, setCode: "m19"},
        ],
        validatedCards: [],
    };
}