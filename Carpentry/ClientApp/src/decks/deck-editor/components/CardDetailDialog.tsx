import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@material-ui/core";
import React from "react";
import CardDetailContainer from "../../card-detail/CardDetailContainer";

declare interface ComponentProps{
    onCardDetailClose: () => void;
    isCardDetailDialogOpen: boolean;
    selectedCardId: number; //selected DETAIL card ID for MODAL
}

// Card detail dialog
//     A dialog that lists all cards in the deck matching a provided name, as well as all inventory cards under the same name
export function CardDetailDialog(props: ComponentProps): JSX.Element {
    // const { flexRow, flexSection } = appStyles();
    return(
        <Dialog open={props.isCardDetailDialogOpen} onClose={() => {}} >
            <DialogTitle>Card Detail</DialogTitle>
            <DialogContent>
                <CardDetailContainer selectedCardId={props.selectedCardId} />
            </DialogContent>
            <DialogActions>
                <Button size="medium" onClick={props.onCardDetailClose}>Close</Button>
            </DialogActions>
        </Dialog>
    );
}

