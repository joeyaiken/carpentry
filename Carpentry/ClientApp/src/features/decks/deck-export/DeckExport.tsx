import React, {ChangeEvent, useState} from "react";
import {useAppDispatch, useAppSelector} from "../../../hooks";
import {
  Box,
  Button,
  Dialog, DialogActions,
  DialogContent,
  DialogTitle,
  MenuItem,
  TextareaAutosize,
  TextField
} from "@material-ui/core";
import {closeExportDialog, getDeckExport} from "./deckExportSlice";
import {ApiStatus} from "../../../enums";
import {getSelectedDeckId} from "../../../decks/deck-editor/deckDetailSlice";

export const DeckExport = (): JSX.Element => {
  const deckId = useAppSelector(getSelectedDeckId);
  const deckExportString = useAppSelector(state => state.decks.deckExport.deckExportPayload);
  const isExportOpen = useAppSelector(state => state.decks.deckExport.isDialogOpen);
  const exportStatus = useAppSelector(state => state.decks.deckExport.status);

  const [selectedExportType, setSelectedExportType] = useState<string>('list');
  
  const dispatch = useAppDispatch();
  const onExportButtonClick = (): void => {
    if(exportStatus === ApiStatus.initialized)
      dispatch(getDeckExport({
        deckId: deckId,
        exportType: selectedExportType
      }));
  }
  const onExportCloseClick = (): void => {
    dispatch(closeExportDialog());
  }

  return(
    <Dialog open={isExportOpen}>
      <DialogTitle>Export Deck</DialogTitle>
      <DialogContent>
        <Box>
          <TextField
            name="Export Type"
            select
            label="Export Type"
            value={selectedExportType}
            onChange={(event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => setSelectedExportType(event.target.value)}
            margin="normal">
            <MenuItem key={'list'} value={'list'}>Deck List</MenuItem>
            <MenuItem key={'detail'} value={'detail'}>Detailed List</MenuItem>
            <MenuItem key={'empty'} value={'empty'}>Empty Cards</MenuItem>
            <MenuItem key={'suggestions'} value={'suggestions'}>Empty Card Suggestions</MenuItem>
          </TextField>
          <Button color={"primary"} variant={"contained"} onClick={onExportButtonClick}>
            Export
          </Button>
        </Box>
        <TextareaAutosize placeholder="Select Export Type" minRows={3} maxRows={10} value={deckExportString} />
      </DialogContent>
      <DialogActions>
        <Button size="medium" onClick={onExportCloseClick}>
          Close
        </Button>
      </DialogActions>
    </Dialog>
  );
}