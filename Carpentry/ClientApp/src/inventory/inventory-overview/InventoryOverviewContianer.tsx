import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../../configureStore'
import { Box, Tabs, AppBar, Typography, Toolbar, Button, IconButton, Tab, Menu, Card, CardMedia, CardContent, Paper, Popper } from '@material-ui/core';
import InventoryCardGrid from './components/InventoryCardGrid';
import InventoryFilterBar from './components/InventoryFilterBar';
import { Link } from 'react-router-dom';
import { Publish } from '@material-ui/icons';
import { push } from 'react-router-redux';
import { requestInventoryOverviews } from '../state/InventoryDataActions';
import LoadingBox from '../../common/components/LoadingBox';
import { cardMenuButtonClick, inventoryOverviewFilterChanged } from './state/InventoryOverviewActions';
import { appStyles } from '../../styles/appStyles';

interface PropsFromState { 
    // searchMethod: "name" | "quantity" | "price";
    isLoading: boolean;

    viewMethod: "grid" | "table";

    // searchResults: InventoryOverviewDto[];

    searchResultsById: { [key: number]: InventoryOverviewDto }
    searchReusltIds: number[];

    searchFilter: InventoryFilterProps;
    filterOptions: AppFiltersDto;
    // visibleFilters: CardFilterVisibilities;

    visibleSection: "inventory" | "trimmingTips" | "wishlistHelper" | "buylistHelper";
    
    //
    cardImageMenuAnchor: HTMLButtonElement | null;
    // cardImageMenuId: number | null;
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
        this.handleCardDetailButtonClick = this.handleCardDetailButtonClick.bind(this);
        this.handleCardDetailMenuClose = this.handleCardDetailMenuClose.bind(this);
    }

    componentDidMount() {
        //IDK what exactly would be the right time to call off 
        this.props.dispatch(requestInventoryOverviews());
    }

    // handleSearchTabClick(name: string): void {
    //     this.props.dispatch(inventorySearchMethodChanged(name));
    // }

    handleCardDetailSelected(cardId: number | null){
        // console.log(`card selected: ${cardId}`);
        this.props.dispatch(push(`/inventory/${cardId}`));
    }

    handleCardDetailButtonClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
        this.props.dispatch(cardMenuButtonClick(event.currentTarget));
    }

    handleCardDetailMenuClose() {
        this.props.dispatch(cardMenuButtonClick(null));
    }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        // this.props.dispatch(inventoryOverviewFilterChanged("inventoryFilterProps", event.target.name, event.target.value));
        this.props.dispatch(inventoryOverviewFilterChanged(event.target.name, event.target.value));
    }

    handleBoolFilterChange(filter: string, value: boolean): void {
        // this.props.dispatch(filterValueChanged("inventoryFilterProps", filter, value));
        this.props.dispatch(inventoryOverviewFilterChanged(filter, value));
    }

    handleSearchButtonClick() {
        // this.props.dispatch(requestInventoryItems());
        this.props.dispatch(requestInventoryOverviews());
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
                // visibleFilters={this.props.visibleFilters}
                />
        );
    }

    renderCardOverviews() {
        const cardImageAnchorId = this.props.cardImageMenuAnchor?.value;
        const selectedOverview: InventoryOverviewDto = this.props.searchResultsById[cardImageAnchorId ?? 0];
        return (
            <React.Fragment>
                <CardImagePopper 
                    menuAnchor={this.props.cardImageMenuAnchor}
                    onClose={this.handleCardDetailMenuClose}
                    image={selectedOverview?.img}
                    />
                { (this.props.isLoading) ? <LoadingBox /> : <InventoryCardGrid 
                // cardOverviews={this.props.searchResults}
                cardOverviewIds={this.props.searchReusltIds}
                cardOverviewsById={this.props.searchResultsById}
                onCardSelected={this.handleCardDetailSelected} 
                
                onInfoButtonEnter={this.handleCardDetailButtonClick}
                onInfoButtonLeave={this.handleCardDetailMenuClose} /> }
            </React.Fragment>
        );
    }
}

interface CardImagePopperProps { 
    menuAnchor: HTMLElement | null;
    image: string;
    onClose: () => void;
}

function CardImagePopper(props: CardImagePopperProps): JSX.Element {
    const classes = appStyles();
    return (
        <Popper open={Boolean(props.menuAnchor)} anchorEl={props.menuAnchor}>
            <Card>
                <CardMedia style={{height:"310px", width: "223px"}} image={props.image} />
                {/* <CardContent className={classes.flexSection}>
                    <Box className={classes.flexCol}>
                        <Box className={classes.flexRow}>
                            {cardItem.count && (<Typography>{cardItem.count} Total {cardItem.isFoil && " - (FOIL)"}</Typography>)}
                        </Box>
                        <Box className={classes.flexRow}>
                            {cardItem.price && (<Typography>${cardItem.price}</Typography>)}
                        </Box>
                    </Box>
                </CardContent> */}
            </Card>
        </Popper>
    );
}

//Oh I cleaned up the deail when I should really clean up this too
// function selectInventoryOverviews(state: AppState): InventoryOverviewDto[] {
//     const { byId, allIds } = state.inventory.data.overviews;
//     const result: InventoryOverviewDto[] = allIds.map(id => byId[id]);
//     return result;
// }


//State
function mapStateToProps(state: AppState): PropsFromState {

    const result: PropsFromState = {
        // searchResults: selectInventoryOverviews(state),
        searchResultsById: state.inventory.data.overviews.byId,
        searchReusltIds: state.inventory.data.overviews.allIds,
        
        // isLoading: state.data.
        isLoading: state.inventory.data.overviews.isLoading,

        viewMethod: state.inventory.overviews.viewMethod,

        searchFilter: state.inventory.overviews.filters,
        // visibleFilters: getFilterVisibilities(state.inventory.overviews.filters.groupBy),
        filterOptions: state.core.data.filterOptions,
        // searchMethod: state.app.inventory.searchMethod,

        visibleSection: "inventory",

        //
        cardImageMenuAnchor: state.inventory.overviews.cardImageMenuAnchor,
        // cardImageMenuId: null,
        // cardImageMenuId: state.inventory.overviews.cardImageMenuAnchor?.value,
        //cardImageMenu
    }
    return result;
}

export default connect(mapStateToProps)(InventoryOverviewContainer);