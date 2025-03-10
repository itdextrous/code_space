import { MatIconRegistry } from "@angular/material/icon";
import { DomSanitizer } from "@angular/platform-browser";

interface IconMetadata {
    name: string;
    path: string;
}

const icons: IconMetadata[] = [
    {
        name: "wf-add",
        path: "add.svg"
    },
    {
        name: "wf-goal-check",
        path: "goal-check.svg"
    },
    {
        name: "wf-dept-finance",
        path: "departments/finance.svg"
    },
    {
        name: "wf-dept-operations",
        path: "departments/operations.svg"
    },
    {
        name: "wf-dept-management",
        path: "departments/management.svg"
    },
    {
        name: "wf-dept-sales-marketing",
        path: "departments/sales-marketing.svg"
    },
];

const baseIconPath = "./assets/images/icons/";

export const addIcons = (iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) => {
    icons.forEach(icon =>
        iconRegistry.addSvgIcon(icon.name, sanitizer.bypassSecurityTrustResourceUrl(baseIconPath + icon.path)));
};
