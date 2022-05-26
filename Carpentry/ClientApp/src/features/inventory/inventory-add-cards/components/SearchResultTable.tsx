import React from 'react';
import {
  Button,
  Box,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
} from '@material-ui/core';
import styles from '../../../../app/App.module.css';
import {useAppDispatch, useAppSelector} from "../../../../app/hooks";
import {
  addPendingCard,
  cardSearchSelectCard,
  removePendingCard,
  selectSearchResultItem
} from "../inventoryAddCardsSlice";

const SearchResultTableRow = (props: {cardId: number}): JSX.Element => {
  const dispatch = useAppDispatch();

  const handleAddPendingCard = (name: string, cardId: number, isFoil: boolean): void =>
    dispatch(addPendingCard({name: name, cardId: cardId, isFoil: isFoil}));

  const handleRemovePendingCard = (name: string, cardId: number, isFoil: boolean): void =>
    dispatch(removePendingCard({name: name, cardId: cardId, isFoil: isFoil}));

  const onCardSelected = (item: CardListItem): void =>
    dispatch(cardSearchSelectCard(item.data));
  
  const result = useAppSelector(state => selectSearchResultItem(state, props.cardId)) 

  return (
    <TableRow onClick={() => { onCardSelected(result) }} key={result.data.cardId}>
      <TableCell>{result.data.name}</TableCell>
      <TableCell>{result.count}</TableCell>
      <TableCell>
        <Box className={styles.flexRow}>
          <Button className="quick-remove-button" variant="contained" size="small" onClick={() => {handleRemovePendingCard(result.data.name, result.data.cardId, false)} } >-</Button>
          <Button className="quick-add-button" variant="contained" size="small" onClick={() => {handleAddPendingCard(result.data.name, result.data.cardId, false)} } >+</Button>
        </Box>
      </TableCell>
    </TableRow>
  )
}

export const SearchResultTable = (): JSX.Element => {
  const searchResultIds = useAppSelector(state => 
    state.inventory.inventoryAddCards.searchResults.allSearchResultIds);
  
  return (
    <Table size="small">
      <TableHead>
        <TableRow>
          <TableCell>Name</TableCell>
          <TableCell># Pending</TableCell>
          <TableCell>Actions</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        { searchResultIds.map(cardId => <SearchResultTableRow key={cardId} cardId={cardId} /> ) }
      </TableBody>
    </Table>
  );
}