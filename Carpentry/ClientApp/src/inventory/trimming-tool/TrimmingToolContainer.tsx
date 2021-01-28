import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { Box, Tabs, AppBar, Typography, Toolbar, Button, IconButton, Tab, CardHeader, Card, CardContent, Dialog, DialogActions, DialogContent, DialogTitle } from '@material-ui/core';
// import CardFilterBar from './CardFilterBar';
// import InventoryCardGrid from './components/InventoryCardGrid';
// import LoadingBox from '../../_components/LoadingBox';

// import {
//     requestTrimmingTips,
//     requestInventoryDetail,
// } from '../../_actions/inventoryActions'


// import InventoryFilterBar from './components/InventoryFilterBar';
import { Link } from 'react-router-dom';
// import { appStyles } from '../../styles/appStyles';
import { Publish } from '@material-ui/icons';

import { push } from 'react-router-redux';
import TrimmingTipsTable from './components/TrimmingTipsTable';
import {
    ensureTrimmingTipsLoaded,
} from './state/TrimmingTipsActions';
import { AppState } from '../../configureStore';
import TrimmingToolLayout from './TrimmingToolLayout';

interface PropsFromState { 
    // // searchMethod: "name" | "quantity" | "price";
    // isLoading: boolean;

    // viewMethod: "grid" | "table";

    // searchResults: TrimmingTipsDto[];

    modalIsOpen: boolean;


    trimmingTipsResults: InventoryOverviewDto[];
    // searchFilter: InventoryFilterProps;
    filterOptions: AppFiltersDto;
    // visibleFilters: CardFilterVisibilities;

    
    pendingCards: { [key:number]: PendingCardsDto }
}

type TrimmingTipsProps = PropsFromState & DispatchProp<ReduxAction>;

class TrimmingTipsContainer extends React.Component<TrimmingTipsProps>{
    constructor(props: TrimmingTipsProps) {
        super(props);
        this.handleFilterChange = this.handleFilterChange.bind(this);
        this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
        this.handleCloseModalClick = this.handleCloseModalClick.bind(this);
        this.handleAddPendingCard = this.handleAddPendingCard.bind(this);
        this.handleRemovePendingCard = this.handleRemovePendingCard.bind(this);
    }

    componentDidMount() {
        //IDK what exactly would be the right time to call off 
        // this.props.dispatch(requestTrimmingTips());
        this.props.dispatch(ensureTrimmingTipsLoaded());
    }

    // handleSearchTabClick(name: string): void {
    //     this.props.dispatch(inventorySearchMethodChanged(name));
    // }

    // handleCardDetailSelected(cardId: number | null){
    //     console.log(`card selected: ${cardId}`);
    //     // this.props.dispatch(requestInventoryDetail(cardId));
    //     this.props.dispatch(push(`/inventory/${cardId}`));

    //     // let history = useHistory();
    //     // history.push(`/inventory/${cardId}`)

