import { connect, DispatchProp } from 'react-redux'
import React from 'react'
// import {
//     // requestSaveDeck,
//     // deckEditorCardSelected,
//     // cardMenuButtonClick,
//     // deckCardRequestAlternateVersions,
//     // requestDeleteDeckCard,
//     // deckPropertyChanged,
//     // openDeckPropsModal,
//     // requestCancelDeckModalChanges,
//     // toggleDeckViewMode,
// } from '../actions/deckEditor.actions'

import { AppState } from '../../reducers'

import { Typography, Box } from '@material-ui/core';

import { useLocation } from 'react-router-dom';
import { 
    ensureDeckDetailLoaded,
    openDeckPropsModal, 
    toggleDeckViewMode 
} from '../../actions/deckEditor.actions';
import DeckStatsBar from './DeckStatsBar';
import DeckPropsBar from './DeckPropsBar';
import DeckCardDetail from './DeckCardDetail';
import CardMenu from './CardMenu';
// import DeckPropertiesLayout from '../components/DeckPropertiesLayout';
// import DeckPropsBar from '../components/DeckPropsBar';
// import DeckCardList from '../components/DeckCardList';
// import DeckCardDetail from '../components/DeckCardDetail';
// import AppModal from '../components/AppModal';
// import CardMenu from '../components/CardMenu';
// import DeckCardGrid from '../components/DeckCardGrid';
// import DeckStatsBar from '../components/DeckStatsBar';
// import DeckEditorPropsBar from './DeckEditorPropsBar';
// import DeckEditorCardOverviews from './DeckEditorCardOverviews';
// import DeckEditorCardDetail from './DeckEditorCardDetail';
// import DeckEditorStatsBar from './DeckEditorStatsBar';
// import DeckEditorPropsModal from './DeckEditorPropsModal';

/**
 * The Deck Editor is basically a fancy data table
 */
// function useQuery() {
//     return new URLSearchParams(useLocation().search);
// }

interface OwnProps {
    match: {
        params: {
            deckId: number
        }
    }
    // viewMode: DeckEditorViewMode;//"list" | "grid";
    // deckProperties: DeckDetailDto | null;
    // cardOverviews: InventoryOverviewDto[];
    // cardMenuAnchor: HTMLButtonElement | null;
    // deckPropsModalOpen: boolean;
    // selectedCard: InventoryOverviewDto | null;
    // selectedInventoryCards: InventoryCard[];
    // deckStats: DeckStats | null;
}


interface PropsFromState {
    deckId: number;
    viewMode: DeckEditorViewMode;//"list" | "grid";
    //deckProperties: DeckDetailDto | null;
    deckProperties: DeckProperties | null;
    // cardOverviews: InventoryOverviewDto[];
    // cardMenuAnchor: HTMLButtonElement | null;
    // deckPropsModalOpen: boolean;
    // selectedCard: InventoryOverviewDto | null;
    // selectedInventoryCards: InventoryCard[];
    deckStats: DeckStats | null;
}

type DeckEditorProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditor extends React.Component<DeckEditorProps> {
    constructor(props: DeckEditorProps) {
        super(props);
        
        this.handleEditPropsClick = this.handleEditPropsClick.bind(this);
        this.handleToggleDeckView = this.handleToggleDeckView.bind(this);

    }

    componentDidMount() {
        this.props.dispatch(ensureDeckDetailLoaded(this.props.deckId));
    }

    handleEditPropsClick(): void {
        this.props.dispatch(openDeckPropsModal());
    }

    handleToggleDeckView(): void {
        this.props.dispatch(toggleDeckViewMode());
    }

    render() {
        if(this.props.deckProperties === null){
            return(
                <Box>
                    <Typography>ERROR - Deck properties === null, cannot render</Typography>
                    <Typography>Selected ID == {this.props.deckId}</Typography>
                </Box>
            )
        } else {
            return(
                <React.Fragment>
                    {/* IDK if this should be an AppModal with a container inside of it or not
                        AKA "DeckEditorPropsModal" that contains an AppModal
                            or 
                        "DeckEditorPropsForm" inside of an AppModal, that belongs to this Deck Editor
                    */}
                    {/* <DeckEditorPropsModal /> */}

                    { this.renderPropsModal() }

                    { this.renderPropsBar() }

                    <Box className="flex-row flex-section" style={{
                        overflow:'auto',
                        alignItems:'stretch'
                        }}>
                        {/* Does this need to be flex 1 0 ? */}
    
                        {/* <Box className="flex-row" > */}
                        <div className="flex-section" 
                            style={{
                                overflow:'auto',
                                flex:'1 1 70%'
                            }}
                        >
                            { this.renderCardOverviews() }
                        </div>
                        <div className="flex-section" 
                            style={{
                                overflow:'auto',
                                flex:'1 1 30%'
                            }}
                        >
                            { this.renderCardDetail() }
                        </div>
                    </Box>
                    { this.renderStatsBar() }
                </React.Fragment>
            )
        }
    }

    renderPropsModal() {

    }

    renderPropsBar() {
        return(
            <React.Fragment>
            {   this.props.deckProperties && 
                <DeckPropsBar deckProperties={this.props.deckProperties} onEditClick={this.handleEditPropsClick} onToggleViewClick={this.handleToggleDeckView} />
            }
            </React.Fragment>
        );
    }

    renderCardOverviews() {
        return(
            <React.Fragment>
                {this.props.viewMode === "list" && 
                    <DeckCardList 
                        //cardOverviews={this.props.cardOverviews} 
                        cardOverviews={this.props.groupedCardOverviews[0].cardOverviews} 
                        onCardSelected={this.handleCardSelected} 
                    />}
                {/* {this.props.viewMode === "grid" && <DeckCardGrid cardOverviews={this.props.cardOverviews} onCardSelected={this.handleCardSelected} />} */}
                {this.props.viewMode === "grid" && <DeckCardGrid cardOverviews={this.props.groupedCardOverviews[0].cardOverviews} onCardSelected={this.handleCardSelected} />}
                {this.props.viewMode === "grouped" && <GroupedDeckCardList groupedCardOverviews={this.props.groupedCardOverviews} onCardSelected={this.handleCardSelected} />}
            </React.Fragment>
        );
    }

    renderCardDetail() {
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

    renderStatsBar() {
        return(
            <React.Fragment>
                {this.props.deckStats && <DeckStatsBar deckStats={this.props.deckStats} />}
            </React.Fragment>
        )
    }

}

// function selectDeckOverviews(state: AppState): InventoryOverviewDto[] {
//     const { allCardOverviewNames, cardOverviewsByName } = state.data.deckDetail;
//     return allCardOverviewNames.map(name => cardOverviewsByName[name]);
// }

// function getSelectedCardOverview(state: AppState): InventoryOverviewDto | null {
//     if(state.app.deckEditor.selectedOverviewCardName){
//         return state.data.deckDetail.cardOverviewsByName[state.app.deckEditor.selectedOverviewCardName];
//     }
//     return null;
// }

// function getSelectedDeckDetails(state: AppState): InventoryCard[] {
//     const { selectedInventoryCardIds, cardDetailsById } = state.data.deckDetail;
//     return selectedInventoryCardIds.map(id => cardDetailsById[id]);
//     //selectedInventoryCards: state.deckEditor.selectedInventoryCards,
// }



function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {

    // console.log('DE state to props');
    // console.log(state);
    // console.log(ownProps);
    const result: PropsFromState = {
        deckId: ownProps.match.params.deckId,
        viewMode: state.app.deckEditor.viewMode,
        // cardMenuAnchor: state.ui.deckEditorMenuAnchor,
        // selectedCard: getSelectedCardOverview(state),
        // selectedInventoryCards: getSelectedDeckDetails(state),
        deckProperties: state.data.deckDetail.deckProps,
        // cardOverviews: selectDeckOverviews(state),
        deckStats: state.data.deckDetail.deckStats,
        // deckPropsModalOpen: state.ui.deckPropsModalOpen,
    }

    return result;
}

export default connect(mapStateToProps)(DeckEditor)