import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
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
    @Input() setDetails: SetDetailDto[];
    // isLoading: boolean;
    //public 
    @Input() showUntracked: boolean; // = false;


    //@Output() valueChange = new EventEmitter<string>();
    // @Output() requestLoadTrackedSets = new EventEmitter<boolean>();

    //TODO - Find a way to reduce the number of output bindings
    @Output() onShowUntrackedToggleChange = new EventEmitter<MatSlideToggleChange>();
    @Output() onRefreshClick = new EventEmitter<void>();

    @Output() onAddTrackedSetClick = new EventEmitter<number>();
    @Output() onUpdateTrackedSetClick = new EventEmitter<number>();
    @Output() onRemoveTrackedSetClick = new EventEmitter<number>();

    constructor(private coreService: CoreService) {}
    ngOnInit(): void {
        // this.loadTrackedSets();
        // this.showUntracked = false;

    }

    // loadTrackedSets(update: boolean = false): void {
    //     this.setDetails = [];
    //     this.coreService.getTrackedSets(this.showUntracked, update).subscribe(result => {
    //         this.setDetails = result;
    //     }, err => console.log(`loadTrackedSets error: ${err}`));
    // }

    showUntrackedToggleChange = (event: MatSlideToggleChange) => this.onShowUntrackedToggleChange.emit(event);
    

    // refreshClicked() {
    //     this.onRefreshClick.emit();
    // }

    
    refreshClicked = () => this.onRefreshClick.emit();
    
    addTrackedSetClick(setId: number): void {
        this.onAddTrackedSetClick.emit(setId);
        // this.coreService.addTrackedSet(setId).subscribe(() => {
        //     // this.loadTrackedSets(false);
        //     this.requestLoadTrackedSets.emit(false);
        // }, err => console.log(err));
    }

    updateTrackedSetClick(setId: number): void {
        this.onUpdateTrackedSetClick.emit(setId);
        // this.coreService.updateTrackedSet(setId).subscribe(() => {
        //     // this.loadTrackedSets(false);
        //     this.requestLoadTrackedSets.emit(false);
        // }, err => console.log(err));
    }

    removeTrackedSetClick(setId: number): void {
        this.onRemoveTrackedSetClick.emit(setId);
        // this.coreService.removeTrackedSet(setId).subscribe(() => {
        //     // this.loadTrackedSets(false);
        //     this.requestLoadTrackedSets.emit(false);
        // }, err => console.log(err));
    }

}