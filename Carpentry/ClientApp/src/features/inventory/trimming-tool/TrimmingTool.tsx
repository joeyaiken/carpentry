import React, {useEffect, useState} from 'react';
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle, IconButton,
  MenuItem,
  Paper, Table, TableBody, TableCell, TableHead, TableRow,
  TextField, Typography
} from "@material-ui/core";
import {useAppDispatch, useAppSelector} from "../../../hooks";
import {useHistory} from "react-router";
import {ApiStatus} from "../../../enums";
import {
  addPendingCard,
  clearPendingCards,
  loadTrimmingToolCards,
  removePendingCard,
  trimCards
} from "./trimmingToolSlice";
import styles from "../../../App.module.css";
import NumericFilter from "../../../common/components/NumericFilter";
import CardImagePopper from "../../../common/components/CardImagePopper";
import {CheckCircleOutline, Delete, InfoOutlined, KeyboardArrowLeft, KeyboardArrowRight} from "@material-ui/icons";

export const TrimmingTool = (): JSX.Element => {
  
  const dispatch = useAppDispatch();
  const history = useHistory();

  const handleCloseModalClick = (): void => {
    history.push('/inventory');
    dispatch(clearPendingCards());
  }

  // Is there some other location where this logic should exist instead?...
  const pendingCardsStatus = useAppSelector(state => state.inventory.trimmingTool.pendingCards.status);
  const pendingCardsById = useAppSelector(state => state.inventory.trimmingTool.pendingCards.byId);
  const handleTrimClick = (): void => {
    if(pendingCardsStatus === ApiStatus.loading) return;

    let cardsToTrim: TrimmedCardDto[] = [];
    Object.keys(pendingCardsById).forEach(id => {
      const card: TrimmedCard = pendingCardsById[id];
      if(card.numberToTrim) {
        cardsToTrim.push({
          cardId: card.data.cardId,
          cardName: card.data.name,
          isFoil: card.data.isFoil ?? false,
          numberToTrim: card.numberToTrim,
        });
      }
    });
    dispatch(trimCards(cardsToTrim));
  }
  
  return (
    <React.Fragment>
      <Dialog maxWidth="xl" open={true} onClose={() => {handleCloseModalClick()}} >
        <DialogTitle>{`Trimming Tool`}</DialogTitle>
        <DialogContent>
          <Box>
            <FilterBar />
            <Paper
              className={[styles.outlineSection, styles.flexRow, styles.flexSection].join(' ')}
              style={{ overflow:'auto', alignItems:'stretch' }}>
              <SearchResultTable />
            </Paper>
            <PendingCardsSection />
          </Box>
        </DialogContent>
        <DialogActions>
          <Button size="medium" onClick={() => handleCloseModalClick()}>Cancel</Button>
          <Button size="medium" color="primary" variant="contained" onClick={() => handleTrimClick()}>Trim</Button>
        </DialogActions>
      </Dialog>
    </React.Fragment>
  );
}

