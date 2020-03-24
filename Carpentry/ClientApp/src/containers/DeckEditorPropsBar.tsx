import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { openDeckPropsModal, toggleDeckViewMode } from '../actions/deckEditor.actions'
import { AppState } from '../reducers'
import { Typography, Box } from '@material-ui/core';
import DeckPropsBar from '../components/DeckPropsBar';

interface PropsFromState {
    deckProperties: DeckProperties | null;
}

type DeckEditorPropsBarProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditorPropsBar extends React.Component<DeckEditorPropsBarProps> {
    constructor(props: DeckEditorPropsBarProps) {
        super(props);
        this.handleEditPropsClick = this.handleEditPropsClick.bind(this);
        this.handleToggleDeckView = this.handleToggleDeckView.bind(this);
    }

    handleEditPropsClick(): void {
        this.props.dispatch(openDeckPropsModal());
    }

    handleToggleDeckView(): void {
        this.props.dispatch(toggleDeckViewMode());
    }

    render() {
        if(this.props.deckProperties == null){
            return(<Box><Typography>ERROR - Deck properties == null, cannot render</Typography></Box>)
        } else {
            return(
                <React.Fragment>
                    <DeckPropsBar deckProperties={this.props.deckProperties} onEditClick={this.handleEditPropsClick} onToggleViewClick={this.handleToggleDeckView} />
                </React.Fragment>
            )
        }
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
        deckProperties: state.data.deckDetail.deckProps,
    }
    return result;
}

export default connect(mapStateToProps)(DeckEditorPropsBar)