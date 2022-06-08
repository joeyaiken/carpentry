import React, {ChangeEvent, useEffect, useState} from 'react'
import {Box, Button, IconButton, TextField, Typography} from "@material-ui/core";
import styles from "../../../App.module.css";
import {Add, Close} from "@material-ui/icons";
import {useAppDispatch, useAppSelector} from "../../../hooks";
import {ApiStatus} from "../../../enums";
import {addCardTag, loadTagDetail, removeCardTag} from "./cardTagsSlice";
import {getSelectedDeckId} from "../../../decks/deck-editor/deckDetailSlice";

export const CardTags = (props: {selectedCardId: number}): JSX.Element => {
  const selectedCardName = useAppSelector(state => state.decks.cardTags.data.cardName);
  const existingTags = useAppSelector(state => state.decks.cardTags.data.existingTags);
  const tagSuggestions = useAppSelector(state => state.decks.cardTags.data.tagSuggestions);

  // const selectedDeckId = useAppSelector(state => state.decks.data.detail.deckId);
  const selectedDeckId = useAppSelector(getSelectedDeckId);
  
  const shouldLoad = useAppSelector(state => {
    const status = state.decks.cardTags.data.status;

    if (!props.selectedCardId) return false;

    if (status === ApiStatus.uninitialized) return true;

    if (status === ApiStatus.initialized && state.decks.cardTags.cardId !== props.selectedCardId) return true;

    return false;
  });

  const canSave = useAppSelector(state =>
    state.decks.cardTags.data.status === ApiStatus.initialized);

  const [newTagName, onNewTagChange] = useState('');

  const dispatch = useAppDispatch();

  const onAddTagButtonClick = (): void => {
    if (!canSave) return;
    const dto: CardTagDto = {
      cardName: selectedCardName,
      deckId: selectedDeckId,
      tag: newTagName,
    }
    dispatch(addCardTag(dto));
  }

  const onAddSuggestionClick = (tag: string): void => {
    if (!canSave) return;
    const dto: CardTagDto = {
      cardName: selectedCardName,
      deckId: selectedDeckId,
      tag: tag,
    };
    dispatch(addCardTag(dto));
  };

  const onRemoveTagClick = (cardTagId: number): void => {
    if (!canSave) return;
    dispatch(removeCardTag(cardTagId));
  }

  useEffect(() => {
    if(shouldLoad) dispatch(loadTagDetail({deckId: selectedDeckId, cardId: props.selectedCardId}));
  })

  return(
    <React.Fragment>
      <Box className={styles.outlineSection}>
        <Typography>{selectedCardName}</Typography>
      </Box>

      <Box className={styles.outlineSection}>
        <Typography>Existing Tags</Typography>
        { !Boolean(existingTags) && <Box className={styles.outlineSection}>no existing tags</Box> }
        {
          Boolean(existingTags) &&
          existingTags.map(tag => {
            return(<Box key={tag.cardTagId} className={styles.outlineSection}>{tag.tag}
              <IconButton color="inherit" size="small" onClick={()=> onRemoveTagClick(tag.cardTagId) }>
                <Close />
              </IconButton>
            </Box>)
          })
        }
      </Box>

      <Box className={styles.outlineSection}>
        <Typography>Tag Suggestions</Typography>
        { !Boolean(tagSuggestions) && <Box className={styles.outlineSection}>no tag suggestions</Box> }
        {
          Boolean(tagSuggestions) &&
          tagSuggestions.map(tag => {
            return(<Box key={tag} className={styles.outlineSection}>{tag}
              <IconButton color="inherit" size="small" onClick={()=> onAddSuggestionClick(tag) }>
                <Add />
              </IconButton>
            </Box>)
          })
        }
      </Box>

      <Box className={styles.outlineSection}>
        <TextField
          name="New Tag"
          label="New Tag"
          value={newTagName}
          onChange={(event: ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => onNewTagChange(event.target.value)} />
        <Button color={"primary"} variant={"contained"} onClick={onAddTagButtonClick}>
          Add
        </Button>
      </Box>
    </React.Fragment>);
}