import {Component, Inject, Input} from "@angular/core";
import {MAT_DIALOG_DATA} from "@angular/material/dialog";

export interface CardImageDialogProps {
  imageUrl: string;
  cardName: string;
}

@Component({
  selector: 'app-card-image-dialog',
  templateUrl: 'card-image-dialog.component.html',
  styleUrls: [],
})
export class CardImageDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: CardImageDialogProps) {}
}
