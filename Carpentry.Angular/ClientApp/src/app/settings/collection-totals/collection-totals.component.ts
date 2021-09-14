import { Component, Input, OnInit } from "@angular/core";
import { InventoryTotalsByStatusResult } from "../models";

//TODO - Does this NEED to implement OnInit? Would anything break if it didn't?
@Component({
    selector: 'app-collection-totals',
    templateUrl: 'collection-totals.component.html',
    styleUrls: ['collection-totals.component.less']
})
export class CollectionTotalsComponent implements OnInit {
    @Input() inventoryStatusTotals: InventoryTotalsByStatusResult[] = [];
    displayedColumns: string[] = ['statusName','totalCount','totalPrice'];
    constructor() { }
    ngOnInit(): void { }
}