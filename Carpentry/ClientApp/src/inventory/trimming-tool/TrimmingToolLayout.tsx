import React from 'react';
import { Table, TableHead, TableRow, TableCell, TableBody, Paper, Box, Button, TextField, MenuItem, Typography, IconButton } from '@material-ui/core';
import { appStyles, combineStyles } from '../../styles/appStyles';
import NumericFilter from '../../common/components/NumericFilter';
import { InfoOutlined, KeyboardArrowLeft, KeyboardArrowRight } from '@material-ui/icons';

interface ComponentProps {
    searchFilters: TrimmingToolRequest;
    filterOptions: AppFiltersDto;
    onFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    onSearchClick: () => void;

    searchResultsById: { [key: number]: InventoryOverviewDto }
    searchReusltIds: number[];

    pendingCardsById: { [id: number]: PendingCardsDto }
    pendingCardsIds: number[];

    onAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    onRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;

    onInfoButtonEnter: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onInfoButtonLeave: () => void;
}

export default function TrimmingToolLayout(props: ComponentProps): JSX.Element {
    const {  flexRow, flexSection, outlineSection } = appStyles();
    return (
        <Box>
            <FilterBar 
                searchFilters={props.searchFilters}
                filterOptions={props.filterOptions}
                onFilterChange={props.onFilterChange}
                onSearchClick={props.onSearchClick} 
                />

        <Paper className={combineStyles(outlineSection, flexRow, flexSection)}
         style={{ overflow:'auto', alignItems:'stretch' }}
         >

            <SearchResultTable
                searchResultsById={props.searchResultsById}
                searchReusltIds={props.searchReusltIds}
                onAddPendingCard={props.onAddPendingCard}
                onRemovePendingCard={props.onRemovePendingCard}

                onInfoButtonEnter={props.onInfoButtonEnter}
                onInfoButtonLeave={props.onInfoButtonLeave}
                />

        </Paper>

        <PendingCardsSection 
            pendingCardsById={props.pendingCardsById} 
            pendingCardsIds={props.pendingCardsIds} />
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

                <NumericFilter name="minCount" 
                    // value={props.searchFilter.minCount}
                    value={props.searchFilters.minCount}
                    handleFilterChange={props.onFilterChange} 
                    />                

                {/* MinBy */}
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="minBy"
                        className={stretch}
                        select
                        SelectProps={{
                            displayEmpty: true
                        }}
                        label="By"
                        //value={props.searchFilters.groupBy}
                        onChange={props.onFilterChange}
                        margin="normal">
                            <MenuItem key="name" value="name">Name</MenuItem>
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
    pendingCardsById: { [id: number]: PendingCardsDto }
    pendingCardsIds: number[];
}

function PendingCardsSection(props: CardSearchPendingCardsProps): JSX.Element {
    const { outlineSection, flexRow } = appStyles();
    return (<Paper className={combineStyles(outlineSection, flexRow)}>
        {
            props.pendingCardsIds.map((id: number) => {
                let thisCard: PendingCardsDto = props.pendingCardsById[id];
                return(
                <Paper key={id}>
                    <Typography variant="h5">{ thisCard.name }</Typography>
                    <Typography variant="h6">{ thisCard.cards.length }</Typography>
                    {/* <Typography variant="h6">{ thisCard.countFoil }</Typography> */}
                </Paper>
            )
        })
        }
    </Paper>);
}

interface SearchResultTableProps {
    searchResultsById: { [key: number]: InventoryOverviewDto }
    searchReusltIds: number[];

    onAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    onRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;

    onInfoButtonEnter: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
    onInfoButtonLeave: () => void;
}

function SearchResultTable(props: SearchResultTableProps): JSX.Element {
    const { flexRow } = appStyles();
    return (
        <Table size="small">
            <TableHead>
                <TableRow>
                    <TableCell>Name</TableCell>
                    <TableCell>Print</TableCell>
                    <TableCell>Count</TableCell>
                    <TableCell>Total</TableCell>
                    <TableCell>Trim</TableCell>
                    <TableCell>Img</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
            {
                props.searchReusltIds.map(overviewId => {
                    const result = props.searchResultsById[overviewId];
                    return (
                  
                    <TableRow key={result.cardId}>
                        <TableCell>{result.name}</TableCell>
                        <TableCell>{`${result.setCode} - ${result.collectorNumber}`}{result.isFoil && " (FOIL)" }</TableCell>
                        <TableCell>{result.totalCount}</TableCell>
                        <TableCell>#</TableCell>
                        <TableCell>
                            <IconButton color="inherit" size="small" onClick={() => {props.onRemovePendingCard(result.name, result.cardId, result.isFoil ?? false)} }>
                                <KeyboardArrowLeft />
                                {/* <Remove /> */}
                            </IconButton>
                            [#]
                            <IconButton color="inherit" size="small" onClick={() => {props.onAddPendingCard(result.name, result.cardId, result.isFoil ?? false)} }>
                                <KeyboardArrowRight />
                                {/* <Add /> */}
                            </IconButton>
                        </TableCell>
                        <TableCell>
                            <IconButton 
                                value={result.id} 
                                color="primary" 
                                size="small" 
                                onMouseEnter={props.onInfoButtonEnter}
                                onMouseLeave={props.onInfoButtonLeave}
                                >
                                <InfoOutlined />
                            </IconButton>
                        </TableCell>
                    </TableRow>
                );})
            }
            </TableBody>
        </Table>
    );
}

