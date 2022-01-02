import {Component, OnInit} from "@angular/core";
import {DeckPropertiesDto} from "../models";
import {FilterOption} from "../../settings/models";
import {DecksService} from "../decks.service";
import {Router} from "@angular/router";
import {FilterService} from "../../common/filter-service";

@Component({
  selector: 'app-new-deck',
  templateUrl: './new-deck.component.html',
  styleUrls: ['./new-deck.component.less']
})
export class NewDeckComponent implements OnInit {
  deckProps: DeckPropertiesDto;
  formatFilters: FilterOption[];
  showLands: boolean = false;

  constructor(
    private decksService: DecksService,
    private filterService: FilterService,
    private router: Router,
  ) {
    this.deckProps = new DeckPropertiesDto();
    this.formatFilters = [];
  }

  ngOnInit(): void {
    this.filterService.getFormatFilters().subscribe(filters => {
      this.formatFilters = filters;
    });
  }

  saveClick(): void {
    this.decksService.addDeck(this.deckProps).subscribe(newDeckId => {
      this.router.navigate(['/decks',newDeckId])
        .then(result=> {}, error => console.log(error));
      // this.router.navigate(['/decks']);
    }, error => console.log(error)); //TODO - replace with toast notification when something goes wrong
  }
}
