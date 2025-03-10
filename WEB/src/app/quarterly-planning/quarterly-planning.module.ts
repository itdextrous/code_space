import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatDialogModule } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatSelectModule } from "@angular/material/select";
import { MatTableModule } from "@angular/material/table";
import { RouterModule } from "@angular/router";
import { TranslateModule } from "@ngx-translate/core";

import { MaterialModule } from "~/app/material-module";
import { SharedModule } from "~shared/shared.module";

import { MaterialComponentsModule } from "../material-component/material-component.module";
import * as components from "./components";
import * as pages from "./pages";
import { routes } from "./quarterly-planning.routing";

@NgModule({
    declarations: [
        pages.NumbersComponent,
        pages.DeleteNumberDialogComponent,
        pages.EditNumberDialogComponent,

        components.NumbersTableComponent,
        components.ExternalDataComponent,
        components.XeroBankAccountSelectorComponent,
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        TranslateModule.forChild({ extend: true }),
        FormsModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatCardModule,
        MatDialogModule,
        MatFormFieldModule,
        MatIconModule,
        MatSelectModule,
        MatProgressSpinnerModule,
        MatTableModule,
        SharedModule,
        MaterialComponentsModule,
        MaterialModule
    ],
    entryComponents: [
        pages.DeleteNumberDialogComponent,
        pages.EditNumberDialogComponent,
    ]
})
export class QuarterlyPlanningModule { }
