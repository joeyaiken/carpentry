import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@material-ui/core";
import React from "react";
import {useAppSelector} from "../../../hooks";
import {useHistory} from "react-router";
import {CardTags} from "../../../features/decks/card-tags/CardTags";
import {getSelectedDeckId} from "../deckDetailSlice";

declare interface ComponentProps{
  // Note - These two both depend on querystring values
  isDialogOpen: boolean;
  selectedCardId: number;
}

export const CardTagsDialog = (props: ComponentProps): JSX.Element => {
  const deckId = useAppSelector(getSelectedDeckId);

  const history = useHistory();
  const onCloseClick = (): void => {
    history.push(`/decks/${deckId}`);
    // this.props.dispatch(reloadDeckDetail(this.props.deckId));
    // TODO - add some re-implementation of 'reloadDeckDetail'
  }
  
  return(
    <Dialog open={props.isDialogOpen} onClose={() => {}} >
      <DialogTitle>Card Tags</DialogTitle>
      <DialogContent>
        <CardTags selectedCardId={props.selectedCardId} />
      </DialogContent>
      <DialogActions>
        <Button size="medium" onClick={onCloseClick}>Close</Button>
      </DialogActions>
    </Dialog>
  );
}

