// import { ADD_MENU_BUTTON_CLICKED } from "./HomeActions";

export interface State {
    // cardMenuAnchor: HTMLButtonElement | null;
}

export const homeReducer = (state = initialState, action: ReduxAction): State => {
    switch(action.type){
        // case ADD_MENU_BUTTON_CLICKED: return { ...state, cardMenuAnchor: action.payload }
        default: return(state);
    }
}

const initialState: State = {
    // cardMenuAnchor: null,
}