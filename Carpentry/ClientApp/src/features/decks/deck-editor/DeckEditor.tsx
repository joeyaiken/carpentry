import React, {useEffect} from 'react';
// import {useAppSelector} from "../../../app/hooks";
// import {DeckPropsDialog} from "./components/DeckPropsDialog";
// import {CardDetailDialog} from "./components/CardDetailDialog";
// import {CardTagsDialog} from "./components/CardTagsDialog";
// import DeckExportContainer from "../deck-export/DeckExportContainer";
// import {DeckEditorLayout} from "./components/DeckEditorLayout";
// import {Box} from "@material-ui/core";
// import DeckCardList from "./components/DeckCardList";
// import {DeckCardGrid} from "./components/DeckCardGrid";
// import GroupedDeckCardList from "./components/GroupedDeckCardList";
// import CardMenu from "./components/CardMenu";
// import DeckCardDetail from "./components/DeckCardDetail";
// import {DeckStatsBar} from "./components/DeckStatsBar";
// import {AppLayout} from "../../../common/components/AppLayout";
import {useAppDispatch, useAppSelector} from "../../../app/hooks";
import {loadDeckDetails} from "../deckDetailSlice";
import {DeckEditorLayout} from "../../../decks/deck-editor/components/DeckEditorLayout";
// import {ensureDeckDetailLoaded} from "../state/decksDataActions";
// import {ensureDeckDetailLoaded} from "../state/decksDataActions";

interface OwnProps {
  match: {
    params: {
      deckId: string;
    }
  }
}

export const DeckEditor = (props: OwnProps): JSX.Element => {
  const deckId = +props.match.params.deckId;
  const dispatch = useAppDispatch();
  
  const shouldLoad = useAppSelector(
    (state) => !state.decks.detail.isLoading && state.decks.detail.deckId !== deckId
  );
  
  useEffect(() => {
    if(shouldLoad) dispatch(loadDeckDetails(deckId));
  })
  
  return (
    <React.Fragment>
      {/*{*/}
      {/*  this.props.deckDialogProperties &&*/}
      {/*  <DeckPropsDialog*/}
      {/*    isOpen={this.props.isPropsDialogOpen}*/}
      {/*    onCloseClick={this.handlePropsModalClose}*/}
      {/*    onFieldChange={this.handleModalPropsChanged}*/}
      {/*    deckProperties={this.props.deckDialogProperties}*/}
      {/*    formatFilterOptions={this.props.formatFilterOptions}*/}
      {/*    onSaveClick={this.handlePropsModalSave}*/}
      {/*    onDisassembleClick={this.handlePropsModalDisassemble}*/}
      {/*    onDeleteClick={this.handlePropsModalDelete} />*/}
      {/*}*/}
      {/*<CardDetailDialog*/}
      {/*  onCloseClick={this.handleDialogClose}*/}
      {/*  isDialogOpen={this.props.isCardDetailDialogOpen}*/}
      {/*  selectedCardId={this.props.selectedCardId} />*/}
      
      {/*<CardTagsDialog*/}
      {/*  onCloseClick={this.handleDialogClose}*/}
      {/*  isDialogOpen={this.props.isCardTagsDialogOpen}*/}
      {/*  selectedCardId={this.props.selectedCardId} />*/}

      {/*<DeckExportContainer />*/}

      <DeckEditorLayout
        deckId={deckId}
        // //props & modal
        // deckProperties={deckProperties}
        // onPropsModalOpen={this.handlePropsModalOpen}
        //
        // onAddCardsClick={this.handleAddCardsClicked}
        // onExportClick={this.handleExportClicked}
        //
        // //View
        // handleToggleDeckView={this.handleToggleDeckView}
        // viewMode={this.props.viewMode}
        //
        // //overview
        // groupedCardOverviews={this.props.groupedCardOverviews}
        // onCardSelected={this.handleCardSelected}
        // cardDetailsById={this.props.cardDetailsById}
        // //detail
        // cardMenuAnchor={this.props.cardMenuAnchor}
        // cardMenuAnchorId={this.props.cardMenuAnchorId}
        // selectedCard={this.props.selectedCard}
        // selectedInventoryCards={this.props.selectedInventoryCards}
        // onCardMenuSelected={this.handleCardMenuSelected}
        // onCardMenuClick={this.handleCardMenuClick}
        // onCardMenuClosed={this.handleCardMenuClosed}
        //
        // //dialogs
        // onCardDetailClick={this.handleCardDetailClick}
        // onCardTagsClick={this.handleCardTagsClick}
        // //stats
        // deckStats={this.props.deckStats}
      />

    </React.Fragment>
  )
}


