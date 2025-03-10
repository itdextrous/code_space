
import "intl";
import "intl/locale-data/jsonp/en";

import { LocationStrategy, PathLocationStrategy } from "@angular/common";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { APP_INITIALIZER, Injector, NgModule } from "@angular/core";
import { FlexLayoutModule } from "@angular/flex-layout";
import { FormsModule } from "@angular/forms";
import { MAT_AUTOCOMPLETE_DEFAULT_OPTIONS } from "@angular/material/autocomplete";
import { MatIconRegistry } from "@angular/material/icon";
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS } from "@angular/material-moment-adapter";
import { DomSanitizer } from "@angular/platform-browser";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { PreloadAllModules, RouterModule } from "@angular/router";
import { TranslateLoader, TranslateModule } from "@ngx-translate/core";
import { TINYMCE_SCRIPT_SRC } from "@tinymce/tinymce-angular";
import { MDBBootstrapModule } from "angular-bootstrap-md";
import { AngularFireModule } from "angularfire2";
import { AngularFireDatabaseModule } from "angularfire2/database";
import { IntercomModule } from "ng-intercom";
import { PdfViewerModule } from "ng2-pdf-viewer";
import { NgxMaskModule } from "ngx-mask";
import { ToastrModule } from "ngx-toastr";
import { ToastrService } from "ngx-toastr";

import { ApiModule, BASE_PATH } from "~/api";
import { TimeMachineGuard } from "~/utils/time-machine-guard";
import { TimeMachineInterceptor } from "~/utils/time-machine-interceptor";
import { AnnualPlanningRepository, QuarterlyPlanningRepository } from "~repositories";
import { GlobalFeatureFlagService, initializeGlobalFlagsFactory } from "~services/global-feature-flag.service";
import { IntercomService } from "~services/intercom.service";

import { environment } from "../environments/environment";
import { apiBaseURL } from "./api-config/app-api.config";
import { AppComponent } from "./app.component";
import { setAppInjector } from "./app.injector";
import { appRoutes } from "./app.routing";
import { AuthComponent } from "./auth/auth.component";
import { AuthService } from "./auth/auth.service";
import { ErrorInterceptor } from "./auth/request-interceptor.service";
import { DialogModule } from "./dialog.module";
import { addIcons } from "./icons";
import { FullComponent } from "./layouts/full/full.component";
import { AppHeaderComponent } from "./layouts/full/header/header.component";
import { MenuListItemComponent } from "./layouts/full/sidebar/menu-list-item/menu-list-item.component";
import { AppSidebarComponent } from "./layouts/full/sidebar/sidebar.component";
import { MaterialComponentsModule } from "./material-component/material-component.module";
import { MaterialModule } from "./material-module";
import { DesiredOptionsEnums, DocumentTypeEnums, Enums, PriorityType, ResourceTypeEnums, YearStatusEnums } from "./pages/pages.enum";
import { PaymentGuard } from "./payment/payment.guard";
import { PrivacypolicyComponent } from "./privacypolicy/privacypolicy.component";
import { RootRedirectComponent } from "./root-redirect/root-redirect.component";
import { AccessService } from "./services/access.service";
import { ActionsService } from "./services/actions.service";
import { FeatureFlagService } from "./services/featureflag.service";
import { GoalsService } from "./services/goals.service";
import { HirePlanService } from "./services/hirePlan.service";
import { IssuesService } from "./services/issues.service";
import { MarketStrategyService } from "./services/marketstrategy.service";
import { NotificationService } from "./services/notification.service";
import { NumberService } from "./services/numbers.service";
import { PlanService } from "./services/plan.service";
import { SetUp } from "./services/setup.service";
import { StripeSevice } from "./services/stripe.service";
import { UserService } from "./services/user.service";
import { VisionService } from "./services/vision.service";
import { WeightingService } from "./services/weighting.service";
import { SharedModule } from "./shared/shared.module";
import { SpinnerComponent } from "./shared/spinner.component";
import { STRIPE_INSTANCE } from "./shared/util/constants";
import { WebpackTranslateLoader } from "./webpack-translate-loader";

@NgModule({
    declarations: [
        AppComponent,
        FullComponent,
        AuthComponent,
        AppHeaderComponent,
        SpinnerComponent,
        AppSidebarComponent,
        PrivacypolicyComponent,
        MenuListItemComponent,
        RootRedirectComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MaterialModule,
        FormsModule,
        FlexLayoutModule,
        HttpClientModule,
        ApiModule,
        SharedModule,
        NgxMaskModule.forRoot(),
        RouterModule.forRoot(appRoutes, { preloadingStrategy: PreloadAllModules }),
        ToastrModule.forRoot(), DialogModule,
        MaterialComponentsModule,
        AngularFireModule.initializeApp(environment.firebaseConfig),
        AngularFireDatabaseModule,
        MDBBootstrapModule,
        PdfViewerModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useClass: WebpackTranslateLoader
            }
        }),
        IntercomModule.forRoot({
            appId: environment.intercomAppId,
            updateOnRouterChange: true
        })
    ],
    entryComponents: [],
    providers: [
        {
            provide: LocationStrategy,
            useClass: PathLocationStrategy,
        },
        { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: TimeMachineInterceptor, multi: true },
        { provide: BASE_PATH, useValue: apiBaseURL.baseURL.replace(/\/$/, "") },
        { provide: STRIPE_INSTANCE, useFactory: () => window.Stripe?.(apiBaseURL.stripeKey) },
        { provide: MAT_AUTOCOMPLETE_DEFAULT_OPTIONS, useValue: { autoActiveFirstOption: true } },
        { provide: TINYMCE_SCRIPT_SRC, useValue: "assets/tinymce/tinymce.min.js" },
        { provide: APP_INITIALIZER, useFactory: initializeGlobalFlagsFactory, deps: [GlobalFeatureFlagService], multi: true },
        AuthService,
        SetUp,
        PlanService,
        NumberService,
        GoalsService,
        MarketStrategyService,
        HirePlanService,
        IssuesService,
        ActionsService,
        FeatureFlagService,
        WeightingService,
        NotificationService,
        ToastrService,
        VisionService,
        AppHeaderComponent,
        StripeSevice,
        AccessService,
        AnnualPlanningRepository,
        QuarterlyPlanningRepository,
        Enums,
        ResourceTypeEnums,
        DocumentTypeEnums,
        YearStatusEnums,
        DesiredOptionsEnums,
        PriorityType,
        FullComponent,
        PaymentGuard,
        TimeMachineGuard
    ],
    bootstrap: [AppComponent]
})

export class AppModule {
    constructor(injector: Injector, userService: UserService, iconRegistry: MatIconRegistry, sanitizer: DomSanitizer, intercomService: IntercomService) {
        addIcons(iconRegistry, sanitizer);
        setAppInjector(injector);
        userService.initialise();
        intercomService.initialise();
    }
}