const FilterBar = (): JSX.Element => {
  // filter bar props should be local state
  const [setCodeFilter, setSetCodeFilter] = useState<string>('znr');
  const [searchGroupFilter, setSearchGroupFilter] = useState<string>('Red');
  const [minCountFilter, setMinCountFilter] = useState<number>(8);
  const [maxPriceFilter, setMaxPriceFilter] = useState<number>(0.1);
  const [filterByFilter, setFilterByFilter] = useState<string>('inventory');

  const searchResultStatus = useAppSelector(state => state.inventory.trimmingTool.searchResults.status);

  const dispatch = useAppDispatch();

  // The initialize load will exist here, since this is where the filter values live
  useEffect(() => {
    if(searchResultStatus === ApiStatus.uninitialized)
      tryLoadTrimmingToolCards();
  });

  const tryLoadTrimmingToolCards = (): void => {
    if(searchResultStatus === ApiStatus.loading) return;
    const requestDto: TrimmingToolRequest = {
      setCode: setCodeFilter,
      searchGroup: searchGroupFilter,
      minCount: minCountFilter,
      maxPrice: maxPriceFilter,
      filterBy: filterByFilter,
    };
    dispatch(loadTrimmingToolCards(requestDto));
  }

  const filterOptions = useAppSelector(state => state.core.filterOptions);

  return(<Box>
    <Paper className={[styles.outlineSection, styles.flexRow].join(' ')}>

      <Box className={[styles.flexSection, styles.flexRow].join(' ')}>
        {/* SET filter */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="setCode"
            className={styles.stretch}
            select
            label="Set filter"
            value={setCodeFilter}
            onChange={event => setSetCodeFilter(event.target.value)}
            margin="normal" >
            <MenuItem key="null" value="" />
            { filterOptions.sets.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
          </TextField>
        </Box>

        {/* Group filter */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="searchGroup"
            className={styles.stretch}
            select
            SelectProps={{
              displayEmpty: true
            }}
            label="Group"
            value={searchGroupFilter}
            onChange={event => setSearchGroupFilter(event.target.value)}
            margin="normal">
            <MenuItem key="null" value="" />
            { filterOptions.searchGroups.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
          </TextField>
        </Box>

        {/* Min # */}
        <NumericFilter
          id="min-count"
          name="minCount"
          value={minCountFilter}
          handleFilterChange={event => setMinCountFilter(+event.target.value)}
        />

        {/* Max Price */}
        <NumericFilter
          id="max-price"
          name="maxPrice"
          value={maxPriceFilter}
          handleFilterChange={event => setMaxPriceFilter(+event.target.value)}
        />

        {/* Filter By */}
        <Box className={`${styles.flexSection} ${styles.sidePadded}`}>
          <TextField
            name="filterBy"
            className={styles.stretch}
            select
            SelectProps={{displayEmpty: true}}
            label="Filter By"
            value={filterByFilter}
            onChange={event => setFilterByFilter(event.target.value)}
            margin="normal">
            <MenuItem key="inventory" value="inventory">Unused</MenuItem>
            {/* owned == inventory + decks, not sell-list */}
            <MenuItem key="owned" value="owned">Owned</MenuItem>
            {/* inventory by name + decks by name */}
            <MenuItem key="total" value="total">Total</MenuItem>
          </TextField>
        </Box>
      </Box>

      <Box className={[styles.staticSection, styles.center, styles.sidePadded].join(' ')}>
        <Button variant="contained" size="medium" color="primary" onClick={tryLoadTrimmingToolCards}>
          Search
        </Button>
      </Box>
    </Paper>
  </Box>);
}

const PendingCardsSection = (): JSX.Element => {
  const pendingCardsById = useAppSelector(state =>
    state.inventory.trimmingTool.pendingCards.byId);
  return (<Paper className={[styles.outlineSection, styles.flexRow].join(' ')}>
    {
      Object.keys(pendingCardsById).map((id) => {
        let thisCard: TrimmedCard = pendingCardsById[id];
        return(
          <Paper key={id}>
            <Typography variant="h5">{ thisCard?.data.name }</Typography>
            <Typography variant="h6">
              {`${thisCard.data.printDisplay}: ${thisCard.numberToTrim}`}
            </Typography>
          </Paper>
        )
      })
    }
  </Paper>);
}

const SearchResultTable = (): JSX.Element => {
  const [cardImageMenuAnchor, setCardImageMenuAnchor] = useState<HTMLButtonElement | null>(null);

  const searchResultIds = useAppSelector(state => state.inventory.trimmingTool.searchResults.allIds);

  const selectedOverviewImage: string = useAppSelector(state => {
    const cardImageAnchorId = +(cardImageMenuAnchor?.value ?? "0");
    const card = state.inventory.trimmingTool.searchResults.byId[cardImageAnchorId];
    return card?.imageUrl ?? "";
  });

  const onInfoButtonEnter = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>): void =>
    setCardImageMenuAnchor(event.currentTarget);

  const onInfoButtonLeave = (): void =>
    setCardImageMenuAnchor(null);

  return (
    <React.Fragment>
      <CardImagePopper
        menuAnchor={cardImageMenuAnchor}
        onClose={onInfoButtonLeave}
        image={selectedOverviewImage} />
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
            searchResultIds.map(overviewId =>
              <SearchResultTableRow
                key={overviewId}
                overviewId={overviewId}
                onInfoButtonEnter={onInfoButtonEnter}
                onInfoButtonLeave={onInfoButtonLeave} />
            )
          }
        </TableBody>
      </Table>
    </React.Fragment>
  );
}
interface SearchResultTableRowProps {
  overviewId: number;
  onInfoButtonEnter: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  onInfoButtonLeave: () => void;
}
const SearchResultTableRow = (props: SearchResultTableRowProps): JSX.Element => {
  const result = useAppSelector(state =>
    state.inventory.trimmingTool.searchResults.byId[props.overviewId]);
  const pendingCard = useAppSelector<TrimmedCard>(state =>
    state.inventory.trimmingTool.pendingCards.byId[result.cardId]);
  const currentlyPending = pendingCard ? (pendingCard?.numberToTrim) : 0;

  const dispatch = useAppDispatch();

  const onAddPendingCard = (card: TrimmingToolResult, count: number): void =>
    dispatch(addPendingCard({card: card, count: count}));

  const onRemovePendingCard = (card: TrimmingToolResult): void =>
    dispatch(removePendingCard(card));

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
                onClick={() => onAddPendingCard(result, result.recommendedTrimCount)}>
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
          onClick={() => {onRemovePendingCard(result)} }>
          <KeyboardArrowLeft />
        </IconButton>
        { currentlyPending }
        <IconButton
          disabled={currentlyPending===result.unusedCount}
          color="inherit"
          size="small"
          onClick={() => {onAddPendingCard(result, 1)} }>
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
}
