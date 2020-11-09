import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import {
    requestSaveDeck,
    deckPropertyChanged,
    requestCancelDeckModalChanges,
} from '../actions/deckEditor.actions'

import { AppState } from '../reducers'

import { Typography, Box } from '@material-ui/core';
import DeckPropertiesLayout from '../components/DeckPropertiesLayout';
import AppModal from '../components/AppModal';

/**
 * The Deck Editor is basically a fancy data table
 */

interface PropsFromState {
    deckProperties: DeckProperties | null;
    deckPropsModalOpen: boolean;
    formatFilterOptions: FilterOption[];
}

type DeckEditorProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditor extends React.Component<DeckEditorProps> {
    constructor(props: DeckEditorProps) {
        super(props);
        this.handleModalPropsChanged = this.handleModalPropsChanged.bind(this);
        this.handleCloseModalClick = this.handleCloseModalClick.bind(this);
        this.handleSavePropsClick = this.handleSavePropsClick.bind(this);
    }

    handleModalPropsChanged(event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>): void {
        // console.log('prop change')
        // console.log(event.target.name)
        // console.log(event.target.value)
        this.props.dispatch(deckPropertyChanged(event.target.name, event.target.value));
    }

    handleCloseModalClick(): void {
        console.log('close Modal click');
        this.props.dispatch(requestCancelDeckModalChanges());
    }

    handleSavePropsClick(): void {
        if(this.props.deckProperties) {
            this.props.dispatch(requestSaveDeck(this.props.deckProperties));
        }
    }

    render() {
        if(this.props.deckProperties === null){
            return(<Box><Typography>ERROR - Deck properties === null, cannot render</Typography></Box>)
        } else {
            return(
                <React.Fragment>

                    <AppModal title="Add New Deck" 
                        //isOpen={Boolean(this.props.deckPropsModalOpen)} 
                        isOpen={this.props.deckPropsModalOpen} 
                        onCloseClick={this.handleCloseModalClick} 
                        // onDeleteClick={this.handle}
                        onSaveClick={this.handleSavePropsClick}>    
                        <DeckPropertiesLayout 
                            onChange={(a) => this.handleModalPropsChanged(a)} 
                            deck={this.props.deckProperties}
                            formatFilters={this.props.formatFilterOptions} />
                    </AppModal>

                    
                </React.Fragment>
            )
        }
    }
}

function mapStateToProps(state: AppState): PropsFromState {

    const result: PropsFromState = {
        deckProperties: state.data.deckDetail.deckProps,
        deckPropsModalOpen: state.ui.deckPropsModalOpen,
        formatFilterOptions: state.data.appFilterOptions.filterOptions.formats,
    }

    return result;
}

export default connect(mapStateToProps)(DeckEditor)