import { Component, EventEmitter, forwardRef, Input, OnDestroy, OnInit, Output } from "@angular/core";
import { ControlValueAccessor, FormControl, NG_VALIDATORS, NG_VALUE_ACCESSOR, Validators } from "@angular/forms";
import { CompanyXeroExternalDataService, XeroAccountDto, XeroCashInBankAccountRetrieverInfoDto } from "@api";
import { Subscription } from "rxjs";

import { SettingsService } from "~services/settings.service";

@Component({
    selector: "app-xero-bank-account-selector",
    templateUrl: "./xero-bank-account-selector.component.html",
    styleUrls: ["./xero-bank-account-selector.component.scss"],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            // eslint-disable-next-line @typescript-eslint/no-use-before-define
            useExisting: forwardRef(() => XeroBankAccountSelectorComponent),
            multi: true
        },
        {
            provide: NG_VALIDATORS,
            // eslint-disable-next-line @typescript-eslint/no-use-before-define
            useExisting: forwardRef(() => XeroBankAccountSelectorComponent),
            multi: true
        }
    ]
})
export class XeroBankAccountSelectorComponent implements OnInit, OnDestroy, ControlValueAccessor {

    @Input() set value(data: XeroCashInBankAccountRetrieverInfoDto) {
        if (!data || data.type !== "xeroCashInBankAccountRetriever") return;

        this.bankAccountInput = data.bankAccountId;
        this.setSelectedAccount(data.bankAccountId);
    }

    @Output() selected = new EventEmitter<XeroCashInBankAccountRetrieverInfoDto>();

    bankAccounts?: XeroAccountDto[];
    bankAccountControl = new FormControl(null, [Validators.required]);

    isLoading = true;

    buttonIconImg = "assets/images/wait.gif";

    private bankAccountInput?: string;

    private onChangedCallback?: (_: XeroCashInBankAccountRetrieverInfoDto) => void;
    private onTouchedCallback?: () => void;

    private subscription?: Subscription;

    constructor(
        private readonly settingsSerivce: SettingsService,
        private readonly xeroDataService: CompanyXeroExternalDataService) {
    }

    ngOnInit(): void {
        this.subscription = this.bankAccountControl.valueChanges.subscribe(this.emitData);

        this.xeroDataService.companyXeroExternalDataGetAccounts(this.settingsSerivce.currentCompanyId ?? "", 1)
            .subscribe(bankAccounts => {
                this.bankAccounts = bankAccounts;
                this.isLoading = false;
                this.buttonIconImg = "";
                this.setSelectedAccount(this.bankAccountInput);
            });
    }

    ngOnDestroy(): void {
        this.subscription?.unsubscribe();
    }

    writeValue(obj: XeroCashInBankAccountRetrieverInfoDto): void {
        this.value = obj;
    }

    registerOnChange(fn: (_: XeroCashInBankAccountRetrieverInfoDto) => void): void {
        this.onChangedCallback = fn;
    }

    registerOnTouched(fn: () => void): void {
        this.onTouchedCallback = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        if (isDisabled) {
            this.bankAccountControl.disable();
        } else {
            this.bankAccountControl.enable();
        }
    }

    validate = (_: FormControl) => this.bankAccountControl.valid ? null : { xeroBankAccount: { valid: false } };

    setSelectedAccount = (bankAccountId?: string) => {
        // Ensure that the account was not deleted and therefore is in the accounts list.
        if (this.bankAccounts?.some(x => x.id === bankAccountId)) {
            this.bankAccountControl.setValue(bankAccountId);
        } else {
            this.emitData();
        }
    };

    private emitData = () => {
        const eventData: XeroCashInBankAccountRetrieverInfoDto = {
            type: "xeroCashInBankAccountRetriever",
            bankAccountId: this.bankAccountControl.value
        };

        this.selected.emit(eventData);
        this.onChangedCallback?.(eventData);
        this.onTouchedCallback?.();
    };
}
