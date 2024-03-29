import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { deckCardRequestAlternateVersions, cardMenuButtonClick, requestDeleteDeckCard, requestUpdateDeckCardStatus } from '../actions/deckEditor.actions'

import { AppState } from '../reducers'

// import { Typography, Box } from '@material-ui/core';
// import DeckPropertiesLayout from '../components/DeckPropertiesLayout';
// import DeckPropsBar from '../components/DeckPropsBar';
// import DeckCardList from '../components/DeckCardList';
import DeckCardDetail from '../components/DeckCardDetail';
import CardMenu from '../components/CardMenu';
import { menuButtonClicked } from '../actions/ui.actions';
// import AppModal from '../components/AppModal';
// import CardMenu from '../components/CardMenu';
// import DeckCardGrid from '../components/DeckCardGrid';
// import DeckStatsBar from '../components/DeckStatsBar';

/**
 * The Deck Editor is basically a fancy data table
 */

interface PropsFromState {
    // viewMode: DeckEditorViewMode;//"list" | "grid";
    // deckProperties: DeckProperties | null;
    // cardOverviews: InventoryOverviewDto[];
    cardMenuAnchor: HTMLButtonElement | null;
    // deckPropsModalOpen: boolean;
    selectedCard: DeckCardOverview | null;
    selectedInventoryCards: DeckCard[];
    // deckStats: DeckStats | null;
}

type DeckEditorCardDetailProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditorCardDetail extends React.Component<DeckEditorCardDetailProps> {
    constructor(props: DeckEditorCardDetailProps) {
        super(props);
        this.handleCardMenuClick = this.handleCardMenuClick.bind(this);
        this.handleCardMenuSelected = this.handleCardMenuSelected.bind(this);
        this.handleCardMenuClosed = this.handleCardMenuClosed.bind(this);
    }
    
    handleCardMenuClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
        this.props.dispatch(menuButtonClicked("deckEditorMenuAnchor", event.currentTarget));
    }

    handleCardMenuSelected(name: DeckEditorCardMenuOption){
        console.log('card anchor');
        console.log(this.props.cardMenuAnchor);
        switch (name){
            case "search":
                if(this.props.cardMenuAnchor != null){
                    this.props.dispatch(deckCardRequestAlternateVersions(this.props.cardMenuAnchor.name))
                }
                break;
            case "delete":
                    if(this.props.cardMenuAnchor != null){
                        const confirmText = `Are you sure you want to delete ${this.props.cardMenuAnchor.name}?`;
                        if(window.confirm(confirmText)){
                            this.props.dispatch(requestDeleteDeckCard(parseInt(this.props.cardMenuAnchor.value)));
                        }
                    }
                    break;
            case "sideboard":
                if(this.props.cardMenuAnchor != null){
                    this.props.dispatch(requestUpdateDeckCardStatus(parseInt(this.props.cardMenuAnchor.value), "sideboard"));
                }
                break;
            case "mainboard":
                if(this.props.cardMenuAnchor != null){
                    this.props.dispatch(requestUpdateDeckCardStatus(parseInt(this.props.cardMenuAnchor.value), "mainboard"));
                }
                break;
            case "commander":
                if(this.props.cardMenuAnchor != null){
                    this.props.dispatch(requestUpdateDeckCardStatus(parseInt(this.props.cardMenuAnchor.value), "commander"));
                }
                break;
        }
    }

    handleCardMenuClosed(): void {
        // this.props.dispatch(cardMenuButtonClick(null));
        this.props.dispatch(menuButtonClicked("deckEditorMenuAnchor", null));
    }
    render() {
        return(
            <React.Fragment>
                <CardMenu cardMenuAnchor={this.props.cardMenuAnchor} onCardMenuSelect={this.handleCardMenuSelected} onCardMenuClose={this.handleCardMenuClosed} />
                <DeckCardDetail 
                    selectedCard={this.props.selectedCard} 
                    inventoryCards={this.props.selectedInventoryCards} 
                    onMenuClick={this.handleCardMenuClick}
                    onMenuClose={this.handleCardMenuClosed}
                    // onMenuSelect={this.handleCardMenuSelected}
                    />
            </React.Fragment>
        )
    }
}

// function selectDeckOverviews(state: AppState): InventoryOverviewDto[] {
//     const { allCardOverviewNames, cardOverviewsByName } = state.data.deckDetail;
//     return allCardOverviewNames.map(name => cardOverviewsByName[name]);
// }

function getSelectedCardOverview(state: AppState): DeckCardOverview | null {
    if(state.app.deckEditor.selectedOverviewCardId){
        return state.data.deckDetail.cardOverviewsById[state.app.deckEditor.selectedOverviewCardId];
    }
    return null;
}

function getSelectedDeckDetails(state: AppState): DeckCard[] {
    const { selectedInventoryCardIds, cardDetailsById } = state.data.deckDetail;
    return selectedInventoryCardIds.map(id => cardDetailsById[id]);
    //selectedInventoryCards: state.deckEditor.selectedInventoryCards,
}

function mapStateToProps(state: AppState): PropsFromState {

    const result: PropsFromState = {
        // viewMode: state.app.deckEditor.viewMode,
        cardMenuAnchor: state.ui.deckEditorMenuAnchor,
        selectedCard: getSelectedCardOverview(state),
        selectedInventoryCards: getSelectedDeckDetails(state),
        // deckProperties: state.data.deckDetail.deckProps,
        // cardOverviews: selectDeckOverviews(state),
        // deckStats: state.data.deckDetail.deckStats,
        // deckPropsModalOpen: state.ui.deckPropsModalOpen,
    }

    return result;
}

export default connect(mapStateToProps)(DeckEditorCardDetail)