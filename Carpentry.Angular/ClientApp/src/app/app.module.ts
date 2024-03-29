import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
// import { HomeComponent } from './home/home.component';
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
import { GroupedDeckCardListComponent } from './decks/deck-editor/grouped-deck-card-list/grouped-deck-card-list.component';
import { DeckCardDetailComponent } from './decks/deck-editor/deck-card-detail/deck-card-detail.component';
import { DeckCardGridComponent } from './decks/deck-editor/deck-card-grid/deck-card-grid.component';
import { VisualCardComponent } from './common/visual-card/visual-card.component';
import { InventoryOverviewComponent } from './inventory/inventory-overview/inventory-overview.component';
import { InventoryFilterBarComponent } from './inventory/inventory-overview/inventory-filter-bar/inventory-filter-bar.component';
import { TextFilterComponent } from './common/text-filter/text-filter.component';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { InventoryCardGridComponent } from './inventory/inventory-overview/inventory-card-grid/inventory-card-grid.component';
import { InventoryCardComponent } from './inventory/inventory-overview/inventory-card/inventory-card.component';
import { DeckPropsDialogComponent } from './decks/deck-editor/deck-props-dialog/deck-props-dialog.component';
import { InventoryDetailComponent } from './inventory/inventory-detail/inventory-detail.component';
import { InventoryAddCardsComponent } from './inventory/inventory-add-cards/inventory-add-cards.component';
import { CardImageComponent } from './common/card-image/card-image.component';
import { NewDeckComponent } from "./decks/new-deck/new-deck.component";
import {DeckAddCardsComponent} from "./decks/deck-add-cards/deck-add-cards.component";
import {DeckPropertiesComponent} from "./common/deck-properties/deck-properties.component";
import {TrimmingToolComponent} from "./inventory/trimming-tool/trimming-tool.component";
import {SelectFieldComponent} from "./common/select-field/select-field.component";
import {NumericFieldComponent} from "./common/numeric-field/numeric-field.component";
import {CardImageDialogComponent} from "./common/card-image-dialog/card-image-dialog.component";

// TODO - 10-31-2022 - I Should probably make a ComponentModule for each section
//  ex: InventoryModule | InventoryComponentModule could contain all of the inventory-specific components
//    It could help reduce the size of this massive declarations block

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    // HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LandingComponent,
    DeckPropertiesComponent,


    ManaChipComponent,
    TextFilterComponent,
    VisualCardComponent,
    CardImageComponent,
    CardImageDialogComponent,

    SelectFieldComponent,
    NumericFieldComponent,


    DeckListComponent,
    DeckEditorComponent,
    DeckPropsBarComponent,
    DeckStatsBarComponent,
    DeckCardListComponent,
    GroupedDeckCardListComponent,
    DeckCardDetailComponent,
    DeckCardGridComponent,
    DeckPropsDialogComponent,
    NewDeckComponent,
    DeckAddCardsComponent,

    InventoryOverviewComponent,
    InventoryFilterBarComponent,
    InventoryCardGridComponent,
    InventoryCardComponent,
    InventoryDetailComponent,
    InventoryAddCardsComponent,
    TrimmingToolComponent,

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
    BrowserAnimationsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
