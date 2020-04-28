//table of deck cards
import React from 'react';

import {  Paper, Table, TableHead, TableRow, TableCell, TableBody } from '@material-ui/core';

interface ComponentProps{
    //totalPrice: number;
    // deckProperties: DeckProperties;
    // onEditClick: () => void;
    cardOverviews: DeckCardOverview[];
    onCardSelected: (card: DeckCardOverview) => void;
}

export default function DeckCardList(props: ComponentProps): JSX.Element {
    // className="flex-section"
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
                    {
                        props.cardOverviews.map(cardItem => 
                            <TableRow onClick={() => props.onCardSelected(cardItem)} onMouseEnter={() => props.onCardSelected(cardItem)}
                                key={cardItem.id+cardItem.name}>
                                <TableCell>{cardItem.name}</TableCell>
                                <TableCell>{cardItem.count}</TableCell>
                                <TableCell>{cardItem.type}</TableCell>
                                <TableCell>{cardItem.cost}</TableCell>
                                <TableCell>{cardItem.category}</TableCell>
                            </TableRow>
                        )
                    }
                </TableBody>
            </Table>
        </Paper>
    );
}