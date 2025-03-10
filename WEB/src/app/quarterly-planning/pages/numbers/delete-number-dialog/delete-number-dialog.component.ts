import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialog,MatDialogRef } from "@angular/material/dialog";
import { GetNumberDto, PlanNumbersService } from "@api";

import { NotificationService } from "~services/notification.service";
import { toFiscalQuarter } from "~shared/commonfunctions";
import { ButtonState } from "~shared/components/status-button/status-button.component";

@Component({
    selector: "app-delete-number-dialog",
    templateUrl: "./delete-number-dialog.component.html",
    styleUrls: ["./delete-number-dialog.component.scss"]
})
export class DeleteNumberDialogComponent {

    buttonState: ButtonState;

    get description(): string {
        return this.number.description;
    }

    get isRecurring(): boolean {
        return this.number.isRecurring;
    }

    constructor(
        private readonly planNumbersService: PlanNumbersService,
        private readonly dialogRef: MatDialogRef<DeleteNumberDialogComponent, boolean>,
        private readonly notificationService: NotificationService,
        @Inject(MAT_DIALOG_DATA) private readonly number: GetNumberDto
    ) { }

    static open(dialog: MatDialog, number: GetNumberDto) {
        return dialog.open<DeleteNumberDialogComponent, GetNumberDto, boolean>(
            DeleteNumberDialogComponent, {
                width: "500px",
                data: number
            });
    }

    delete = () => {
        if (this.buttonState) return;

        this.buttonState = "loading";

        this.planNumbersService.planNumbersDeleteNumber(
            this.number.company.id,
            this.number.team.id,
            toFiscalQuarter({ financialYear: this.number.financialYear, quarter: this.number.planningPeriod }),
            this.number.id
        ).subscribe(
            () => {
                this.buttonState = "success";
                setTimeout(() => {
                    this.dialogRef.close(true);
                }, 1000);
            },
            () => {
                this.buttonState = "error";
                setTimeout(() => {
                    this.buttonState = undefined;
                }, 2000);
                this.notificationService.errorUnexpected();
            });
    };

    close = () => this.dialogRef.close();
}
