import { connect, DispatchProp } from 'react-redux';
import React from 'react';
import { AppState } from '../reducers'

import {
    requestInventoryDetail,
} from '../actions/inventory.actions'

import InventoryCardGrid from '../components/InventoryCardGrid';
import LoadingBox from '../components/LoadingBox';

interface PropsFromState { 
    isLoading?: boolean;
    searchResults: InventoryOverviewDto[];
}

type InventoryProps = PropsFromState & DispatchProp<ReduxAction>;

class Inventory extends React.Component<InventoryProps>{
    constructor(props: InventoryProps) {
        super(props);
        this.handleCardDetailSelected = this.handleCardDetailSelected.bind(this);
    }

    handleCardDetailSelected(card: string | null){
        this.props.dispatch(requestInventoryDetail(card));
    }

    render() {
        return (
            <React.Fragment>
                { (this.props.isLoading) ? <LoadingBox /> : <InventoryCardGrid cardOverviews={this.props.searchResults} onCardSelected={this.handleCardDetailSelected} /> }
            </React.Fragment>
        );
    }
}

function selectInventoryOverviews(state: AppState): InventoryOverviewDto[] {
    const { byName, allNames } = state.data.inventoryOverviews;
    const result: InventoryOverviewDto[] = allNames.map(id => byName[id]);
    return result;
}

//State
function mapStateToProps(state: AppState): PropsFromState {

    const result: PropsFromState = {
        searchResults: selectInventoryOverviews(state),
        isLoading: state.data.isLoading.inventoryOverview,
    }
    return result;
}

export default connect(mapStateToProps)(Inventory);



