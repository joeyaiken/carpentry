import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { AppFiltersDto } from "src/app/settings/models";
import { InventoryFilterProps } from "../../models";

@Component({
    selector: 'app-inventory-filter-bar',
    templateUrl: 'inventory-filter-bar.component.html',
    styleUrls: ['inventory-filter-bar.component.less'],
})
export class InventoryFilterBarComponent implements OnInit {
    @Input() searchFilter: InventoryFilterProps;
    @Input() viewMethod: "grid" | "table";
    @Input() filterOptions: AppFiltersDto;

    // @Output() onFilterChange = new EventEmitter<void>(); //: (event: React.ChangeEvent<HTMLInputElement>) => void;
    // @Output() onBoolFilterChange = new EventEmitter<{filter: string, value: boolean}>(); //: (filter: string, value: boolean) => void;
    @Output() onSearchButtonClick = new EventEmitter<void>(); //: () => void;

    constructor() {}

    ngOnInit(): void {
        // this.searchFilter = {
        //     groupBy: "print", // "name" | "print" | "unique", //InventoryGroupMethod,
        //     sortBy: "collectorNumber", //"name" | "price" | "cmc" | "count" | "collectorNumber", //InventorySortMethod,
        //     set: '',
        //     text: '',
        //     type: '',
        //     colorIdentity: [],
        //     exclusiveColorFilters: false,
        //     multiColorOnly: false,
        //     rarity: [],
        //     minCount: 0,
        //     maxCount: 0,
        //     skip: 0,
        //     take: 0,
        //     sortDescending: false,
        // }
    }

    searchButtonClick(): void {
        this.onSearchButtonClick.emit();
    }

    // filterChange(filter: string, value: boolean): void {

    // }
}