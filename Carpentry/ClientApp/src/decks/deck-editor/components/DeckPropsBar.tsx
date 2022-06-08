import React from 'react';
import { Typography, Box, AppBar, Chip, Toolbar, Avatar, Button } from '@material-ui/core';
import {useAppDispatch, useAppSelector} from "../../../hooks";
import {openDeckPropsModal, toggleDeckViewMode} from "../state/DeckEditorActions";
import {getSelectedDeckId} from "../deckDetailSlice";
import {useHistory} from "react-router";
import {openExportDialog} from "../../../features/decks/deck-export/deckExportSlice";

const ManaChip = (type: String, value: number): JSX.Element =>
  (<Chip size="small" avatar={<Avatar src={`/img/${type}.svg`}/>} label={ value }/>);

export const DeckPropsBar = (): JSX.Element => {
  const deckProperties = useAppSelector(state => state.decks.deckDetailData.deckProps);
  const deckId = useAppSelector(getSelectedDeckId);

  const { basicW, basicU, basicB, basicR, basicG } = deckProperties;

  const dispatch = useAppDispatch();
  const history = useHistory();
  
  const onEditClick = (): void => {
    dispatch(openDeckPropsModal());
  }
  
  const onToggleViewClick = (): void => {
    dispatch(toggleDeckViewMode());
  }

  const onAddCardsClick = (): void => {
    history.push(`/decks/${deckId}/addCards`);
  }
  
  const onExportClick = (): void => {
    dispatch(openExportDialog());
  }
  
  return (
    <AppBar color="default" position="relative">
      <Toolbar>
        <Typography variant="h5">{deckProperties.name}</Typography>
        <Box>
          { basicW > 0 && ManaChip('W',basicW)}
          { basicU > 0 && ManaChip('U',basicU)}
          { basicB > 0 && ManaChip('B',basicB)}
          { basicR > 0 && ManaChip('R',basicR)}
          { basicG > 0 && ManaChip('G',basicG)}
        </Box>
        <Box>
          <Button onClick={onToggleViewClick} color="primary" variant="contained">
            Toggle View
          </Button>
          <Button onClick={onEditClick} color="primary" variant="contained">
            Edit
          </Button>
          <Button onClick={onExportClick} color="primary" variant="contained">
            Export
          </Button>
          <Button onClick={onAddCardsClick} color="primary" variant="contained" className="add-cards-button">
            Add Cards
          </Button>
        </Box>
      </Toolbar>
    </AppBar>
  );
}