import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
// import { pathToFileURL } from 'url';

import { CounterComponent } from './counter/counter.component';
import { DeckEditorComponent } from './decks/deck-editor/deck-editor.component';
import { DeckListComponent } from './decks/deck-list/deck-list.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component'; //TODO - delete the fetch-data class
// import { HomeComponent } from './home/home.component';
import { InventoryAddCardsComponent } from './inventory/inventory-add-cards/inventory-add-cards.component';
import { InventoryOverviewComponent } from './inventory/inventory-overview/inventory-overview.component';
import { LandingComponent } from './landing/landing.component';
import { SettingsComponent } from './settings/settings.component';
import { NewDeckComponent } from "./decks/new-deck/new-deck.component";
import { DeckAddCardsComponent } from "./decks/deck-add-cards/deck-add-cards.component";
import {TrimmingToolComponent} from "./inventory/trimming-tool/trimming-tool.component";

const routes: Routes = [
  {
    path: '',
    // component: HomeComponent,
    children: [
      { path: '', component: LandingComponent },
      { path: 'decks', component: DeckListComponent,  },
      { path: 'decks/:deckId', component: DeckEditorComponent },

      //http://localhost:23374/decks/15/addCards
      { path: 'decks/:deckId/add-cards', component: DeckAddCardsComponent },

      // { path}
      { path: 'inventory', component: InventoryOverviewComponent },
      { path: 'inventory/add-cards', component: InventoryAddCardsComponent },

      // { path: 'inventory/trimming-tool', component: InventoryOverviewComponent, data: { showTrimmingTools: true} },
      { path: 'inventory/trimming-tool', component: TrimmingToolComponent },

      { path: 'inventory/:cardId', component: InventoryOverviewComponent },

      { path: 'add-deck', component: NewDeckComponent },

      // {
      //   path: 'decks',
      //   children: [
      //     // { path: '/decks/addCards', },
      //     // { path: '/decks', },

      //     // <Route path="/decks/:deckId/addCards" component={DeckAddCardsContainer} />
      //     // <Route path="/decks/:deckId" component={DeckEditorContainer} />
      //     { path: '', component: DeckListComponent },
      //     { path: 'umm', }
      //   ]
      // },

      // { path: 'inventory',  },

      // {
      //   path: 'inventory',
      //   children: [
      //     // { path: '', component: InventoryOverviewsComponent }
      //     // <Route path="/inventory/addCards" />
      //     // <Route path="/inventory/trimming-tool" component={TrimmingToolContainer}/>
      //     // <Route path="/inventory/:cardId" component={InventoryDetailContainer} />
      //     // <Route path="/inventory"/>
      //   ]
      // },

      { path: 'settings', component: SettingsComponent },

      // { path: 'counter', component: CounterComponent },
      // { path: 'fetch-data', component: FetchDataComponent },
    ]
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
