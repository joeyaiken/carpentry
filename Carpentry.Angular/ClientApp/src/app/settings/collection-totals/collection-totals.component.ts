import { Component, OnInit } from "@angular/core";
import { CoreService } from "../core.service";
import { InventoryTotalsByStatusResult } from "../models";


@Component({
    selector: 'app-collection-totals',
    templateUrl: 'collection-totals.component.html',
    styleUrls: ['collection-totals.component.less']
})
export class CollectionTotalsComponent implements OnInit {
    public displayedColumns: string[] = ['statusName','totalCount','totalPrice'];
    public deckOverviews: InventoryTotalsByStatusResult[] = [];

    constructor(
        private coreService: CoreService,
    ) { }

    ngOnInit(): void {
        this.getCollectionTotals();
    }

    getCollectionTotals(): Promise<InventoryTotalsByStatusResult[]> {
        //TODO - replace promise with observable
        return this.coreService.getCollectionTotalsAsync().then(result => {
            this.deckOverviews = result;
            return result;
        });
    }
}