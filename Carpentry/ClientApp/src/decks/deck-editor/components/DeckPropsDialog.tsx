import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from "@material-ui/core";
import React, {useState} from "react";
import {useAppDispatch, useAppSelector} from "../../../hooks";
import {closeDeckPropsModal, requestSavePropsModal} from "../state/DeckEditorActions";
import {Deck} from "@material-ui/icons";
import {DeckPropertiesLayout} from "../../../features/common/components/DeckPropertiesLayout";

declare interface ComponentProps{
  // onCloseClick: () => void;
  // onSaveClick: () => void;
  // onSaveClick: (deckProps: DeckPropertiesDto) => void;
  onDisassembleClick: () => void;
  onDeleteClick: () => void;
  // onFieldChange: (name: string, value: string | number) => void;
  // formatFilterOptions: FilterOption[];
  // deckProperties: DeckPropertiesDto;
  isOpen: boolean;
}

export function DeckPropsDialog(props: ComponentProps): JSX.Element {
  
  const formatFilterOptions = useAppSelector(state => state.core.filterOptions.formats);
  const deckProperties = useAppSelector(state => state.decks.deckDetailData.deckProps);
  
  // So, what is the appropriate place to be updating this form state?
  //  I know useEffect probably isn't right
  
  
  
  
  
  const [deckPropsFormValues, setDeckPropsFormValues] = useState<DeckPropertiesDto>({
    ...deckProperties,
  });

  // const [deckPropsFormValues, setDeckPropsFormValues] = useState<DeckPropertiesDto>({
  //   id: 0,
  //   name: 'deck name',
  //   format: null,
  //   notes: '',
  //   basicW: 0,
  //   basicU: 0,
  //   basicB: 0,
  //   basicR: 0,
  //   basicG: 0,
  // });
  
  const onFieldChange = (name: string, value: string | number): void => {
    
    setDeckPropsFormValues({
      ...deckPropsFormValues,
      [name]: value
    })
  }
  
  const dispatch = useAppDispatch();

  const onSaveClick = (): void => {
    dispatch(requestSavePropsModal(deckPropsFormValues));
  }
  
  const onCloseClick = (): void => {
    dispatch(closeDeckPropsModal());
  }
  // TODO - Apparently the existence of this Dialog tag causes an error in React.Strict mode, look into it
  return(
    <Dialog open={props.isOpen} onClose={onCloseClick} >
      <DialogTitle>Deck Properties</DialogTitle>
      <DialogContent>
        <DeckPropertiesLayout 
          showLands={true}
          formatFilters={formatFilterOptions} 
          deck={deckPropsFormValues}
          onChange={event => onFieldChange(event.target.name, event.target.value)}
        />
      </DialogContent>
      <DialogTitle>Advanced</DialogTitle>
      <DialogActions>
        <Button size="medium" variant="contained" color="secondary" onClick={props.onDisassembleClick}>Disassemble</Button>
        <Button size="medium" variant="contained" color="secondary" onClick={props.onDeleteClick}>Delete</Button>
      </DialogActions>
      <DialogActions>
        <Button size="medium" onClick={onCloseClick}>Cancel</Button>
        <Button size="medium" variant="contained" color="primary" onClick={onSaveClick}>Save</Button>
      </DialogActions>
    </Dialog>
  );
}