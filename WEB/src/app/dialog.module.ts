import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE } from "@angular/material/core";
import { RouterModule } from "@angular/router";

import { MaterialModule } from "./material-module";
import { CompareHoursDirective } from "./shared/directive/max-hours.directive";
export const MY_FORMATS = {
  parse: {
    dateInput: "DD/MM/YYYY"
  },
  display: {
    dateInput: "DD/MM/YYYY",
    monthYearLabel: "MMM-YYYY",
  }
};
@NgModule({
  imports: [
    MaterialModule,
    RouterModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [{
    provide: MAT_DATE_LOCALE,
    useValue: "en-US"
  },
  {
    provide: MAT_DATE_FORMATS,
    useValue: MY_FORMATS
  }
  ],
  entryComponents: [
  ],
  declarations: [
    CompareHoursDirective
  ]
})

export class DialogModule { }
