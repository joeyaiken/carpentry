import {Injectable} from "@angular/core";
import {HttpService} from "../common/HttpService";
import {HttpClient} from "@angular/common/http";


@Injectable({
  providedIn: "root",
})
export class TrimmingToolService extends HttpService {
  constructor(
    http: HttpClient
  ) {
    super(http);
  }


}
