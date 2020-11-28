import React from 'react';

import {
    Table,
    TableHead,
    TableRow,
    TableCell,
    TableBody,
    IconButton,
} from '@material-ui/core';

import { Star } from '@material-ui/icons';

interface SearchResultTableProps {
    searchResults: CardListItem[];
    onCardSelected: (item: CardListItem) => void;
}

export default function SearchResultTable(props: SearchResultTableProps): JSX.Element {
    return (
        <Table size="small">
            <TableHead>
                <TableRow>
                    <TableCell>Name</TableCell>
                    {/* <TableCell>Set</TableCell> */}
                    <TableCell>Type</TableCell>
                    <TableCell>Cost</TableCell>
                    <TableCell></TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
            {
                props.searchResults.map(result => (
                    <TableRow onClick={() => { props.onCardSelected(result) }} key={result.data.cardId}>
                        <TableCell>{result.data.name}</TableCell>
                        {/* <TableCell>{result.data.set}</TableCell> */}
                        <TableCell>{result.data.type}</TableCell>
                        <TableCell>{result.data.manaCost}</TableCell>
                        <TableCell>
                            {
                                Boolean(result.count) && 
                                <IconButton color="inherit" disabled={true} size="small">
                                    <Star />
                                </IconButton> 
                            }
                        </TableCell>       
                    </TableRow>
                ))
            }
            </TableBody>
        </Table>
    );
}