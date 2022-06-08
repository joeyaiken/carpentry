import React, {useEffect} from "react";
import {DeckCardDetail} from "./components/DeckCardDetail";
import {DeckPropsDialog} from "./components/DeckPropsDialog";
import {CardDetailDialog} from "./components/CardDetailDialog";
import {CardTagsDialog} from "./components/CardTagsDialog";
import {DeckEditorLayout} from "./components/DeckEditorLayout";
import {
  requestDeleteDeck,
  requestDisassembleDeck, 
} from "./state/DeckEditorActions";
import {useAppDispatch, useAppSelector} from "../../hooks";
import {useHistory} from "react-router";
import {getSelectedDeckId, loadDeckDetails} from "./deckDetailSlice";
import {ApiStatus} from "../../enums";
import {DeckExport} from "../../features/decks/deck-export/DeckExport";

declare interface ComponentProps {
  deckId: number;
  deckProperties: DeckPropertiesDto | null;
  deckStats: DeckStats | null;
  isPropsDialogOpen: boolean;

  //Overview
  groupedCardOverviews: CardOverviewGroup[];
  //Non-grouped views will just snag cards from item at position 0
  cardDetailsById: { [deckCardId: number]: DeckCardDetail };

  //Detail
  // cardMenuAnchor: HTMLButtonElement | null;
  // cardMenuAnchorId: number;

  selectedCard: DeckCardOverview | null;
  selectedInventoryCards: DeckCardDetail[];

  //card detail
  selectedCardId: number;
  isCardDetailDialogOpen: boolean;
  isCardTagsDialogOpen: boolean;

  //export
  // isExportDialogOpen: boolean;
  // selectedExportType: string;
  
}

export const DeckEditorNewComponent = (props: ComponentProps): JSX.Element => {

  const dispatch = useAppDispatch();
  const history = useHistory();
  
  const routDeckId = +props.deckId;
  
  const selectedDeckId = useAppSelector(getSelectedDeckId);
  
  const shouldLoad = useAppSelector(state => {
    // no matter what, if there isn't a valid id, don't load
    if(!routDeckId){
      return false;
    }
    
    // else, if there is a good id, and it's not initialized, then we do load
    if(state.decks.deckDetailData.status === ApiStatus.uninitialized){
      return true;
    }
    
    // then, if it's initialized and the IDs don't match, also load
    if(state.decks.deckDetailData.status === ApiStatus.initialized && selectedDeckId !== routDeckId) {
      return true;
    }
    
    // Finally, just return false
    return false;
  });
  
  useEffect(() => {
    //This was getting called a lot, that sounds like a red flag
    
    if(shouldLoad) dispatch(loadDeckDetails(routDeckId));
  })
  
  // const handlePropsModalSave = (): void => {
  //   dispatch(requestSavePropsModal());
  // }

  // const handleModalPropsChanged = (name: string, value: string | number): void => {
  //   dispatch(deckPropsModalChanged(name, value));
  // }

  const handlePropsModalDisassemble = (): void => {
    //TODO - replace this alert with something classier
    const confirmText = `Are you sure you want to disassemble this deck? This will return all cards to the inventory, but keep the deck definition.`;
    if(window.confirm(confirmText)){
    dispatch(requestDisassembleDeck());
  }
}

  const handlePropsModalDelete = (): void => {
    //TODO - replace this alert with something classier
    const confirmText = `Are you sure you want to delete this deck?  This cannot be undone.`;
    if(window.confirm(confirmText)){
      dispatch(requestDeleteDeck());
    }
  }

  return (
    <React.Fragment>
      { selectedDeckId !== 0 &&
        // props.deckDialogProperties &&
        <DeckPropsDialog
          isOpen={props.isPropsDialogOpen}
          // onCloseClick={handlePropsModalClose}
          // onFieldChange={handleModalPropsChanged}
          // onSaveClick={handlePropsModalSave}
          onDisassembleClick={handlePropsModalDisassemble}
          onDeleteClick={handlePropsModalDelete} />
      }
      <CardDetailDialog
        isDialogOpen={props.isCardDetailDialogOpen}
        selectedCardId={props.selectedCardId} />

      <CardTagsDialog
        isDialogOpen={props.isCardTagsDialogOpen}
        selectedCardId={props.selectedCardId} />

      <DeckExport />

      <DeckEditorLayout />

    </React.Fragment>
  )
}