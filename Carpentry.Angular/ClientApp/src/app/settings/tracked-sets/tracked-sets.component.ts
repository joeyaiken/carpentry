import { Component, OnInit } from "@angular/core";
import { MatSlideToggleChange } from "@angular/material/slide-toggle";
import { CoreService } from "../core.service";
import { SetDetailDto } from "../models";

//TODO - title/buttons should be all on the same line
@Component({
    selector: 'app-tracked-sets',
    templateUrl: 'tracked-sets.component.html',
    styleUrls: ['tracked-sets.component.less']
})
export class TrackedSetsComponent implements OnInit {
    // setsById: { [id: number]: SetDetailDto };
    // setIds: number[];
    // isLoading: boolean;
    
    displayedColumns: string[] = ['code','name','owned','collected','lastUpdated','actions'];
    setDetails: SetDetailDto[];
    // isLoading: boolean;
    public showUntracked: boolean; // = false;
    
    constructor(private coreService: CoreService) {}
    ngOnInit(): void {
        this.loadTrackedSets();
        this.showUntracked = false;

    }

    loadTrackedSets(update: boolean = false): void {
        this.setDetails = [];
        this.coreService.getTrackedSets(this.showUntracked, update).subscribe(result => {
            this.setDetails = result;
        }, err => console.log(`loadTrackedSets error: ${err}`));
    }

    showUntrackedToggleChange(event: MatSlideToggleChange) {
        this.showUntracked = event.checked;
        this.loadTrackedSets(false);
    }

    addTrackedSetClick(setId: number): void {
        this.coreService.addTrackedSet(setId).subscribe(() => {
            this.loadTrackedSets(false);
        }, err => console.log(err));
    }

    updateTrackedSetClick(setId: number): void {
        this.coreService.updateTrackedSet(setId).subscribe(() => {
            this.loadTrackedSets(false);
        }, err => console.log(err));
    }

    removeTrackedSetClick(setId: number): void {
        this.coreService.removeTrackedSet(setId).subscribe(() => {
            this.loadTrackedSets(false);
        }, err => console.log(err));
    }

}