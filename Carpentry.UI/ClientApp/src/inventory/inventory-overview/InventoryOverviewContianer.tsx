import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../../_reducers'
import { Paper, Box, Tabs, AppBar, Typography, Toolbar, TextField, MenuItem, makeStyles, Button, IconButton, Tab } from '@material-ui/core';
// import CardFilterBar from './CardFilterBar';
import InventoryCardGrid from './components/InventoryCardGrid';
import LoadingBox from '../../_components/LoadingBox';

import {
    requestInventoryOverviews,
    requestInventoryDetail,
} from '../../_actions/inventoryActions'

import {
    // requestInventoryOverviews,
    // requestInventoryDetail,
} from '../../_actions'
import InventoryFilterBar from './components/InventoryFilterBar';
import { Link, useHistory } from 'react-router-dom';
import { appStyles } from '../../styles/appStyles';
import { Publish } from '@material-ui/icons';

import { push } from 'react-router-redux';
import InventoryDetailContainer from '../inventory-detail/InventoryDetailContainer';

const useStyles = makeStyles({
    
});

interface PropsFromState { 
    // searchMethod: "name" | "quantity" | "price";
    isLoading: boolean;

    viewMethod: "grid" | "table";

    searchResults: InventoryOverviewDto[];

    searchFilter: InventoryFilterProps;
    filterOptions: AppFiltersDto;
    visibleFilters: CardFilterVisibilities;

    visibleSection: "inventory" | "trimmingTips" | "wishlistHelper" | "buylistHelper";
}

type InventoryOverviewProps = PropsFromState & DispatchProp<ReduxAction>;

class InventoryOverviewContainer extends React.Component<InventoryOverviewProps>{
    constructor(props: InventoryOverviewProps) {
        super(props);
        this.handleCardDetailSelected = this.handleCardDetailSelected.bind(this);
        this.handleFilterChange = this.handleFilterChange.bind(this);
        this.handleBoolFilterChange = this.handleBoolFilterChange.bind(this);
        this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
        this.handleExportClick = this.handleExportClick.bind(this);
    }

    componentDidMount() {
        //IDK what exactly would be the right time to call off 
        this.props.dispatch(requestInventoryOverviews());
    }

    // handleSearchTabClick(name: string): void {
    //     this.props.dispatch(inventorySearchMethodChanged(name));
    // }

    handleCardDetailSelected(cardId: number | null){
        console.log(`card selected: ${cardId}`);
        // this.props.dispatch(requestInventoryDetail(cardId));
        this.props.dispatch(push(`/inventory/${cardId}`));

        // let history = useHistory();
        // history.push(`/inventory/${cardId}`)

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

    handleExportClick() {

    }

    render() {
        // const {  flexCol } = appStyles();
        return (
            <React.Fragment>
                {/* <InventoryDetailModal /> */}
                {/* <Box className={flexCol}> */}
                <Box >
                    <AppBar color="default" position="relative">
                        <Toolbar>
                            <Typography variant="h6">
                                Inventory
                            </Typography>

                            <Tabs value={this.props.visibleSection}>
                                <Tab value='inventory' label='Inventory' component={Link} to={'/inventory'} />
                                <Tab value='trimmingTips' label='Trimming Tips' component={Link} to={'/inventory/trimming-tips'} />
                                <Tab value='wishlistHelper' label='Wishlist Helper' component={Link} to={'/inventory/wishlist-helper'} />
                                <Tab value='buylistHelper' label='Buylist Helper' component={Link} to={'/inventory/buylist-helper'} />
                            </Tabs>

                            <Link to={'/inventory/addCards/'}>
                                <Button>Add Cards</Button>
                            </Link>
                            <IconButton size="medium" onClick={this.handleExportClick}><Publish /></IconButton>
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
            <InventoryFilterBar 
                viewMethod={this.props.viewMethod}
                filterOptions={this.props.filterOptions}
                handleBoolFilterChange={this.handleBoolFilterChange}
                handleFilterChange={this.handleFilterChange}
                handleSearchButtonClick={this.handleSearchButtonClick}
                searchFilter={this.props.searchFilter}
                visibleFilters={this.props.visibleFilters}
                />
        );
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

function getFilterVisibilities(groupBy: string): CardFilterVisibilities {
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
    // group by: name | print | unique
    switch(groupBy){
        case "name":
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
        case "print":
            visibleFilters = {
                ...visibleFilters,
            }
            break;
        case "unique":
            visibleFilters = {
                ...visibleFilters,
                set: true,
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

        viewMethod: state.app.inventory.viewMethod,

        searchFilter: state.app.inventory.filters,
        visibleFilters: getFilterVisibilities(state.app.inventory.filters.groupBy),
        filterOptions: state.data.appFilterOptions.filterOptions,
        // searchMethod: state.app.inventory.searchMethod,

        visibleSection: "inventory",

    }
    return result;
}

export default connect(mapStateToProps)(InventoryOverviewContainer);