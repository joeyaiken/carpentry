//table of deck cards
import React from 'react';

import { Table, TableHead, TableRow, TableCell, TableBody, Paper, Box, Button, TextField, FormControl, FormControlLabel, Checkbox, MenuItem, Typography, Card, CardMedia, CardContent } from '@material-ui/core';
import { appStyles, combineStyles } from '../../styles/appStyles';

import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'
import NumericFilter from '../../common/components/NumericFilter';

interface ComponentProps {
    //totalPrice: number;
    // deckProperties: DeckProperties;
    // onEditClick: () => void;
    // cardOverviews: DeckCardOverview[];
    // onCardSelected: (card: DeckCardOverview) => void;

    filterOptions: AppFiltersDto;
    onFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    onSearchClick: () => void;

    cards: InventoryOverviewDto[];

    pendingCards: { [key:number]: PendingCardsDto }

    onAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    onRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;

}

export default function TrimmingToolLayout(props: ComponentProps): JSX.Element {
    const {  flexRow, flexSection, outlineSection } = appStyles();
    return (
        <React.Fragment>
            <FilterBar 

                filterOptions={props.filterOptions}
                onFilterChange={props.onFilterChange}
                // searchFilterProps={props.searchFilterProps}
                onSearchClick={props.onSearchClick} 
                />

        <Paper className={combineStyles(outlineSection, flexRow, flexSection)}
         style={{ overflow:'auto', alignItems:'stretch' }}
         >

            <SearchResultTable
                cards={props.cards}
                onAddPendingCard={props.onAddPendingCard}
                onRemovePendingCard={props.onRemovePendingCard}

                // onCardSelected={props.handleCardSelected}
                />

        </Paper>
        <Box className={combineStyles(flexRow,flexSection)} style={{ overflow:'auto', alignItems:'stretch' }}>
            <Paper style={{ overflow:'auto', flex:'1 1 70%' }} >
            {/* { props.viewMode === "list" && 
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
            } */}
            </Paper>
        </Box>
        <PendingCardsSection 
            pendingCards={props.pendingCards} 
            />
        <Paper className={combineStyles(outlineSection, flexRow)}>
            <Button 
            // onClick={props.handleCancelClick}
            >
                Cancel
            </Button>
            <Button color="primary" variant="contained" 
            // onClick={props.handleSaveClick}
            >
                Save
            </Button>
        </Paper>


            {/* <Table size="small">
                <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        <TableCell>Set</TableCell>
                        <TableCell>Variant</TableCell>
                        <TableCell># to Trim</TableCell>
                        <TableCell>Reason to Trim</TableCell>
                        <TableCell>[options]</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {
                        // Table Row:
                        // onClick={() => props.onCardSelected(cardItem)} onMouseEnter={() => props.onCardSelected(cardItem)}
                        props.cards.map(cardItem => 
                            <TableRow 
                                key={cardItem.id+cardItem.name}>
                                <TableCell>{cardItem.name}</TableCell>
                                <TableCell>{cardItem.type}</TableCell>
                                <TableCell>{cardItem.manaCost}</TableCell>
                            </TableRow>
                        )
                    }
                </TableBody>
            </Table> */}
        </React.Fragment>
    );
}

interface FilterBarProps {
    filterOptions: AppFiltersDto;
    // handleBoolFilterChange: (filter: string, value: boolean) => void;
    onFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    // searchFilterProps: CardFilterProps;
    // cardSearchMethod: "set" | "web" | "inventory";
    onSearchClick: () => void;
}

function FilterBar(props: FilterBarProps): JSX.Element{
    const {  flexRow, outlineSection, staticSection, center, sidePadded,flexSection, stretch } = appStyles();
    return(<React.Fragment>
        <Paper className={combineStyles(outlineSection, flexRow)}>

            <Box className={combineStyles(flexSection, flexRow)}>
                {/* SET filter */}
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="set"
                        className={stretch}
                        select
                        label="Set filter"
                        // value={props.searchFilterProps.set}
                        onChange={props.onFilterChange}
                        margin="normal" >
                            <MenuItem key="null" value=""></MenuItem>
                            { props.filterOptions.sets.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
                        </TextField>
                </Box>

                {/* Group filter */}
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="group"
                        className={stretch}
                        select
                        SelectProps={{
                            displayEmpty: true
                        }}
                        label="Group"
                        // value={props.searchFilterProps.group}
                        onChange={props.onFilterChange}
                        margin="normal">
                            <MenuItem key="null" value=""></MenuItem>
                            { props.filterOptions.searchGroups.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
                    </TextField>
                </Box>

                {/* Min # */}

                <NumericFilter name="minCount" 
                    // value={props.searchFilter.minCount}
                    value={0}
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
                        // value={props.searchFilterProps.group}
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
    </React.Fragment>);
}

interface CardSearchPendingCardsProps {
    pendingCards: { [key:number]: PendingCardsDto }
}

function PendingCardsSection(props: CardSearchPendingCardsProps): JSX.Element {
    const { outlineSection, flexRow } = appStyles();
    return (<Paper className={combineStyles(outlineSection, flexRow)}>
        {/* [Pending Cards] */}
        {
            Object.keys(props.pendingCards).map((id: string) => {
                let thisCard: PendingCardsDto = props.pendingCards[id];
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
    cards: InventoryOverviewDto[];
    // searchResults: CardListItem[];
    onAddPendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    onRemovePendingCard: (name: string, cardId: number, isFoil: boolean) => void;
    // onCardSelected: (item: CardListItem) => void;
}

function SearchResultTable(props: SearchResultTableProps): JSX.Element {
    const { flexRow } = appStyles();
    return (
        <Table size="small">
            <TableHead>
                <TableRow>
                    <TableCell>Name</TableCell>
                    <TableCell>Set | num | foil</TableCell>
                    <TableCell>Print #</TableCell>
                    <TableCell>Total #</TableCell>
                    <TableCell>Trim</TableCell>
                    <TableCell>Img</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
            {
                props.cards.map(result => (
                    <TableRow key={result.cardId}>
                        <TableCell>{result.name}</TableCell>
                        <TableCell>Set | num | foil</TableCell>
                        {/* <TableCell>Print #</TableCell> */}
                        <TableCell>{result.totalCount}</TableCell>
                        <TableCell>Total #</TableCell>
                        {/* <TableCell>[-] [# to trim] [+]</TableCell> */}
                        <Box className={flexRow}>
                            <Button variant="contained" size="small" onClick={() => {props.onRemovePendingCard(result.name, result.cardId, result.isFoil ?? false)} } >-</Button>
                            [# to trim]
                            <Button variant="contained" size="small" onClick={() => {props.onAddPendingCard(result.name, result.cardId, result.isFoil ?? false)} } >+</Button>
                        </Box>
                        <TableCell>[img]</TableCell>
                      
                    </TableRow>
                ))
            }
            </TableBody>
        </Table>
    );
}

