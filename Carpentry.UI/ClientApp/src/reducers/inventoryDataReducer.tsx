


export interface InventoryDataReducerState {

}


export const inventoryDataReducer = (state = initialState, action: ReduxAction): InventoryDataReducerState => {
    switch(action.type){

        // case API_DATA_REQUESTED:
        //     return apiDataRequested(state, action);

        // case API_DATA_RECEIVED:
        //     return apiDataReceived(state, action);

        default:
            return(state)
    }
}

const initialState: InventoryDataReducerState = {
    // byName: {},
    // allNames: [],
    
}
