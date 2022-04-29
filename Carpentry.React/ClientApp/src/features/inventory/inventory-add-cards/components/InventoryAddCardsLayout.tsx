import { AppBar, Box, Button, Paper, Tab, Tabs, Toolbar, Typography } from '@material-ui/core';
// import React from 'react';
// import { appStyles, combineStyles } from '../../../styles/appStyles';
// import FilterBar from './FilterBar';
// import PendingCardsSection from './PendingCardsSection';
// import SearchResultGrid from './SearchResultGrid';
// import SearchResultTable from './SearchResultTable';
// import SelectedCardSection from './SelectedCardSection';
// import AppLayout from "../../../common/components/AppLayout";
//
// interface ComponentProps {
//   filterOptions: AppFiltersDto;
//   searchFilterProps: CardFilterProps;
//   viewMode: CardSearchViewMode;
//   selectedCard: CardSearchResultDto | null;
//   searchResults: CardListItem[];
//   pendingCards: { [key:number]: PendingCardsDto }
//
//   isLoading: boolean;
//
//   handleCancelClick: () => void;
//   handleSaveClick: () => void;
//   handleToggleViewClick: () => void;
//   handleBoolFilterChange: (filter: string, value: boolean) => void;
//   handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
//   handleSearchButtonClick: () => void;
//   handleAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
//   handleRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;
//   handleCardSelected: (item: CardListItem) => void;
// }
//
// //TODO - Refactor out those single-use components, they should either actually be reused or merged into this file
// export default function InventoryAddCardsLayout(props: ComponentProps): JSX.Element {
//   const {  flexRow, outlineSection, flexSection } = appStyles();
//
//   function renderAppBar(): JSX.Element {
//     return (
//       <AppBar color="default" position="relative">
//         <Toolbar>
//           <Typography variant="h6">
//             Card Search
//           </Typography>
//           <Button onClick={props.handleToggleViewClick} color="primary" variant="contained">
//             Toggle View
//           </Button>
//         </Toolbar>
//       </AppBar>
//     );
//   }
//  
//   function renderFilterBar(): JSX.Element {
//     return (<FilterBar
//       filterOptions={props.filterOptions}
//       handleBoolFilterChange={props.handleBoolFilterChange}
//       handleFilterChange={props.handleFilterChange}
//       searchFilterProps={props.searchFilterProps}
//       handleSearchButtonClick={props.handleSearchButtonClick} />);
//   }
//  
//   function renderSearchResultsTable(): JSX.Element {
//     return (
//       <SearchResultTable
//         searchResults={props.searchResults}
//         handleAddPendingCard={props.handleAddPendingCard}
//         handleRemovePendingCard={props.handleRemovePendingCard}
//         onCardSelected={props.handleCardSelected}
//       />
//     );
//   }
//
//   function renderSearchResultsGrid(): JSX.Element {
//     return (
//       <SearchResultGrid
//         searchResults={props.searchResults}
//         onCardSelected={props.handleCardSelected}
//       />
//     );
//   }
//
//
//   function renderSelectedCardSection(): JSX.Element {
//     return (
//       <SelectedCardSection
//         selectedCard={props.selectedCard!}
//         pendingCards={props.pendingCards[props.selectedCard!.name]}
//         handleAddPendingCard={props.handleAddPendingCard}
//         handleRemovePendingCard={props.handleRemovePendingCard}
//         selectedCardDetail={null} />
//     );
//   }
//
//   return(
//     <React.Fragment>
//       <AppLayout title="Inventory" isLoading={props.isLoading}>
//         { renderAppBar() }
//         { renderFilterBar() }
//         <Box className={combineStyles(flexRow,flexSection)} style={{ overflow:'auto', alignItems:'stretch' }}>
//           <Paper id="search-results" style={{ overflow:'auto', flex:'1 1 70%' }} >
//             { props.viewMode === "list" && renderSearchResultsTable() }
//             { props.viewMode === "grid" && renderSearchResultsGrid() }
//           </Paper>
//           <Paper style={{ overflow:'auto', flex:'1 1 30%' }} >
//             { props.selectedCard && renderSelectedCardSection() }
//           </Paper>
//         </Box>
//         <PendingCardsSection pendingCards={props.pendingCards} />
//         <Paper className={combineStyles(outlineSection, flexRow)}>
//           <Button onClick={props.handleCancelClick}>
//             Cancel
//           </Button>
//           <Button color="primary" variant="contained" onClick={props.handleSaveClick}>
//             Save
//           </Button>
//         </Paper>
//       </AppLayout>
//     </React.Fragment>);
// }