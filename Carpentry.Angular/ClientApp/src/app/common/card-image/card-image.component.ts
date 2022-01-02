import {Component, Input} from "@angular/core";

@Component({
    selector: 'app-card-image',
    templateUrl: 'card-image.component.html',
    styleUrls: ['card-image.component.less']
})
export class CardImageComponent {
    @Input() imageUrl: string;
    @Input() cardName: string;
    constructor() {}
}
