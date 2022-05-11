import React, {useEffect} from 'react';
import {
  Box,
  Button,
  FormControlLabel,
  IconButton,
  Paper,
  Switch,
  Table, TableBody, TableCell,
  TableHead, TableRow,
  Typography
} from "@material-ui/core";
import {Add, Delete, Refresh} from "@material-ui/icons";
import {AppLayout} from "../../../common/components/AppLayout";
import styles from "../../../app/App.module.css";
import {SetDetailDto} from "../../../../../../Carpentry.Angular/ClientApp/src/app/settings/models";
import {useAppSelector} from "../../../app/hooks";


const TrackedSetsRow = (props: { setDetail: SetDetailDto}): JSX.Element => {
  const setDetail = props.setDetail; // lazy

  const onAddSetClick = (setId: number): void => {

  }
  const onRemoveSetClick = (setId: number): void => {

  }
  const onUpdateSetClick = (setId: number): void => {

  }
  
  return (
    <TableRow key={setDetail.code} className="set-row">
      <TableCell>{setDetail.code}</TableCell>
      <TableCell>{setDetail.name}</TableCell>
      <TableCell>{setDetail.inventoryCount}</TableCell>
      <TableCell>{setDetail.isTracked && `${setDetail.collectedCount}/${setDetail.totalCount}`}</TableCell>
      <TableCell>{setDetail.dataLastUpdated}</TableCell>
      <TableCell>
        { !setDetail.isTracked &&
        <IconButton className="add-button" color="inherit" onClick={() => {onAddSetClick(setDetail.setId)}}>
            <Add />
        </IconButton>
        }
        { setDetail.isTracked &&
        <React.Fragment>
            <IconButton color="inherit" onClick={() => {onUpdateSetClick(setDetail.setId)}}>
                <Refresh />
            </IconButton>
            <IconButton color="inherit" onClick={() => {onRemoveSetClick(setDetail.setId)}}>
                <Delete />
            </IconButton>
        </React.Fragment>
        }
      </TableCell>
    </TableRow>
  );
}

export const TrackedSets = (): JSX.Element => {
  
  const trackedSetDetails: SetDetailDto[] = [];
  const showUntrackedValue = false;
  const isLoading = false;// useAppSelector(state => state.se)
  
  
  // useEffect - ensure loaded
  
  useEffect(() => {
    //ensure tracked sets are loaded
    //     this.props.dispatch(requestTrackedSets(this.props.showUntracked, false));
  })
  

  const onRefreshClick = (): void => {

  }
  
  const onShowUntrackedClick = (): void => {
    
  }

  return(
    <AppLayout title="Settings - Tracked Sets" isLoading={isLoading}>
      <Box>
        <Box className={styles.flexRow}>
          <Box className={styles.flexSection}>
            <Typography variant="h4">
              Tracked Sets
            </Typography>
          </Box>
          <FormControlLabel id='show-untracked-toggle'
                            onClick={onShowUntrackedClick}
                            control={
                              <Switch
                                checked={showUntrackedValue}
                                name="checkedB"
                                color="primary" />
                            }
                            label="Show Untracked"
          />
          <Button disabled={true} color="primary" variant="contained" >Update All</Button>
          <IconButton color="inherit" onClick={onRefreshClick} id="refresh-button" >
            <Refresh />
          </IconButton>
        </Box>
        <Paper>
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell>Code</TableCell>
                <TableCell>Name</TableCell>
                <TableCell>Owned</TableCell>
                <TableCell>Collected</TableCell>
                <TableCell>Last Updated</TableCell>
                <TableCell>Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {/* TODO - pass in an ID instead of a whole object*/}
              { trackedSetDetails.map(setDetail =>
                <TrackedSetsRow setDetail={setDetail} />
              )}
            </TableBody>
          </Table>
        </Paper>
      </Box>
    </AppLayout>
  );
}