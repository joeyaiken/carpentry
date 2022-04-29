//table of deck cards
import React from 'react';
import {  Paper, Table, TableHead, TableRow, TableCell, TableBody } from '@material-ui/core';
import {useAppDispatch, useAppSelector} from "../../../../app/hooks";
import {selectOverviewCard, selectOverviewIds} from "../../deckDetailSlice";
import {deckEditorCardSelected} from "../deckEditorSlice";

export const DeckCardList = (): JSX.Element => {
  const overviewIds = useAppSelector(selectOverviewIds);
  return (
    <Paper>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Name</TableCell>
            <TableCell>Count</TableCell>
            <TableCell>Type</TableCell>
            <TableCell>Cost</TableCell>
            <TableCell>Category</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          { overviewIds.map(cardId => <DeckCardListRow cardId={cardId} />) }
        </TableBody>
      </Table>
    </Paper>
  );
}

const DeckCardListRow = (props: {cardId: number}): JSX.Element => {
  const cardItem: DeckCardOverview = useAppSelector(state => selectOverviewCard(state, props.cardId));

  const dispatch = useAppDispatch();
  const onCardSelected = (): void => {
    dispatch(deckEditorCardSelected(cardItem))
  }

  return (
    <TableRow onClick={() => onCardSelected()} onMouseEnter={() => onCardSelected()}
              key={cardItem.id+cardItem.name}>
      <TableCell>{cardItem.name}</TableCell>
      <TableCell>{cardItem.count}</TableCell>
      <TableCell>{cardItem.type}</TableCell>
      <TableCell>{cardItem.cost}</TableCell>
      <TableCell>{cardItem.category}</TableCell>
    </TableRow>
  );
};