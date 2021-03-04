import { Component, OnInit } from "@angular/core";
import { forkJoin, Observable } from "rxjs";
import { FilterService } from "src/app/common/filter-service";
import { AppFiltersDto } from "src/app/settings/models";
import { InventoryFilterProps, InventoryOverviewDto, InventoryQueryParameter } from "../models";
import { tap } from 'rxjs/operators';
import { InventoryService } from "../inventory.service";

@Component({
    selector: 'app-inventory-overview',
    templateUrl: 'inventory-overview.component.html',
    styleUrls: ['inventory-overview.component.less']
})
export class InventoryOverviewComponent implements OnInit {
    isLoading: boolean;

    viewMethod: "grid" | "table";

    // searchResultsById: { [key: number]: InventoryOverviewDto }
    // searchReusltIds: number[];
    searchResults: InventoryOverviewDto[] = [];

    searchFilter: InventoryFilterProps;

    filterOptions: AppFiltersDto = {
        sets: [],
        colors: [],
        rarities: [],
        types: [],
        formats: [],
        statuses: [],
        searchGroups: [],
        groupBy: [],
        sortBy: [],
    };
    
    visibleSection: "inventory" | "trimmingTips" | "wishlistHelper" | "buylistHelper";
    
    cardImageMenuAnchor: HTMLButtonElement | null;
    
    constructor(
        private filterService: FilterService,
        private inventoryService: InventoryService,
    ) {

    }

    ngOnInit(): void {
        this.loadFilterOptions();
        this.searchFilter = this.defaultFilterProps();
        this.getInventoryOverviews(this.searchFilter);
    }

    loadFilterOptions(): void {
        let observables = [];

        this.isLoading = true;

        //sets
        observables.push(
            this.filterService.getCardSetFilters().pipe(
                tap(
                    (data) => {
                        this.filterOptions.sets = data;
                    },
                    (error) => {
                        console.log(error);
                    },
                )
            )
        );

        //types
        observables.push(
            this.filterService.getCardTypeFilters().pipe(
                tap(
                    (data) => {
                        this.filterOptions.types = data;
                    },
                    (error) => {
                        console.log(error);
                    },
                )
            )
        );
    
        //formats
        observables.push(
            this.filterService.getFormatFilters().pipe(
                tap(
                    (data) => {
                        this.filterOptions.formats = data;
                    },
                    (error) => {
                        console.log(error);
                    },
                )
            )
        );
    
        //colors
        observables.push(
            this.filterService.getManaTypeFilters().pipe(
                tap(
                    (data) => {
                        this.filterOptions.colors = data;
                    },
                    (error) => {
                        console.log(error);
                    },
                )
            )
        );
    
        //rarities
        observables.push(
            this.filterService.getRarityFilters().pipe(
                tap(
                    (data) => {
                        this.filterOptions.rarities = data;
                    },
                    (error) => {
                        console.log(error);
                    },
                )
            )
        );
        
        //statuses
        observables.push(
            this.filterService.getStatusFilters().pipe(
                tap(
                    (data) => {
                        this.filterOptions.statuses = data;
                    },
                    (error) => {
                        console.log(error);
                    },
                )
            )
        );
        
        //groupBy
        observables.push(
            this.filterService.getInventoryGroupOptions().pipe(
                tap(
                    (data) => {
                        this.filterOptions.groupBy = data;
                    },
                    (error) => {
                        console.log(error);
                    },
                )
            )
        );
        
        //sortBy
        observables.push(
            this.filterService.getInventorySortOptions().pipe(
                tap(
                    (data) => {
                        this.filterOptions.sortBy = data;
                    },
                    (error) => {
                        console.log(error);
                    },
                )
            )
        );
        
        //searchGroups
        observables.push(
            this.filterService.getCardGroupFilters().pipe(
                tap(
                    (data) => {
                        this.filterOptions.searchGroups = data;
                    },
                    (error) => {
                        console.log(error);
                    },
                )
            )
        );
    
        forkJoin(observables).subscribe(
            (_) => {
                this.isLoading = false;
            },
            (error) => console.log(error)
        );
    }

    defaultFilterProps(): InventoryFilterProps {
        return {
            groupBy: "unique",
            sortBy: "price",
            set: "",
            text: "",
            exclusiveColorFilters: false,
            multiColorOnly: false,
            skip: 0,
            take: 25,
            type: "",
            colorIdentity: [],
            rarity: [],
            minCount: 0,
            maxCount: 0,
            sortDescending: true,
        }
    }

    onSearchClick(): void {
        this.getInventoryOverviews(this.searchFilter);
    }

    getInventoryOverviews(existingFilters: InventoryFilterProps): any {

        // if(this.isLoading) return;
        
        this.isLoading = true;
        this.searchResults = [];
    
        const param: InventoryQueryParameter = { 
            groupBy: existingFilters.groupBy,
            text: existingFilters.text,
            colors: existingFilters.colorIdentity,
            skip: +existingFilters.skip,
            take: +existingFilters.take,
            sort: existingFilters.sortBy,
            sortDescending: existingFilters.sortDescending,
            set: existingFilters.set,
            exclusiveColorFilters: existingFilters.exclusiveColorFilters,
            multiColorOnly: existingFilters.multiColorOnly,
            maxCount: +existingFilters.maxCount,
            minCount: +existingFilters.minCount,
            type: existingFilters.type,
            rarity: existingFilters.rarity,
        }
    
        this.inventoryService.searchCards(param).subscribe(result => {
            this.searchResults = result;
            this.isLoading = false;
        }, err => console.log(err));
    }


    
    cardDetailSelected(): void {

    }
    cardDetailButtonClick(): void {

    }
    cardDetailMenuClose(): void {

    }
}