import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from "@material-ui/core";
import React from "react";
import {DeckPropertiesLayout} from "../../../common/components/DeckPropertiesLayout";

declare interface ComponentProps{
    onCloseClick: () => void;
    onSaveClick: () => void;
    onDisassembleClick: () => void;
    onDeleteClick: () => void;
    onFieldChange: (name: string, value: string | number) => void;
    formatFilterOptions: FilterOption[];
    deckProperties: DeckPropertiesDto;
    isOpen: boolean;
}

export function DeckPropsDialog(props: ComponentProps): JSX.Element {
    return(
        <Dialog open={props.isOpen} onClose={props.onCloseClick} >
            <DialogTitle>Deck Properties</DialogTitle>
            <DialogContent>
                <DeckPropertiesLayout showLands={true} formatFilters={props.formatFilterOptions} deck={props.deckProperties}
                    onChange={event => props.onFieldChange(event.target.name, event.target.value)} />
            </DialogContent>
            <DialogTitle>Advanced</DialogTitle>
            <DialogActions>
                <Button size="medium" variant="contained" color="secondary" onClick={props.onDisassembleClick}>Disassemble</Button>
                <Button size="medium" variant="contained" color="secondary" onClick={props.onDeleteClick}>Delete</Button>
            </DialogActions>
            <DialogActions>
                <Button size="medium" onClick={props.onCloseClick}>Cancel</Button>
                {/* <Button size="medium" variant="contained" color="secondary">Disassemble</Button> */}
                {/* <Button size="medium" variant="contained" color="secondary">Delete</Button> */}
                <Button size="medium" variant="contained" color="primary" onClick={props.onSaveClick}>Save</Button>
            </DialogActions>
        </Dialog>
    );
}