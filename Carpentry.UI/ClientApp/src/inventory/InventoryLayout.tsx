import React from 'react';
import {
} from '@material-ui/core';
import { Route, Switch } from 'react-router-dom';
// import CardSearchContainer from '../containers/CardSearch/CardSearchContainer';
import InventoryOverviewContianer from './inventory-overview/InventoryOverviewContianer';
import CardSearchContainer from '../_containers/CardSearch/CardSearchContainer';

interface LayoutProps {
    
}

export default function InventoryLayout(props: LayoutProps): JSX.Element {
    return (
        <React.Fragment>
            <Switch>
                <Route path="/inventory/addCards" render={(props) => <CardSearchContainer {...props} searchContext="inventory" />} />    
                <Route path="/inventory" component={InventoryOverviewContianer} />
            </Switch>
        </React.Fragment>
    );
}
