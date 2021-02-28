import { Component, OnInit } from "@angular/core";
import { CoreService } from "../core.service";
import { InventoryTotalsByStatusResult } from "../models";

@Component({
    selector: 'app-collection-totals',
    templateUrl: 'collection-totals.component.html',
    styleUrls: ['collection-totals.component.less']
})
export class CollectionTotalsComponent implements OnInit {
    displayedColumns: string[] = ['statusName','totalCount','totalPrice'];
    deckOverviews: InventoryTotalsByStatusResult[] = [];
    totalCount: number = 0;
    totalPrice: number = 0;

    constructor(
        private coreService: CoreService,
    ) { }

    ngOnInit(): void {
        this.getCollectionTotals();
    }

    getCollectionTotals(): void {
        this.coreService.getCollectionTotals().subscribe(result => {
            this.deckOverviews = result;
            this.totalCount = result.reduce((accum, curr) => accum + curr.totalCount, 0);
            this.totalPrice = result.reduce((accum, curr) => accum + curr.totalPrice, 0);
        }, err => console.log(`getCollectionTotals error: ${err}`));
    }
}