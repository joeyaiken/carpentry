


//property changed
export const ADD_DECK_PROP_CHANGED = 'ADD_DECK_PROP_CHANGED';
export const addDeckPropChanged = (name: string, value: string): ReduxAction  => ({
    type: ADD_DECK_PROP_CHANGED,
    payload: {
        name: name,
        value: value
    }
});


