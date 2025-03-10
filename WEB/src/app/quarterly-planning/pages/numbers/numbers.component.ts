import { Component, OnDestroy, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { GetNumberDto, ListNumbersDto, PlanNumbersService } from "@api";
import { BehaviorSubject, EMPTY, Observable, of, Subject, Subscription } from "rxjs";
import { catchError, filter, map, switchMap, tap, withLatestFrom } from "rxjs/operators";

import {
    DialogInformationDialogComponent
} from "~/app/material-component/dialogs/dialog-information-dialog/dialog-information-dialog.component";
import { IQuarter } from "~repositories";
import { NotificationService } from "~services/notification.service";
import { SettingsService } from "~services/settings.service";
import { CompanyTeam, UserService } from "~services/user.service";
import { CommonFunctions, toFiscalQuarter } from "~shared/commonfunctions";
import { PlanningStatus } from "~shared/enums";
import { sortString } from "~shared/util/sorters";

import { INumberEvent } from "../../components";
import { DeleteNumberDialogComponent } from "./delete-number-dialog/delete-number-dialog.component";
import { EditNumberDialogComponent } from "./edit-number-dialog/edit-number-dialog.component";

@Component({
    selector: "app-numbers",
    templateUrl: "./numbers.component.html",
    styleUrls: ["./numbers.component.scss"]
})
export class NumbersComponent implements OnInit, OnDestroy {

    companyId$: Observable<string>;

    get isLoading() {
        return this.isLoadingInternal;
    }

    set isLoading(value: boolean) {
        if (value) {
            CommonFunctions.showLoader();
        } else {
            CommonFunctions.hideLoader();
        }
        this.isLoadingInternal = value;
    }

    hasFailed = false;

    set quarter(value: IQuarter) {
        this.quarterSubject.next(value);
    }

    get isCommitted(): boolean {
        return this.isCommittedOverride ?? this.planningStatus === PlanningStatus.locked;
    }

    get hasData(): boolean {
        return !!this.companyTeamSubject.value && !!this.quarterSubject.value;
    }

    get canEditStatus(): boolean {
        return !!this.numbers && this.numbers.length > 0 && !this.isLoading && !this.hasFailed;
    }

    numbers?: GetNumberDto[];

    private isCommittedOverride: boolean | null = null;
    private planningStatus?: PlanningStatus;
    private isLoadingInternal = false;

    private quarterSubject = new BehaviorSubject<IQuarter | null>(null);
    private companyTeamSubject: BehaviorSubject<CompanyTeam | null>;
    private dataSubject = new Subject<ListNumbersDto | null>();

    private subscriptions: Subscription[] = [];

    constructor(
        private readonly planNumbersService: PlanNumbersService,
        private readonly userService: UserService,
        private readonly notificationService: NotificationService,
        private readonly dialog: MatDialog,
        private readonly settingsService: SettingsService,
    ) {
        const company = this.settingsService.currentCompany;
        const team = this.settingsService.currentTeam;

        this.companyTeamSubject = new BehaviorSubject<CompanyTeam | null>(
            company ? { company, team } : null
        );

        this.companyId$ = this.companyTeamSubject.pipe(
            filter((ct): ct is CompanyTeam => !!ct),
            map(ct => ct.company.id));
    }

    ngOnInit(): void {

        this.subscriptions.push(this.userService.companyTeam$.subscribe(this.companyTeamSubject));
        this.subscriptions.push(this.dataSubject.subscribe(data => {
            this.planningStatus = data?.planningStatus;
            this.numbers = data?.numbers.sort(sortString<GetNumberDto>(n => n.description).ascending()) || [];
        }));

        this.quarterSubject.pipe(
            withLatestFrom(this.companyTeamSubject),
            switchMap(([qtr, ct]) => {
                if (!qtr || !ct || !ct.team) return EMPTY;
                this.initLoad();
                return this.planNumbersService.planNumbersListNumbers(
                    ct.company.id,
                    ct.team.id,
                    toFiscalQuarter(qtr)).pipe(catchError(this.loadFailed));
            }),
            tap(() => this.isLoading = false)
        ).subscribe(
            (data) => {
                this.dataSubject.next(data);
            }
        );
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    setCommittedStatus = (isCommitted: boolean) => {
        if (this.isCommittedOverride !== null || !this.canEditStatus) return;

        const companyTeam = this.companyTeamSubject.value;
        const quarter = this.quarterSubject.value;

        if (!companyTeam || !quarter || !companyTeam.team) return;

        this.isCommittedOverride = isCommitted;
        this.isLoading = true;

        this.planNumbersService.planNumbersUpdateStatus(
            companyTeam.company.id,
            companyTeam.team.id,
            toFiscalQuarter(quarter),
            {
                planningStatus: isCommitted ? PlanningStatus.locked : PlanningStatus.draft
            }
        ).subscribe(
            (data) => {
                this.dataSubject.next(data);
                this.notificationService.success("Quarter Status Changed");
                this.isCommittedOverride = null;
                this.isLoading = false;
            },
            () => {
                this.notificationService.errorUnexpected();
                this.isCommittedOverride = null;
                this.isLoading = false;
            }
        );
    };

    refresh = () => {
        const quarter = this.quarterSubject.value;
        if (!quarter) return;
        this.quarterSubject.next(quarter);
    };

    informationDialog = () => {
        this.dialog.open(DialogInformationDialogComponent, {
            width: "450px",
            data: {
                showVideo: true,
                informationText: "Create a new number to help track your business activity. " +
                    "The numbers should track primary activities & metrics and be measurable weekly. " +
                    "You can also edit existing numbers here. Numbers you have created to track your business. " +
                    "These numbers can be edited by clicking on the description field - " +
                    "which then allows the numbers to be edited in the New Number area above.",
                newURL: "assets/videos/11232018121109PM_file_example.mp4",
                posterUrl: "assets/images/saas2.jpg"
            }
        });
    };

    addNumber = () => {
        const companyTeam = this.companyTeamSubject.value;
        const quarter = this.quarterSubject.value;
        if (!quarter || !companyTeam || !companyTeam.team) return;

        EditNumberDialogComponent.openForAdd(this.dialog, {
            companyId: companyTeam.company.id,
            teamId: companyTeam.team.id,
            financialYear: quarter.financialYear,
            planningPeriod: quarter.quarter
        }).afterClosed().subscribe(res => res ? this.refresh() : null);
    };

    handleNumberEvent = (event: INumberEvent) => {
        switch (event.event) {
            case "edit":
                this.editNumber(event.number);
                break;
            case "copy":
                this.copyNumber(event.number);
                break;
            case "delete":
                this.deleteNumber(event.number);
                break;
            case "attachments":
                break;
            case "updated":
                this.refresh();
                break;
        }
    };

    private initLoad = () => {
        this.isLoading = true;
        this.hasFailed = false;
    };

    private loadFailed = () => {
        this.notificationService.errorUnexpected();
        this.hasFailed = true;
        return of(null);
    };

    private editNumber = (number: GetNumberDto) => {
        if (this.isCommitted) {
            this.notificationService.warning("QuarterlyPlanningNumberScreen.editLockWarning", undefined, undefined, true);
            return;
        }

        EditNumberDialogComponent.openForEdit(this.dialog, number)
            .afterClosed().subscribe(res => res ? this.refresh() : null);
    };

    private deleteNumber = (number: GetNumberDto) => {
        if (this.isCommitted) {
            this.notificationService.warning("QuarterlyPlanningNumberScreen.deleteLockWarning", undefined, undefined, true);
            return;
        }
        DeleteNumberDialogComponent.open(this.dialog, number)
            .afterClosed().subscribe(res => res ? this.refresh() : null);
    };

    private copyNumber = (number: GetNumberDto) => {
        EditNumberDialogComponent.openForEdit(this.dialog, number, /* isCopy */ true)
            .afterClosed().subscribe(res => res ? this.refresh() : null);
    };
}
