import React from 'react';

// import { 
//     cardSearchSearchMethodChanged,
//     cardSearchClearPendingCards,
//     toggleCardSearchViewMode,
// } from '../actions/cardSearch.actions';

// import CardSearchPendingCards from './CardSearchPendingCards'
// import { 
//     requestAddCardsFromSearch
// } from '../actions/inventory.actions';

import {
    Button,
    AppBar,
    Toolbar,
    Typography,
    Paper,
    Box,
    Tabs,
    Tab,
    Card,
    CardMedia,
} from '@material-ui/core';
import { appStyles } from '../../../styles/appStyles';

interface SearchResultGridProps {
    searchResults: CardListItem[];
    onCardSelected: (item: CardListItem) => void;
}

export default function SearchResultGrid(props: SearchResultGridProps): JSX.Element {
    // <VisualCard key={card.data.name} cardOverview={card} onCardSelected={() => {props.onCardSelected(card)}} />
    const { outlineSection, flexRowWrap, } = appStyles();
    return (
        <Box className={flexRowWrap}>
            {props.searchResults.map((card) => (            
                <Card 
                    key={card.data.name} 
                    className={outlineSection}
                    onClick={() => props.onCardSelected(card)}
                    >
                    {/* <CardHeader titleTypographyProps={{variant:"body1"}} title={ `${card.data.name} - (${props.cardOverview.count})` } /> */}
                    {/* <CardHeader titleTypographyProps={{variant:"body1"}} title={`${card.name} (${card.count})`}/> */}
                    <CardMedia 
                        style={{height:"310px", width: "223px"}}
                        // className={itemImage}
                        //image={card.data.variants['normal'] || ''}
                        image={card.data.details[0].imageUrl}
                        title={card.data.name} />
                    {/* {props.children} */}

                </Card>
            ))}
        </Box>
    );
}