    // }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        // this.props.dispatch(filterValueChanged("inventoryFilterProps", event.target.name, event.target.value));
    }

    // handleBoolFilterChange(filter: string, value: boolean): void {
    //     // this.props.dispatch(filterValueChanged("inventoryFilterProps", filter, value));
    // }

    handleSearchButtonClick() {
        // this.props.dispatch(requestInventoryItems());
        // this.props.dispatch(requestInventoryOverviews());
    }

    handleCloseModalClick(){
        this.props.dispatch(push('/inventory'));
    }

    handleAddPendingCard(name: string, cardId: number, isFoil: boolean){
        // this.props.dispatch(addPendingCard(name, cardId, isFoil));
    }

    handleRemovePendingCard(name: string, cardId: number, isFoil: boolean){
        // this.props.dispatch(removePendingCard(name, cardId, isFoil));
    }

    render() {
        return (
            <React.Fragment>
                <Dialog open={this.props.modalIsOpen} onClose={() => {this.handleCloseModalClick()}} >
                    <DialogTitle>{`Trimming Tool`}</DialogTitle>
                    <DialogContent>
                        <TrimmingToolLayout
                            cards={this.props.trimmingTipsResults}

                            pendingCards={[]}
                            
                            //onSaveClick

                            
                            onSearchClick={this.handleSearchButtonClick}

                            filterOptions={this.props.filterOptions}

                            onAddPendingCard={this.handleAddPendingCard}
                            onRemovePendingCard={this.handleRemovePendingCard}


                            onFilterChange={this.handleFilterChange}

                        //  selectedDetailItem={this.props.selectedDetailItem}
                        // selectedCardId={this.props.selectedCardId}
                        // cardGroups={this.props.cardGroups}
                        // cardsById={this.props.cardsById}
                        // allCardIds={this.props.allCardIds}
                        // inventoryCardsById={this.props.inventoryCardsById}
                         />
                    </DialogContent>
                    <DialogActions>
                        <Button size="medium" onClick={() => this.handleCloseModalClick()}>Close</Button>
                    </DialogActions>
                </Dialog>
            </React.Fragment>
        );
    }

    // renderFilterBar() {
    //     // Need this to cache?
    //     // try this?
    //     // https://material-ui.com/components/autocomplete
    //     return(
    //         <InventoryFilterBar 
    //             viewMethod={this.props.viewMethod}
    //             filterOptions={this.props.filterOptions}
    //             handleBoolFilterChange={this.handleBoolFilterChange}
    //             handleFilterChange={this.handleFilterChange}
    //             handleSearchButtonClick={this.handleSearchButtonClick}
    //             searchFilter={this.props.searchFilter}
    //             visibleFilters={this.props.visibleFilters}
    //             />
    //     );
    // }

    // renderCardOverviews() {
    //     return (
    //         <React.Fragment>
    //             { (this.props.isLoading) ? <LoadingBox /> : <InventoryCardGrid cardOverviews={this.props.searchResults} onCardSelected={this.handleCardDetailSelected} /> }
    //         </React.Fragment>
    //     );
    // }
}

// function selectTrimmingTips(state: AppState): TrimmingTipsDto[] {
//     const { byId, allIds } = state.data.inventory.overviews;
//     const result: TrimmingTipsDto[] = allIds.map(id => byId[id]);
//     return result;
// }

// function getFilterVisibilities(groupBy: string): CardFilterVisibilities {
//     let visibleFilters: CardFilterVisibilities = {
//         name: false,
//         color: false,
//         rarity: false,
//         set: false,
//         type: false,
//         count: false,
//         format: false,
//         text: false,
//     }
//     // group by: name | print | unique
//     switch(groupBy){
//         case "name":
//             visibleFilters = {
//                 ...visibleFilters,
//                 set: true,
//                 count: true,
//                 color: true,
//                 type: true,
//                 rarity: true,
//                 format: true,
//                 text: true,
//             }
//             break;
//         case "print":
//             visibleFilters = {
//                 ...visibleFilters,
//             }
//             break;
//         case "unique":
//             visibleFilters = {
//                 ...visibleFilters,
//                 set: true,
//             }
//             break;
//     }

//     return visibleFilters;
// }


//State
function mapStateToProps(state: AppState): PropsFromState {
    // console.log('---state-----');
    // console.log(state);
    const result: PropsFromState = {
        modalIsOpen: true,
        // searchResults: selectTrimmingTips(state),
        
        // // isLoading: state.data.
        // isLoading: state.data.inventory.overviews.isLoading,

        // viewMethod: state.app.inventory.viewMethod,

        // searchFilter: state.app.inventory.filters,
        // visibleFilters: getFilterVisibilities(state.app.inventory.filters.groupBy),
        filterOptions: state.core.data.filterOptions,
        // // searchMethod: state.app.inventory.searchMethod,
        trimmingTipsResults: [],
        // visibleSection: "trimmingTips",
        pendingCards: [],
    }
    return result;
}




export default connect(mapStateToProps)(TrimmingTipsContainer);