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
    Table,
    TableHead,
    TableRow,
    TableCell,
    TableBody,
    IconButton,
} from '@material-ui/core';

import { Star } from '@material-ui/icons';

interface SearchResultTableProps {
    searchContext: "deck" | "inventory";
    searchResults: CardListItem[];
    handleAddPendingCard: (data: MagicCard, isFoil: boolean, variant: string) => void;
    handleRemovePendingCard: (multiverseId: number, isFoil: boolean, variant: string) => void;
    onCardSelected: (item: CardListItem) => void;
}

export default function SearchResultTable(props: SearchResultTableProps): JSX.Element {

    return (
        <Paper className="flex-section">
            <Table size="small">
                <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        {
                            props.searchContext === "inventory" &&
                            (   <>
                                    <TableCell># Pending</TableCell>
                                    <TableCell>Actions</TableCell>
                                </>
                            )
                        }
                        {
                            props.searchContext === "deck" &&
                            (   <>
                                    {/* <TableCell>Set</TableCell> */}
                                    <TableCell>Type</TableCell>
                                    <TableCell>Cost</TableCell>
                                    <TableCell></TableCell>
                                </>
                            )
                        }
                    </TableRow>
                </TableHead>
                <TableBody>
                    {
                        props.searchResults.map(result => (
                            <TableRow 
                                onClick={() => { props.onCardSelected(result) }}
                                key={result.data.multiverseId}>
                                <TableCell>{result.data.name}</TableCell>
                                {
                                        props.searchContext === "inventory" &&
                                        (   <>
                                                <TableCell>{result.count}</TableCell>
                                                <TableCell>
                                                    <Box className="flex-row">
                                                        <Button variant="contained" size="small" onClick={() => {props.handleRemovePendingCard(result.data.multiverseId, false, "normal")} } >-</Button>
                                                        {/* <Typography>({result.count})</Typography> */}
                                                        <Button variant="contained" size="small" onClick={() => {props.handleAddPendingCard(result.data, false, "normal")} } >+</Button>       
                                                    </Box>
                                                </TableCell>
                                            </>
                                        )
                                    }
                                    {
                                        props.searchContext === "deck" &&
                                        (   <>
                                                {/* <TableCell>{result.data.set}</TableCell> */}
                                                <TableCell>{result.data.type}</TableCell>
                                                <TableCell>{result.data.manaCost}</TableCell>
                                                <TableCell>{Boolean(result.count) && 
                                                    <IconButton color="inherit" disabled={true} size="small">
                                                        <Star />
                                                    </IconButton> 
                                                }</TableCell>
                                            </>
                                        )
                                    }                                            
                            </TableRow>
                        ))
                    }
                </TableBody>
            </Table>
        </Paper>
    );
}