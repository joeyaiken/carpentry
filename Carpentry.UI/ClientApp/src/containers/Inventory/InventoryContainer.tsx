import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../../reducers'
import { Paper, Box, Tabs, AppBar, Typography, Toolbar, TextField, MenuItem } from '@material-ui/core';
import CardFilterBar from './CardFilterBar';
import InventoryCardGrid from './InventoryCardGrid';
import LoadingBox from '../../components/LoadingBox';

import {
    requestInventoryOverviews,
    requestInventoryDetail,
} from '../../actions/inventory.actions'


import {
    // requestInventoryOverviews,
    // requestInventoryDetail,
} from '../../actions/'


// import SectionLayout from '../components/SectionLayout';
// import {
//     requestInventoryItems, 
//     inventorySearchMethodChanged,
// } from '../actions/inventory.actions'

// import InventoryFilterBar from './InventoryFilterBar';
// import InventoryOverviews from './InventoryOverviews';
// import InventoryDetailModal from './InventoryDetailModal';

interface PropsFromState { 
    // searchMethod: "name" | "quantity" | "price";
    isLoading: boolean;

    searchResults: InventoryOverviewDto[];

    searchFilter: CardFilterProps;
    filterOptions: AppFiltersDto;
    visibleFilters: CardFilterVisibilities;
}

type InventoryProps = PropsFromState & DispatchProp<ReduxAction>;

class Inventory extends React.Component<InventoryProps>{
    constructor(props: InventoryProps) {
        super(props);
        // this.handleSearchTabClick = this.handleSearchTabClick.bind(this);
    }

    componentDidMount() {
        //IDK what exactly would be the right time to call off 
        this.props.dispatch(requestInventoryOverviews());
    }

    // handleSearchTabClick(name: string): void {
    //     this.props.dispatch(inventorySearchMethodChanged(name));
    // }

    handleCardDetailSelected(card: string | null){
        console.log(`card selected: ${card}`);
        this.props.dispatch(requestInventoryDetail(card));
    }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        // this.props.dispatch(filterValueChanged("inventoryFilterProps", event.target.name, event.target.value));
    }

    handleBoolFilterChange(filter: string, value: boolean): void {
        // this.props.dispatch(filterValueChanged("inventoryFilterProps", filter, value));
    }

    handleSearchButtonClick() {
        // this.props.dispatch(requestInventoryItems());
    }

    render() {
        return (
            <React.Fragment>
                {/* <InventoryDetailModal /> */}
                
                <Box className="flex-col">
                    <AppBar color="default" position="relative">
                        <Toolbar>
                            <Typography variant="h6">
                                Inventory
                            </Typography>
                            {/* {
                                props.tabNames &&
                                <Tabs value={props.activeTab} onChange={(e, value) => {props.onTabClick && props.onTabClick(value)}} >
                                    <Tab value={tabName} label={tabName} />

                                    {
                                        props.tabNames.map(tabName =><Tab value={tabName} label={tabName} /> )
                                    }
                                </Tabs>
                            } */}
                        </Toolbar>
                    </AppBar>
                    <Box>
                        { this.renderFilterBar() }
                        { this.renderCardOverviews() }
                    </Box>
                </Box>
            </React.Fragment>
        );
    }

    renderFilterBar() {
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
                                onChange={this.handleFilterChange}
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
                            onChange={this.handleFilterChange}
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
                        this.props.visibleFilters.count &&
                        <Box className="flex-section side-padded">
                            <TextField
                                name="minCount"
                                className="stretch"
                                label="Min"
                                value={this.props.searchFilter.minCount}
                                onChange={this.handleFilterChange}
                                margin="normal"/>
                        </Box>
                    }
                    {   //Max Count
                        this.props.visibleFilters.count &&
                        <Box className="flex-section side-padded">
                            <TextField
                                name="maxCount"
                                className="stretch"
                                label="Max"
                                value={this.props.searchFilter.maxCount}
                                onChange={this.handleFilterChange}
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
                            value={this.props.searchFilter.text}
                            onChange={this.handleFilterChange}
                            margin="normal"/>
                    </Box>
        
                    <Box className="flex-section side-padded">
                        <TextField
                            name="text"
                            className="stretch"
                            label="Take"
                            value={this.props.searchFilter.text}
                            onChange={this.handleFilterChange}
                            margin="normal"/>
                    </Box>
        
                    
            </Box>);
    }

    renderCardOverviews() {
        return (
            <React.Fragment>
                { (this.props.isLoading) ? <LoadingBox /> : <InventoryCardGrid cardOverviews={this.props.searchResults} onCardSelected={this.handleCardDetailSelected} /> }
            </React.Fragment>
        );
    }
}

function selectInventoryOverviews(state: AppState): InventoryOverviewDto[] {
    const { byId, allIds } = state.data.inventory.overviews;
    const result: InventoryOverviewDto[] = allIds.map(id => byId[id]);
    return result;
}

function getFilterVisibilities(searchMethod: string): CardFilterVisibilities {
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

    switch(searchMethod){
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

    return visibleFilters;
}

//State
function mapStateToProps(state: AppState): PropsFromState {
    // console.log('---state-----');
    // console.log(state);
    const result: PropsFromState = {
        searchResults: selectInventoryOverviews(state),
        
        // isLoading: state.data.
        isLoading: state.data.inventory.overviews.isLoading,


        searchFilter: state.ui.inventoryFilterProps,
        visibleFilters: getFilterVisibilities(state.app.inventory.searchMethod),
        filterOptions: state.data.appFilterOptions.filterOptions,
        // searchMethod: state.app.inventory.searchMethod,
    }
    return result;
}

export default connect(mapStateToProps)(Inventory);



