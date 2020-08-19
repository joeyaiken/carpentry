import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../../reducers'
import { Paper, Box, Tabs, AppBar, Typography, Toolbar, TextField, MenuItem, makeStyles, Button, IconButton } from '@material-ui/core';
import CardFilterBar from './CardFilterBar';
import InventoryCardGrid from './InventoryCardGrid';
import LoadingBox from '../../components/LoadingBox';

import {
    requestInventoryOverviews,
    requestInventoryDetail,
} from '../../actions/inventoryActions'


import {
    // requestInventoryOverviews,
    // requestInventoryDetail,
} from '../../actions/'
import InventoryFilterBar from './InventoryFilterBar';
import { Link } from 'react-router-dom';
import { appStyles } from '../../styles/appStyles';
import { Publish } from '@material-ui/icons';


// import SectionLayout from '../components/SectionLayout';
// import {
//     requestInventoryItems, 
//     inventorySearchMethodChanged,
// } from '../actions/inventory.actions'

// import InventoryFilterBar from './InventoryFilterBar';
// import InventoryOverviews from './InventoryOverviews';
// import InventoryDetailModal from './InventoryDetailModal';

const useStyles = makeStyles({
    
});

interface PropsFromState { 
    // searchMethod: "name" | "quantity" | "price";
    isLoading: boolean;

    searchResults: InventoryOverviewDto[];

    searchFilter: CardFilterProps;
    filterOptions: AppFiltersDto;
    visibleFilters: CardFilterVisibilities;
}

type InventoryProps = PropsFromState & DispatchProp<ReduxAction>;

class InventoryContainer extends React.Component<InventoryProps>{
    constructor(props: InventoryProps) {
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
        this.props.dispatch(requestInventoryDetail(cardId));
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

export default connect(mapStateToProps)(InventoryContainer);