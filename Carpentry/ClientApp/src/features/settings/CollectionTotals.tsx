import React, {useEffect} from 'react';
import {Box, Paper, Table, TableBody, TableCell, TableHead, TableRow, Typography} from "@material-ui/core";
import {useAppDispatch, useAppSelector} from "../../hooks";
import {ApiStatus} from "../../enums";
import {loadCollectionTotals} from "./settingsSlice";

export const CollectionTotals = (): JSX.Element => {
  
  const shouldLoad = useAppSelector(state => 
    state.settings.collectionTotals.status === ApiStatus.uninitialized);
  
  const statusTotalIds = useAppSelector(state => state.settings.collectionTotals.allIds);
  
  const dispatch = useAppDispatch();
  useEffect(() => {
    if(shouldLoad) dispatch(loadCollectionTotals());
  })
  
  return (
    <React.Fragment>
      
      <Typography variant="h4">
        Collection Totals
      </Typography>
      
      <Box>
        <Paper>
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell>Status</TableCell>
                <TableCell>Count</TableCell>
                <TableCell>Price</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              { statusTotalIds.map(statusId =>
                <CollectionTotalsRow statusId={statusId} />)
              }
            </TableBody>
          </Table>
        </Paper>
      </Box>
      
    </React.Fragment>
  )
}

export const CollectionTotalsRow = (props: {statusId: number}): JSX.Element => {
  const statusTotal = useAppSelector(state => state.settings.collectionTotals.byId[props.statusId]);
  return (
    <TableRow>
      <TableCell>{statusTotal.statusName}</TableCell>
      <TableCell>{statusTotal.totalCount}</TableCell>
      <TableCell>${statusTotal.totalPrice}</TableCell>
    </TableRow>
  );
}