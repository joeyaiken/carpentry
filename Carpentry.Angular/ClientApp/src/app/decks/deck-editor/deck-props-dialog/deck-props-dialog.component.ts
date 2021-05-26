import { Component, Inject, OnInit } from "@angular/core";
import { DeckPropertiesDto } from "../../models";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog"
import { FilterOption } from "src/app/settings/models";
import { FilterService } from "src/app/common/filter-service";

export interface DeckPropertiesComponentProps {
    deck: DeckPropertiesDto,
}

export interface DeckPropertiesComponentResult {
    action: string,
    props: DeckPropertiesDto | null,
}

@Component({
    selector: 'app-deck-props-dialog',
    templateUrl: 'deck-props-dialog.component.html',
})
export class DeckPropsDialogComponent implements OnInit {
    formatFilterOptions: FilterOption[];
    deckProps: DeckPropertiesDto;

    constructor(
        public dialogRef: MatDialogRef<DeckPropsDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DeckPropertiesComponentProps,
        public filterService: FilterService,
    ) {
        this.deckProps = {...data.deck};
    }
    ngOnInit(): void {
        this.filterService.getFormatFilters().subscribe(result => {
            this.formatFilterOptions = result;
        }, (error) => console.log(error));
    }
    onCancelClick(): void {
        this.closeDialog('cancel');
    }
    onSaveClick(): void {
        this.closeDialog('save', this.deckProps);
    }
    onDisassembleClick(): void {
        const confirmText = `Are you sure you want to dissemble the deck: '${this.data.deck.name}?`
        if(window.confirm(confirmText)){ //TODO - replace with an angular component instead of window.confirm
            this.closeDialog('disassemble');
        }
    }
    onDeleteClick(): void {
        this.closeDialog('delete')
    }

    private closeDialog(action: string, props: DeckPropertiesDto = null){
        this.dialogRef.close({
            action, props
        } as DeckPropertiesComponentResult);
    }
}