import { trigger } from "@angular/animations";
import { Component, EventEmitter, Input, OnDestroy, Output, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { CompanyTeamActionDto, GetNumberDto, PlanNumbersService, SimpleUserDto, UpdateScheduleDto } from "@api";
import { TranslateService } from "@ngx-translate/core";

import { IQuarter } from "~repositories";
import { FeatureFlagService } from "~services/featureflag.service";
import { actionColorType, CommonFunctions, toFiscalQuarter } from "~shared/commonfunctions";
import { AddIssueFullData } from "~shared/components/add-issue-button/add-issue-button.component";
import { EditNumberAttachmentsDialogComponent, NumberChartDialogComponent } from "~shared/dialogs";
import { expandOnEnterAnimation } from "~shared/util/animations";
import { getCaptureMethodNameKey, getPlanningStatusNameKey, getUpdateScheduleDescription } from "~shared/util/translation-helper";

export declare type EventType = "edit" | "delete" | "copy" | "updated" | "attachments";

export interface INumberEvent {
    event: EventType;
    number: GetNumberDto;
}

@Component({
    selector: "app-numbers-table",
    templateUrl: "./numbers-table.component.html",
    styleUrls: ["./numbers-table.component.scss"],
    animations: [
        trigger("detailExpand", expandOnEnterAnimation ),
    ],
})
export class NumbersTableComponent implements OnDestroy {

    @Input() set numbers(value: GetNumberDto[]) {
        this.dataSource.data = value;
    }

    @Output() numberEvent = new EventEmitter<INumberEvent>();

    @ViewChild(MatSort) set matSort(value: MatSort) {
        this.dataSource.sort = value;
    }

    expandedNumberIds = new Set<string>();
    numberActions: { [numberId: string]: CompanyTeamActionDto[] } = {};
    dataSource = new MatTableDataSource<GetNumberDto>();

    displayedColumns = [
        "arrow",
        "actionsCount",
        "trend",
        "description",
        "result",
        "target",
        "owner",
        "updater",
        "captureMethod",
        "captureFrequency",
        "department",
        "category",
        "status",
        "action",
        "issue",
        "actions"
    ];

    actionColorType = actionColorType;
    getCaptureMethodNameKey = getCaptureMethodNameKey;
    getPlanningStatusNameKey = getPlanningStatusNameKey;
    commonFunction = CommonFunctions;
    isCollapse = true;

    get isEditable() {
        return CommonFunctions.checkPermissions("numbers").isEditable;
    }

    constructor(
        private readonly translate: TranslateService,
        private readonly dialog: MatDialog,
        private readonly planNumbersService: PlanNumbersService,
        private readonly featureFlagService: FeatureFlagService
    ) {
        this.dataSource.filterPredicate = (data, filter) =>
            this.getUserName(data.owner) === filter;

        this.dataSource.sortingDataAccessor = (data, property) => {
            switch (property) {
                case "result":
                    return data.resultToDate ?? Number.NEGATIVE_INFINITY;
                case "target":
                    return data.targetToDate?.lowerBound ?? data.targetToDate?.upperBound ?? Number.NEGATIVE_INFINITY;
                case "owner":
                    return this.getUserName(data.owner);
                case "department":
                    return data.department?.name;
                case "captureMethod":
                    return this.translate.instant(getCaptureMethodNameKey(data.captureMethod));
                case "captureFrequency":
                    return this.getUpdateScheduleDescription(data.scheduleDefinition);
                case "updater":
                    return this.getUserName(data.updater);
                case "status":
                    return getPlanningStatusNameKey(data.numberStatus);
                case "category":
                    return data.category?.description ?? "";
                default:
                    return (data as never)[property] ?? "";
            }
        };

        // Remove arrow and actionsCount column if feature not enabled
        if(!this.featureFlagService.numberActionsEnabled) {
            this.displayedColumns = ["trend", "description", "result", "target", "owner", "updater", "captureMethod",
                "captureFrequency", "department", "category", "status", "action", "issue", "actions"];
        }
    }

    ngOnDestroy() {
        this.numberEvent.complete();
    }

    showNumberGraph = (number: GetNumberDto) => {
        NumberChartDialogComponent.open(this.dialog, number);
    };

    showNumberAttachments = (number: GetNumberDto) => {
        EditNumberAttachmentsDialogComponent.open(this.dialog, number).
            afterClosed().subscribe(res => res ? this.numberUpdated(number) : null);
    };

    editNumber = (number: GetNumberDto) => this.numberEvent.next({ event: "edit", number });
    copyNumber = (number: GetNumberDto) => this.numberEvent.next({ event: "copy", number });
    deleteNumber = (number: GetNumberDto) => this.numberEvent.next({ event: "delete", number });
    numberUpdated = (number: GetNumberDto) => {
        delete this.numberActions[number.id];
        this.collapseNumber(number);
        this.numberEvent.next({ event: "updated", number });
    };

    getUpdateScheduleDescription = (schedule: UpdateScheduleDto): string | null =>
        getUpdateScheduleDescription(this.translate, schedule);

    getUserName = (user?: SimpleUserDto): string =>
        !user ? "" : `${user.firstName} ${user.lastName}`;

    applyOwnerFilter = (filter?: string) => this.dataSource.filter = filter ?? "";

    mapIssueData = (number: GetNumberDto): AddIssueFullData => ({
        id: number.id,
        description: number.description,
        owner: number.owner?.userId ?? "",
        teamId: number.team.id,
        companyId: number.company.id,
        isPrivateAction: false,
        issuesCount: number.issuesCount
    });

    /** Required for the app-add-issue-button component */
    mapWeekModel = (number: GetNumberDto) => ({
        fiscalYear: number.financialYear.toString(),
        qtr: number.planningPeriod.toString()
    });

    isNumberExpanded = (number: GetNumberDto) => this.expandedNumberIds.has(number.id);

    expandAllNumbers() {
        this.dataSource.data.forEach(this.expandNumber);
        this.isCollapse = false;
    }

    collapseAllNumbers() {
        this.dataSource.data.forEach(this.collapseNumber);
        this.isCollapse = true;
    }

    expandNumber = (number: GetNumberDto) => {
        this.expandedNumberIds.add(number.id);
        this.getChildActions(number);
    };

    collapseNumber = (number: GetNumberDto) => {
        this.expandedNumberIds.delete(number.id);
    };

    // Get actions for specific number
    getChildActions(number: GetNumberDto) {
        const quarter: IQuarter = {
            financialYear: number.financialYear,
            quarter: number.planningPeriod
        };
        CommonFunctions.showLoader();
        this.planNumbersService.planNumbersGetActionsForNumber(number.company.id, number.team.id, toFiscalQuarter(quarter), number.id)
            .subscribe(actions => {
                this.numberActions[number.id] = actions;
                CommonFunctions.hideLoader();
            });
    }

}
