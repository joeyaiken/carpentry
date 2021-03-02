import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { AppRoutingModule } from './app-routing.module';
import { LandingComponent } from './landing/landing.component';
import { MaterialModule } from './material.module';
import { DeckListComponent } from './decks/deck-list/deck-list.component';
import { SettingsComponent } from './settings/settings.component';
import { TrackedSetsComponent } from './settings/tracked-sets/tracked-sets.component';
import { CollectionTotalsComponent } from './settings/collection-totals/collection-totals.component';
import { DeckEditorComponent } from './decks/deck-editor/deck-editor.component';
import { DeckPropsBarComponent } from './decks/deck-editor/deck-props-bar/deck-props-bar.component';
import { ManaChipComponent } from './common/mana-chip/mana-chip.component';
import { DeckStatsBarComponent } from './decks/deck-editor/deck-stats-bar/deck-stats-bar.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { DeckCardListComponent } from './decks/deck-editor/deck-card-list/deck-card-list.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LandingComponent,

    ManaChipComponent,
    
    DeckListComponent,
    DeckEditorComponent,
    DeckPropsBarComponent,
    DeckStatsBarComponent,
    DeckCardListComponent,
    
    SettingsComponent,
    CollectionTotalsComponent,
    TrackedSetsComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    MaterialModule,
    FlexLayoutModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
