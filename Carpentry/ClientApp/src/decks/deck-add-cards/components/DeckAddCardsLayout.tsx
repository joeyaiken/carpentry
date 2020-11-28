import { AppBar, Box, Button, Paper, Tab, Tabs, Toolbar, Typography } from '@material-ui/core';
import React from 'react';
import { appStyles, combineStyles } from '../../../styles/appStyles';
import SelectedCardSection from './SelectedCardSection';
import FilterBar from './FilterBar';
// import PendingCardsSection from './PendingCardsSection';
import SearchResultGrid from './SearchResultGrid';
import SearchResultTable from './SearchResultTable';

interface ContainerLayoutProps {
    filterOptions: AppFiltersDto;
    cardSearchMethod: "set" | "web" | "inventory"; //TODO - make / use enum
    searchFilterProps: CardFilterProps;
    viewMode: CardSearchViewMode;
    selectedCard: CardSearchResultDto | null;
    selectedCardDetail: InventoryDetailDto | null;
    searchResults: CardListItem[];

    handleCancelClick: () => void;
    // handleSaveClick: () => void;
    handleSearchMethodTabClick: (name: string) => void;
    handleToggleViewClick: () => void;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleSearchButtonClick: () => void;
    handleCardSelected: (item: CardListItem) => void;
    handleAddNewCardClick: (cardId: number, isFoil: boolean) => void;
    handleAddExistingCardClick: (inventoryCard: InventoryCard) => void;

}

export default function DeckAddCardsLayout(props: ContainerLayoutProps): JSX.Element {
    const {  flexRow, outlineSection, flexSection } = appStyles();
    return(
    <React.Fragment>
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
        <FilterBar 
            filterOptions={props.filterOptions}
            handleBoolFilterChange={props.handleBoolFilterChange}
            handleFilterChange={props.handleFilterChange}
            searchFilterProps={props.searchFilterProps}
            cardSearchMethod={props.cardSearchMethod}
            handleSearchButtonClick={props.handleSearchButtonClick} />

        <Box className={combineStyles(flexRow,flexSection)} style={{ overflow:'auto', alignItems:'stretch' }}>
            <Paper style={{ overflow:'auto', flex:'1 1 70%' }} >
            {
                props.viewMode === "list" && 
                    <SearchResultTable searchResults={props.searchResults} onCardSelected={props.handleCardSelected} />
            }
            {
                props.viewMode === "grid" &&
                    <SearchResultGrid searchResults={props.searchResults} onCardSelected={props.handleCardSelected} />
            }
            </Paper>
            <Paper style={{ overflow:'auto', flex:'1 1 30%' }} > 
            { props.selectedCard && 
                <SelectedCardSection 
                    selectedCard={props.selectedCard}
                    selectedCardDetail={props.selectedCardDetail}
                    handleAddInventoryCard={props.handleAddExistingCardClick}
                    handleAddNewCard={props.handleAddNewCardClick} />
            }
            </Paper>
        </Box>
        <Paper className={combineStyles(outlineSection, flexRow)}>
            <Button onClick={props.handleCancelClick}>
                Close
            </Button>
            {/* <Button color="primary" variant="contained" onClick={props.handleSaveClick}>
                Save
            </Button> */}
        </Paper>
    </React.Fragment>);
}