import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { pathToFileURL } from 'url';

import { CounterComponent } from './counter/counter.component';
import { DeckListComponent } from './decks/deck-list/deck-list.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';
import { LandingComponent } from './landing/landing.component';
import { SettingsComponent } from './settings/settings.component';

const routes: Routes = [
  { 
    path: '', 
    component: HomeComponent, 
    children: [
      { path: '', component: LandingComponent },
      { 
        path: 'decks', 
        children: [
          // { path: '/decks/addCards', },
          // { path: '/decks', },
          { path: '', component: DeckListComponent },
        ]
      },
      {
        path: 'inventory',
        children: [
          // { path: '', component: InventoryOverviewsComponent }
          // <Route path="/inventory/addCards" />
          // <Route path="/inventory/trimming-tool" component={TrimmingToolContainer}/>
          // <Route path="/inventory/:cardId" component={InventoryDetailContainer} />
          // <Route path="/inventory"/>
        ]
      },
      
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
