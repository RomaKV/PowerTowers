import { Injectable } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";
import { IndicatorService } from "../indicator/indicator.service";

@Injectable({
  providedIn: "root"
})
export class ErrorService {
  public isOffline?: boolean;
  public isError?: boolean;
  public message = "";
  public errorCode?: number;
  public url?: string;

  constructor(
    public router: Router,
    private indicatorService: IndicatorService
  ) {
  }

  reset() {
    this.isOffline = false;
    this.message = "";
    this.errorCode = 0;
    this.url = "";
  }

  public setError(err: HttpErrorResponse): void {
    if (err.ok === false && err.status === 0) {
      if (this.isOffline === false) {
        this.isOffline = true;
      }
    } else {
      this.message = err.message;
      this.errorCode = err.status;
      this.url = err.url;

      if (err.status === 403 || err.status === 401) {
        this.router.navigate([""]);
      } else if (err.status === 404) {
        this.router.navigate(["/error404"]);
      } else if (err.status === 401) {
        this.errorCode = 401;

      }
    }

    this.indicatorService.load = false;
    this.indicatorService.silentLoad = false;
  }
}
