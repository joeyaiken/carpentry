import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@material-ui/core";
import React from "react";
import CardDetailContainer from "../../card-detail/CardDetailContainer";
import {useAppDispatch, useAppSelector} from "../../../hooks";
import {push} from "react-router-redux";
import {getSelectedDeckId} from "../deckDetailSlice";
import {useHistory} from "react-router";

declare interface ComponentProps{
  // onCloseClick: () => void;
  isDialogOpen: boolean;
  
  // loaded from querystring
  selectedCardId: number; //selected DETAIL card ID for MODAL
}

// Card detail dialog
//     A dialog that lists all cards in the deck matching a provided name, as well as all inventory cards under the same name
export function CardDetailDialog(props: ComponentProps): JSX.Element {

  const selectedDeckId = useAppSelector(getSelectedDeckId);

  const history = useHistory();
  const onCloseClick = (): void => {
    history.push(`/decks/${selectedDeckId}`);
  }
  
  return(
    <Dialog open={props.isDialogOpen} onClose={() => {}} >
      <DialogTitle>Card Detail</DialogTitle>
      <DialogContent>
        <CardDetailContainer selectedCardId={props.selectedCardId} />
      </DialogContent>
      <DialogActions>
        <Button size="medium" onClick={onCloseClick}>Close</Button>
      </DialogActions>
    </Dialog>
  );
}

