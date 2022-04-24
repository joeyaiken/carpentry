import React from 'react';
import {Box} from '@material-ui/core';
import {VisualCard} from '../../../../common/components/VisualCard';
import styles from "../../../../app/App.module.css"
import {useAppDispatch, useAppSelector} from "../../../../app/hooks";
import {deckEditorCardSelected} from "../deckEditorSlice";
import {selectOverviewCard, selectOverviewIds} from "../../deckDetailSlice";

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
          <React.Fragment>
            <DeckCardGridCell cardId={cardId} />
          </React.Fragment>
        ))}
      </Box>
    </React.Fragment>
  );
}