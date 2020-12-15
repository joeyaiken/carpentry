import { connect, DispatchProp } from 'react-redux'
import React from 'react'
import { Typography, Box } from '@material-ui/core';
import { 
    toggleDeckViewMode, 
    deckEditorCardSelected, 
    openDeckPropsModal, 
    closeDeckPropsModal, 
    requestSavePropsModal, 
    deckPropsModalChanged, 
    requestDisassembleDeck, 
    requestDeleteDeck,
    cardMenuButtonClicked,
    requestUpdateDeckCardStatus,
} from './state/DeckEditorActions';

import { AppState } from '../../configureStore';
import { ensureDeckDetailLoaded } from '../state/decksDataActions';
import { DeckEditorLayout } from './components/DeckEditorLayout';
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
    location: {
        search: string;
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
    selectedInventoryCards: DeckCardDetail[];

    //card detail
    selectedCardId: number;
    isCardDetailDialogOpen: boolean;
    
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
        this.handleCardDetailClick = this.handleCardDetailClick.bind(this);
        this.handleCardDetailClose = this.handleCardDetailClose.bind(this);
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
        this.props.dispatch(cardMenuButtonClicked(event.currentTarget));
    }

    handleCardMenuSelected(name: DeckEditorCardMenuOption){
        // console.log('card anchor');
        // console.log(this.props.cardMenuAnchor);
        switch (name){
            // case "search":
            //     if(this.props.cardMenuAnchor != null){
            //         // this.props.dispatch(deckCardRequestAlternateVersions(this.props.cardMenuAnchor.name))
            //     }
            //     break;
            // case "delete":
            //         if(this.props.cardMenuAnchor != null){
            //             const confirmText = `Are you sure you want to delete ${this.props.cardMenuAnchor.name}?`;
            //             if(window.confirm(confirmText)){
            //                 // this.props.dispatch(requestDeleteDeckCard(parseInt(this.props.cardMenuAnchor.value)));
            //             }
            //         }
            //         break;
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

        this.props.dispatch(cardMenuButtonClicked(null));
    }

    handleCardMenuClosed(): void {
        this.props.dispatch(cardMenuButtonClicked(null));
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
    
    //handleCardDetailClick(cardName: string){
    handleCardDetailClick(cardId: number){
        // console.log(`handleCardDetailClick: ${cardName}`)
        // const encodedCardName = encodeURI(cardName);
        //this.props.dispatch(push(`/decks/${this.props.deckId}?card=${encodedCardName}`));
        this.props.dispatch(push(`/decks/${this.props.deckId}?cardId=${cardId}`));
    }

    handleCardDetailClose(){
        this.props.dispatch(push(`/decks/${this.props.deckId}`));
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

                    //detail dialog
                    onCardDetailClick={this.handleCardDetailClick}
                    selectedCardId={this.props.selectedCardId}
                    isCardDetailDialogOpen={this.props.isCardDetailDialogOpen}
                    onCardDetailClose={this.handleCardDetailClose}

                    //stats
                    deckStats={this.props.deckStats} />
            )
        }
    }
}

function selectDeckOverviews(state: AppState): CardOverviewGroup[] {
    const { cardOverviews, cardGroups } = state.decks.data.detail; //state.data.deckDetail;

    if(state.decks.deckEditor.viewMode === "grouped"){
        const result = cardGroups.map(group => {
            const groupResult: CardOverviewGroup = {
                name: group.name,
                cardOverviews: group.cardOverviewIds.map(id => cardOverviews.byId[id]),
            }
            return groupResult;
        });
        return result;

    } else {

        return [{
            name: "All",
            cardOverviews: cardOverviews.allIds.map(id => cardOverviews.byId[id]),
        }];

    }
}

function getSelectedCardOverview(state: AppState): DeckCardOverview | null {
    const selectedOverviewCardId = state.decks.deckEditor.selectedOverviewCardId;
    if(selectedOverviewCardId){
        return state.decks.data.detail.cardOverviews.byId[selectedOverviewCardId];
    }
    return null;
}

function getSelectedDeckDetails(state: AppState): DeckCardDetail[] {
    const { cardOverviews, cardDetails } = state.decks.data.detail;
    const { selectedOverviewCardId } = state.decks.deckEditor;
    if(selectedOverviewCardId){
        const match = cardOverviews.byId[selectedOverviewCardId];
        if(match){
            return match.detailIds.map(id => cardDetails.byId[id]);
        }
        // return cardOverviews.byId[selectedOverviewCardId].detailIds.map(id => cardDetails.byId[id]);
    }
    return [];
}



interface ParsedQueryString { [key: string]:string }

function parseQueryString(queryString: string): ParsedQueryString {
    const result: ParsedQueryString = {};
    //leading char will be '?'
    if(queryString.length === 0) return result;

    const substring = queryString.substring(1).split('&');

    if(substring && substring.length){
        substring.forEach(element => {
            var keyVal = element.split('=');
            const key = keyVal[0];
            const val = keyVal[1];
            result[key] = val;
        });
    }

    return result;
}

function mapStateToProps(state: AppState, ownProps: OwnProps): PropsFromState {

    const queryString = parseQueryString(ownProps.location.search);

    //const selectedCardName = decodeURI(queryString['card'] || '');
    const selectedCardId = +queryString['cardId'] || 0;

    const result: PropsFromState = {
        deckId: ownProps.match.params.deckId,
        viewMode: state.decks.deckEditor.viewMode,  //state.app.deckEditor.viewMode,
        // cardMenuAnchor: state.ui.deckEditorMenuAnchor,
        // selectedCard: getSelectedCardOverview(state),
        // selectedInventoryCards: getSelectedDeckDetails(state),
        
        
        deckProperties: state.decks.data.detail.deckProps,
        formatFilterOptions: state.core.data.filterOptions.formats,

        isPropsDialogOpen: state.decks.deckEditor.isPropsModalOpen,
        deckDialogProperties: state.decks.deckEditor.deckModalProps,
        deckStats: state.decks.data.detail.deckStats,
        
        //Overview
        groupedCardOverviews: selectDeckOverviews(state),

        //Detail
        cardMenuAnchor: state.decks.deckEditor.cardMenuAnchor, //state.ui.deckEditorMenuAnchor,
        selectedCard: getSelectedCardOverview(state),
        selectedInventoryCards: getSelectedDeckDetails(state),

        //card detail
        //cardDetailName: selectedCardName,
        selectedCardId: selectedCardId,
        isCardDetailDialogOpen: selectedCardId !== 0,
    }

    return result;
}

export default connect(mapStateToProps)(DeckEditor);