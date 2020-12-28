import { 
    TAG_DETAIL_REQUESTED,
    TAG_DETAIL_RECEIVED,
    NEW_TAG_CHANGE,

    ADD_TAG_REQUESTED,
    ADD_TAG_RECEIVED,
    REMOVE_TAG_REQUESTED,
    REMOVE_TAG_RECEIVED,
} from "./CardTagsActions";

export interface State {
    isLoading: boolean;

    cardId: number;
    cardName: string;
    existingTags: CardTagDetailTag[];
    tagSuggestions: string[];

    newTagName: string;
}

export const cardTagsReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        case TAG_DETAIL_REQUESTED: return tagDetailRequested(state, action);
        case TAG_DETAIL_RECEIVED: return tagDetailReceived(state, action);
        case NEW_TAG_CHANGE: return { ...state, newTagName: action.payload };
        case ADD_TAG_REQUESTED: return { ...state, isLoading: true };
        case ADD_TAG_RECEIVED: return { ...state, isLoading: false };
        case REMOVE_TAG_REQUESTED: return { ...state, isLoading: true };
        case REMOVE_TAG_RECEIVED: return { ...state, isLoading: false };
        default: return(state);
    }
}

const initialState: State = {
    isLoading: false,
    
    cardId: 0,
    cardName: '',
    existingTags: [],
    tagSuggestions: [],

    newTagName: '',
}

function tagDetailRequested(state: State, action: ReduxAction): State {
    const newState: State = {
        ...initialState,
        cardId: state.cardId,
        isLoading: true,
        
    };
    return newState;
}

function tagDetailReceived(state: State, action: ReduxAction): State {
    const detailResult: CardTagDetailDto = action.payload;

    const newState: State = {
        ...state,
        isLoading: false,
        
        cardId: detailResult.cardId,
        cardName: detailResult.cardName,
        existingTags: detailResult.existingTags,
        tagSuggestions: detailResult.tagSuggestions,

        newTagName: '',
    };
    
    return newState;
}