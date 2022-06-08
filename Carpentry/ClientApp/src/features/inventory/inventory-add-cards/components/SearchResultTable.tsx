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
import styles from '../../../../App.module.css';
import {useAppDispatch, useAppSelector} from "../../../../hooks";
import {addPendingCard, cardSearchSelectCard, removePendingCard} from "../inventoryAddCardsSlice";

const SearchResultTableRow = (props: {searchResultId: number}): JSX.Element => {
  const searchResult = useAppSelector(state => state.inventory.inventoryAddCards.searchResults.searchResultsById[props.searchResultId]);

  //Wait this works? What is the state actually using now?......
  
  // This should be breaking right?....
  const pendingCount = useAppSelector(state => 
    state.inventory.inventoryAddCards.pendingCards.byName[searchResult.name] 
    && state.inventory.inventoryAddCards.pendingCards.byName[searchResult.name].cards.length);

  const dispatch = useAppDispatch();
  const onCardSelected = (card: CardSearchResultDto): void =>
    dispatch(cardSearchSelectCard(card));

  const handleAddPendingCard = (name: string, cardId: number, isFoil: boolean): void => 
    dispatch(addPendingCard({name: name, cardId: cardId, isFoil: isFoil}));

  const handleRemovePendingCard = (name: string, cardId: number, isFoil: boolean): void => 
    dispatch(removePendingCard({name: name, cardId: cardId, isFoil: isFoil}));

  return(
    <TableRow onClick={() => { onCardSelected(searchResult) }} key={searchResult.cardId}>
      <TableCell>{searchResult.name}</TableCell>
      <TableCell>{pendingCount}</TableCell>
      <TableCell>
        <Box className={styles.flexRow}>
          <Button className="quick-remove-button" variant="contained" size="small" onClick={() => {handleRemovePendingCard(searchResult.name, searchResult.cardId, false)} } >-</Button>
          <Button className="quick-add-button" variant="contained" size="small" onClick={() => {handleAddPendingCard(searchResult.name, searchResult.cardId, false)} } >+</Button>
        </Box>
      </TableCell>
    </TableRow>
  )
}

export const SearchResultTable = (): JSX.Element => {
  const searchResultIds = useAppSelector(state => state.inventory.inventoryAddCards.searchResults.allSearchResultIds);
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
        { searchResultIds.map(searchResultId => 
          <SearchResultTableRow key={searchResultId} searchResultId={searchResultId} />
        )}
      </TableBody>
    </Table>
  );
}
