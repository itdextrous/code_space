import { Component, EventEmitter, forwardRef, Input, OnDestroy, OnInit, Output } from "@angular/core";
import { ControlValueAccessor, FormBuilder, FormControl, NG_VALIDATORS, NG_VALUE_ACCESSOR, Validator, Validators } from "@angular/forms";
import { CompanyTeamNumberExternalDataDto, XeroCashInBankAccountRetrieverInfoDto, XeroCashInBankTotalRetrieverInfoDto } from "@api";
import { Subscription } from "rxjs";

import { FeatureFlagService } from "~services/featureflag.service";
import { valueAndChanges } from "~shared/util/util";

// TODO - Setup CompanyTeamNumberExternalDataRetrieverInfoDto as base class in Swashbuckle.
type CompanyTeamNumberExternalDataRetrieverInfoDto = XeroCashInBankTotalRetrieverInfoDto | XeroCashInBankAccountRetrieverInfoDto;

type XeroExternalDataRetrieverInfo =
    XeroCashInBankTotalRetrieverInfoDto.TypeEnum |
    XeroCashInBankAccountRetrieverInfoDto.TypeEnum;

interface CompanyTeamNumberExternalDataRetrieverInfoState {
    data: CompanyTeamNumberExternalDataRetrieverInfoDto;
    isEnabled: boolean;
}

type ExternalDataRetrieverInfo = XeroExternalDataRetrieverInfo;
type ExternalDataRetrieverInfoAll = { [K in ExternalDataRetrieverInfo]: CompanyTeamNumberExternalDataRetrieverInfoState };

@Component({
    selector: "app-external-data",
    templateUrl: "./external-data.component.html",
    styleUrls: ["./external-data.component.scss"],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            // eslint-disable-next-line @typescript-eslint/no-use-before-define
            useExisting: forwardRef(() => ExternalDataComponent),
            multi: true
        },
        {
            provide: NG_VALIDATORS,
            // eslint-disable-next-line @typescript-eslint/no-use-before-define
            useExisting: forwardRef(() => ExternalDataComponent),
            multi: true
        }
    ]
})
export class ExternalDataComponent implements ControlValueAccessor, Validator, OnInit, OnDestroy {

    @Input() set value(value: CompanyTeamNumberExternalDataDto) {
        if (!value) return; // No external data has been setup.

        const retrieverInfo = value.retrieverInfo as CompanyTeamNumberExternalDataRetrieverInfoDto;
        this.retrieverInfoControl.setValue(retrieverInfo.type);
        this.scheduleControl.setValue(value.retrieveAtTimeOfDay);
        switch (retrieverInfo.type) {
            case "xeroCashInBankAccountRetriever":
                this.xeroAccountControl.setValue(retrieverInfo);
                break;
        }
    }

    @Output() updated = new EventEmitter<CompanyTeamNumberExternalDataDto>();

    readonly externalRetrievers: ExternalDataRetrieverInfoAll = {
        xeroCashInBankAccountRetriever: {
            data: { type: "xeroCashInBankAccountRetriever", bankAccountId: "" },
            isEnabled: this.featureFlagService.xeroIntegrationEnabled,
        },
        xeroCashInBankTotalRetriever: {
            data: { type: "xeroCashInBankTotalRetriever" },
            isEnabled: this.featureFlagService.xeroIntegrationEnabled
        }
    };

    retrieverInfoControl = this.fb.control(null);
    scheduleControl = this.fb.control({ value: null, disabled: true }, [Validators.required]);
    xeroAccountControl = this.fb.control(
        { value: this.externalRetrievers.xeroCashInBankAccountRetriever.data, disabled: true },
        [Validators.required]);

    form = this.fb.group({
        retrieverInfo: this.retrieverInfoControl,
        schedule: this.scheduleControl,
        xeroAccount: this.xeroAccountControl
    });

    get externalDataSelected() {
        return !!this.retrieverInfoControl.value;
    }

    private onChangedCallback?: (_: CompanyTeamNumberExternalDataDto | undefined) => void;
    private onTouchedCallback?: () => void;

    private subscriptions: Subscription[] = [];

    constructor(
        private readonly featureFlagService: FeatureFlagService,
        private readonly fb: FormBuilder,
    ) {
    }

    ngOnInit(): void {
        this.subscriptions.push(valueAndChanges(this.retrieverInfoControl).subscribe(this.onRetrieverInfoChange));
        this.subscriptions.push(valueAndChanges(this.xeroAccountControl).subscribe(this.onXeroAccountChange));
        this.subscriptions.push(valueAndChanges(this.scheduleControl).subscribe(this.emitData));
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    writeValue(obj: CompanyTeamNumberExternalDataDto): void {
        this.value = obj;
    }

    registerOnChange(fn: (_: CompanyTeamNumberExternalDataDto | undefined) => void): void {
        this.onChangedCallback = fn;
    }

    registerOnTouched(fn: () => void): void {
        this.onTouchedCallback = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        if (isDisabled) {
            this.form.disable();
        } else {
            this.form.enable();
        }
    }

    validate = (_: FormControl) => this.form.valid ? null : { externalData: { valid: false } };

    get externalRetrieversEnabled(): string[] {
        return Object.keys(this.externalRetrievers)
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            .map(x => (this.externalRetrievers as any)[x] as CompanyTeamNumberExternalDataRetrieverInfoState)
            .filter(x => x.isEnabled)
            .map(x => x.data.type);
    }

    getExternalRetrieverKey(key: ExternalDataRetrieverInfo) {
        return "externalData." + key;
    }

    get retrieveInfoType(): ExternalDataRetrieverInfo {
        return this.retrieverInfoControl.value;
    }

    private emitData = (): void => {
        const data = this.getFormState();
        this.updated.emit(data);
        this.onChangedCallback?.(data);
        this.onTouchedCallback?.();
    };

    private getFormState = (): CompanyTeamNumberExternalDataDto | undefined => {
        // Indicate that the retriever has been de-selected.
        if (!this.externalDataSelected) {
            return undefined;
        }

        return {
            retrieverInfo: this.externalRetrievers[this.retrieveInfoType].data,
            retrieveAtTimeOfDay: this.scheduleControl.value
        };
    };

    private onRetrieverInfoChange = (type: ExternalDataRetrieverInfo): void => {
        if (type === "xeroCashInBankAccountRetriever") {
            this.xeroAccountControl.enable({ emitEvent: false });
            this.xeroAccountControl.updateValueAndValidity({ emitEvent: false });
        } else {
            this.xeroAccountControl.disable();
        }

        // If the external data type is actually set then enable the schedule control.
        // Otherwise, allow no external data source to be set (but the number can still be automatic).
        if (type) {
            this.scheduleControl.enable({ emitEvent: false });
        } else {
            this.scheduleControl.disable({ emitEvent: false });
        }

        this.emitData();
    };

    private onXeroAccountChange = (retrieverInfo: XeroCashInBankAccountRetrieverInfoDto) => {
        this.externalRetrievers.xeroCashInBankAccountRetriever.data = retrieverInfo;
        this.xeroAccountControl.updateValueAndValidity({ emitEvent: false });
        this.emitData();
    };
}
