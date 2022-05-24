import React, {useEffect} from 'react';
// import AppLayout from "../../common/components/AppLayout";
import {AppBar, Box, Button, Toolbar, Typography} from "@material-ui/core";
import {InventoryFilterBar} from "./components/InventoryFilterBar";
// import CardImagePopper from "../../common/components/CardImagePopper";
// import LoadingBox from "../../common/components/LoadingBox";
import InventoryCardGrid from "./components/InventoryCardGrid";
import {useAppDispatch, useAppSelector} from "../../../app/hooks";
import {ApiStatus} from "../../../enums";
import {AppLayout} from "../../../common/components/AppLayout";
import {CardImagePopper} from "../../../common/components/CardImagePopper";
import {LoadingBox} from "../../../common/components/LoadingBox";
import {useHistory} from "react-router";
import {getInventoryOverviews} from "./inventoryOverviewSlice";

// import { connect, DispatchProp } from 'react-redux';
// import React from 'react';
// import { AppState } from '../../configureStore'
// import { Box, Tabs, AppBar, Typography, Toolbar, Button, IconButton, Tab, Menu, Card, CardMedia, CardContent, Paper, Popper } from '@material-ui/core';
// import InventoryCardGrid from './components/InventoryCardGrid';
// import InventoryFilterBar from './components/InventoryFilterBar';
// import { Link } from 'react-router-dom';
// import { Publish } from '@material-ui/icons';
// import { push } from 'react-router-redux';
// import { requestInventoryOverviews } from '../state/InventoryDataActions';
// import LoadingBox from '../../common/components/LoadingBox';
// import { cardMenuButtonClick, inventoryOverviewFilterChanged, quickFilterApplied } from './state/InventoryOverviewActions';
// import { appStyles } from '../../styles/appStyles';
// import { group } from 'console';
// import CardImagePopper from '../../common/components/CardImagePopper';
// import AppLayout from "../../common/components/AppLayout";

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

// type InventoryOverviewProps = PropsFromState & DispatchProp<ReduxAction>;
//

