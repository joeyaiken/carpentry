import React from 'react';
import {
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  Paper,
  Box,
  Button,
  TextField,
  MenuItem,
  Typography,
  IconButton,
} from '@material-ui/core';
import { appStyles, combineStyles } from '../../styles/appStyles';
import NumericFilter from '../../common/components/NumericFilter';
import { InfoOutlined, KeyboardArrowLeft, KeyboardArrowRight, Delete, CheckCircleOutline } from '@material-ui/icons';

interface ComponentProps {
  searchFilters: TrimmingToolRequest;
  filterOptions: AppFiltersDto;
  onFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onSearchClick: () => void;

  searchResultsById: { [key: number]: TrimmingToolResult }
  searchResultIds: number[];
  pendingCardsById: { [id: number]: TrimmedCard }
  
  onAddPendingCard: (card: TrimmingToolResult, count: number) => void;
  onRemovePendingCard: (card: TrimmingToolResult) => void;

  onInfoButtonEnter: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  onInfoButtonLeave: () => void;
}

export default function TrimmingToolLayout(props: ComponentProps): JSX.Element {
  const {flexRow, flexSection, outlineSection} = appStyles();
  return (
    <Box>
      <FilterBar
        searchFilters={props.searchFilters}
        filterOptions={props.filterOptions}
        onFilterChange={props.onFilterChange}
        onSearchClick={props.onSearchClick}
      />
      <Paper 
        className={combineStyles(outlineSection, flexRow, flexSection)}
        style={{ overflow:'auto', alignItems:'stretch' }}>
        <SearchResultTable
          searchResultsById={props.searchResultsById}
          searchResultIds={props.searchResultIds}
          pendingCardsById={props.pendingCardsById}
          onAddPendingCard={props.onAddPendingCard}
          onRemovePendingCard={props.onRemovePendingCard}
          onInfoButtonEnter={props.onInfoButtonEnter}
          onInfoButtonLeave={props.onInfoButtonLeave} />
      </Paper>
      <PendingCardsSection pendingCardsById={props.pendingCardsById} />
    </Box>
  );
}

interface FilterBarProps {
  searchFilters: TrimmingToolRequest;
  filterOptions: AppFiltersDto;
  onFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  onSearchClick: () => void;
}

