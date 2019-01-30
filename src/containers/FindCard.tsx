import React from 'react';
import { connect } from 'react-redux'
import Modal from 'react-modal';
import {
    openFindCardModal,
    closeFindCardModal,
    findModalFilterChange
} from '../actions';

// import Magic from 'mtgsdk-ts';
import Magic, { Cards } from 'mtgsdk-ts';
//import Magic = require("mtgsdk-ts");

import InputField from '../components/InputField'

export interface FindCardProps {
    modalIsOpen: boolean;
    searchFilter: SearchFilterProps;
    // onAfterOpen: () => void;
    // onRequestClose: () => void;
    // onButtonClick: () => void;
    dispatch?: any;
}

class FindCard extends React.Component<FindCardProps> {
    constructor(props: FindCardProps){
        super(props)
        this.openModal = this.openModal.bind(this);
        this.afterOpenModal = this.afterOpenModal.bind(this);
        this.closeModal = this.closeModal.bind(this);

        this.handleFilterChange = this.handleFilterChange.bind(this);
    }

    //whatever those function bindings are
    openModal() {
        this.props.dispatch(openFindCardModal())
    }

    afterOpenModal() {
        
    }

    closeModal() {
        this.props.dispatch(closeFindCardModal())
    }

    handleFilterChange(event: any) {
        // const updatedDeck: CardDeck = {
        //     ...selectedDeck, [event.target.name]: event.target.value
        // }
        const updatedFilter: SearchFilterProps ={
            ...this.props.searchFilter, [event.target.name]: event.target.value
        }
        // console.log('changing filter....')
        this.props.dispatch(findModalFilterChange(updatedFilter))
        //this.props.dispatch()
    }

    render() {
        // console.log('rendering findCards control')
        //Magic.Cards
        return (
            <div>
                <button onClick={ this.openModal }>
                    wut
                </button>
                <Modal
                    isOpen={this.props.modalIsOpen}
                    onAfterOpen={this.afterOpenModal}
                    onRequestClose={this.closeModal}
                    // style={customStyles}
                    contentLabel="Example Modal">
                    <div className="flex-container-vertical">
                        <div className="static-section flex-container-horizontal">
                            <h2>Modal!</h2>
                            <div className="flex-section">
                                <button onClick={this.closeModal}>close</button>
                            </div>
                        </div>
                        <div className="flex-section">
                            <div>I am a modal</div>
                            <InputField label="Active Deck" value={this.props.searchFilter.name} onChange={this.handleFilterChange} property="name" />
                        </div>

                    
                    </div>
                </Modal>
            </div>
        );
    }
}

function mapStateToProps(state: State): FindCardProps {
    // const { selectedSubreddit, postsBySubreddit } = state
    // const { isFetching, lastUpdated, items: posts } = postsBySubreddit[
    //   selectedSubreddit
    // ] || {
    //   isFetching: true,
    //   items: []1
    // }

    const result: FindCardProps = {
        modalIsOpen: state.actions.ui.isFindModalVisible,
        searchFilter: state.actions.searchFilter

        // deckList: state.actions.deckList,
        // selectedDeckId: state.actions.selectedDeckId
    }
    // console.log('props');
    // console.log(state);
    // console.log(result);
    return result;
}

export default connect(mapStateToProps)(FindCard)