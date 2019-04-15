import { connect, MapStateToProps, DispatchProp } from 'react-redux'
import React, { Component, SyntheticEvent } from 'react'

import {
    csActionApplied,
    csCardSelected,
    csFilterChanged,
    csInitialized,
    csSearchApplied
} from '../actions/cardSearch.actions'

interface PropsFromState {

}

type CardSearchProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSearch extends React.Component<CardSearchProps> {
    constructor(props: CardSearchProps) {
        super(props)
        this.handleFilterChanged = this.handleFilterChanged.bind(this);
        this.handleSearchApplied = this.handleSearchApplied.bind(this);
        this.handleCardSelected = this.handleCardSelected.bind(this);
        this.handleActionApplied = this.handleActionApplied.bind(this);
    }

    componentDidMount(): void {
        this.props.dispatch(csInitialized())
    }

    handleFilterChanged(): void {
        this.props.dispatch(csFilterChanged())
    }

    handleSearchApplied(): void {
        this.props.dispatch(csSearchApplied())
    }

    handleCardSelected(): void {
        this.props.dispatch(csCardSelected())
    }

    handleActionApplied(): void {
        this.props.dispatch(csActionApplied())
    }

    render(){
        
        return(
            <div className="architect-container">
                <div className="architect-card-section">
                    <h1>Hallo, wurld</h1>
                    <div className="notes">
                        <p>This component will allow the user to browse individual sets, and propose cards for decks</p>
                        <p>Q: Should the "Set" filter be optional?</p>
                        <p>It shouldn't be able to directly add cards to decks</p>
                        <p>If a user wants to add specific cards to specific deck s, the QuickAdd component from the deck editor is more appropriate</p>
                        <p>Maybe provide an expanded & collapsed version of the deck list</p>
                        <p>Compact could have an summary, card count, and a + icon (when a card is selected)</p>
                        <p>It shouldn't show that pesky QuickAdd component</p>
                        <p>This component isn't going to rely on some local index, it's going to always query the MTG API</p>
                        <p>Q: Should the queries continue to log to an index to keep building out a local library?</p>
                        <p>Does this get its own reducer?</p>
                        <p></p>
                    </div>
                </div>
                <div className="architect-deck-section">
                    <p>This should list decks</p>
                </div>
            </div>
        )
    }

}

function mapStateToProps(state: State): PropsFromState {

    const result: PropsFromState = {
    }
    return result;
}

export default connect(mapStateToProps)(CardSearch)