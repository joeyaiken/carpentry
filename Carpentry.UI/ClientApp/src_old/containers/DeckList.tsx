//This would be the Deck List CONTAINER
//V1 was a literal list of buttons
//v2 should be a table instead
import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import {
    requestDeckList, 
    // deckListMenuButtonClick, 
    // deckListMenuSelect, 
    requestDeleteDeck,
} from '../actions/deckList.actions'

import {
    menuButtonClicked,
    menuOptionSelected,
} from '../actions/ui.actions'

import { requestDeckDetail } from '../actions/core.actions';

import { AppState } from '../reducers'

import DeckListLayout from '../components/DeckListLayout';
// import { deckList } from '../reducers/deckList.reducer';

interface PropsFromState {
    decks: DeckOverviewDto[];
    deckMenuAnchor: HTMLButtonElement | null;
}

type DeckListProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckList extends React.Component<DeckListProps> {
    constructor(props: DeckListProps) {
        super(props);
        this.handleDeckSelected = this.handleDeckSelected.bind(this);
        this.handleDeckMenuClick = this.handleDeckMenuClick.bind(this);
        this.handleCloseDeckMenuClick = this.handleCloseDeckMenuClick.bind(this);
        this.handleDeckMenuSelect = this.handleDeckMenuSelect.bind(this);
    }

    //IDK if the app should init with data eventually
    //For now, it should load the list (or ensure it's loaded) when the component mounts
    componentDidMount() {
        this.props.dispatch(requestDeckList())
    }

    handleDeckSelected(deckId: number):void {
        this.props.dispatch(requestDeckDetail(deckId));
    }

    handleDeckMenuClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
        this.props.dispatch(menuButtonClicked("deckListMenuAnchor", event.currentTarget));
    }

    handleCloseDeckMenuClick(): void {
        this.props.dispatch(menuButtonClicked("deckListMenuAnchor", null));
    }

    handleDeckMenuSelect(option: string): void {
        // console.log('option');
        // console.log(option)
        const deckAnchor = this.props.deckMenuAnchor;
        if(deckAnchor && deckAnchor.value){
            const deckId: number = parseInt(deckAnchor.value);
            this.props.dispatch(menuOptionSelected("deckListMenuAnchor"));
            // console.log('anchor found')
            // console.log(deckAnchor.name)
            // console.log(deckAnchor.value)

            switch (option){
                case "edit":
                    this.props.dispatch(requestDeckDetail(deckId));
                    break;
                case "delete":
                    const confirmText = `Are you sure you want to delete ${deckAnchor.name}?`;
                    if(window.confirm(confirmText)){
                        // console.log('Prentending to delete deck')
                        this.props.dispatch(requestDeleteDeck(deckId));
                        // this.props.dispatch(requestDeleteDeckCard(parseInt(this.props.cardMenuAnchor.value)));
                    }
                    break;
            }
        }



        // this.props.dispatch(deckListMenuSelect());
        // console.log('ping');
        
    }

    //add deck ?!?
    // onMenuClick: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    // onMenuClose: () => void;
    // onMenuSelect: (option: string) => void;
    render() {
        return (
            <DeckListLayout 
                decks={this.props.decks}
                onDeckClick={this.handleDeckSelected}
                deckMenuAnchor={this.props.deckMenuAnchor}
                onMenuClick={this.handleDeckMenuClick}
                onMenuClose={this.handleCloseDeckMenuClick}
                onMenuSelect={this.handleDeckMenuSelect}
            />
        )
    }

}

function selectDeckList(state: AppState): DeckOverviewDto[] {
    const { deckIds, decksById } = state.data.deckList;
    const result: DeckOverviewDto[] = deckIds.map( id => decksById[id]);
    return result;
}

function mapStateToProps(state: AppState): PropsFromState {

    const result: PropsFromState = {
        decks: selectDeckList(state),
        deckMenuAnchor: state.ui.deckListMenuAnchor,
    }

    return result;
}

export default connect(mapStateToProps)(DeckList)
