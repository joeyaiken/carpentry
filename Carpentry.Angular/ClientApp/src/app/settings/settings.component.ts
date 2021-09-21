import { Component, OnInit } from "@angular/core";
import { MatSlideToggleChange } from "@angular/material/slide-toggle";
import { forkJoin, Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { CoreService } from "./core.service";
import { InventoryTotalsByStatusResult, SetDetailDto } from "./models";

//TODO - Should this be loading collection totals / tracked sets?

@Component({
  selector: 'app-settings',
  templateUrl: 'settings.component.html',
  styleUrls: ['settings.component.less'],
})
export class SettingsComponent implements OnInit {
  isBusy: boolean;

  setDetails: SetDetailDto[];

  //deck overviews | Collection Total Group | 
  inventoryStatusTotals: InventoryTotalsByStatusResult[] = [];
  // deckOverviews: InventoryTotalsByStatusResult[] = [];

  showUntrackedSets: boolean;

  constructor(
    private coreService: CoreService,
  ) {
    this.isBusy = false;
    this.showUntrackedSets = false;
  }
  ngOnInit(): void {
    this.loadData();
  }


  loadData(): void {
    this.isBusy = true;
    forkJoin([
      this.getCollectionTotalsObservable(),
      this.loadTrackedSetsObseervable(),
    ]).subscribe(() => {
      this.isBusy = false;
    }/* err => console.log(`loadData error: ${err}`) */);
  }
  
  getCollectionTotalsObservable(): Observable<InventoryTotalsByStatusResult[]> {
    return this.coreService.getCollectionTotals().pipe(tap<InventoryTotalsByStatusResult[]>(
      result => {
        this.inventoryStatusTotals = result;
        const totalCount = result.reduce((accum, curr) => accum + curr.totalCount, 0);
        const totalPrice = result.reduce((accum, curr) => accum + curr.totalPrice, 0);
        this.inventoryStatusTotals.push({ 
          statusId: 0, 
          statusName: 'Total', 
          totalCount: totalCount, 
          totalPrice: totalPrice,
        });
      },
      err => console.log(`getCollectionTotals error: ${err}`)
    ));
  }

  getCollectionTotals(): void {
    this.coreService.getCollectionTotals().subscribe(result => {
        // this.deckOverviews = result;
        this.inventoryStatusTotals = result;
        const totalCount = result.reduce((accum, curr) => accum + curr.totalCount, 0);
        const totalPrice = result.reduce((accum, curr) => accum + curr.totalPrice, 0);
        // this.totalCount = result.reduce((accum, curr) => accum + curr.totalCount, 0);
        // this.totalPrice = result.reduce((accum, curr) => accum + curr.totalPrice, 0);
        this.inventoryStatusTotals.push({ 
          statusId: 0, 
          statusName: 'Total', 
          totalCount: totalCount, 
          totalPrice: totalPrice,
        });

    }, err => console.log(`getCollectionTotals error: ${err}`));
  }

  loadTrackedSetsObseervable(update: boolean = false): Observable<SetDetailDto[]> {
    return this.coreService.getTrackedSets(this.showUntrackedSets, update)
    .pipe(tap<SetDetailDto[]>(
      result => {
        this.setDetails = result;
      }
    ))
  }

  loadTrackedSets(update: boolean = false): void {
      this.setDetails = [];
      this.coreService.getTrackedSets(this.showUntrackedSets, update).subscribe(result => {
          this.setDetails = result;
      }, err => console.log(`loadTrackedSets error: ${err}`));
  }

  showUntrackedToggleChange(event: MatSlideToggleChange) {
    this.showUntrackedSets = event.checked;
    this.isBusy = true;
    this.loadTrackedSetsObseervable().subscribe(() => this.isBusy = false);
  }

  trackedSetsRefreshClick() {
    this.isBusy = true;
    this.loadTrackedSetsObseervable(true).subscribe(() => this.isBusy = false);
  }

  //TODO Ensure I'm chaining observables correctly
  //TODO - Refactor this into something more elegant
  addTrackedSetClick(setId: number): void {
    this.isBusy = true;
    this.coreService.addTrackedSet(setId).subscribe(() => {
      this.loadTrackedSetsObseervable().subscribe(() => {
        this.isBusy = false;
      });
    });
  }

  updateTrackedSetClick(setId: number): void {
    this.isBusy = true;
    this.coreService.updateTrackedSet(setId).subscribe(() => {
      this.loadTrackedSetsObseervable().subscribe(()=> {
        this.isBusy = false;
      });
    });
  }

  removeTrackedSetClick(setId: number): void {
    this.isBusy = true;
    this.coreService.removeTrackedSet(setId).subscribe(() => {
      this.loadTrackedSetsObseervable().subscribe(() => {
        this.isBusy = false;
      });
    });
  }
}