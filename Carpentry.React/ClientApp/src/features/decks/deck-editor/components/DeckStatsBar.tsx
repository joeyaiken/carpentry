import React from 'react';

import { Typography, Box, Paper, TableRow, TableHead, Table, TableCell, TableBody } from '@material-ui/core';
import { combineStyles, appStyles } from '../../../styles/appStyles';

interface ComponentProps{
    deckStats: DeckStats;
}

export default function DeckStatsBar(props: ComponentProps): JSX.Element {
  const { outlineSection, flexCol, flexRow, } = appStyles();
  
  return (
    <Paper className={combineStyles(flexRow, outlineSection)}>
      <Box className={outlineSection}>
        <Typography variant="h6">Card Count</Typography>
        <Typography variant="h5" id="deck-stats-count">{props.deckStats.totalCount}</Typography>
      </Box>

      <Box className={outlineSection}>
        <Table size="small">
          <TableHead>
            <TableRow>
              {
                Object.keys(props.deckStats.typeCounts).map((key) => 
                  <TableCell key={`stats-type-head-${key}`} className="stats-type-head" size="small">{key}</TableCell>
                )
              }
            </TableRow>
          </TableHead>
          <TableBody>
            <TableRow>
              {
                Object.keys(props.deckStats.typeCounts).map((key) => 
                  <TableCell key={`stats-type-cell-${key}`} className="stats-type-cell" size="small">{props.deckStats.typeCounts[key]}</TableCell>
                )
              }
            </TableRow>
          </TableBody>
        </Table>
      </Box>

      <Box className={outlineSection}>
        <Table size="small">
          <TableHead>
            <TableRow>
              {
                Object.keys(props.deckStats.costCounts).map((key) => 
                  <TableCell key={`stats-cmc-head-${key}`} className="stats-cmc-head" size="small">{key}</TableCell>
                )
              }
            </TableRow>
          </TableHead>
          <TableBody>
            <TableRow>
              {
                Object.keys(props.deckStats.costCounts).map((key) => 
                  <TableCell key={`stats-cmc-cell-${key}`} className="stats-cmc-cell" size="small">{props.deckStats.costCounts[key]}</TableCell>
                )
              }
            </TableRow>
          </TableBody>
        </Table>
      </Box>

      <Box className={outlineSection}>
        <Table size="small">
          <TableHead>
            <TableRow>
              {
                Object.keys(props.deckStats.tagCounts).map((key) => <TableCell key={`stats-type-head-${key}`} size="small">{key}</TableCell>)
              }
            </TableRow>
          </TableHead>
          <TableBody>
            <TableRow>
              {
                Object.keys(props.deckStats.tagCounts).map((key) => <TableCell key={`stats-type-cell-${key}`} size="small">{props.deckStats.tagCounts[key]}</TableCell>)
              }
            </TableRow>
          </TableBody>
        </Table>
      </Box>
      <Box className={combineStyles(outlineSection, flexCol)}>
        <Typography variant="h6">Total Cost</Typography>
        <Typography variant="h5">{props.deckStats.totalCost}</Typography>
      </Box>
    </Paper>
  );
}