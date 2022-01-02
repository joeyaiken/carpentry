import {Component, Input, OnInit} from "@angular/core";
import {DeckPropertiesDto} from "../../decks/models";
import { FilterOption } from "src/app/settings/models";

@Component({
  selector: 'app-deck-properties',
  templateUrl: 'deck-properties.component.html',
  styleUrls: ['deck-properties.component.less']
})
export class DeckPropertiesComponent implements OnInit{
  @Input() deck: DeckPropertiesDto;
  @Input() formatFilters: FilterOption[];
  @Input() showLands: boolean = false;

  constructor() {}

  ngOnInit(): void {}
}
