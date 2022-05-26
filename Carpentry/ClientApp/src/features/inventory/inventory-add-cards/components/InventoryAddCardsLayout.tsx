import {AppBar, Box, Button, Paper, Tab, Tabs, Toolbar, Typography} from '@material-ui/core';
import React from 'react';
import styles from '../../../../app/App.module.css';
import {FilterBar} from './FilterBar';
import {PendingCardsSection} from './PendingCardsSection';
import {SearchResultGrid} from './SearchResultGrid';
import {SearchResultTable} from './SearchResultTable';
import {SelectedCardSection} from './SelectedCardSection';
import {AppLayout} from "../../../../common/components/AppLayout";

interface ComponentProps {
  filterOptions: AppFiltersDto;
  searchFilterProps: CardFilterProps;
  viewMode: CardSearchViewMode;
  selectedCard: CardSearchResultDto | null;
  searchResults: CardListItem[];
  pendingCards: { [key:number]: PendingCardsDto }

  isLoading: boolean;

  handleCancelClick: () => void;
  handleSaveClick: () => void;
  handleToggleViewClick: () => void;
  handleBoolFilterChange: (filter: string, value: boolean) => void;
  handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  handleSearchButtonClick: () => void;
  handleAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
  handleRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;
  handleCardSelected: (item: CardListItem) => void;
}

//TODO - Refactor out those single-use components, they should either actually be reused or merged into this file
export const InventoryAddCardsLayout = (props: ComponentProps): JSX.Element => {

  return(
    <React.Fragment>
      <AppLayout title="Inventory" isLoading={props.isLoading}>
        <AppBar color="default" position="relative">
          <Toolbar>
            <Typography variant="h6">
              Card Search
            </Typography>
            <Button onClick={props.handleToggleViewClick} color="primary" variant="contained">
              Toggle View
            </Button>
          </Toolbar>
        </AppBar>
        <FilterBar
          filterOptions={props.filterOptions}
          handleBoolFilterChange={props.handleBoolFilterChange}
          handleFilterChange={props.handleFilterChange}
          searchFilterProps={props.searchFilterProps}
          handleSearchButtonClick={props.handleSearchButtonClick} />
        <Box className={[styles.flexRow,styles.flexSection].join(' ')} style={{ overflow:'auto', alignItems:'stretch' }}>
          <Paper id="search-results" style={{ overflow:'auto', flex:'1 1 70%' }} >
            { props.viewMode === "list" && <SearchResultTable /> }
            { props.viewMode === "grid" && <SearchResultGrid /> }
          </Paper>
          <Paper style={{ overflow:'auto', flex:'1 1 30%' }} >
            { props.selectedCard &&
              <SelectedCardSection
                selectedCard={props.selectedCard!}
                pendingCards={props.pendingCards[props.selectedCard!.name]} />
            }
          </Paper>
        </Box>
        <PendingCardsSection />
        <Paper className={[styles.outlineSection, styles.flexRow].join(' ')}>
          <Button onClick={props.handleCancelClick}>
            Cancel
          </Button>
          <Button color="primary" variant="contained" onClick={props.handleSaveClick}>
            Save
          </Button>
        </Paper>
      </AppLayout>
    </React.Fragment>
  );
}