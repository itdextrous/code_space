import { TranslateLoader } from "@ngx-translate/core";
import { from,Observable } from "rxjs";

/**
 * Translation loader adapted from ngx-translate example.
 * See: https://github.com/ngx-translate/http-loader/blob/master/README.md#angular-cliwebpack-translateloader-example
 */
export class WebpackTranslateLoader implements TranslateLoader {
    getTranslation(lang: string): Observable<any> {
        return from(import(`../assets/i18n/${lang}.json`));
    }
}
