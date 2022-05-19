import React, {useEffect} from 'react';
import {
  Box,
  Button,
  FormControlLabel,
  IconButton,
  Paper,
  Switch,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Typography
} from "@material-ui/core";
import {Add, Delete, Refresh} from "@material-ui/icons";
import {AppLayout} from "../../common/components/AppLayout";
import styles from "../../app/App.module.css";
import {useAppDispatch, useAppSelector} from "../../app/hooks";
import {loadTrackedSets, modifyTrackedSets, selectSettingsApiStatus, TrackedSetsApiAction} from "./settingsSlice";
import {ApiStatus} from "../../enums";

const TrackedSetsRow = (props: { setId: number}): JSX.Element => {
  const setDetail = useAppSelector(state => state.settings.trackedSets.byId[props.setId]);
  const showUntracked = useAppSelector(state => state.settings.showUntrackedSets);
  const dispatch = useAppDispatch();
  
  const onAddSetClick = (setId: number): void => 
    dispatch(modifyTrackedSets({
      action: TrackedSetsApiAction.add, 
      setId: setId, 
      showUntracked: showUntracked
    }));
  
  const onRemoveSetClick = (setId: number): void => 
    dispatch(modifyTrackedSets({
      action: TrackedSetsApiAction.update,
      setId: setId,
      showUntracked: showUntracked
    }));
  
  const onUpdateSetClick = (setId: number): void => 
    dispatch(modifyTrackedSets({
      action: TrackedSetsApiAction.remove,
      setId: setId,
      showUntracked: showUntracked
    }));
  
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
  const settingsApiStatus = useAppSelector(selectSettingsApiStatus);
  const isLoading = settingsApiStatus == ApiStatus.loading;
  
  const showUntrackedValue = useAppSelector(state => 
    state.settings.showUntrackedSets);
  
  const trackedSetIds = useAppSelector(state => 
    state.settings.trackedSets.allIds);

  const dispatch = useAppDispatch();
  useEffect(() => {
    if(settingsApiStatus == ApiStatus.uninitialized) dispatch(loadTrackedSets({showUntracked: showUntrackedValue, update: false}))
  })
  
  const onRefreshClick = (): void => {
    dispatch(loadTrackedSets({showUntracked: showUntrackedValue, update: true}));
  }
  
  const onShowUntrackedClick = (): void => {
    dispatch(loadTrackedSets({showUntracked: !showUntrackedValue, update: false}));
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
          <FormControlLabel 
            id='show-untracked-toggle'
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
              {trackedSetIds.map(id => <TrackedSetsRow key={id} setId={id} />)}
            </TableBody>
          </Table>
        </Paper>
      </Box>
    </AppLayout>
  );
}