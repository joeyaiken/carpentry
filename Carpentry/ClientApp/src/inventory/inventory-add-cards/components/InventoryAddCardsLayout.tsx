import { AppBar, Box, Button, Paper, Tab, Tabs, Toolbar, Typography } from '@material-ui/core';
import React from 'react';
import { appStyles, combineStyles } from '../../../styles/appStyles';
import FilterBar from './FilterBar';
import PendingCardsSection from './PendingCardsSection';
import SearchResultGrid from './SearchResultGrid';
import SearchResultTable from './SearchResultTable';
import SelectedCardSection from './SelectedCardSection';
import AppLayout from "../../../common/components/AppLayout";

interface ComponentProps {
  filterOptions: AppFiltersDto;
  cardSearchMethod: "set" | "web" | "inventory"; //TODO - make / use enum
  searchFilterProps: CardFilterProps;
  viewMode: CardSearchViewMode;
  selectedCard: CardSearchResultDto | null;
  searchResults: CardListItem[];
  pendingCards: { [key:number]: PendingCardsDto }

  isLoading: boolean;

  handleCancelClick: () => void;
  handleSaveClick: () => void;
  handleSearchMethodTabClick: (name: string) => void;
  handleToggleViewClick: () => void;
  handleBoolFilterChange: (filter: string, value: boolean) => void;
  handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  handleSearchButtonClick: () => void;
  handleAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
  handleRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;
  handleCardSelected: (item: CardListItem) => void;
}

export default function InventoryAddCardsLayout(props: ComponentProps): JSX.Element {
  const {  flexRow, outlineSection, flexSection } = appStyles();
  return(
    <React.Fragment>
      <AppLayout title="Inventory" isLoading={props.isLoading}>
        {/*Loading bar can go here...*/}
        {/*See how settings is handling isLoading*/}
        <AppBar color="default" position="relative">
          <Toolbar>
            <Typography variant="h6">
              Card Search
            </Typography>
            <Tabs value={props.cardSearchMethod} onChange={(e, value) => {props.handleSearchMethodTabClick(value)}} >
              <Tab value="set" label="Set" />
              <Tab value="web" label="Web" />
              <Tab value="inventory" label="Inventory" />
            </Tabs>
            <Button onClick={props.handleToggleViewClick} color="primary" variant="contained">
              Toggle View
            </Button>
          </Toolbar>
        </AppBar>
        {/*Or maybe the loading bar should go here*/}
        <FilterBar
          filterOptions={props.filterOptions}
          handleBoolFilterChange={props.handleBoolFilterChange}
          handleFilterChange={props.handleFilterChange}
          searchFilterProps={props.searchFilterProps}
          cardSearchMethod={props.cardSearchMethod}
          handleSearchButtonClick={props.handleSearchButtonClick} />
        <Box className={combineStyles(flexRow,flexSection)} style={{ overflow:'auto', alignItems:'stretch' }}>
          <Paper id="search-results" style={{ overflow:'auto', flex:'1 1 70%' }} >
            { props.viewMode === "list" &&
            <SearchResultTable
                searchResults={props.searchResults}
                handleAddPendingCard={props.handleAddPendingCard}
                handleRemovePendingCard={props.handleRemovePendingCard}
                onCardSelected={props.handleCardSelected}
            />
            }
            { props.viewMode === "grid" &&
            <SearchResultGrid
                searchResults={props.searchResults}
                onCardSelected={props.handleCardSelected}
            />
            }
          </Paper>
          <Paper style={{ overflow:'auto', flex:'1 1 30%' }} >
            { props.selectedCard &&
            <SelectedCardSection
                selectedCard={props.selectedCard}
                pendingCards={props.pendingCards[props.selectedCard.name]}
                handleAddPendingCard={props.handleAddPendingCard}
                handleRemovePendingCard={props.handleRemovePendingCard}
                selectedCardDetail={null} />
            }
          </Paper>
        </Box>
        <PendingCardsSection pendingCards={props.pendingCards} />
        <Paper className={combineStyles(outlineSection, flexRow)}>
          <Button onClick={props.handleCancelClick}>
            Cancel
          </Button>
          <Button color="primary" variant="contained" onClick={props.handleSaveClick}>
            Save
          </Button>
        </Paper>
      </AppLayout>
    </React.Fragment>);
}