import { NgModule } from "@angular/core";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatCardModule } from "@angular/material/card";
import { MatButtonModule } from "@angular/material/button";
import { MatTableModule } from "@angular/material/table";

@NgModule({
    imports: [
        MatToolbarModule,
        MatCardModule,
        MatButtonModule,
        MatTableModule,
    ],
    exports: [
        MatToolbarModule,
        MatCardModule,
        MatButtonModule,
        MatTableModule,
    ],
    providers: [

    ],
})
export class MaterialModule { }