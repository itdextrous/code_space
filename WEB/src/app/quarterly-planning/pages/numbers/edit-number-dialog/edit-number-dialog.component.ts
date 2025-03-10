
import { Component, Inject, OnDestroy, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, ValidatorFn, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from "@angular/material/dialog";
import {
    CategoryTreeVM, GetNumberDto, NumberTargetDto, PlanNumbersService,
    RecurrenceDto, RegistrationModel, SubCategoryTreeVM, UpdateNumberDto,
    UpdateScheduleDto
} from "@api";
import { Department } from "@api";
import { combineLatest, Observable, ReplaySubject, Subscription } from "rxjs";
import { debounceTime, distinctUntilChanged, filter, map, shareReplay, switchMap, tap } from "rxjs/operators";

import { DepartmentRepository, IQuarter, TeamRepository, WorkCategoryRepository } from "~repositories";
import { NotificationService } from "~services/notification.service";
import { SettingsService } from "~services/settings.service";
import { toFiscalQuarter } from "~shared/commonfunctions";
import { ButtonState } from "~shared/components/status-button/status-button.component";
import { CaptureMethod, NumberEntryType, NumberTargetType, NumberType, PlanningStatus, UpdateScheduleType } from "~shared/enums";
import { retryWithDelay } from "~shared/util/caching";
import { greaterThanControl } from "~shared/util/custom-validators";
import { supportsAddingNumbers } from "~shared/util/number-helper";
import {
    getCaptureMethodNameKey, getNumberTargetTypeNameKey, getNumberTypeNameKey,
    getPlanningStatusNameKey, getUpdateScheduleTypeNameKey
} from "~shared/util/translation-helper";
import { valueAndChanges } from "~shared/util/util";


interface IWeekTargets {
    [week: string]: NumberTargetDto | undefined;
};

interface INumberDialogData {
    number?: GetNumberDto;
    isCopy?: boolean;
    companyId: string;
    teamId: string;
    financialYear: number;
    planningPeriod: number;
}

export interface IAddNumberDialogData {
    companyId: string;
    teamId: string;
    financialYear: number;
    planningPeriod: number;
};

const recurrenceDefinitionEqual = (left: RecurrenceDto, right: RecurrenceDto) =>
    left.type === right.type &&
    left.interval === right.interval &&
    left.index === right.index &&
    left.dayOfWeek === right.dayOfWeek &&
    left.referenceDate === right.referenceDate;

const updateDefinitionEqual = (left: UpdateScheduleDto, right: UpdateScheduleDto) =>
    left.type === right.type &&
    (
        left.recurrence === right.recurrence ||
        (!!left.recurrence && !!right.recurrence && recurrenceDefinitionEqual(left.recurrence, right.recurrence))
    );

const cloneSchedule = (schedule: UpdateScheduleDto): UpdateScheduleDto => ({
    type: schedule.type,
    recurrence: !schedule.recurrence ? undefined : {
        type: schedule.recurrence.type,
        interval: schedule.recurrence.interval,
        index: schedule.recurrence.index,
        dayOfWeek: schedule.recurrence.dayOfWeek,
        referenceDate: schedule.recurrence.referenceDate
    }
});

const weekGreaterThan = (weekLowerTarget: AbstractControl, upperTarget: AbstractControl, lowerTarget: AbstractControl): ValidatorFn =>
    (weekUpperTarget: AbstractControl) => {
        if (weekLowerTarget.disabled) return null;
        if ((weekUpperTarget.value === null || weekUpperTarget.value === undefined) &&
            (weekLowerTarget.value === null || weekLowerTarget.value === undefined)) {
            return null;
        }

        const upper = weekUpperTarget.value ?? upperTarget.value ?? null;
        const lower = weekLowerTarget.value ?? lowerTarget.value ?? null;

        if (upper === null || lower === null || upper >= lower) return null;
        return { greaterThan: true };
    };

@Component({
    templateUrl: "./edit-number-dialog.component.html",
    styleUrls: ["./edit-number-dialog.component.scss"]
})
export class EditNumberDialogComponent implements OnInit, OnDestroy {

    buttonState: ButtonState;

    targetTypes = [
        NumberTargetType.aboveTarget,
        NumberTargetType.belowTarget,
        NumberTargetType.withinRange
    ];

    planningStatuses = [
        PlanningStatus.draft,
        PlanningStatus.locked
    ];

    numberTypes = [
        NumberType.normal,
        NumberType.currency,
        NumberType.percentage
    ];

    entryTypes = [
        NumberEntryType.deltas,
        NumberEntryType.totals
    ];

    captureMethods = [
        CaptureMethod.manual,
        CaptureMethod.automatic
    ];

    readonly users$: Observable<RegistrationModel[]>;
    readonly departments$: Observable<Department[]>;
    readonly categories$: Observable<CategoryTreeVM[]>;
    readonly subCategories$: Observable<SubCategoryTreeVM[]>;

    descriptionControl = this.fb.control(null, [Validators.required, Validators.maxLength(250)]);
    quarterControl = this.fb.control(null, [Validators.required, Validators.max(4), Validators.min(1)]);
    targetTypeControl = this.fb.control(null, [Validators.required]);
    targetLowerBoundControl = this.fb.control(null);
    targetUpperBoundControl = this.fb.control(null, [greaterThanControl("targetLowerBound")]);
    allowWeeklyTargetsControl = this.fb.control(false);
    enteringTotalsControl = this.fb.control(false, [Validators.required]);
    isRecurringControl = this.fb.control(false);
    numberTypeControl = this.fb.control(null, [Validators.required]);

    scheduleTypeControl = this.fb.control(null, [Validators.required]);
    recurrenceControl = this.fb.control(null);

    captureFrequencyControl = this.fb.group({
        type: this.scheduleTypeControl,
        recurrence: this.recurrenceControl
    });

    ownerControl = this.fb.control(null, [Validators.required]);
    updaterControl = this.fb.control(null, [Validators.required]);
    captureMethodControl = this.fb.control(null, [Validators.required]);
    categoryControl = this.fb.control(null);
    subCategoryControl = this.fb.control(null, [Validators.required]);
    numberStatusControl = this.fb.control(null, [Validators.required]);
    departmentControl = this.fb.control(null);
    externalDataControl = this.fb.control({ value: undefined, disabled: true });

    weekTargetsControl = this.fb.array([]);

    form = this.fb.group({
        description: this.descriptionControl,
        quarter: this.quarterControl,
        targetType: this.targetTypeControl,
        targetLowerBound: this.targetLowerBoundControl,
        targetUpperBound: this.targetUpperBoundControl,
        allowWeeklyTargets: this.allowWeeklyTargetsControl,
        enteringTotals: this.enteringTotalsControl,
        isRecurring: this.isRecurringControl,
        numberType: this.numberTypeControl,
        captureFrequency: this.captureFrequencyControl,
        owner: this.ownerControl,
        updater: this.updaterControl,
        captureMethod: this.captureMethodControl,
        category: this.categoryControl,
        subCategory: this.subCategoryControl,
        numberStatus: this.numberStatusControl,
        department: this.departmentControl,
        externalData: this.externalDataControl,
        weekTargets: this.weekTargetsControl
    });

    weeks: number[] = [];
    updateScheduleTypeOptions = [UpdateScheduleType.everyWeek, UpdateScheduleType.everyMeeting, UpdateScheduleType.custom];

    getUpdateScheduleTypeNameKey = getUpdateScheduleTypeNameKey;
    getNumberTargetTypeNameKey = getNumberTargetTypeNameKey;
    getNumberTypeNameKey = getNumberTypeNameKey;
    getPlanningStatusNameKey = getPlanningStatusNameKey;
    getCaptureMethodNameKey = getCaptureMethodNameKey;

    get minQuarter(): number {
        return this.planningPeriod;
    }

    get hasLowerTarget(): boolean {
        return this.targetTypeControl.value !== NumberTargetType.belowTarget;
    }

    get hasUpperTarget(): boolean {
        return this.targetTypeControl.value !== NumberTargetType.aboveTarget;
    }

    get hasRecurrence(): boolean {
        return this.scheduleTypeControl.value === UpdateScheduleType.custom;
    }

    get lowerTargetSet(): boolean {
        const target = this.targetLowerBoundControl.value;
        return target !== null && target !== undefined;
    }

    get upperTargetSet(): boolean {
        const target = this.targetUpperBoundControl.value;
        return target !== null && target !== undefined;
    }

    get supportsEnteringDeltas(): boolean {
        return supportsAddingNumbers(this.numberTypeControl.value);
    }

    get captureMethodIsAutomatic(): boolean {
        return this.captureMethodControl.value === CaptureMethod.automatic;
    }

    private readonly companyId: string;
    private readonly teamId: string;
    private readonly numberId?: string;

    private readonly financialYear: number;
    private readonly planningPeriod: number;

    private weekTargetStore: IWeekTargets = {};
    private lastEnteringTotalsValue = false;

    private scheduleSubject = new ReplaySubject<UpdateScheduleDto>(1);
    private quarterSubject = new ReplaySubject<IQuarter>(1);
    private categorySubject = new ReplaySubject<string | undefined | null>(1);

    private subscriptions: Subscription[] = [];
    private weekTargetSubscriptions: Subscription[] = [];

    constructor(
        private readonly planNumbersService: PlanNumbersService,
        private readonly teamRepository: TeamRepository,
        private readonly departmentRepository: DepartmentRepository,
        private readonly workCategoryService: WorkCategoryRepository,
        private readonly fb: FormBuilder,
        private readonly dialogRef: MatDialogRef<EditNumberDialogComponent, boolean>,
        private readonly notificationService: NotificationService,
        private readonly settingsService: SettingsService,
        @Inject(MAT_DIALOG_DATA) data: INumberDialogData
    ) {
        this.companyId = data.companyId;
        this.teamId = data.teamId;
        this.numberId = data.isCopy ? undefined : data.number?.id;
        this.financialYear = data.financialYear;
        this.planningPeriod = data.planningPeriod;

        if (this.numberId) {
            this.quarterControl.disable();
        } else {
            this.quarterControl.setValidators([
                Validators.required,
                Validators.max(4),
                Validators.min(this.planningPeriod),
                Validators.pattern(/^\d*$/)
            ]);
        }

        let users$ = this.teamRepository.getTeamMembers(this.companyId, this.teamId);
        if (!data.number) {
            // If we are creating a new number (and not copying), choose a default owner/updater after loading team members.
            users$ = users$.pipe(tap(this.setDefaultOwnerUpdater));
        }
        this.users$ = users$;
        this.departments$ = this.departmentRepository.getDepartments(this.companyId);
        this.categories$ = this.workCategoryService.getWorkCategories(this.companyId);
        this.subCategories$ = combineLatest([
            this.categories$,
            this.categorySubject
        ]).pipe(
            map(([categories, categoryId]) => {
                const category = categories.find(c => c.id === categoryId);
                if (!category) return [];
                return category.children ?? [];
            }),
            tap(subcats => {
                if (!subcats.length) {
                    this.subCategoryControl.setValue(null);
                    this.subCategoryControl.disable();
                } else {
                    this.subCategoryControl.enable();
                    if (subcats.length === 1) {
                        this.subCategoryControl.setValue(subcats[0].id);
                    } else {
                        this.subCategoryControl.setValue(null);
                    }
                }
            }),
            shareReplay(1)
        );

        this.bindData(data);
    }

    static openForAdd(dialog: MatDialog, data: IAddNumberDialogData) {
        return this.openInternal(dialog, {
            number: undefined,
            companyId: data.companyId,
            teamId: data.teamId,
            financialYear: data.financialYear,
            planningPeriod: data.planningPeriod,
            isCopy: false,
        });
    }

    static openForEdit(dialog: MatDialog, number: GetNumberDto, isCopy = false) {
        return this.openInternal(dialog, {
            number: number,
            companyId: number.company.id,
            teamId: number.team.id,
            financialYear: number.financialYear,
            planningPeriod: number.planningPeriod,
            isCopy: isCopy
        });
    }

    private static openInternal(dialog: MatDialog, data: INumberDialogData) {
        return dialog.open<EditNumberDialogComponent, INumberDialogData, boolean>(EditNumberDialogComponent, {
            width: "750px",
            data: data
        });
    }

    ngOnInit(): void {

        this.subscriptions.push(valueAndChanges(this.captureMethodControl).subscribe(this.handleCaptureMethodChange));
        this.subscriptions.push(valueAndChanges(this.numberTypeControl).subscribe(this.handleNumberTypeChange));

        this.subscriptions.push(this.targetLowerBoundControl.valueChanges.subscribe(() => {
            this.targetUpperBoundControl.updateValueAndValidity();
            this.revalidateWeekUpperTargets();
        }));
        this.subscriptions.push(this.targetUpperBoundControl.valueChanges.subscribe(this.revalidateWeekUpperTargets));
        this.subscriptions.push(this.targetTypeControl.valueChanges.subscribe(this.updateTargetState));
        this.subscriptions.push(this.categoryControl.valueChanges.subscribe(this.categorySubject));

        this.subscriptions.push(this.captureFrequencyControl.valueChanges.subscribe(this.scheduleSubject));
        this.subscriptions.push(this.quarterControl.valueChanges.pipe(
            filter(() => this.quarterControl.valid),
            map(this.mapQuarter)
        ).subscribe(this.quarterSubject));

        const schedule$ = this.scheduleSubject.pipe(
            map(cloneSchedule), // Prevents mutations to the original schedule from breaking the distinct check
            distinctUntilChanged(updateDefinitionEqual)
        );
        this.subscriptions.push(combineLatest([schedule$, this.quarterSubject]).pipe(
            debounceTime(100),
            switchMap(([schedule, quarter]) =>
                this.planNumbersService.planNumbersGetExpectedScheduleWeeks(
                    this.companyId,
                    this.teamId,
                    toFiscalQuarter(quarter),
                    schedule).pipe(retryWithDelay()
                ))
        ).subscribe(weeks => {
            const dedupe = weeks.filter((v, i, a) => a.indexOf(v) === i); // Removes duplicates from the list
            this.refreshWeeks(dedupe);
        }));

        this.scheduleSubject.next(this.captureFrequencyControl.value);
        this.quarterSubject.next(this.mapQuarter(this.quarterControl.value));
        this.categorySubject.next(this.categoryControl.value);
    }

    ngOnDestroy() {
        this.subscriptions.forEach(s => s.unsubscribe());
        this.weekTargetSubscriptions.forEach(s => s.unsubscribe());
    }

    save() {
        this.targetUpperBoundControl.updateValueAndValidity();
        this.revalidateWeekUpperTargets();
        if (this.form.invalid || this.buttonState) return;

        this.buttonState = "loading";

        const dto: UpdateNumberDto = {
            ownerUserId: this.ownerControl.value,
            updaterUserId: this.updaterControl.value,
            departmentId: this.departmentControl.value,
            categoryId: this.categoryControl.value,
            subCategoryId: this.subCategoryControl.value,
            description: this.descriptionControl.value,
            type: this.numberTypeControl.value,
            entryType: this.enteringTotalsControl.value ? NumberEntryType.totals : NumberEntryType.deltas,
            captureMethod: this.captureMethodControl.value,
            externalData: this.captureMethodControl.value === CaptureMethod.automatic ? this.externalDataControl.value : undefined,
            numberStatus: this.numberStatusControl.value,
            targetType: this.targetTypeControl.value,
            isRecurring: this.isRecurringControl.value,
            allowWeeklyTargetOverrides: this.allowWeeklyTargetsControl.value,
            target: {
                lowerBound: this.hasLowerTarget ? this.targetLowerBoundControl.value : null,
                upperBound: this.hasUpperTarget ? this.targetUpperBoundControl.value : null
            },
            weekTargets: this.weeks.reduce((targets, week, i) => {
                targets[week.toString()] = {
                    lowerBound: this.hasLowerTarget ? this.weekTargetsControl.at(i).get("lower")?.value : null,
                    upperBound: this.hasUpperTarget ? this.weekTargetsControl.at(i).get("upper")?.value : null,
                };
                return targets;
            }, {} as { [week: string]: NumberTargetDto }),
            scheduleDefinition: this.captureFrequencyControl.value
        };

        let obs$: Observable<GetNumberDto>;
        if (this.numberId) {
            obs$ = this.planNumbersService.planNumbersUpdateNumber(
                this.companyId,
                this.teamId,
                toFiscalQuarter(this.mapQuarter(this.planningPeriod)),
                this.numberId,
                dto
            );
        } else {
            obs$ = this.planNumbersService.planNumbersAddNumber(
                this.companyId,
                this.teamId,
                toFiscalQuarter(this.mapQuarter(this.quarterControl.value)),
                dto
            );
        }

        obs$.subscribe(
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
    }

    close = () => this.dialogRef.close();

    getUserId = (user: RegistrationModel) => user.userId;
    getUserName = (user?: RegistrationModel) => !user ? "" : `${user.FirstName} ${user.LastName}`;

    getCategoryId = (category: CategoryTreeVM): string => category?.id ?? "";
    getCategoryDisplay = (category: CategoryTreeVM | null | undefined) => category?.Name ?? "";
    getSubCategoryId = (subCategoryTreeVM: SubCategoryTreeVM): string => subCategoryTreeVM?.id ?? "";
    getSubCategoryDisplay = (subCategoryTreeVM: SubCategoryTreeVM | null | undefined): string => subCategoryTreeVM?.Name ?? "";

    filterNonNumeric = ($event: KeyboardEvent) => {
        const charCode = $event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            $event.preventDefault();
            return false;
        }
        return true;
    };

    private refreshWeeks = (weekArray: number[]) => {
        this.saveTargets();
        this.weeks = weekArray;
        this.loadTargets();
    };

    private saveTargets = (): void => {
        this.weeks.forEach((w, i) => {
            this.weekTargetStore[w.toString()] = {
                lowerBound: this.weekTargetsControl.at(i).get("lower")?.value,
                upperBound: this.weekTargetsControl.at(i).get("upper")?.value,
            };
        });
    };

    private loadTargets = (): void => {
        this.weekTargetSubscriptions.forEach(s => s.unsubscribe());
        this.weekTargetSubscriptions = [];
        this.weekTargetsControl.clear();
        this.weeks.forEach(w => {
            const storedTarget = this.weekTargetStore[w.toString()];
            const lowerControl = this.fb.control(storedTarget?.lowerBound);
            const upperControl = this.fb.control(storedTarget?.upperBound, [
                weekGreaterThan(lowerControl, this.targetUpperBoundControl, this.targetLowerBoundControl)
            ]);
            if (!this.hasLowerTarget) lowerControl.disable();
            if (!this.hasUpperTarget) upperControl.disable();
            this.weekTargetsControl.push(this.fb.group({
                lower: lowerControl,
                upper: upperControl,
            }));
            this.weekTargetSubscriptions.push(lowerControl.valueChanges.subscribe(() => upperControl.updateValueAndValidity()));
        });
    };

    private bindData = (data: INumberDialogData) => {
        if (data.number) {
            this.bindNumber(data.number);
        } else {
            this.setDefaults();
        }
    };

    private bindNumber = (n: GetNumberDto) => {
        this.descriptionControl.setValue(n.description);
        this.quarterControl.setValue(n.planningPeriod);
        this.targetTypeControl.setValue(n.targetType);
        this.targetLowerBoundControl.setValue(n.target.lowerBound);
        this.targetUpperBoundControl.setValue(n.target.upperBound);
        this.allowWeeklyTargetsControl.setValue(n.allowWeeklyTargetOverrides);
        this.enteringTotalsControl.setValue(n.entryType === NumberEntryType.totals);
        this.isRecurringControl.setValue(n.isRecurring);
        this.numberTypeControl.setValue(n.type);
        this.scheduleTypeControl.setValue(n.scheduleDefinition.type);
        this.recurrenceControl.setValue(n.scheduleDefinition.recurrence);
        this.ownerControl.setValue(n.owner?.userId);
        this.updaterControl.setValue(n.updater?.userId);
        this.captureMethodControl.setValue(n.captureMethod);
        this.categoryControl.setValue(n.category?.id);
        this.subCategoryControl.setValue(n.category?.subCategory?.id);
        this.numberStatusControl.setValue(n.planningStatus);
        this.departmentControl.setValue(n.department?.id);
        this.externalDataControl.setValue(n.externalData);

        const weeks: number[] = [];
        for (const weekStr in n.weekTargets) {
            if (!Object.prototype.hasOwnProperty.call(n.weekTargets, weekStr)) continue;
            const week = parseInt(weekStr, 10);
            weeks.push(week);
        }
        this.weeks = weeks;
        this.weekTargetStore = { ...n.weekTargets };
        this.loadTargets();
    };

    private setDefaults = () => {
        this.quarterControl.setValue(this.planningPeriod);
        this.targetTypeControl.setValue(NumberTargetType.aboveTarget);
        this.allowWeeklyTargetsControl.setValue(false);
        this.enteringTotalsControl.setValue(false);
        this.isRecurringControl.setValue(true);
        this.numberTypeControl.setValue(NumberType.normal);
        this.scheduleTypeControl.setValue(UpdateScheduleType.everyWeek);
        this.recurrenceControl.setValue(null);
        this.captureMethodControl.setValue(CaptureMethod.manual);
        this.numberStatusControl.setValue(PlanningStatus.draft);

        this.weekTargetsControl.clear();
        this.weeks = [];
    };

    private mapQuarter = (quarter: number): IQuarter =>
        ({ financialYear: this.financialYear, quarter: quarter });

    private revalidateWeekUpperTargets = () => {
        this.weekTargetsControl.controls.forEach(c => {
            c.get("upper")?.updateValueAndValidity();
        });
    };

    private updateTargetState = () => {
        if (this.hasUpperTarget) {
            this.targetUpperBoundControl.enable();
        } else {
            this.targetUpperBoundControl.disable();
        }
        if (this.hasLowerTarget) {
            this.targetLowerBoundControl.enable();
        } else {
            this.targetLowerBoundControl.disable();
        }
        this.weekTargetsControl.controls.forEach(c => {
            const upper = c.get("upper");
            const lower = c.get("lower");
            if (this.hasUpperTarget) {
                upper?.enable();
            } else {
                upper?.disable();
            }
            if (this.hasLowerTarget) {
                lower?.enable();
            } else {
                lower?.disable();
            }
        });
    };

    private setDefaultOwnerUpdater = (users: RegistrationModel[]): void => {
        // Ensure the number is a new number and the owner is not yet set.
        if (this.numberId || this.ownerControl.value) return;

        const currentUserId = this.settingsService.userId;
        // Ensure the user is in the current team.
        if (!users.some(u => u.userId === currentUserId)) return;

        this.ownerControl.setValue(currentUserId);
        if (this.updaterControl.enabled) {
            this.updaterControl.setValue(currentUserId);
        }
    };

    private handleCaptureMethodChange = (captureMethod: CaptureMethod) => {
        if (captureMethod === CaptureMethod.automatic) {
            this.updaterControl.setValue(null);
            this.updaterControl.disable();
            this.externalDataControl.enable();
        } else {
            this.updaterControl.enable();
            this.externalDataControl.disable();
        }
    };

    private handleNumberTypeChange = (type: NumberType) => {
        if (!supportsAddingNumbers(type)) {
            if (this.enteringTotalsControl.enabled) {
                this.lastEnteringTotalsValue = this.enteringTotalsControl.value;
                this.enteringTotalsControl.disable();
                this.enteringTotalsControl.setValue(true);
            }
        } else {
            if (this.enteringTotalsControl.disabled) {
                this.enteringTotalsControl.setValue(this.lastEnteringTotalsValue);
                this.enteringTotalsControl.enable();
            }
        }
    };
}
