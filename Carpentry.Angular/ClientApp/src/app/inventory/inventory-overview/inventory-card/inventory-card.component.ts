import { Component, Input, OnInit } from "@angular/core";
import { InventoryOverviewDto } from "../../models";

@Component({
  selector: 'app-inventory-card',
  templateUrl: 'inventory-card.component.html',
  styleUrls: ['inventory-card.component.less'],
})
export class InventoryCardComponent implements OnInit {
  @Input() cardItem: InventoryOverviewDto;

  constructor() {}
  ngOnInit(): void {}
}
