import React from 'react';
// import {} from '@material-ui/core';
import { Route, Switch } from 'react-router-dom';
import {InventoryOverview} from "./inventory-overview/InventoryOverview";
// import InventoryOverviewContianer from './inventory-overview/InventoryOverviewContianer';
// import CardSearchContainer from '../_containers/CardSearch/CardSearchContainer';
// import TrimmingToolContainer from './trimming-tool/TrimmingToolContainer';
// import InventoryDetailContainer from './inventory-detail/InventoryDetailContainer';
// import AppLayout from '../common/components/AppLayout';
// import CardSearchContainer from '../common/card-search/CardSearchContainer';
// import InventoryAddCardsContainer from './inventory-add-cards/InventoryAddCardsContainer';

export const InventoryLayout = (): JSX.Element => {
  return (
    <React.Fragment>
      <Switch>
        {/*Why was this ever a route up here?*/}
        {/*<Route path="/inventory/addCards" />*/}
      
        {/*<Route exact path="/inventory/trimming-tool" component={TrimmingToolContainer}/>*/}
        {/*<Route exact path="/inventory/:cardId" component={InventoryDetailContainer} />*/}
      
        {/*<Route path="/inventory"/>*/}
      </Switch>
      <Switch>
        {/* <Route path="/inventory/addCards" render={(props) => <CardSearchContainer {...props} searchContext="inventory" />} /> */}
        {/*<Route path="/inventory/addCards" component={InventoryAddCardsContainer} />*/}
        {/* <Route path="/inventory/trimming-tips" render={(props) => <TrimmingToolContainer {...props}  />} /> */}
        
      
        {/*<Route path="/inventory" component={InventoryOverviewContianer} />*/}
        <Route path="/inventory" component={InventoryOverview} />
      </Switch>
    </React.Fragment>
  );
}
