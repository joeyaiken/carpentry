import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../reducers'

import SectionLayout from '../components/SectionLayout';
import {
    requestInventoryItems, 
    inventorySearchMethodChanged,
} from '../actions/inventory.actions'

import InventoryFilterBar from './InventoryFilterBar';
import InventoryOverviews from './InventoryOverviews';
import InventoryDetailModal from './InventoryDetailModal';

interface PropsFromState { 
    searchMethod: "name" | "quantity" | "price";
}

type InventoryProps = PropsFromState & DispatchProp<ReduxAction>;

class Inventory extends React.Component<InventoryProps>{
    constructor(props: InventoryProps) {
        super(props);
        this.handleSearchTabClick = this.handleSearchTabClick.bind(this);
    }

    componentDidMount() {
        //IDK what exactly would be the right time to call off 
        this.props.dispatch(requestInventoryItems());
    }

    handleSearchTabClick(name: string): void {
        this.props.dispatch(inventorySearchMethodChanged(name));
    }

    render() {
        return (
            <React.Fragment>
                <InventoryDetailModal />
                <SectionLayout 
                    title="Inventory"
                    tabNames={["name", "quantity", "price"]}
                    activeTab={this.props.searchMethod}
                    onTabClick={this.handleSearchTabClick}
                    >
                    <InventoryFilterBar />
                    <InventoryOverviews />
                </SectionLayout>
            </React.Fragment>
        );
    }
}

//State
function mapStateToProps(state: AppState): PropsFromState {
    const result: PropsFromState = {
        searchMethod: state.app.inventory.searchMethod,
    }
    return result;
}

export default connect(mapStateToProps)(Inventory);



