import { combineReducers } from 'redux';

import { ui } from './ui'
import { deckEditor } from './deckEditor'
import { actions } from './actions'

export default combineReducers({
    actions,
    ui,
    deckEditor
});
