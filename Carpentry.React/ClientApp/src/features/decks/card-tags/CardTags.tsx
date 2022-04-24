import React, {ChangeEvent} from "react";
import {Box, Button, IconButton, TextField, Typography} from "@material-ui/core";
import {Add, Close} from "@material-ui/icons";
import styles from "../../../app/App.module.css";
import {requestAddCardTag, requestRemoveCardTag} from "./state/CardTagsActions";
import {useAppDispatch, useAppSelector} from "../../../app/hooks"; 

export const CardTags = (): JSX.Element => {

  //selectors
  const selectedDeckId = useAppSelector(state => state.decks.data.detail.deckId);
  const selectedCardId = useAppSelector(state => state.decks.cardTags.cardId);
  const selectedCardName = useAppSelector(state => state.decks.cardTags.cardName);
  const newTagName = useAppSelector(state => state.decks.cardTags.newTagName);
  const existingTags = useAppSelector(state => state.decks.cardTags.existingTags);
  const tagSuggestions = useAppSelector(state => state.decks.cardTags.tagSuggestions);
  
  //actions
  const dispatch = useAppDispatch();
  
  const onAddSuggestionClick = (tag: string): void => {
    const dto: CardTagDto = {
      cardName: selectedCardName,
      deckId: selectedDeckId,
      tag: tag,
    };
    dispatch(requestAddCardTag(dto));
  }
  
  const onRemoveTagClick = (cardTagId: number): void => {
    dispatch(requestRemoveCardTag(cardTagId));
  }
  
  
  return (
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
            return(<Box className={styles.outlineSection}>{tag.tag}
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
            return(<Box className={styles.outlineSection}>{tag}

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

    </React.Fragment>
  )
}