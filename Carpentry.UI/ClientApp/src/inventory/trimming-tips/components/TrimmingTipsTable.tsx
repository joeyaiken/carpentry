//table of deck cards
import React from 'react';

import {  Paper, Table, TableHead, TableRow, TableCell, TableBody } from '@material-ui/core';

interface ComponentProps{
    //totalPrice: number;
    // deckProperties: DeckProperties;
    // onEditClick: () => void;
    // cardOverviews: DeckCardOverview[];
    // onCardSelected: (card: DeckCardOverview) => void;
    trimmingTips: InventoryOverviewDto[];
}

export default function TrimmingTipsTable(props: ComponentProps): JSX.Element {
    return (
        <React.Fragment>
            <Table size="small">
                <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        <TableCell>Set</TableCell>
                        <TableCell>Variant</TableCell>
                        <TableCell># to Trim</TableCell>
                        <TableCell>Reason to Trim</TableCell>
                        <TableCell>{/*Options*/}</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {
                        // Table Row:
                        // onClick={() => props.onCardSelected(cardItem)} onMouseEnter={() => props.onCardSelected(cardItem)}
                        props.trimmingTips.map(cardItem => 
                            <TableRow 
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
        </React.Fragment>
    );
}