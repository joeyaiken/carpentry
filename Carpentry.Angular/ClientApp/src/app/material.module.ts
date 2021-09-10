import { NgModule } from "@angular/core";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatCardModule } from "@angular/material/card";
import { MatButtonModule } from "@angular/material/button";
import { MatTableModule } from "@angular/material/table";
import { MatIconModule } from "@angular/material/icon";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { MatChipsModule } from "@angular/material/chips";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule } from "@angular/material/dialog";
import { MatProgressBarModule } from "@angular/material/progress-bar"

@NgModule({
    imports: [
        MatToolbarModule,
        MatCardModule,
        MatButtonModule,
        MatTableModule,
        MatIconModule,
        MatSlideToggleModule,
        MatChipsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatCheckboxModule,
        MatDialogModule,
        MatProgressBarModule,
    ],
    exports: [
        MatToolbarModule,
        MatCardModule,
        MatButtonModule,
        MatTableModule,
        MatIconModule,
        MatSlideToggleModule,
        MatChipsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatCheckboxModule,
        MatDialogModule,
        MatProgressBarModule,
    ],
    providers: [

    ],
})
export class MaterialModule { }