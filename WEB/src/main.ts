import { enableProdMode } from "@angular/core";
import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";
import * as countries from "i18n-iso-countries";

import { AppModule } from "./app/app.module";
import { environment } from "./environments/environment";
import { initTimeMachine } from "./utils/time-machine";

// eslint-disable-next-line @typescript-eslint/no-var-requires
const countryLocale = require("i18n-iso-countries/langs/en.json");

countries.registerLocale(countryLocale);

if (environment.production) {
    enableProdMode();
}
if (environment.timeMachine) {
    initTimeMachine();
}

platformBrowserDynamic().bootstrapModule(AppModule)
    .catch(err => console.log(err));
