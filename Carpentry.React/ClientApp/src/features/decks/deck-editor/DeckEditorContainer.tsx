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
    requestUpdateDeckCard,
    requestDeleteDeckCard,
} from './state/DeckEditorActions';

import { AppState } from '../../configureStore';
import { ensureDeckDetailLoaded, reloadDeckDetail } from '../state/decksDataActions';
import { DeckEditorLayout } from './components/DeckEditorLayout';
import { push } from 'react-router-redux';
import { DeckPropsDialog } from './components/DeckPropsDialog';
import { CardDetailDialog } from './components/CardDetailDialog';
// import { DeckExportDialog } from './components/DeckExportDialog';
import DeckExportContainer from '../deck-export/DeckExportContainer';
import { openExportDialog } from '../deck-export/state/DeckExportActions';
import CardTagsContainer from '../card-tags/CardTagsContainer';
import { CardTagsDialog } from './components/CardTagsDialog';

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
    cardDetailsById: { [deckCardId: number]: DeckCardDetail };

    //Detail
    cardMenuAnchor: HTMLButtonElement | null;
    cardMenuAnchorId: number;

    selectedCard: DeckCardOverview | null;
    selectedInventoryCards: DeckCardDetail[];

    //card detail
    selectedCardId: number;
    isCardDetailDialogOpen: boolean;
    isCardTagsDialogOpen: boolean;

    //export
    isExportDialogOpen: boolean;
    selectedExportType: DeckExportType;
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
        this.handleDialogClose = this.handleDialogClose.bind(this);
        // this.handleExportOpenClick = this.handleExportOpenClick.bind(this);
        // this.handleExportTypechange = this.handleExportTypechange.bind(this);
        // this.handleExportDialogButtonClick = this.handleExportDialogButtonClick.bind(this);
        // this.handleExportCloseClick = this.handleExportCloseClick.bind(this);

        this.handleCardTagsClick = this.handleCardTagsClick.bind(this);

        this.handleExportClicked = this.handleExportClicked.bind(this);

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
        // switch (name){
        //     // case "search":
        //     //     if(this.props.cardMenuAnchor != null){
        //     //         // this.props.dispatch(deckCardRequestAlternateVersions(this.props.cardMenuAnchor.name))
        //     //     }
        //     //     break;
        //     // case "delete":
        //     //         if(this.props.cardMenuAnchor != null){
        //     //             const confirmText = `Are you sure you want to delete ${this.props.cardMenuAnchor.name}?`;
        //     //             if(window.confirm(confirmText)){
        //     //                 // this.props.dispatch(requestDeleteDeckCard(parseInt(this.props.cardMenuAnchor.value)));
        //     //             }
        //     //         }
        //     //         break;

        //hasInventoryCard={Boolean(props.cardDetailsById[props.cardMenuAnchorId]?.inventoryCardId)}
        const deckCardDetail = this.props.cardDetailsById[this.props.cardMenuAnchorId];
        switch(name){
            case "sideboard":
                deckCardDetail.category = name;
                this.props.dispatch(requestUpdateDeckCard(deckCardDetail));
                break;
            case "mainboard":
                deckCardDetail.category = "";
                this.props.dispatch(requestUpdateDeckCard(deckCardDetail));
                break;
            case "commander":
                deckCardDetail.category = name;
                this.props.dispatch(requestUpdateDeckCard(deckCardDetail));
                break;
            case "inventory":
                deckCardDetail.inventoryCardId = null;
                this.props.dispatch(requestUpdateDeckCard(deckCardDetail));
                break;
            case "delete":
                const confirmText = `Are you sure you want to delete ${this.props.cardMenuAnchor?.name}?`;
                if(window.confirm(confirmText)){
                    deckCardDetail.inventoryCardId = null;
                    this.props.dispatch(requestDeleteDeckCard(deckCardDetail.id));
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
        this.props.dispatch(push(`/decks/${this.props.deckId}?cardId=${cardId}&show=detail`));
    }

    handleDialogClose(){
        this.props.dispatch(push(`/decks/${this.props.deckId}`));
        // this.props.dispatch(reloadDeckDetail(this.props.deckId));
    }

    handleCardTagsClick(cardId: number){
        this.props.dispatch(push(`/decks/${this.props.deckId}?cardId=${cardId}&show=tags`));
    }

    handleExportClicked(): void {
        this.props.dispatch(openExportDialog());
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
                <React.Fragment>
                    {
                        this.props.deckDialogProperties && 
                        <DeckPropsDialog 
                            isOpen={this.props.isPropsDialogOpen}
                            onCloseClick={this.handlePropsModalClose}
                            onFieldChange={this.handleModalPropsChanged}
                            deckProperties={this.props.deckDialogProperties}
                            formatFilterOptions={this.props.formatFilterOptions}
                            onSaveClick={this.handlePropsModalSave} 
                            onDisassembleClick={this.handlePropsModalDisassemble}
                            onDeleteClick={this.handlePropsModalDelete} />
                    }
                    <CardDetailDialog 
                        onCloseClick={this.handleDialogClose}
                        isDialogOpen={this.props.isCardDetailDialogOpen}
                        selectedCardId={this.props.selectedCardId} />

                    <CardTagsDialog
                        onCloseClick={this.handleDialogClose}
                        isDialogOpen={this.props.isCardTagsDialogOpen}
                        selectedCardId={this.props.selectedCardId} />

                    <DeckExportContainer />

                    <DeckEditorLayout 
                        //props & modal
                        deckProperties={this.props.deckProperties}
                        onPropsModalOpen={this.handlePropsModalOpen}

                        onAddCardsClick={this.handleAddCardsClicked}
                        onExportClick={this.handleExportClicked}

                        //View
                        handleToggleDeckView={this.handleToggleDeckView}
                        viewMode={this.props.viewMode}
                    
                        //overview
                        groupedCardOverviews={this.props.groupedCardOverviews}
                        onCardSelected={this.handleCardSelected} 
                        cardDetailsById={this.props.cardDetailsById}
                        //detail
                        cardMenuAnchor={this.props.cardMenuAnchor}
                        cardMenuAnchorId={this.props.cardMenuAnchorId}
                        selectedCard={this.props.selectedCard} 
                        selectedInventoryCards={this.props.selectedInventoryCards} 
                        onCardMenuSelected={this.handleCardMenuSelected}
                        onCardMenuClick={this.handleCardMenuClick}
                        onCardMenuClosed={this.handleCardMenuClosed}

                        //dialogs
                        onCardDetailClick={this.handleCardDetailClick}
                        onCardTagsClick={this.handleCardTagsClick}
                        //stats
                        deckStats={this.props.deckStats} />

                </React.Fragment>
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

//So, this should be a 'memoized' selector in the slice?
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

    const show = queryString['show'];

    let isDetailDialogOpen = false;
    let isTagsDialogOpen = false;

    if(show === "tags"){
        isTagsDialogOpen = true;
    }
    if(show === "detail"){
        isDetailDialogOpen = true;
    }


    //const x = state.decks.data.detail.cardDetails.byId

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
        cardDetailsById: state.decks.data.detail.cardDetails.byId,

        //Detail
        cardMenuAnchor: state.decks.deckEditor.cardMenuAnchor, //state.ui.deckEditorMenuAnchor,
        cardMenuAnchorId: state.decks.deckEditor.cardMenuAnchorId,
        selectedCard: getSelectedCardOverview(state),
        selectedInventoryCards: getSelectedDeckDetails(state),

        //card detail
        //cardDetailName: selectedCardName,
        selectedCardId: selectedCardId,
        isCardDetailDialogOpen: isDetailDialogOpen,
        isCardTagsDialogOpen: isTagsDialogOpen,

        //export
        isExportDialogOpen: true,
        selectedExportType: 'list',
    }

    return result;
}

export default connect(mapStateToProps)(DeckEditor);