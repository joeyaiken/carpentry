import React from 'react';
// import {} from '@material-ui/core';
import { Route, Switch } from 'react-router-dom';
import InventoryOverviewContianer from './inventory-overview/InventoryOverviewContianer';
// import CardSearchContainer from '../_containers/CardSearch/CardSearchContainer';
import TrimmingTipsContainer from './trimming-tips/TrimmingTipsContainer';
import InventoryDetailContainer from './inventory-detail/InventoryDetailContainer';

interface LayoutProps {
    
}

export default function InventoryLayout(props: LayoutProps): JSX.Element {
    return (
        <React.Fragment>
            <Switch>
                <Route path="/inventory/:cardId" component={InventoryDetailContainer} />
                <Route path="/inventory"/>
            </Switch>
            <Switch>
                {/* <Route path="/inventory/addCards" render={(props) => <CardSearchContainer {...props} searchContext="inventory" />} /> */}
                <Route path="/inventory/trimming-tips" render={(props) => <TrimmingTipsContainer {...props}  />} />
                <Route path="/inventory" component={InventoryOverviewContianer} />
            </Switch>
        </React.Fragment>
    );
}