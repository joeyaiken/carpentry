import { combineReducers } from 'redux';
//core
import { data } from './data'
import { ui } from './ui'
//components
import { architectCardSearch } from './architectCardSearch'
import { deckEditor } from './deckEditor'
import { cardSearch } from './cardSearch'

export default combineReducers({
    //core
    data,
    ui,
    //components
    architectCardSearch,
    deckEditor,
    cardSearch
});