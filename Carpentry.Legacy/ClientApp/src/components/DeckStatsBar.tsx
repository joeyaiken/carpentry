import React from 'react';

import { Typography, Box, Paper, TextField, TableRow, TableHead, Table, TableCell, Tab, TableBody } from '@material-ui/core';

interface ComponentProps{
    deckStats: DeckStats;
}

export default function DeckStatsBar(props: ComponentProps): JSX.Element {
    // console.log('deck stats bar')
    // console.log(props);
    return (
        <Paper className="flex-row outline-section">
            <Box className="outline-section">
                <Typography variant="h6">Card Count</Typography>
                <Typography variant="h5">{props.deckStats.totalCount}</Typography>
                {/* <TextField 
                    disabled
                    label="Card Count"
                    defaultValue={props.deckStats.totalCount}
                /> */}
            </Box>

            <Box className="outline-section">
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            {
                                Object.keys(props.deckStats.typeCounts).map((key) => <TableCell key={`stats-type-head-${key}`} size="small">{key}</TableCell>)
                            }
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        <TableRow>
                            {
                                Object.keys(props.deckStats.typeCounts).map((key) => <TableCell key={`stats-type-cell-${key}`} size="small">{props.deckStats.typeCounts[key]}</TableCell>)
                            }
                        </TableRow>
                    </TableBody>
                </Table>
                {/* {
                    Object.keys(props.deckStats.typeCounts).map((key) => {
                        return (
                            <TextField 
                                disabled
                                label={key}
                                defaultValue={props.deckStats.typeCounts[key]}
                            />
                        );
                    })
                } */}
            </Box>

            <Box className="outline-section">
                {/* <Typography>CMC Breakdown</Typography> */}
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            {
                                Object.keys(props.deckStats.costCounts).map((key) => <TableCell key={`stats-cmc-head-${key}`} size="small">{key}</TableCell>)
                            }
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        <TableRow>
                            {
                                Object.keys(props.deckStats.costCounts).map((key) => <TableCell key={`stats-cmc-cell-${key}`} size="small">{props.deckStats.costCounts[key]}</TableCell>)
                            }
                        </TableRow>
                    </TableBody>
                </Table>
                {/* {
                    

                    Object.keys(props.deckStats.costCounts).map((key) => {
                        return (
                            <TextField 
                                disabled
                                label={key}
                                defaultValue={props.deckStats.costCounts[key]}
                            />
                        );
                    })
                } */}
            </Box>

            <Box className="outline-section flex-col">
                <Typography variant="h6">Total Cost</Typography>
                <Typography variant="h5">{props.deckStats.totalCost}</Typography>
                {/* <TextField 
                    disabled
                    label="Total Cost"
                    defaultValue={props.deckStats.totalCost}
                /> */}
            </Box>
        </Paper>
    );
}