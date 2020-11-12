// import { connect, DispatchProp } from 'react-redux';
import React from 'react';
// import { AppState } from '../../reducers'

import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'
import { Paper, Box, TextField, MenuItem, FormControl, FormControlLabel, Checkbox, Button, Select } from '@material-ui/core';
import { appStyles, combineStyles } from '../../../styles/appStyles';

interface InventoryFilterBarProps{
    searchFilter: InventoryFilterProps,
    viewMethod: "grid" | "table";
    visibleFilters: CardFilterVisibilities;
    filterOptions: AppFiltersDto,
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
    handleSearchButtonClick: () => void;
}

//This bar should just be a flex-grid of filter elements
//It probably shouldn't even have the "search" button
export default function InventoryFilterBar(props: InventoryFilterBarProps): JSX.Element {
    const { outlineSection, flexCol, flexRow, flexSection, sidePadded, stretch, staticSection, center } = appStyles();
    // Need this to cache?
    // try this?
    // https://material-ui.com/components/autocomplete

    return(
        <Paper className={combineStyles(outlineSection, flexCol)}>
            <Box className={combineStyles(flexSection, flexRow)}>

                {/* <SelectFilter name="view" value={props.viewMethod} selectMultiple={false} 
                    options={[{name: "grid", value: "grid"},{name: "table", value: "table"}]}
                    handleFilterChange={props.handleFilterChange} /> */}

                <SelectFilter name="groupBy" value={props.searchFilter.groupBy} selectMultiple={false} 
                    options={[{name: "name", value: "name"}, {name: "print", value: "print"}, {name: "unique", value: "unique"}]}
                    handleFilterChange={props.handleFilterChange} />

                <SelectFilter name="sortBy" value={props.searchFilter.sortBy} selectMultiple={false}
                    options={[{name:"name", value:"name"},{name:"quantity", value:"quantity"},{name:"price", value:"price"}]}
                    handleFilterChange={props.handleFilterChange} />

                <SelectFilter name="set" value={props.searchFilter.set} selectMultiple={false} options={props.filterOptions.sets}
                    handleFilterChange={props.handleFilterChange} />

                <SelectFilter name="type" value={props.searchFilter.type} selectMultiple={false} options={props.filterOptions.types}
                    handleFilterChange={props.handleFilterChange} />

                <SelectFilter name="colorIdentity" selectMultiple={true} value={props.searchFilter.colorIdentity} options={props.filterOptions.colors}
                    handleFilterChange={props.handleFilterChange} />

                <BooleanColorFilter
                    exclusiveColorFiltersChecked={props.searchFilter.exclusiveColorFilters}
                    multiColorOnlyChecked={props.searchFilter.multiColorOnly}
                    handleBoolFilterChange={props.handleBoolFilterChange} />

            </Box>
            <Box className={combineStyles(flexSection, flexRow)}>
                <Box className={combineStyles(flexSection, flexRow)}>

                    <TextFilter name="text" value={props.searchFilter.text}
                        handleFilterChange={props.handleFilterChange} />
                

                    {/* <SelectFilter name="format" value={props.searchFilter.format} selectMultiple={false} 
                        options={[{name: "none", value:""},{name: "Standard", value:"standard"},{name: "Modern", value:"modern"},{name: "Commander", value:"commander"},
                        {name: "Pioneer", value:"pioneer"},{name: "Brawl", value:"brawl"},{name: "Pauper", value:"pauper"}]}
                        handleFilterChange={props.handleFilterChange} /> */}

                    <SelectFilter name="rarity" value={props.searchFilter.rarity} options={props.filterOptions.rarities} selectMultiple={true}
                        handleFilterChange={props.handleFilterChange} />

                    <NumericFilter name="minCount" value={props.searchFilter.minCount}
                        handleFilterChange={props.handleFilterChange} />

                    <NumericFilter name="maxCount" value={props.searchFilter.maxCount}
                        handleFilterChange={props.handleFilterChange} />

                    <NumericFilter name="skip" value={props.searchFilter.skip}
                        handleFilterChange={props.handleFilterChange} />

                    <NumericFilter name="take" value={props.searchFilter.take}
                        handleFilterChange={props.handleFilterChange} />

                </Box>
                <Box className={combineStyles(staticSection, center, sidePadded)}>
                    <Button variant="contained" size="medium" color="primary" onClick={() => props.handleSearchButtonClick()}>
                        Search
                    </Button>
                </Box>
            </Box>

        </Paper>
    );
}



interface NumericFilterProps {
    name: string;
    value: number;
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}
function NumericFilter(props: NumericFilterProps): JSX.Element {
    const { flexSection, sidePadded, stretch } = appStyles();
    return(
        <Box className={`${flexSection} ${sidePadded}`}>
            <TextField
                name={props.name}
                className={stretch}
                label={props.name}
                value={props.value}
                onChange={props.handleFilterChange}
                style={{textTransform: "capitalize"}}
                margin="normal"/>
        </Box>
    )
}

interface SelectFilterProps {
    name: string;
    options: FilterOption[];
    value: string | string[];
    selectMultiple: boolean;
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}
function SelectFilter(props: SelectFilterProps): JSX.Element {
    const { flexSection, sidePadded, stretch } = appStyles();
    return(
        <Box className={`${flexSection} ${sidePadded}`}>
                <TextField
                    name={props.name}
                    className={stretch}
                    select
                    SelectProps={{ multiple: props.selectMultiple }}
                    label={props.name}
                    value={props.value}
                    onChange={props.handleFilterChange}
                    style={{textTransform: "capitalize"}}
                    margin="normal" >
                    { props.options.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
                </TextField>
            </Box>
    );
}

interface TextFilterProps {
    name: string;
    value: string;
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}
function TextFilter(props: TextFilterProps): JSX.Element {
    const { flexSection, sidePadded, stretch } = appStyles();
    return(
        <Box className={`${flexSection} ${sidePadded}`}>
            <TextField
                name={props.name}
                className={stretch}
                label={props.name}
                value={props.value}
                onChange={props.handleFilterChange}
                style={{textTransform: "capitalize"}}
                margin="normal"/>
        </Box>
    )
}

interface BooleanColorFilterProps {
    exclusiveColorFiltersChecked: boolean;
    multiColorOnlyChecked: boolean;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
}

function BooleanColorFilter(props: BooleanColorFilterProps): JSX.Element {
    
    const { sidePadded, staticSection } = appStyles();
    return(
        <Box className={combineStyles(staticSection, sidePadded)}>
            <FormControl component="fieldset">
                <FormControlLabel
                    name="exclusiveColorFilters"
                    onChange={(e, checked) => {props.handleBoolFilterChange("exclusiveColorFilters",checked)}}
                    checked={props.exclusiveColorFiltersChecked}
                    control={
                        <Checkbox 
                            color="primary"
                            icon={<CheckBoxOutlineBlankIcon fontSize="small" />}
                            checkedIcon={<CheckBoxIcon fontSize="small" />} />
                    }
                    label="Exclusive" />
                <FormControlLabel
                    name="multiColorOnly"
                    onChange={(e, checked) => {props.handleBoolFilterChange("multiColorOnly",checked)}}
                    checked={props.multiColorOnlyChecked}
                    control={
                        <Checkbox
                            color="primary"
                            icon={<CheckBoxOutlineBlankIcon fontSize="small" />}
                            checkedIcon={<CheckBoxIcon fontSize="small" />} />
                    }
                    label="Multi" />
            </FormControl>
        </Box>
    );
}