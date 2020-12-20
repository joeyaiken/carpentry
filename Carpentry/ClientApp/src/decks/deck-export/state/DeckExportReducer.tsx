import { 
    DECK_EXPORT_RECEIVED,
    DECK_EXPORT_REQUESTED,
    EXPORT_TYPE_CHANGED,
    OPEN_EXPORT_DIALOG,
    CLOSE_DECK_DIALOG,
} from './DeckExportActions';

export interface State {
    isLoading: boolean;
    isDialogOpen: boolean;
    selectedExportType: DeckExportType;
    deckExportPayload: string;
}

export const deckExportReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case DECK_EXPORT_REQUESTED: return { ...state, isLoading: true, deckExportPayload: '' };
        case DECK_EXPORT_RECEIVED: return { ...state, isLoading: false, deckExportPayload: action.payload };
        case EXPORT_TYPE_CHANGED: return { ...state, selectedExportType: action.payload };
        case OPEN_EXPORT_DIALOG: return { ...state, isDialogOpen: true };
        case CLOSE_DECK_DIALOG: return { ...state, isDialogOpen: false, deckExportPayload: '' };
        default: return(state);
    }
}

const initialState: State = {
    isLoading: false,
    isDialogOpen: false,
    selectedExportType: 'list',
    deckExportPayload: '',
}