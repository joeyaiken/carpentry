import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@material-ui/core';
import {useAppSelector} from "../../../hooks";
import {DeckPropertiesLayout} from "../../../features/common/components/DeckPropertiesLayout";

interface ComponentProps {
  //Should be local state probably...
  deckProps: DeckPropertiesDto;

  formatFilters: FilterOption[];
  onChange: (name: string, value: string) => void;
  onSaveClick: () => void;
  onCloseClick: () => void;
}

export default function NewDeckLayout(props: ComponentProps): JSX.Element {

  const formatFilters = useAppSelector(state => state.core.filterOptions.formats);

  return(
    <React.Fragment>
      <Dialog open={true} onClose={props.onCloseClick} >
        <DialogTitle>New Deck</DialogTitle>
        <DialogContent>
          <DeckPropertiesLayout 
            showLands={false}
            formatFilters={props.formatFilters}
            deck={props.deckProps}
            onChange={event => props.onChange(event.target.name, event.target.value)} />
        </DialogContent>
        <DialogActions>
          <Button size="medium" onClick={props.onCloseClick}>Cancel</Button>
          <Button id="save-new-deck-button" size="medium" variant="contained" color="primary" onClick={props.onSaveClick}>Save</Button>
        </DialogActions>
      </Dialog>
    </React.Fragment>
  );
}