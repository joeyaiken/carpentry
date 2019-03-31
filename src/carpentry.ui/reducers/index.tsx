import { combineReducers } from 'redux';
//core
import { data } from './data'
import { ui } from './ui'
//components
import { deckEditor } from './deckEditor'
import { cardSearch } from './cardSearch'

export default combineReducers({
    //core
    data,
    ui,
    //components
    deckEditor,
    cardSearch
});