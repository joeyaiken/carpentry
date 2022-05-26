import React, { } from "react";
import {useDispatch} from "react-redux";
import {push} from "react-router-redux";
import {AppLayout} from "../../common/components/AppLayout";
import {
  Typography,
  Box,
  Card,
  CardHeader,
  IconButton,
} from '@material-ui/core';
import { Add, ArrowForward, SaveAlt } from '@material-ui/icons';

import styles from './Home.module.css';
import {DeckList} from "../decks/deck-list/DeckList";

export const Home = (): JSX.Element => {
  
  const dispatch = useDispatch();
  
  const onAddClick = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void => {
    dispatch(push('add-deck'));
  }
  
  const onImportClick = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void => {
    dispatch(push('import-deck'));
  }

  const onSettingsClick = (): void => {
    dispatch(push('/settings/sets'));
  }

  const onInventoryClick = (): void => {
    dispatch(push('/inventory'));
  }
  
  return (
    <React.Fragment>
      <AppLayout title="Carpentry">
        <Box id="home-container" className={styles.containerLayout}>
          <Box className={styles.titleContainer}>
            <Typography variant="h4" id="title">
              Carpentry
            </Typography>
            <Typography variant="h6" id="subtitle">
              A deck & inventory management tool for Magic the Gathering
            </Typography>
          </Box>
      
          <Box className={styles.availableDecks}>
            <Card>
              <CardHeader
                titleTypographyProps={{variant:"h5"}}
                title={"Available Decks"}
                action={
                  <>
                    <IconButton className="add-deck-button" size="medium" onClick={onAddClick}><Add /></IconButton>
                    <IconButton className="import-deck-button" size="medium" onClick={onImportClick}><SaveAlt /></IconButton>
                  </>
                } />
              <DeckList />
            </Card>
          </Box>
      
          <Box>
            <Card>
              <CardHeader
                titleTypographyProps={{variant:"h5"}}
                title={"Settings"}
                action={
                  <IconButton size="medium" onClick={onSettingsClick}><ArrowForward /></IconButton>
                } />
            </Card>
          </Box>
      
          <Box>
            <Card>
              <CardHeader
                titleTypographyProps={{variant:"h5"}}
                title={"Inventory"}
                action={
                  <IconButton size="medium" onClick={onInventoryClick}><ArrowForward /></IconButton>
                } />
            </Card>
          </Box>
        </Box>
      </AppLayout>
    </React.Fragment>
  );
}