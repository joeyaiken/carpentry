import React from 'react';
import {Box,Card,CardMedia} from '@material-ui/core';
import styles from "../../../../app/App.module.css";

interface SearchResultGridProps {
  searchResults: CardListItem[];
  onCardSelected: (item: CardListItem) => void;
}

export default function SearchResultGrid(props: SearchResultGridProps): JSX.Element {
  return (
    <Box className={styles.flexRowWrap}>
      {props.searchResults.map((card) => (
        <Card
          key={card.data.name}
          className={styles.outlineSection}
          onClick={() => props.onCardSelected(card)}
        >
          <CardMedia
            style={{height:"310px", width: "223px"}}
            image={card.data.details[0].imageUrl}
            title={card.data.name} />
        </Card>
      ))}
    </Box>
  );
}