import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { Typography, Box } from '@material-ui/core';
import { toggleDeckViewMode, deckEditorCardSelected, openDeckPropsModal, closeDeckPropsModal, requestSavePropsModal, deckPropsModalChanged, requestDisassembleDeck, requestDeleteDeck } from './state/DeckEditorActions';

import { AppState } from '../../configureStore';
import { ensureDeckDetailLoaded } from '../state/decksDataActions';
import { DeckEditorLayout } from './components/DeckEditorLayout';
import { disconnect } from 'process';
import { push } from 'react-router-redux';

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
}


interface PropsFromState {
    deckId: number;
    viewMode: DeckEditorViewMode;//"list" | "grid";
    //deckProperties: DeckDetailDto | null;
    deckProperties: DeckPropertiesDto | null;

    formatFilterOptions: FilterOption[];

    // cardOverviews: InventoryOverviewDto[];
    // cardMenuAnchor: HTMLButtonElement | null;
    // deckPropsModalOpen: boolean;
    // selectedCard: InventoryOverviewDto | null;
    // selectedInventoryCards: InventoryCard[];
    deckStats: DeckStats | null;


    isPropsDialogOpen: boolean;
    deckDialogProperties: DeckPropertiesDto | null;


    //Overview
    groupedCardOverviews: CardOverviewGroup[];
    //Non-grouped views will just snag cards from item at position 0


    //Detail
    cardMenuAnchor: HTMLButtonElement | null;
    selectedCard: DeckCardOverview | null;
    selectedInventoryCards: DeckCard[];
}

type DeckEditorProps = PropsFromState & DispatchProp<ReduxAction>;

class DeckEditor extends React.Component<DeckEditorProps> {
    constructor(props: DeckEditorProps) {
        super(props);
        this.handleCardSelected = this.handleCardSelected.bind(this);
        this.handlePropsModalOpen = this.handlePropsModalOpen.bind(this);
        this.handlePropsModalClose = this.handlePropsModalClose.bind(this);
        this.handleModalPropsChanged = this.handleModalPropsChanged.bind(this);
        this.handlePropsModalSave = this.handlePropsModalSave.bind(this);
        this.handleToggleDeckView = this.handleToggleDeckView.bind(this);
        this.handleCardMenuClick = this.handleCardMenuClick.bind(this);
        this.handleCardMenuSelected = this.handleCardMenuSelected.bind(this);
        this.handleCardMenuClosed = this.handleCardMenuClosed.bind(this);
        this.handlePropsModalDisassemble = this.handlePropsModalDisassemble.bind(this);
        this.handlePropsModalDelete = this.handlePropsModalDelete.bind(this);
        this.handleAddCardsClicked = this.handleAddCardsClicked.bind(this);
    }

    componentDidMount() {
        this.props.dispatch(ensureDeckDetailLoaded(this.props.deckId));
    }

    handleCardSelected(cardOverview: DeckCardOverview) {
        this.props.dispatch(deckEditorCardSelected(cardOverview))
    }

    handleToggleDeckView(): void {
        this.props.dispatch(toggleDeckViewMode());
    }

    handleCardMenuClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
        // this.props.dispatch(menuButtonClicked("deckEditorMenuAnchor", event.currentTarget));
    }

    handleCardMenuSelected(name: DeckEditorCardMenuOption){
        console.log('card anchor');
        console.log(this.props.cardMenuAnchor);
        switch (name){
            case "search":
                if(this.props.cardMenuAnchor != null){
                    // this.props.dispatch(deckCardRequestAlternateVersions(this.props.cardMenuAnchor.name))
                }
                break;
            case "delete":
                    if(this.props.cardMenuAnchor != null){
                        const confirmText = `Are you sure you want to delete ${this.props.cardMenuAnchor.name}?`;
                        if(window.confirm(confirmText)){
                            // this.props.dispatch(requestDeleteDeckCard(parseInt(this.props.cardMenuAnchor.value)));
                        }
                    }
                    break;
            case "sideboard":
                if(this.props.cardMenuAnchor != null){
                    // this.props.dispatch(requestUpdateDeckCardStatus(parseInt(this.props.cardMenuAnchor.value), "sideboard"));
                }
                break;
            case "mainboard":
                if(this.props.cardMenuAnchor != null){
                    // this.props.dispatch(requestUpdateDeckCardStatus(parseInt(this.props.cardMenuAnchor.value), "mainboard"));
                }
                break;
            case "commander":
                if(this.props.cardMenuAnchor != null){
                    // this.props.dispatch(requestUpdateDeckCardStatus(parseInt(this.props.cardMenuAnchor.value), "commander"));
                }
                break;
        }
    }

    handleCardMenuClosed(): void {
        // this.props.dispatch(cardMenuButtonClick(null));
        // this.props.dispatch(menuButtonClicked("deckEditorMenuAnchor", null));
    }

    //props modal
    //handleEditPropsClick(): void {
    handlePropsModalOpen(): void {
        this.props.dispatch(openDeckPropsModal(this.props.deckProperties));
    }
    
    handlePropsModalClose(): void {
        this.props.dispatch(closeDeckPropsModal());
    }

    handlePropsModalSave(): void {
        this.props.dispatch(requestSavePropsModal());
    }

    handleModalPropsChanged(name: string, value: string | number): void {
        this.props.dispatch(deckPropsModalChanged(name, value));
    }

    handlePropsModalDisassemble(): void {
        //TODO - replace this alert with something classier
        const confirmText = `Are you sure you want to disassemble this deck? This will return all cards to the inventory, but keep the deck definition.`;
        if(window.confirm(confirmText)){
            this.props.dispatch(requestDisassembleDeck());
        }
    }
    
    handlePropsModalDelete(): void {
        //TODO - replace this alert with something classier
        const confirmText = `Are you sure you want to delete this deck?  This cannot be undone.`;
        if(window.confirm(confirmText)){
            this.props.dispatch(requestDeleteDeck());
        }
    }

    handleAddCardsClicked(): void {
        this.props.dispatch(push(`/decks/${this.props.deckId}/addCards`))
    }
    
    render() {
        if(this.props.deckProperties === null || this.props.deckStats === null){
            return(
                <Box>
                    <Typography>ERROR - Deck properties === null, cannot render</Typography>
                    <Typography>Selected ID == {this.props.deckId}</Typography>
                </Box>
            )
        } else {
            return(
                <DeckEditorLayout 
                    //props & modal
                    deckProperties={this.props.deckProperties}
                    dialogDeckProperties={this.props.deckDialogProperties}
                    isPropsDialogOpen={this.props.isPropsDialogOpen}
                    onPropsModalOpen={this.handlePropsModalOpen}
                    onPropsModalClose={this.handlePropsModalClose}
                    onModalPropsChange={this.handleModalPropsChanged}
                    onPropsModalSave={this.handlePropsModalSave}
                    formatFilterOptions={this.props.formatFilterOptions}
                    onPropsModalDisassembleClick={this.handlePropsModalDisassemble}
                    onPropsModalDeleteClick={this.handlePropsModalDelete}

                    onAddCardsClick={this.handleAddCardsClicked}

                    //View
                    handleToggleDeckView={this.handleToggleDeckView}
                    viewMode={this.props.viewMode}
                
                    //overview
                    groupedCardOverviews={this.props.groupedCardOverviews}
                    onCardSelected={this.handleCardSelected} 
                    
                    //detail
                    cardMenuAnchor={this.props.cardMenuAnchor}
                    selectedCard={this.props.selectedCard} 
                    selectedInventoryCards={this.props.selectedInventoryCards} 
                    onCardMenuSelected={this.handleCardMenuSelected}
                    onCardMenuClick={this.handleCardMenuClick}
                    onCardMenuClosed={this.handleCardMenuClosed}
                
                    //stats
                    deckStats={this.props.deckStats} />
            )
        }
    }
}

function selectDeckOverviews(state: AppState): CardOverviewGroup[] {
    const { allCardOverviewIds, cardOverviewsById, cardGroups } = state.decks.data.detail; //state.data.deckDetail;

    if(state.decks.deckEditor.viewMode === "grouped"){
        const result = cardGroups.map(group => {
            const groupResult: CardOverviewGroup = {
                name: group.name,
                cardOverviews: group.cardOverviewIds.map(id => cardOverviewsById[id]),
            }
            return groupResult;
        });
        return result;

    } else {

        return [{
            name: "All",
            cardOverviews: allCardOverviewIds.map(id => cardOverviewsById[id]),
        }];

    }
}

function getSelectedCardOverview(state: AppState): DeckCardOverview | null {
    const selectedOverviewCardId = state.decks.deckEditor.selectedOverviewCardId;
    if(selectedOverviewCardId){
        return state.decks.data.detail.cardOverviewsById[selectedOverviewCardId];
    }
    return null;
}

function getSelectedDeckDetails(state: AppState): DeckCard[] {
    const { selectedInventoryCardIds, cardDetailsById } = state.decks.data.detail;
    return selectedInventoryCardIds.map(id => cardDetailsById[id]);
    //selectedInventoryCards: state.deckEditor.selectedInventoryCards,
}

function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {

    // console.log('DE state to props');
    // console.log(state);
    // console.log(ownProps);
    const result: PropsFromState = {
        deckId: ownProps.match.params.deckId,
        viewMode: state.decks.deckEditor.viewMode,  //state.app.deckEditor.viewMode,
        // cardMenuAnchor: state.ui.deckEditorMenuAnchor,
        // selectedCard: getSelectedCardOverview(state),
        // selectedInventoryCards: getSelectedDeckDetails(state),
        
        //deckProperties: state.data.deckDetail.deckProps,
        deckProperties: state.decks.data.detail.deckProps,
        formatFilterOptions: state.core.data.filterOptions.formats,

        isPropsDialogOpen: state.decks.deckEditor.isPropsModalOpen,
        deckDialogProperties: state.decks.deckEditor.deckModalProps,
        // cardOverviews: selectDeckOverviews(state),

        //deckStats: state.data.deckDetail.deckStats,
        deckStats: state.decks.data.detail.deckStats,
        
        
        // deckPropsModalOpen: state.ui.deckPropsModalOpen,
        

        
        //Overview
        groupedCardOverviews: selectDeckOverviews(state),

        //Detail
        cardMenuAnchor: null, //state.ui.deckEditorMenuAnchor,
        selectedCard: getSelectedCardOverview(state),
        selectedInventoryCards: getSelectedDeckDetails(state),
    }

    return result;
}

export default connect(mapStateToProps)(DeckEditor);