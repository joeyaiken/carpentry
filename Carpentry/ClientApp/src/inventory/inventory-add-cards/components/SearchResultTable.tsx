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
import { appStyles } from '../../../styles/appStyles';

interface SearchResultTableProps {
    searchResults: CardListItem[];
    handleAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    handleRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    onCardSelected: (item: CardListItem) => void;
}

export default function SearchResultTable(props: SearchResultTableProps): JSX.Element {
    const { flexRow } = appStyles();
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
            {
                props.searchResults.map(result => (
                    <TableRow onClick={() => { props.onCardSelected(result) }} key={result.data.cardId}>
                        <TableCell>{result.data.name}</TableCell>
                        <TableCell>{result.count}</TableCell>
                        <TableCell>
                            <Box className={flexRow}>
                                {/* <Button variant="contained" size="small" onClick={() => {props.handleRemovePendingCard(result.data.cardId, false, "normal")} } >-</Button> */}
                                <Button className="quick-remove-button" variant="contained" size="small" onClick={() => {props.handleRemovePendingCard(result.data.name, result.data.cardId, false)} } >-</Button>
                                {/* <Typography>({result.count})</Typography> */}
                                {/* <Button variant="contained" size="small" onClick={() => {props.handleAddPendingCard(result.data, false, "normal")} } >+</Button> */}
                                <Button className="quick-add-button" variant="contained" size="small" onClick={() => {props.handleAddPendingCard(result.data.name, result.data.cardId, false)} } >+</Button>
                            </Box>
                        </TableCell>                                                
                    </TableRow>
                ))
            }
            </TableBody>
        </Table>
    );
}