export const InventoryOverview = (): JSX.Element => {
  const dispatch = useAppDispatch();
  const history = useHistory();
  
//   componentDidMount() {
//     //IDK what exactly would be the right time to call off 
//     this.props.dispatch(requestInventoryOverviews());
//   }

  const overviewDataStatus = useAppSelector(state => state.inventory.overviews.data.status);
  
  const existingFilters = useAppSelector(state => state.inventory.overviews.filters);
  
  useEffect(() => {
    if(overviewDataStatus === ApiStatus.uninitialized) {
      const param: InventoryQueryParameter = {
        groupBy: existingFilters.groupBy,
        text: existingFilters.text,
        colors: existingFilters.colorIdentity,
        skip: +existingFilters.skip,
        take: +existingFilters.take,
        sort: existingFilters.sortBy,
        sortDescending: existingFilters.sortDescending,
        set: existingFilters.set,
        exclusiveColorFilters: existingFilters.exclusiveColorFilters,
        multiColorOnly: existingFilters.multiColorOnly,
        maxCount: +existingFilters.maxCount,
        minCount: +existingFilters.minCount,
        type: existingFilters.type,
        rarity: existingFilters.rarity,
      }
      dispatch(getInventoryOverviews(param));
    }
  })
  
//   // handleSearchTabClick(name: string): void {
//   //     this.props.dispatch(inventorySearchMethodChanged(name));
//   // }

//   handleCardDetailSelected(cardId: number | null){
//     // console.log(`card selected: ${cardId}`);
//     this.props.dispatch(push(`/inventory/${cardId}`));
//   }

//   handleCardDetailButtonClick(event: React.MouseEvent<HTMLButtonElement, MouseEvent>) {
//     this.props.dispatch(cardMenuButtonClick(event.currentTarget));
//   }

//   handleCardDetailMenuClose() {
//     this.props.dispatch(cardMenuButtonClick(null));
//   }

//   handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
//     // this.props.dispatch(inventoryOverviewFilterChanged("inventoryFilterProps", event.target.name, event.target.value));
//     this.props.dispatch(inventoryOverviewFilterChanged(event.target.name, event.target.value));
//   }

//   handleBoolFilterChange(filter: string, value: boolean): void {
//     // this.props.dispatch(filterValueChanged("inventoryFilterProps", filter, value));
//     this.props.dispatch(inventoryOverviewFilterChanged(filter, value));
//   }

//   handleSearchButtonClick() {
//     // this.props.dispatch(requestInventoryItems());
//     this.props.dispatch(requestInventoryOverviews());
//   }

//   // handleExportClick() {
//
//   // }

  const handleQuickFilterClick = (filter: string): void => {
//     //apply filter changes
//     //  do I just start applying props to the existing filter? Clear everything?
//     let newFilter =  {...this.props.searchFilter};
//
//     switch(filter){
//       case "Most Expensive": //by unique, price descending
//         newFilter = {
//           ...newFilter,
//           groupBy: 'unique',
//           sortBy: 'price',
//           sortDescending: true,
//         }
//         break;
//       case "Highest Count": //by name, owned count descending
//         newFilter = {
//           ...newFilter,
//           groupBy: 'name',
//           sortBy: 'count',
//           sortDescending: true,
//         }
//         break;
//       case "Owned Cards": //by name, by name, where MinCount == 1
//         newFilter = {
//           ...newFilter,
//           groupBy: 'name',
//           sortBy: 'name',
//           sortDescending: false,
//           minCount: 1,
//         }
//         break;
//       case "Clear Secondary": //
//         newFilter = {
//           ...newFilter,
//           set: "",
//           type: "",
//           colorIdentity: [],
//           exclusiveColorFilters: false,
//           multiColorOnly: false,
//           text: "",
//           rarity: [],
//         }
//         break;
//     }
//
//     this.props.dispatch(quickFilterApplied(newFilter));
  }

  const handleAddCardsClick = (): void =>
    history.push('/inventory/add-cards/');
  

  const handleTrimmingToolClick = (): void =>
    history.push('/inventory/trimming-tool');

//   render() {
//     // const {  flexCol } = appStyles();
  
  const isLoading = useAppSelector(state => state.inventory.overviews.data.status == ApiStatus.loading);

  const renderCardOverviews = (): JSX.Element => {
    // const cardImageAnchorId = this.props.cardImageMenuAnchor?.value;
    // const selectedOverview: InventoryOverviewDto = this.props.searchResultsById[cardImageAnchorId ?? 0];
    return (
      <React.Fragment>
        {/*<CardImagePopper*/}
        {/*  menuAnchor={this.props.cardImageMenuAnchor}*/}
        {/*  onClose={this.handleCardDetailMenuClose}*/}
        {/*  image={selectedOverview?.imageUrl}*/}
        {/*/>*/}
        { (isLoading) ? <LoadingBox /> : <InventoryCardGrid /> }
      </React.Fragment>
    );
  }
  
  return (
    <AppLayout title="Inventory" isLoading={isLoading}>
      {/*<React.Fragment>*/}
      {/* <InventoryDetailModal /> */}
      {/* <Box className={flexCol}> */}
      <Box >
        <AppBar color="default" position="relative">
          <Toolbar>
            <Typography variant="h6">
              Inventory
            </Typography>
            <QuickFilterButton name="Most Expensive" onClick={()=> handleQuickFilterClick("Most Expensive")} />
            <QuickFilterButton name="Highest Count" onClick={()=> handleQuickFilterClick("Highest Count")} />
            <QuickFilterButton name="Owned Cards" onClick={()=> handleQuickFilterClick("Owned Cards")} />

            <QuickFilterButton name="Clear Secondary" secondary={true} onClick={()=>handleQuickFilterClick("Clear Secondary")} />
            
            {/* <Link to={'/inventory/addCards/'}> */}
            <Button onClick={handleAddCardsClick}>Add Cards</Button>
            {/* </Link> */}

            <Button onClick={handleTrimmingToolClick}>Trimming Tool</Button>

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
          <InventoryFilterBar />
          { renderCardOverviews() }
        </Box>
      </Box>
      {/*</React.Fragment>*/}
    </AppLayout>
  );
  // }
//
  
}

interface QuickFilterProps {
  name: string;
  secondary?: boolean;
  onClick?: () => void;
}

const QuickFilterButton = (props: QuickFilterProps): JSX.Element => {
  return(
    <Button color={Boolean(props.secondary) ? "secondary" : "primary"} variant="outlined" size="small" style={{textTransform:"none"}} onClick={props.onClick}>
      {props.name}
    </Button>
  );
}

//
// //Oh I cleaned up the deail when I should really clean up this too
// // function selectInventoryOverviews(state: AppState): InventoryOverviewDto[] {
// //     const { byId, allIds } = state.inventory.data.overviews;
// //     const result: InventoryOverviewDto[] = allIds.map(id => byId[id]);
// //     return result;
// // }
//
//
// //State
// function mapStateToProps(state: AppState): PropsFromState {
//
//   const result: PropsFromState = {
//     // searchResults: selectInventoryOverviews(state),
//     searchResultsById: state.inventory.data.overviews.byId,
//     searchReusltIds: state.inventory.data.overviews.allIds,
//
//     // isLoading: state.data.
//     isLoading: state.inventory.data.overviews.isLoading,
//
//     viewMethod: state.inventory.overviews.viewMethod,
//
//     searchFilter: state.inventory.overviews.filters,
//     // visibleFilters: getFilterVisibilities(state.inventory.overviews.filters.groupBy),
//     filterOptions: state.core.data.filterOptions,
//     // searchMethod: state.app.inventory.searchMethod,
//
//     visibleSection: "inventory",
//
//     //
//     cardImageMenuAnchor: state.inventory.overviews.cardImageMenuAnchor,
//     // cardImageMenuId: null,
//     // cardImageMenuId: state.inventory.overviews.cardImageMenuAnchor?.value,
//     //cardImageMenu
//   }
//   return result;
// }

// export default connect(mapStateToProps)(InventoryOverviewContainer);