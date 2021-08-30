import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AppConfigResult } from "./models";

@Injectable({
    providedIn: 'root',
})
export class AppConfigService {
    constructor(protected http: HttpClient) { }

    getAppConfig(): Observable<AppConfigResult> {
        const url = 'api/AppConfig';
        return this.http.get<AppConfigResult>(url);
    }
}