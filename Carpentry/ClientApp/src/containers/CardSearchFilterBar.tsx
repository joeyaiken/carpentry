import { connect, DispatchProp } from 'react-redux';
import React, { ReactNode} from 'react';
import { AppState } from '../reducers';

import { 
    requestCardSearch,
    // cardSearchFilterChanged,
} from '../actions/cardSearch.actions';

import {
    filterValueChanged
} from '../actions/ui.actions';

import CardFilterBar from '../components/CardFilterBar';
import { Paper } from '@material-ui/core';
import FilterBarSearchButton from '../components/FilterBarSearchButton';

interface PropsFromState { 
    searchFilterProps: CardFilterProps;
    visibleFilters: CardFilterVisibilities;
    filterOptions: CoreFilterOptions;
}

type CardSearchProps = PropsFromState & DispatchProp<ReduxAction>;

class CardSearch extends React.Component<CardSearchProps>{
    constructor(props: CardSearchProps) {
        super(props);
        this.handleSearchButtonClick = this.handleSearchButtonClick.bind(this);
        this.handleFilterChange = this.handleFilterChange.bind(this);
        this.handleBoolFilterChange = this.handleBoolFilterChange.bind(this);
    }

    handleSearchButtonClick(){
        this.props.dispatch(requestCardSearch())
    }

    handleFilterChange(event: React.ChangeEvent<HTMLInputElement>): void {
        this.props.dispatch(filterValueChanged("cardSearchFilterProps", event.target.name, event.target.value));
    }

    handleBoolFilterChange(filter: string, value: boolean): void {
        console.log(`search filter bar change filter: ${filter} val: ${value}`)
        this.props.dispatch(filterValueChanged("cardSearchFilterProps", filter, value));
    }

    render() {
        return (
        <React.Fragment>
            <Paper className="outline-section flex-row">
                <CardFilterBar 
                    filterOptions={this.props.filterOptions}
                    handleBoolFilterChange={this.handleBoolFilterChange}
                    handleFilterChange={this.handleFilterChange}
                    
                    searchFilter={this.props.searchFilterProps}
                    visibleFilters={this.props.visibleFilters}
                />
                <FilterBarSearchButton handleSearchButtonClick={this.handleSearchButtonClick}/>

            </Paper>


            
        </React.Fragment>);
    }
}

function mapStateToProps(state: AppState): PropsFromState {
    // console.log(state.cardSearch.inventoryDetail);

    //I'm going to need to map pending card totals to the inventory query result
    
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

    switch(state.app.cardSearch.cardSearchMethod){
        case "inventory":
            visibleFilters = {
                ...visibleFilters,
                format: true,
                color: true,
                type: true,
                set: true,
                rarity: true,
                text: true,
            }
            break;
        case "set":
            visibleFilters = {
                ...visibleFilters,
                color: true,
                rarity: true,
                set: true,
                type: true,
            }
            break;
        case "web":
            visibleFilters = {
                ...visibleFilters,
                name: true,
            }
            break;
    }

    const result: PropsFromState = {
        searchFilterProps: state.ui.cardSearchFilterProps,
        filterOptions: state.data.appFilterOptions.filterOptions,
        visibleFilters: visibleFilters,
    }



    return result;
}

export default connect(mapStateToProps)(CardSearch);
