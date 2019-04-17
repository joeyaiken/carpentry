import { combineReducers } from 'redux';
//core
import { data } from './data'
import { ui } from './ui'
//components
import { cardScoutCardSearch } from './cardScoutCardSearch'
import { deckEditor } from './deckEditor'
import { cardSearch } from './cardSearch'
import { cardInventory } from './cardInventory.reducer';
import { addPack } from './addPack.reducer';
export default combineReducers({
    //core
    data,
    ui,
    //components
    addPack,
    cardScoutCardSearch,
    deckEditor,
    cardSearch,
    cardInventory
});