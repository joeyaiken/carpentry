import React from 'react';
import {Route, Switch} from 'react-router-dom';
import {TrimmingTool} from "./trimming-tool/TrimmingTool";
import {InventoryDetail} from "./inventory-detail/InventoryDetail";
import {InventoryAddCards} from "./inventory-add-cards/InventoryAddCards";
import {InventoryOverview} from "./inventory-overview/InventoryOverview";

export default function InventoryLayout(): JSX.Element {
  return (
    <React.Fragment>
      <Switch>
        <Route path="/inventory/add-cards" />
        <Route path="/inventory/trimming-tool" component={TrimmingTool}/>
        <Route path="/inventory/:cardId" component={InventoryDetail} />
        <Route path="/inventory"/>
      </Switch>
      <Switch>
        <Route path="/inventory/add-cards" component={InventoryAddCards} />
        <Route path="/inventory" component={InventoryOverview} />
      </Switch>
    </React.Fragment>
  );
}
