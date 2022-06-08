import React from 'react';
import {Box, Card, CardMedia} from '@material-ui/core';
import styles from '../../../../App.module.css';
import {useAppDispatch, useAppSelector} from "../../../../hooks";
import {cardSearchSelectCard} from "../inventoryAddCardsSlice";

const SearchResultGridItem = (props: {searchResultId: number}): JSX.Element => {
  const searchResult = useAppSelector(state => state.inventory.inventoryAddCards.searchResults.searchResultsById[props.searchResultId]);
  
  const dispatch = useAppDispatch();
  const onCardSelected = (card: CardSearchResultDto): void =>
    dispatch(cardSearchSelectCard(card));
  
  return (
    <Card
      key={searchResult.name}
      className={styles.outlineSection}
      onClick={() => onCardSelected(searchResult)}>
      <CardMedia
        style={{height:"310px", width: "223px"}}
        image={searchResult.details[0].imageUrl}
        title={searchResult.name} />
    </Card>
  )
}

export const SearchResultGrid = (): JSX.Element => {
  const searchResultIds = useAppSelector(state => state.inventory.inventoryAddCards.searchResults.allSearchResultIds);
  return (
    <Box className={styles.flexRowWrap}>
      { searchResultIds.map((searchResultId) => 
        <SearchResultGridItem key={searchResultId} searchResultId={searchResultId} />
      )}
    </Box>
  );
}