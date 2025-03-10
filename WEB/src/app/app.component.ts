import { Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { AngularFireModule } from "angularfire2";

import { environment } from "../environments/environment";
import { SettingsService } from "./services/settings.service";
@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"]
})

export class AppComponent implements OnInit {
    constructor(translateService: TranslateService, settingsService: SettingsService) {
        translateService.setDefaultLang("en");
        translateService.use(settingsService.defaultLanguage || "en");
    }

    ngOnInit() {
        AngularFireModule.initializeApp(environment.firebaseConfig);
    }
}