function FilterBar(props: FilterBarProps): JSX.Element{
  const {  flexRow, outlineSection, staticSection, center, sidePadded,flexSection, stretch } = appStyles();
  return(<Box>
    <Paper className={combineStyles(outlineSection, flexRow)}>

      <Box className={combineStyles(flexSection, flexRow)}>
        {/* SET filter */}
        <Box className={`${flexSection} ${sidePadded}`}>
          <TextField
            name="setCode"
            className={stretch}
            select
            label="Set filter"
            value={props.searchFilters.setCode}
            onChange={props.onFilterChange}
            margin="normal" >
            <MenuItem key="null" value=""></MenuItem>
            { props.filterOptions.sets.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
          </TextField>
        </Box>

        {/* Group filter */}
        <Box className={`${flexSection} ${sidePadded}`}>
          <TextField
            name="searchGroup"
            className={stretch}
            select
            SelectProps={{
              displayEmpty: true
            }}
            label="Group"
            value={props.searchFilters.searchGroup}
            onChange={props.onFilterChange}
            margin="normal">
            <MenuItem key="null" value=""></MenuItem>
            { props.filterOptions.searchGroups.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
          </TextField>
        </Box>

        {/* Min # */}
        <NumericFilter
          id="min-count"
          name="minCount"
          // value={props.searchFilter.minCount}
          value={props.searchFilters.minCount}
          handleFilterChange={props.onFilterChange}
        />

        {/* Max Price */}
        <NumericFilter
          id="max-price"
          name="maxPrice"
          // value={props.searchFilter.minCount}
          value={props.searchFilters.maxPrice}
          handleFilterChange={props.onFilterChange}
        />

        {/* Filter By */}
        <Box className={`${flexSection} ${sidePadded}`}>
          <TextField
            name="filterBy"
            className={stretch}
            select
            SelectProps={{displayEmpty: true}}
            label="Filter By"
            value={props.searchFilters.filterBy}
            onChange={props.onFilterChange}
            margin="normal">
            <MenuItem key="inventory" value="inventory">Unused</MenuItem>
            {/* owned == inventory + decks, not sell-list */}
            <MenuItem key="owned" value="owned">Owned</MenuItem>
            {/* inventory by name + decks by name */}
            <MenuItem key="total" value="total">Total</MenuItem>
          </TextField>
        </Box>
      </Box>

      <Box className={combineStyles(staticSection, center, sidePadded)}>
        <Button variant="contained" size="medium" color="primary" onClick={() => props.onSearchClick()}>
          Search
        </Button>
      </Box>
    </Paper>
  </Box>);
}

interface CardSearchPendingCardsProps {
  pendingCardsById: { [id: number]: TrimmedCard }
}

function PendingCardsSection(props: CardSearchPendingCardsProps): JSX.Element {
  const { outlineSection, flexRow } = appStyles();
  return (<Paper className={combineStyles(outlineSection, flexRow)}>
    {
      Object.keys(props.pendingCardsById).map((id) => {
        let thisCard: TrimmedCard = props.pendingCardsById[id];
        return(
          <Paper key={id}>
            <Typography variant="h5">{ thisCard?.data.name }</Typography>
            {/*<Typography variant="h6">{ thisCard.numberToTrim }{(thisCard.isFoil && ` (${thisCard.foilToTrim} foil)`) }</Typography>*/}
            <Typography variant="h6">
              {`${thisCard.data.printDisplay}: ${thisCard.numberToTrim}`}
            </Typography>
          </Paper>
        )
      })
    }
  </Paper>);
}

interface SearchResultTableProps {
  searchResultsById: { [key: number]: TrimmingToolResult }
  searchResultIds: number[];
  pendingCardsById: { [id: number]: TrimmedCard }
  onAddPendingCard: (card: TrimmingToolResult, count: number) => void;
  onRemovePendingCard: (card: TrimmingToolResult) => void;
  onInfoButtonEnter: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  onInfoButtonLeave: () => void;
}

function SearchResultTable(props: SearchResultTableProps): JSX.Element {
  return (
    <Table size="small">
      <TableHead>
        <TableRow>
          <TableCell>Name</TableCell>
          <TableCell>Print</TableCell>
          <TableCell>Unused</TableCell>
          <TableCell>Total</TableCell>
          <TableCell>All Prints</TableCell>
          <TableCell>Recommended</TableCell>
          <TableCell>Trim</TableCell>
          <TableCell>Img</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {
          props.searchResultIds.map(overviewId => {
            const result = props.searchResultsById[overviewId];
            const pendingCard = props.pendingCardsById[result.cardId]
            const currentlyPending = pendingCard ? (pendingCard?.numberToTrim) : 0;
            return (
              <TableRow key={result.cardId}>
                <TableCell>{result.name}</TableCell>
                <TableCell>{`${result.printDisplay} ($${result.price})`}</TableCell>
                <TableCell>{result.unusedCount}</TableCell>
                <TableCell>{result.totalCount}</TableCell>
                <TableCell>{result.allPrintsCount}</TableCell>
                <TableCell>
                  {(result.recommendedTrimCount > currentlyPending) &&
                    <Button variant="contained" color="primary" 
                            onClick={() => props.onAddPendingCard(result, result.recommendedTrimCount)}>
                        <Delete />
                      {result.recommendedTrimCount}
                    </Button>
                  }
                  {(result.recommendedTrimCount <= currentlyPending) && 
                    <CheckCircleOutline color="primary" />
                  }
                </TableCell>
                <TableCell>
                  <IconButton 
                    disabled={currentlyPending===0}
                    color="inherit" 
                    size="small" 
                    onClick={() => {props.onRemovePendingCard(result)} }>
                    <KeyboardArrowLeft />
                  </IconButton>
                  { currentlyPending }
                  <IconButton 
                    disabled={currentlyPending===result.unusedCount}
                    color="inherit" 
                    size="small" 
                    onClick={() => {props.onAddPendingCard(result, 1)} }>
                    <KeyboardArrowRight />
                  </IconButton>
                </TableCell>
                <TableCell>
                  <IconButton
                    value={result.id}
                    color="primary"
                    size="small"
                    onMouseEnter={props.onInfoButtonEnter}
                    onMouseLeave={props.onInfoButtonLeave}>
                    <InfoOutlined />
                  </IconButton>
                </TableCell>
              </TableRow>
            );
          })
        }
      </TableBody>
    </Table>
  );
}

