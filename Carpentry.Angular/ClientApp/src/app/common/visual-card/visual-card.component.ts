import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { DeckCardOverview } from "src/app/decks/models";


@Component({
    selector: 'app-visual-card',
    templateUrl: 'visual-card.component.html',
    styleUrls: ['visual-card.component.less']
})
export class VisualCardComponent implements OnInit {
    @Input() cardOverview: DeckCardOverview;

    @Output() onCardSelected = new EventEmitter<void>();
    
    // children?: ReactNode;

    constructor() {}

    ngOnInit(): void {}
}