import {AppBar, Box, Button, Paper, Toolbar, Typography} from '@material-ui/core';
import React from 'react';
import {combineStyles} from '../../../styles/appStyles';
import {FilterBar} from './components/FilterBar';
import {PendingCardsSection} from './components/PendingCardsSection';
import {SearchResultGrid} from './components/SearchResultGrid';
import {SearchResultTable} from './components/SearchResultTable';
import SelectedCardSection from './components/SelectedCardSection';
import {clearPendingCards, savePendingCards, toggleSearchViewMode} from './inventoryAddCardsSlice';
import styles from '../../../App.module.css';
import {useAppDispatch, useAppSelector} from "../../../hooks";
import {AppLayout} from "../../common/components/AppLayout";
import {ApiStatus} from "../../../enums";

import {unwrapResult} from '@reduxjs/toolkit';
import {useHistory} from "react-router";
 
export const InventoryAddCards = (): JSX.Element => {
  const viewMode = useAppSelector(state => state.inventory.inventoryAddCards.viewMode);
  const isLoading = useAppSelector(state => state.inventory.inventoryAddCards.searchResults.status === ApiStatus.loading);
  const isSaving = useAppSelector(state => state.inventory.inventoryAddCards.pendingCards.status === ApiStatus.loading);
  
  const dispatch = useAppDispatch();
  const history = useHistory();
  
  const handleToggleViewClick = (): void =>
    dispatch(toggleSearchViewMode());

  const handleSaveClick = (): void => {
    if(isSaving) return;
    const resultAction = dispatch(savePendingCards());
    try{
      unwrapResult(resultAction);
      history.push('/inventory');
    } catch (err) { }
  }
  
  const handleCancelClick = (): void =>{
    dispatch(clearPendingCards());
    history.push('/inventory');
  } 
  
  return(
    <React.Fragment>
      <AppLayout title="Inventory" isLoading={isLoading}>
        <AppBar color="default" position="relative">
          <Toolbar>
            <Typography variant="h6">
              Card Search
            </Typography>
            <Button onClick={handleToggleViewClick} color="primary" variant="contained">
              Toggle View
            </Button>
          </Toolbar>
        </AppBar>
        <FilterBar />
        <Box className={combineStyles(styles.flexRow,styles.flexSection)} style={{ overflow:'auto', alignItems:'stretch' }}>
          <Paper id="search-results" style={{ overflow:'auto', flex:'1 1 70%' }} >
            { viewMode === "list" &&
              <SearchResultTable /> }
            { viewMode === "grid" &&
              <SearchResultGrid /> }
          </Paper>
          <Paper style={{ overflow:'auto', flex:'1 1 30%' }} >
            <SelectedCardSection />
          </Paper>
        </Box>
        <PendingCardsSection />
        <Paper className={combineStyles(styles.outlineSection, styles.flexRow)}>
          <Button onClick={handleCancelClick}>
            Cancel
          </Button>
          <Button color="primary" variant="contained" onClick={handleSaveClick}>
            Save
          </Button>
        </Paper>
      </AppLayout>
    </React.Fragment>);
}