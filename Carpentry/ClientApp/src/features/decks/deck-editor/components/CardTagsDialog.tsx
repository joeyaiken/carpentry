import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@material-ui/core";
import React from "react";
import CardTagsContainer from "../../card-tags/CardTagsContainer";

declare interface ComponentProps{
    onCloseClick: () => void;
    isDialogOpen: boolean;
    selectedCardId: number;
}

export function CardTagsDialog(props: ComponentProps): JSX.Element {
    // const { flexRow, flexSection } = appStyles();
    return(
        <Dialog open={props.isDialogOpen} onClose={() => {}} >
            <DialogTitle>Card Tags</DialogTitle>
            <DialogContent>
                <CardTagsContainer selectedCardId={props.selectedCardId} />
            </DialogContent>
            <DialogActions>
                <Button size="medium" onClick={props.onCloseClick}>Close</Button>
            </DialogActions>
        </Dialog>
    );
}

