import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../reducers'

import CheckBoxOutlineBlankIcon from '@material-ui/icons/CheckBoxOutlineBlank'
import CheckBoxIcon from '@material-ui/icons/CheckBox'

import {
    requestInventoryItems, 
    // inventoryFilterChanged,
} from '../actions/inventory.actions'

import {
    filterValueChanged
} from '../actions/ui.actions'

import CardFilterBar from '../components/CardFilterBar';
import { Paper, Box, TextField, MenuItem, FormControl, FormControlLabel, Checkbox, Button } from '@material-ui/core';
import FilterBarSearchButton from '../components/FilterBarSearchButton';

interface PropsFromState { 
    searchFilterProps: CardFilterProps;
    filterOptions: CoreFilterOptions;
    visibleFilters: CardFilterVisibilities;
}

type InventoryProps = PropsFromState & DispatchProp<ReduxAction>;

class Inventory extends React.Component<InventoryProps>{
    constructor(props: InventoryProps) {
        super(props);
        this.handleFilterChange = this.handleFilterChange.bind(this);
        this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
        this.handleBoolFilterChange = this.handleBoolFilterChange.bind(this);
    }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        this.props.dispatch(filterValueChanged("inventoryFilterProps", event.target.name, event.target.value));
    }

    handleBoolFilterChange(filter: string, value: boolean): void {
        this.props.dispatch(filterValueChanged("inventoryFilterProps", filter, value));
    }

    handleSearchButtonClick() {
        this.props.dispatch(requestInventoryItems());
    }

    render() {
        return (
            <Paper className="outline-section flex-col">

            {/* <Box className="flex-col"> */}
                {/* <Paper className="outline-section flex-row"> */}
                {/* <Paper className="outline-section"> */}
                <CardFilterBar 
                    filterOptions={this.props.filterOptions}
                    handleBoolFilterChange={this.handleBoolFilterChange}
                    handleFilterChange={this.handleFilterChange}
                    // handleSearchButtonClick={this.handleSearchButtonClick}
                    searchFilter={this.props.searchFilterProps}
                    visibleFilters={this.props.visibleFilters}
                />
                {/* </Paper> */}
                
                    

                {/* </Paper> */}

                
                {/* <Paper className="outline-section">
                    <InventoryFilterBar 
                        filterOptions={this.props.filterOptions}
                        handleBoolFilterChange={this.handleBoolFilterChange}
                        handleFilterChange={this.handleFilterChange}
                        searchFilter={this.props.searchFilterProps}
                        visibleFilters={this.props.visibleFilters}
                    />
                </Paper> */}
                <Box className="flex-section flex-row">
                    {/* <Box className="flex-section">

                    </Box> */}

                    <InventoryFilterBar
                        filterOptions={this.props.filterOptions}
                        handleBoolFilterChange={this.handleBoolFilterChange}
                        handleFilterChange={this.handleFilterChange}
                        searchFilter={this.props.searchFilterProps}
                        visibleFilters={this.props.visibleFilters}
                    />
                    <FilterBarSearchButton handleSearchButtonClick={this.handleSearchButtonClick}/>
                </Box>
            {/* </Box> */}
                {/* </Box> */}
            </Paper>
        );
    }
}






interface InventoryFilterBarProps{
    searchFilter: CardFilterProps,
    visibleFilters: CardFilterVisibilities;
    filterOptions: CoreFilterOptions,
    handleFilterChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleBoolFilterChange: (filter: string, value: boolean) => void;
}

//This bar should just be a flex-grid of filter elements
//It probably shouldn't even have the "search" button
function InventoryFilterBar(props: InventoryFilterBarProps): JSX.Element {
    // Need this to cache?
    // try this?
    // https://material-ui.com/components/autocomplete

    return(
  
        <Box className="flex-section flex-row">
{/* 
            <Box className="flex-section outline-section">
                [x] Exclude Lands
            </Box>

            include unowned / only show owned

            exclude lands (this is so I don't have to see all the gates)

            */}

            {/* exclude lands / include unowned / exclude owned */}
            {/* <Box className="static-section side-padded">
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
            {/* <Box className="flex-section side-padded">
                <TextField
                    name="text"
                    className="stretch"
                    label="Group"
                    value={props.searchFilter.text}
                    onChange={props.handleFilterChange}
                    margin="normal"/>
            </Box> */}

            {/* Sort by name/quantity/price */}
            <Box className="flex-section side-padded">
                    <TextField
                        name="group"
                        className="stretch"
                        select
                        label="Sort by"
                        // value={props.searchFilter.group}
                        onChange={props.handleFilterChange}
                        margin="normal" >
                            {
                                ["name","quantity", "price"].map(
                                    val => <MenuItem key={val} value={val} style={{textTransform: "capitalize"}}>{val}</MenuItem>
                                )
                            }
                        </TextField>
                </Box>
                <Box className="flex-section side-padded">
                    
                    <Box className="outline-section">
                        Filter idea: Hide non-normal variants
                        {/* Can be used when trying to see what I have > 4 or 6 or whatever of */}
                    </Box>
                </Box>

            {/* cardStatus (deck/sellList/buyList/inventory) */}
            <Box className="flex-section side-padded">
                <TextField
                    name="status"
                    className="stretch"
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
            </Box>

            {/* {
                this.props.searchFilter.group != "quantity" &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="group"
                        className="stretch"
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
                // <Box className="flex-section side-padded">
                //     <TextField
                //         name="sort"
                //         className="stretch"
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

            {   //Min Count
                props.visibleFilters.count &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="minCount"
                        className="stretch"
                        label="Min"
                        value={props.searchFilter.minCount}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            }
            {   //Max Count
                props.visibleFilters.count &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="maxCount"
                        className="stretch"
                        label="Max"
                        value={props.searchFilter.maxCount}
                        onChange={props.handleFilterChange}
                        margin="normal"/>
                </Box>
            }
            {/* {   //Format
                props.visibleFilters.format &&
                <Box className="flex-section side-padded">
                    <TextField
                        name="format"
                        className="stretch"
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

            <Box className="flex-section side-padded">
                <TextField
                    name="text"
                    className="stretch"
                    label="Skip"
                    value={props.searchFilter.text}
                    onChange={props.handleFilterChange}
                    margin="normal"/>
            </Box>

            <Box className="flex-section side-padded">
                <TextField
                    name="text"
                    className="stretch"
                    label="Take"
                    value={props.searchFilter.text}
                    onChange={props.handleFilterChange}
                    margin="normal"/>
            </Box>

            
    </Box>);
}

function mapStateToProps(state: AppState): PropsFromState {

    let visibleFilters: CardFilterVisibilities = {
        name: false,
        color: false,
        rarity: false,
        set: false,
        type: false,
        count: false,
        format: false,
        text: false,
    }

    switch(state.app.inventory.searchMethod){
        case "quantity":
            visibleFilters = {
                ...visibleFilters,
                set: true,
                count: true,
                color: true,
                type: true,
                rarity: true,
                format: true,
                text: true,
            }
            break;
        case "name":
            visibleFilters = {
                ...visibleFilters,
            }
            break;
        case "price":
            visibleFilters = {
                ...visibleFilters,
            }
            break;
    }

    const result: PropsFromState = {
        searchFilterProps: state.ui.inventoryFilterProps,
        visibleFilters: visibleFilters,
        filterOptions: state.data.appFilterOptions.filterOptions,
    }
    return result;
}

export default connect(mapStateToProps)(Inventory);



