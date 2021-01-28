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
import { cardMenuButtonClick, inventoryOverviewFilterChanged, quickFilterApplied } from './state/InventoryOverviewActions';
import { appStyles } from '../../styles/appStyles';
import { group } from 'console';

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
        // this.handleExportClick = this.handleExportClick.bind(this);
        this.handleCardDetailButtonClick = this.handleCardDetailButtonClick.bind(this);
        this.handleCardDetailMenuClose = this.handleCardDetailMenuClose.bind(this);
        this.handleQuickFilterClick = this.handleQuickFilterClick.bind(this);
        this.handleAddCardsClick = this.handleAddCardsClick.bind(this);
        this.handleTrimmingToolClick = this.handleTrimmingToolClick.bind(this);
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

    // handleExportClick() {

    // }

    handleQuickFilterClick(filter: string) {
        //apply filter changes
        //  do I just start applying props to the existing filter? Clear everything?
        let newFilter =  {...this.props.searchFilter};

        switch(filter){
            case "Most Expensive": //by unique, price descending
                newFilter = {
                    ...newFilter,
                    groupBy: 'unique',
                    sortBy: 'price',
                    sortDescending: true,
                }
                break;
            case "Highest Count": //by name, owned count descending
                newFilter = {
                    ...newFilter,
                    groupBy: 'name',
                    sortBy: 'count',
                    sortDescending: true,
                }
                break;
            case "Owned Cards": //by name, by name, where MinCount == 1
                newFilter = {
                    ...newFilter,
                    groupBy: 'name',
                    sortBy: 'name',
                    sortDescending: false,
                    minCount: 1,
                }
                break;
            case "Clear Secondary": //
                newFilter = {
                    ...newFilter,
                    set: "",
                    type: "",
                    colorIdentity: [],
                    exclusiveColorFilters: false,
                    multiColorOnly: false,
                    text: "",
                    rarity: [],
                }
                break;
        }

        this.props.dispatch(quickFilterApplied(newFilter));
    }

    handleAddCardsClick() {
        this.props.dispatch(push('/inventory/addCards/')); //todo - this should be renamed to add-cards
    }

    handleTrimmingToolClick() {
        this.props.dispatch(push('/inventory/trimming-tool'))
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
                            <QuickFilter name="Most Expensive" onClick={()=>this.handleQuickFilterClick("Most Expensive")} /> {/* by unique, price descending */}
                            <QuickFilter name="Highest Count" onClick={()=>this.handleQuickFilterClick("Highest Count")} />{/*  */}
                            <QuickFilter name="Owned Cards" onClick={()=>this.handleQuickFilterClick("Owned Cards")} />{/*  */}

                            <QuickFilter name="Clear Secondary" secondary={true} onClick={()=>this.handleQuickFilterClick("Clear Secondary")} />
                            {/* <QuickFilter name="Wish List" onClick={()=>this.handleQuickFilterClick("")} />
                            <QuickFilter name="Sell List" onClick={()=>this.handleQuickFilterClick("")} /> */}
                            {/* <Button>(Trimming Tips replacement)</Button>
                            <Button>(Wishlist Helper replacement)</Button>
                            <Button>( replacement)</Button>
                            <Button>( replacement)</Button> */}

                            {/* <Tabs value={this.props.visibleSection}>
                                <Tab value='inventory' label='Inventory' component={Link} to={'/inventory'} />
                                <Tab value='trimmingTips' label='Trimming Tips' component={Link} to={'/inventory/trimming-tips'} />
                                <Tab value='wishlistHelper' label='Wishlist Helper' component={Link} to={'/inventory/wishlist-helper'} />
                                <Tab value='buylistHelper' label='Buylist Helper' component={Link} to={'/inventory/buylist-helper'} />
                            </Tabs> */}

                            {/* <Link to={'/inventory/addCards/'}> */}
                                <Button onClick={this.handleAddCardsClick}>Add Cards</Button>
                            {/* </Link> */}

                            <Button onClick={this.handleTrimmingToolClick}>Trimming Tool</Button>
                            
                            {/* <IconButton size="medium" onClick={this.handleExportClick}><Publish /></IconButton> */}
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
                    image={selectedOverview?.imageUrl}
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

interface QuickFilterProps {
    name: string;
    secondary?: boolean;
    onClick?: () => void;
}

function QuickFilter(props: QuickFilterProps): JSX.Element {
    return(
        <Button color={Boolean(props.secondary) ? "secondary" : "primary"} variant="outlined" size="small" style={{textTransform:"none"}} onClick={props.onClick}>
            {props.name}
        </Button>
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