import {Component, OnInit} from "@angular/core";
import {TrimmingToolRequest, TrimmingToolResult, TrimmedCard, TrimmedCardDto} from "../models";
import {FilterOption} from "../../settings/models";
import {forkJoin, Observable} from "rxjs";
import {FilterService} from "../../common/filter-service";
import {map} from "rxjs/operators";
import {InventoryService} from "../inventory.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {CardImageDialogComponent} from "../../common/card-image-dialog/card-image-dialog.component";

@Component({
  selector: 'app-trimming-tool',
  templateUrl: 'trimming-tool.component.html',
  styleUrls: ['./trimming-tool.component.less']
})
export class TrimmingToolComponent implements OnInit {
  displayedColumns: string[] = ['name','print','unused','total','all','recommended','trim','img']
  searchFilter: TrimmingToolRequest;
  setFilters: FilterOption[];
  groupFilters: FilterOption[];
  filterByOptions: FilterOption[];
  searchResultsById: {[id: string]: TrimmingToolResult }
  searchResultsAllIds: string[];
  isLoading: boolean;
  pendingCardsById: {[id: string]: TrimmedCard }
  pendingCardsAllIds: string[];

  constructor(
    private filterService: FilterService,
    private inventoryService: InventoryService,
    private router: Router,
    private dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.searchResultsById = {};
    this.searchResultsAllIds = [];
    this.searchFilter = new TrimmingToolRequest();
    this.pendingCardsById = {};
    this.pendingCardsAllIds = [];

    this.searchFilter.setCode = 'znr';
    this.searchFilter.searchGroup = 'Red';
    this.searchFilter.minCount = 8;
    this.searchFilter.maxPrice = 0.1;
    this.searchFilter.filterBy = "inventory";

    this.filterByOptions = [
      {
        name: "Unused",
        value: "inventory",
      },
      {
        name: "Owned",
        value: "owned",
      },
      {
        name: "Total",
        value: "total",
      }
    ];

    this.loadSearchFilters().subscribe();
    this.loadTrimmingToolCards().subscribe();
  }

  loadSearchFilters(): Observable<void> {
    this.isLoading = true;

    return forkJoin({
      sets: this.filterService.getCardSetFilters(),
      searchGroups: this.filterService.getCardGroupFilters(),
    }).pipe(map(result => {
        this.setFilters = result.sets;
        this.groupFilters = result.searchGroups;
        this.isLoading = false;
      }
    ));
  }

  loadTrimmingToolCards(): Observable<void> {
    this.isLoading = true;
    this.searchResultsById = {};
    this.searchResultsAllIds = [];

    console.log('loading')
    return this.inventoryService
      .getTrimmingToolCards(this.searchFilter)
      .pipe(map(result =>{
        result.forEach(card => this.searchResultsById[card.id] = card);
        this.searchResultsAllIds = result.map(c => c.id);
        this.isLoading = false;
        console.log('loaded')
      }));
  }

  trimPendingCards(): Observable<void> {
    this.isLoading = true;
    let cardsToTrim = this.pendingCardsAllIds.map(id => {
      const card = this.pendingCardsById[id];
      return {
        cardId: card.data.cardId,
        cardName: card.data.name,
        isFoil: card.data.isFoil,
        numberToTrim: card.numberToTrim,
      } as TrimmedCardDto;
    });

    return this.inventoryService
      .trimCards(cardsToTrim)
      .pipe(map(result =>{
        this.pendingCardsById = {};
        this.pendingCardsAllIds = [];
        this.isLoading = false;
      }));
  }

  searchClick(): void {
    this.loadTrimmingToolCards().subscribe();
  }

  addPendingCardClick(result: TrimmingToolResult, count: number = 1): void {
    let pendingCard = this.pendingCardsById[result.id];
    if(!pendingCard){
      pendingCard = {
        data: result,
        numberToTrim: 0,
      };
      this.pendingCardsById[result.id] = pendingCard;
      this.pendingCardsAllIds = Object.keys(this.pendingCardsById);
    }
    pendingCard.numberToTrim += count;
  }

  removePendingCardClick(result: TrimmingToolResult): void {
    let pendingCard = this.pendingCardsById[result.id];
    if(pendingCard){
      pendingCard.numberToTrim--;
      if(pendingCard.numberToTrim === 0){
        delete this.pendingCardsById[result.id];
        this.pendingCardsAllIds = Object.keys(this.pendingCardsById);
      }
    }
  }

  cardImageClick(id: string): void {
    const card = this.searchResultsById[id];
    this.dialog.open(
      CardImageDialogComponent, {
        data: {
          imageUrl: card.imageUrl,
          cardName: card.name,
        },
      }


    );
  }

  trimClick(): void {
    this.trimPendingCards().subscribe();
  }

  cancelClick(): void {
    this.pendingCardsById = {};
    this.pendingCardsAllIds = [];
    this.searchResultsById = {};
    this.searchResultsAllIds = [];
    this.router.navigate(['inventory']).then();
  }

}

