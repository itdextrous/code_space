import { Role } from "~shared/enums";
import { AuthorizedRoutes, RoleGuard } from "~shared/role.guard";

import { AuthComponent } from "./auth/auth.component";
import { AuthGuard } from "./auth/auth.guard";
import { FullComponent } from "./layouts/full/full.component";
import { OnboardingGuard } from "./onboarding/onboarding.guard";
import { PaymentGuard } from "./payment/payment.guard";
import { PrivacypolicyComponent } from "./privacypolicy/privacypolicy.component";
import { RootRedirectComponent } from "./root-redirect/root-redirect.component";
import { SignupGuard } from "./signup/signup.guard";

export const appRoutes: AuthorizedRoutes = [
    {
        path: "",
        component: AuthComponent,
        children: [
            {
                path: "",
                component: RootRedirectComponent,
                pathMatch: "full"
            },
            {
                path: "auth",
                loadChildren: () => import("./auth/auth.module").then(m => m.AuthComponentsModule)
            }
        ]
    },
    {
        path: "signup",
        loadChildren: () => import("./signup/signup.module").then(m => m.SignupModule),
        canActivate: [SignupGuard],
    },
    {
        path: "onboarding",
        loadChildren: () => import("./onboarding/onboarding.module").then(m => m.OnboardingModule),
        canActivate: [OnboardingGuard],
    },
    {
        path: "payment",
        loadChildren: () => import("./payment/payment.module").then(m => m.PaymentModule),
        canActivate: [PaymentGuard],
    },
    {
        path: "privacypolicy",
        component: PrivacypolicyComponent
    },
    {
        path: "",
        component: FullComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "pages",
                loadChildren: () => import("./pages/pages.module").then(m => m.PagesComponentsModule)
            },
            {
                path: "dashboard",
                loadChildren: () => import("./dashboard/dashboard.module").then(m => m.DashboardModule)
            },
            {
                path: "quarterly-planning",
                loadChildren: () => import("./quarterly-planning/quarterly-planning.module").then(m => m.QuarterlyPlanningModule)
            },
            {
                path: "help",
                loadChildren: () => import("./help/help.module").then(m => m.HelpModule),
                canActivate: [RoleGuard],
                data: {
                    roles: [Role.superAdmin]
                }
            },
            {
                path: "platform-admin",
                loadChildren: () => import("./platform-admin/platform-admin.module").then(m => m.PlatformAdminModule),
                canActivate: [RoleGuard],
                data: {
                    roles: [Role.superAdmin]
                }
            }
        ]
    },
    {
        path: "**",
        redirectTo: "auth/login",
        pathMatch: "full"
    }
];

