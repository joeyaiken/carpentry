// import { connect, DispatchProp } from 'react-redux';
import React from 'react';
// import { AppState } from '../../reducers'

import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'

// import {
//     requestInventoryItems, 
//     // inventoryFilterChanged,
// } from '../actions/inventory.actions'


// import CardFilterBar from '../components/CardFilterBar';
import { Paper, Box, TextField, MenuItem, FormControl, FormControlLabel, Checkbox, Button } from '@material-ui/core';
import { appStyles, combineStyles } from '../../../styles/appStyles';
// import FilterBarSearchButton from '../components/FilterBarSearchButton';

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

    // console.log('visible filters');
    // console.log(props.visibleFilters);

    return(
<Paper className={combineStyles(outlineSection, flexCol)}>

{/* <Box className={flexCol}> */}

    <Box className={combineStyles(flexSection, flexRow)}>
            {   //Text filter
                props.visibleFilters.text &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="text"
                        className={stretch}
                        label="Text"
                        value={props.searchFilter.text}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            }
            
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
            
            {   //RARITY filter
                props.visibleFilters.rarity &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="rarity"
                        className={stretch}
                        select
                        SelectProps={{ multiple: true }}
                        label="Rarity filter"
                        value={props.searchFilter.rarity}
                        onChange={props.handleFilterChange}
                        margin="normal" >
                        { props.filterOptions.rarities.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
                    </TextField>
                </Box>
            }
          
{/* 
            {   //Min Count
                props.visibleFilters.count &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="minCount"
                        className={stretch}
                        label="Min"
                        value={props.searchFilter.minCount}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            }
            {   //Max Count
                props.visibleFilters.count &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="maxCount"
                        className={stretch}
                        label="Max"
                        value={props.searchFilter.maxCount}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            } */}
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
    {/* </Paper> */}
    
        

    {/* </Paper> */}

    
    {/* <Paper className={outlineSection}>
        <InventoryFilterBar 
            filterOptions={this.props.filterOptions}
            handleBoolFilterChange={this.handleBoolFilterChange}
            handleFilterChange={this.handleFilterChange}
            searchFilter={this.props.searchFilterProps}
            visibleFilters={this.props.visibleFilters}
        />
    </Paper> */}
    <Box className={combineStyles(flexSection, flexRow)}>

<Box className={combineStyles(flexSection, flexRow)}>
{/* 
            <Box className ="flex-section outline-section">
                [x] Exclude Lands
            </Box>

            include unowned / only show owned

            exclude lands (this is so I don't have to see all the gates)

            */}

            {/* exclude lands / include unowned / exclude owned */}
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

            {/*  Group by Name / Group by MID / unique prints */}
            {/* <Box className={`${flexSection} ${sidePadded}`}>
                <TextField
                    name="text"
                    className={stretch}
                    label="Group"
                    value={props.searchFilter.text}
                    onChange={props.handleFilterChange}
                    margin="normal"/>
            </Box> */}

            {/* Sort by name/quantity/price */}
            
                {/* <Box className={`${flexSection} ${sidePadded}`}>
                    
                    <Box className={outlineSection}>
                        Filter idea: Hide non-normal variants
                        Can be used when trying to see what I have > 4 or 6 or whatever of
                    </Box>
                </Box> */}

                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
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
                        </TextField>
                </Box>

                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
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
                        </TextField>
                </Box>

                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
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
                        </TextField>
                </Box>

                {   //SET filter
                    props.visibleFilters.set &&
                    <Box className={`${flexSection} ${sidePadded}`}>
                        <TextField
                            name="set"
                            className={stretch}
                            select
                            label="Set filter"
                            value={props.searchFilter.set}
                            onChange={props.handleFilterChange}
                            margin="normal" >
                                <MenuItem key="null" value=""></MenuItem>
                                { props.filterOptions.sets.map((item) => (<MenuItem key={item.value} value={item.value}>{item.name}</MenuItem>)) }
                            </TextField>
                    </Box>
                }

                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="text"
                        className={stretch}
                        label="Text"
                        // value={props.searchFilter.group}
                        onChange={props.handleFilterChange}
                        margin="normal" >
      
                        </TextField>
                </Box>
                
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
            {/* <Box className={`${flexSection} ${sidePadded}`}>
                <TextField
                    name="status"
                    className={stretch}
                    select
                    SelectProps={{ multiple: true }}
                    label="Status"
                    // value={props.searchFilter.rarity}
                    value={["deck","inventory"]}
                    onChange={props.handleFilterChange}
                    margin="normal" >
                    { 
                        ["deck","sellList","buyList","inventory"].map(
                            (item) => (<MenuItem key={item} value={item} style={{textTransform: "capitalize"}}>{item}</MenuItem>)
                        ) 
                    }
                </TextField>
            </Box> */}

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

            {/* {   //Min Count
                props.visibleFilters.count &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="minCount"
                        className={stretch}
                        label="Min"
                        value={props.searchFilter.minCount}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            }
            {   //Max Count
                props.visibleFilters.count &&
                <Box className={`${flexSection} ${sidePadded}`}>
                    <TextField
                        name="maxCount"
                        className={stretch}
                        label="Max"
                        value={props.searchFilter.maxCount}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            } */}
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

            <Box className={`${flexSection} ${sidePadded}`}>
                <TextField
                    name="text"
                    className={stretch}
                    label="Skip"
                    value={props.searchFilter.text}
                    onChange={props.handleFilterChange}
                    margin="normal"/>
            </Box>

            <Box className={`${flexSection} ${sidePadded}`}>
                <TextField
                    name="text"
                    className={stretch}
                    label="Take"
                    value={props.searchFilter.text}
                    onChange={props.handleFilterChange}
                    margin="normal"/>
            </Box>

            
    </Box>
    <Box className={combineStyles(staticSection, center, sidePadded)}>
            <Button variant="contained" size="medium" color="primary" onClick={() => props.handleSearchButtonClick()}>
                Search
            </Button>
        </Box>
    </Box>
{/* </Box> */}
    {/* </Box> */}
</Paper>
    );
}

