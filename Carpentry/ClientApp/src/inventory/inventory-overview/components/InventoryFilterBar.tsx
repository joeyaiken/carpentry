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

      

                <Box className={`${flexSection} ${sidePadded}`}>
                    View: Grid
                    {/* <TextField
                        name="view"
                        className={stretch}
                        select
                        label="View"
                        value={props.viewMethod}
                        onChange={props.handleFilterChange}
                        margin="normal" >
                            {
                                ["grid","table"].map(
                                    val => <MenuItem key={val} value={val} style={{textTransform: "capitalize"}}>{val}</MenuItem>
                                )
                            }
                    </TextField> */}
                </Box>

                <SelectFilter name="groupBy" value={props.searchFilter.groupBy} selectMultiple={false} 
                    options={[{name: "name", value: "name"}, {name: "print", value: "print"}, {name: "unique", value: "unique"}]}
                    handleFilterChange={props.handleFilterChange} />

                <Box className={`${flexSection} ${sidePadded}`}>
                    Group: By Unique
                    {/* <TextField
                        name="group"
                        className={stretch}
                        select
                        label="Group by"
                            value={props.searchFilter.groupBy}
                        onChange={props.handleFilterChange}
                        margin="normal" >
                            {
                                ["name","print", "unique"].map(
                                    val => <MenuItem key={val} value={val} style={{textTransform: "capitalize"}}>{val}</MenuItem>
                                )
                            }
                    </TextField> */}
                </Box>

                <Box className={`${flexSection} ${sidePadded}`}>
                    Sort: By Price, descending
                    {/* <TextField
                        name="sort"
                        className={stretch}
                        select
                        label="Sort by"
                        // value={props.searchFilter.group}
                        value={props.searchFilter.sortBy}
                        onChange={props.handleFilterChange}
                        margin="normal" >
                            {
                                ["name","quantity", "price"].map(
                                    val => <MenuItem key={val} value={val} style={{textTransform: "capitalize"}}>{val}</MenuItem>
                                )
                            }
                    </TextField> */}
                </Box>

                <SelectFilter name="set" value={props.searchFilter.set} selectMultiple={false} options={props.filterOptions.sets}
                    handleFilterChange={props.handleFilterChange} />

                <SelectFilter name="type" value={props.searchFilter.type} selectMultiple={false} options={props.filterOptions.types}
                    handleFilterChange={props.handleFilterChange} />

        {/* Old implementation, should remove the concept of VisibleFilters.  To complicated for what it provides */}

            {   //Type filter
                props.visibleFilters.type &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="type"
                        className={stretch}
                        select
                        SelectProps={{
                            displayEmpty: true
                        }}
                        label="Type filter"
                        value={props.searchFilter.type}
                        onChange={props.handleFilterChange}
                        margin="normal">
                            { props.filterOptions.types.map((item) => (<MenuItem key={item.name} value={item.value}> {item.name} </MenuItem>))}
                    </TextField>
                </Box>
            }
            {   //Color Color Identity
                props.visibleFilters.color &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="colorIdentity"
                        className={stretch}
                        label="Color filter"
                        select
                        SelectProps={{
                            multiple: true
                        }}
                        value={props.searchFilter.colorIdentity}
                        onChange={props.handleFilterChange}
                        margin="normal" >
                        { props.filterOptions.colors.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
                    </TextField>
                </Box>
            }
            {   //color booleans
                props.visibleFilters.color &&
                <Box className={combineStyles(staticSection, sidePadded)}>
                    <FormControl component="fieldset">
                        <FormControlLabel
                            name="exclusiveColorFilters"
                            onChange={(e, checked) => {props.handleBoolFilterChange("exclusiveColorFilters",checked)}}
                            checked={props.searchFilter.exclusiveColorFilters}
                            control={
                                <Checkbox 
                                    color="primary"
                                    icon={<CheckBoxOutlineBlankIcon fontSize="small" />}
                                    checkedIcon={<CheckBoxIcon fontSize="small" />}
                                />
                            }
                            label="Exclusive"
                        />
                        <FormControlLabel
                            name="multiColorOnly"
                            onChange={(e, checked) => {props.handleBoolFilterChange("multiColorOnly",checked)}}
                            checked={props.searchFilter.multiColorOnly}
                            control={
                                <Checkbox
                                    color="primary"
                                    icon={<CheckBoxOutlineBlankIcon fontSize="small" />}
                                    checkedIcon={<CheckBoxIcon fontSize="small" />}
                                />
                            }
                            label="Multi"
                        />
                    </FormControl>
                </Box>
            }
            
            {   //Format
                props.visibleFilters.format &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="format"
                        className={stretch}
                        select
                        label="Format"
                        // value={props.searchFilter.format}
                        onChange={props.handleFilterChange}
                        SelectProps={{
                            displayEmpty: true
                        }}
                        margin="normal" >
                        <MenuItem key="none" value=""></MenuItem>
                        <MenuItem key="Standard" value="standard">Standard</MenuItem>
                        <MenuItem key="Modern" value="modern">Modern</MenuItem>
                        <MenuItem key="Commander" value="commander">Commander</MenuItem>
                        <MenuItem key="Pioneer" value="pioneer">Pioneer</MenuItem>
                        <MenuItem key="Brawl" value="brawl">Brawl</MenuItem>
                    </TextField>
                </Box>
            }
    </Box>
    <Box className={combineStyles(flexSection, flexRow)}>

<Box className={combineStyles(flexSection, flexRow)}>

            {/* <Box className ="static-section side-padded">
                    <FormControl component="fieldset">
                        <FormControlLabel
                            name="exclusiveColorFilters"
                            onChange={(e, checked) => {props.handleBoolFilterChange("exclusiveColorFilters",checked)}}
                            checked={props.searchFilter.exclusiveColorFilters}
                            control={
                                <Checkbox 
                                    color="primary"
                                    icon={<CheckBoxOutlineBlankIcon fontSize="small" />}
                                    checkedIcon={<CheckBoxIcon fontSize="small" />}
                                />
                            }
                            label="Exclusive"
                        />
                        <FormControlLabel
                            name="multiColorOnly"
                            onChange={(e, checked) => {props.handleBoolFilterChange("multiColorOnly",checked)}}
                            checked={props.searchFilter.multiColorOnly}
                            control={
                                <Checkbox
                                    color="primary"
                                    icon={<CheckBoxOutlineBlankIcon fontSize="small" />}
                                    checkedIcon={<CheckBoxIcon fontSize="small" />}
                                />
                            }
                            label="Multi"
                        />
                    </FormControl>
                </Box>
             */}

                <TextFilter name="text" value={props.searchFilter.text}
                    handleFilterChange={props.handleFilterChange} />
                
                <Box className={`${flexSection} ${sidePadded}`}>
                    <Box className={outlineSection}>
                        Type
                    </Box>
                </Box>
                <Box className={`${flexSection} ${sidePadded}`}>
                    <Box className={outlineSection}>
                        Colors
                    </Box>
                </Box>
                <Box className={`${flexSection} ${sidePadded}`}>
                    <Box className={outlineSection}>
                        [Exclusive/Multi]
                    </Box>
                </Box>

            {/* cardStatus (deck/sellList/buyList/inventory) */}

            {/* {
                this.props.searchFilter.group != "quantity" &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="group"
                        className={stretch}
                        select
                        label="Group by"
                        value={this.props.searchFilter.group}
                        onChange={this.handleFilterChange}
                        margin="normal" >
                            <MenuItem key="name" value="name">Name</MenuItem>
                            <MenuItem key="set" value="set">Name & Set</MenuItem>
                            <MenuItem key="unique" value="unique">Unique</MenuItem>
                        </TextField>
                </Box>
            } */}
            {
                // this.props.searchFilter.group != "quantity" &&
                // <Box className={`${flexSection} ${sidePadded}`}>
                //     <TextField
                //         name="sort"
                //         className={stretch}
                //         select
                //         label="Sort by"
                //         value={this.props.searchFilter.sort}
                //         onChange={this.handleFilterChange}
                //         margin="normal" >
                //             {/* { this.props.setFilterOptions.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) } */}
                //             <MenuItem key="name" value="name">Name</MenuItem>
                //             <MenuItem key="count" value="count">Count DESC</MenuItem>
                //             <MenuItem key="multiverseid" value="multiverseid">Multiverse Id</MenuItem>
                //         </TextField>
                // </Box>
            }

            
            {/* {   //Format
                props.visibleFilters.format &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="format"
                        className={stretch}
                        select
                        label="Format"
                        value={props.searchFilter.format}
                        onChange={props.handleFilterChange}
                        SelectProps={{
                            displayEmpty: true
                        }}
                        margin="normal" >
                        <MenuItem key="none" value=""></MenuItem>
                        <MenuItem key="Standard" value="standard">Standard</MenuItem>
                        <MenuItem key="Modern" value="modern">Modern</MenuItem>
                        <MenuItem key="Commander" value="commander">Commander</MenuItem>
                        <MenuItem key="Pioneer" value="pioneer">Pioneer</MenuItem>
                        <MenuItem key="Brawl" value="brawl">Brawl</MenuItem>
                    </TextField>
                </Box>
            } */}



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
