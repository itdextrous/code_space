import { AuthorizedRoutes } from "~shared/role.guard";

import { NumbersComponent } from "./pages";

export const routes: AuthorizedRoutes = [
    {
        path: "numbers",
        component: NumbersComponent
    }
];
