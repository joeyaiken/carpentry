import { MatButtonModule } from "@angular/material/button";
import { MatProgressBarModule } from "@angular/material/progress-bar"
import { MatToolbarModule } from "@angular/material/toolbar"
import { NgModule } from "@angular/core";

@NgModule({
    imports: [
        MatButtonModule,
        MatProgressBarModule,
        MatToolbarModule,
    ],
    exports: [
        MatButtonModule,
        MatProgressBarModule,
        MatToolbarModule,
    ],
    providers: [],
})
export class MaterialModule { }