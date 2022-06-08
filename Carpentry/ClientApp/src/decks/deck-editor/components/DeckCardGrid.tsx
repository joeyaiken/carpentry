import React from 'react';
import {Box} from '@material-ui/core';
import {deckEditorCardSelected} from "../../../features/decks/deck-editor/deckEditorSlice";
import {useAppDispatch, useAppSelector} from "../../../hooks";
import styles from "../../../App.module.css";
import {selectOverviewCard, selectOverviewIds} from "../deckDetailSlice";
import {VisualCard} from "../../../features/common/components/VisualCard";

const DeckCardGridCell = (props: {cardId: number}): JSX.Element => {
  const card = useAppSelector(state => selectOverviewCard(state, props.cardId));

  const dispatch = useAppDispatch();
  const onCardSelected = (): void => {
    dispatch(deckEditorCardSelected(card))
  }

  return (
    <VisualCard key={card.name} cardOverview={card} onCardSelected={() => {onCardSelected()}} />
  );
}

export const DeckCardGrid = (): JSX.Element => {
  const overviewIds = useAppSelector(selectOverviewIds);
  
  return (
    <React.Fragment>
      <Box className={styles.flexRowWrap}>
        {overviewIds.map((cardId) => (
          <DeckCardGridCell cardId={cardId} />
        ))}
      </Box>
    </React.Fragment>
  );
}