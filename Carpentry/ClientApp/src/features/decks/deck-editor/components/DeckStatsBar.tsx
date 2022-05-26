import React from 'react';
import { Typography, Box, Paper, TableRow, TableHead, Table, TableCell, TableBody } from '@material-ui/core';
import styles from "../../../../app/App.module.css"
import {useAppSelector} from "../../../../app/hooks";

export const DeckStatsBar = (): JSX.Element => {
  const deckStats = useAppSelector(state => state.decks.detail.deckStats);
  
  if(!deckStats)return (<React.Fragment/>);
  
  return (
    <Paper className={[styles.flexRow, styles.outlineSection].join(' ')}>
      <Box className={styles.outlineSection}>
        <Typography variant="h6">Card Count</Typography>
        <Typography variant="h5" id="deck-stats-count">{deckStats.totalCount}</Typography>
      </Box>

      <Box className={styles.outlineSection}>
        <Table size="small">
          <TableHead>
            <TableRow>
              {
                Object.keys(deckStats.typeCounts).map((key) => 
                  <TableCell key={`stats-type-head-${key}`} className="stats-type-head" size="small">{key}</TableCell>
                )
              }
            </TableRow>
          </TableHead>
          <TableBody>
            <TableRow>
              {
                Object.keys(deckStats.typeCounts).map((key) => 
                  <TableCell key={`stats-type-cell-${key}`} className="stats-type-cell" size="small">{deckStats.typeCounts[key]}</TableCell>
                )
              }
            </TableRow>
          </TableBody>
        </Table>
      </Box>

      <Box className={styles.outlineSection}>
        <Table size="small">
          <TableHead>
            <TableRow>
              {
                Object.keys(deckStats.costCounts).map((key) => 
                  <TableCell key={`stats-cmc-head-${key}`} className="stats-cmc-head" size="small">{key}</TableCell>
                )
              }
            </TableRow>
          </TableHead>
          <TableBody>
            <TableRow>
              {
                Object.keys(deckStats.costCounts).map((key) => 
                  <TableCell key={`stats-cmc-cell-${key}`} className="stats-cmc-cell" size="small">{deckStats.costCounts[key]}</TableCell>
                )
              }
            </TableRow>
          </TableBody>
        </Table>
      </Box>

      <Box className={styles.outlineSection}>
        <Table size="small">
          <TableHead>
            <TableRow>
              {
                Object.keys(deckStats.tagCounts).map((key) => <TableCell key={`stats-type-head-${key}`} size="small">{key}</TableCell>)
              }
            </TableRow>
          </TableHead>
          <TableBody>
            <TableRow>
              {
                Object.keys(deckStats.tagCounts).map((key) => <TableCell key={`stats-type-cell-${key}`} size="small">{deckStats.tagCounts[key]}</TableCell>)
              }
            </TableRow>
          </TableBody>
        </Table>
      </Box>
      <Box className={[styles.outlineSection, styles.flexCol].join(' ')}>
        <Typography variant="h6">Total Cost</Typography>
        <Typography variant="h5">{deckStats.totalCost}</Typography>
      </Box>
    </Paper>
  );
}