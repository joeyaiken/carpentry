import { combineReducers } from 'redux';
import { inventoryDataReducer } from './inventoryDataReducer';

const rootReducer = combineReducers({
    data: inventoryDataReducer
});

export default rootReducer;
export type AppState = ReturnType<typeof rootReducer>;