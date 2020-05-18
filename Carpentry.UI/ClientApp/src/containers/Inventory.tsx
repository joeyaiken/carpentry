import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../reducers'
import { Paper, Box, Tabs, AppBar, Typography, Toolbar } from '@material-ui/core';

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
}

type InventoryProps = PropsFromState & DispatchProp<ReduxAction>;

class Inventory extends React.Component<InventoryProps>{
    constructor(props: InventoryProps) {
        super(props);
        // this.handleSearchTabClick = this.handleSearchTabClick.bind(this);
    }

    // componentDidMount() {
    //     //IDK what exactly would be the right time to call off 
    //     this.props.dispatch(requestInventoryItems());
    // }

    // handleSearchTabClick(name: string): void {
    //     this.props.dispatch(inventorySearchMethodChanged(name));
    // }

    render() {
        return (
            <React.Fragment>
                {/* <InventoryDetailModal /> */}
                
                <Box className="flex-col">
                    {/* <AppBar color="default" position="relative">
                        <Toolbar>
                            <Typography variant="h6">
                                {props.title}
                            </Typography>
                            {
                                props.tabNames &&
                                <Tabs value={props.activeTab} onChange={(e, value) => {props.onTabClick && props.onTabClick(value)}} >
                                    {
                                        props.tabNames.map(tabName =><Tab value={tabName} label={tabName} /> )
                                    }
                                </Tabs>
                            }
                        </Toolbar>
                    </AppBar> */}
                    <Box>
                        { this.renderFilterBar() }
                        { this.renderCardOverviews() }
                    </Box>
                </Box>
            </React.Fragment>
        );
    }

    renderFilterBar() {
        // return(

        // );
    }

    renderCardOverviews() {
        return (
            <React.Fragment>
                {/* { (this.props.isLoading) ? <LoadingBox /> : <InventoryCardGrid cardOverviews={this.props.searchResults} onCardSelected={this.handleCardDetailSelected} /> } */}
            </React.Fragment>
        );
    }
}

//State
function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
        // isLoading: state.data.
        isLoading: false,
        // searchMethod: state.app.inventory.searchMethod,
    }
    return result;
}

export default connect(mapStateToProps)(Inventory);



