import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, MenuItem, TextareaAutosize, TextField } from "@material-ui/core";
import React, { ChangeEvent } from "react";

declare interface ComponentProps{
    isDialogOpen: boolean;
    selectedExportType: DeckExportType;
    deckExportString: string;
    onExportTypeChange: (exportType: string) => void;
    onExportButtonClick: () => void;
    onExportCloseClick: () => void;
}

export function DeckExportLayout(props: ComponentProps): JSX.Element {
    // const { flexRow, flexSection } = appStyles();
    return(
        <Dialog open={props.isDialogOpen}>
            <DialogTitle>Export Deck</DialogTitle>
            <DialogContent>
                <Box>
                    <TextField
                        name="Export Type"
                        // className={stretch}
                        select
                        label="Export Type"
                        value={props.selectedExportType}
                        onChange={(event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => props.onExportTypeChange(event.target.value)}
                        margin="normal">
                        <MenuItem key={'list'} value={'list'}>Deck List</MenuItem>
                        <MenuItem key={'detail'} value={'detail'}>Detailed List</MenuItem>
                        <MenuItem key={'empty'} value={'empty'}>Empty Cards</MenuItem>
                        <MenuItem key={'suggestions'} value={'suggestions'}>Empty Card Suggestions</MenuItem>
                    </TextField>
                    <Button color={"primary"} variant={"contained"} onClick={props.onExportButtonClick}>
                        Export
                    </Button>
                </Box>
                <TextareaAutosize placeholder="Select Export Type" rowsMin={3} rowsMax={10} value={props.deckExportString} />
            </DialogContent>
            <DialogActions>
                <Button size="medium" onClick={props.onExportCloseClick}>
                    Close
                </Button>
            </DialogActions>
        </Dialog>
    );
}