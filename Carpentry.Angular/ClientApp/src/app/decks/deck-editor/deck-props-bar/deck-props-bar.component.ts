import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { DeckPropertiesDto } from "../../models";

@Component({
    selector: 'app-deck-props-bar',
    templateUrl: 'deck-props-bar.component.html',
    styleUrls: ['deck-props-bar.component.less']
})
export class DeckPropsBarComponent implements OnInit {
    
    @Input() deckProperties: DeckPropertiesDto;
    
    @Output() onToggleViewClick = new EventEmitter<void>();
    @Output() onEditClick = new EventEmitter<void>();
    @Output() onExportClick = new EventEmitter<void>();
    @Output() onAddCardsClick = new EventEmitter<void>();

    constructor() { }

    ngOnInit(): void { }
    
    onToggleView(): void {
        this.onToggleViewClick.emit();
    }

    onEdit(): void {
        this.onEditClick.emit();
    }

    onExport(): void {
        this.onExportClick.emit();
    }

    onAddCards(): void {
        this.onAddCardsClick.emit();
    }
}