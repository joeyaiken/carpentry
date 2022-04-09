import { AppBar, Box, Button, Paper, Toolbar, Typography } from '@material-ui/core';
import React from 'react';
import { appStyles, combineStyles } from '../../../styles/appStyles';
import SelectedCardSection from './SelectedCardSection';
import FilterBar from './FilterBar';
import SearchResultGrid from './SearchResultGrid';
import SearchResultTable from './SearchResultTable';
import AppLayout from "../../../common/components/AppLayout";

interface ContainerLayoutProps {
  filterOptions: AppFiltersDto;
  searchFilterProps: CardFilterProps;
  viewMode: CardSearchViewMode;
  selectedCard: CardSearchResultDto | null;

  // selectedCardDetail: InventoryDetailDto | null;
  inventoryCardsById: { [id: number]: InventoryCard };
  inventoryCardsAllIds: number[];

  searchResults: CardListItem[];

  isLoading: boolean;

  handleCloseClick: () => void;
  handleToggleViewClick: () => void;
  handleBoolFilterChange: (filter: string, value: boolean) => void;
  handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  handleSearchButtonClick: () => void;
  handleCardSelected: (item: CardListItem) => void;
  handleAddNewCardClick: (cardName: string, cardId: number, isFoil: boolean) => void;
  handleAddExistingCardClick: (inventoryCard: InventoryCard) => void;
  handleAddEmptyCard: (cardName: string) => void;

}

export default function DeckAddCardsLayout(props: ContainerLayoutProps): JSX.Element {
  const {  flexRow, outlineSection, flexSection } = appStyles();
  // console.log("props.selectedCardDetail");
  // console.log(props.selectedCardDetail);


  return(
    <AppLayout title="Decks" isLoading={props.isLoading}>
      <AppBar color="default" position="relative" >
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

      <Box className={combineStyles(flexRow,flexSection)} style={{ overflow:'auto', alignItems:'stretch' }}>
        <Paper style={{ overflow:'auto', flex:'1 1 70%' }} >
          { props.viewMode === "list" && <SearchResultTable searchResults={props.searchResults} onCardSelected={props.handleCardSelected} /> }
          { props.viewMode === "grid" && <SearchResultGrid searchResults={props.searchResults} onCardSelected={props.handleCardSelected} /> }
        </Paper>
        <Paper style={{ overflow:'auto', flex:'1 1 30%' }} >
          { props.selectedCard &&
          <SelectedCardSection
              selectedCard={props.selectedCard}
            // selectedCardDetail={props.selectedCardDetail}
              inventoryCardsById={props.inventoryCardsById}
              inventoryCardsAllIds={props.inventoryCardsAllIds}
              handleAddInventoryCard={props.handleAddExistingCardClick}
              handleAddNewCard={props.handleAddNewCardClick}
              handleAddEmptyCard={props.handleAddEmptyCard} />
          }
        </Paper>
      </Box>
      <Paper className={combineStyles(outlineSection, flexRow)}>
        <Button onClick={props.handleCloseClick} id="close-button">
          Close
        </Button>
      </Paper>
    </AppLayout>);
}