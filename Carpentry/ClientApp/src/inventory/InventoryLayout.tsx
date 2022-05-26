import React from 'react';
import { Route, Switch } from 'react-router-dom';
import InventoryOverviewContainer from './inventory-overview/InventoryOverviewContianer';
// import CardSearchContainer from '../_containers/CardSearch/CardSearchContainer';
import InventoryDetailContainer from './inventory-detail/InventoryDetailContainer';
// import CardSearchContainer from '../common/card-search/CardSearchContainer';
import InventoryAddCardsContainer from './inventory-add-cards/InventoryAddCardsContainer';
import {TrimmingTool} from "../features/inventory/trimming-tool/TrimmingTool";

export default function InventoryLayout(): JSX.Element {
  return (
    <React.Fragment>
      <Switch>
        <Route path="/inventory/addCards" />
        <Route path="/inventory/trimming-tool" component={TrimmingTool}/>
        <Route path="/inventory/:cardId" component={InventoryDetailContainer} />
        <Route path="/inventory"/>
      </Switch>
      <Switch>
        {/* <Route path="/inventory/addCards" render={(props) => <CardSearchContainer {...props} searchContext="inventory" />} /> */}
        <Route path="/inventory/addCards" component={InventoryAddCardsContainer} />
        {/* <Route path="/inventory/trimming-tips" render={(props) => <TrimmingToolContainer {...props}  />} /> */}
        <Route path="/inventory" component={InventoryOverviewContainer} />
      </Switch>
    </React.Fragment>
  );
}
