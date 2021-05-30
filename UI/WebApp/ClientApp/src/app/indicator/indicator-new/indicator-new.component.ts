import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { ErrorService } from "src/app/services/error/error.service";
import { HttpErrorResponse, HttpResponse } from "@angular/common/http";
import { SubscriptionLike } from "rxjs";
import { IndicatorService, IndicatorDto as Indicator } from "../../services/indicator/indicator.service";
import { Router } from "@angular/router";


@Component({
  selector: "indicator-new",
  templateUrl: "./indicator-new.component.html",
  styleUrls: ["./indicator-new.component.css"]
})
export class IndicatorNewComponent implements OnInit, OnDestroy {
  public formNew: FormGroup;
  private querySubscriptions: SubscriptionLike[] = [];
  private allLifeSubscriptions: SubscriptionLike[] = [];

  constructor(
    public indicatorService: IndicatorService,
    private errorService: ErrorService,
    private router: Router
  ) {
  }

  ngOnInit() {

    this.formNew = new FormGroup({
      title: new FormControl("", Validators.required),
      minValue: new FormControl(0, Validators.required),
      maxValue: new FormControl(0, Validators.required),
    });


  }

  submit() {
    if (this.formNew.valid) {
      const indicator: Indicator = {
        id: "",
        title: this.formNew.value.title,
        minValue: this.formNew.value.minValue,
        maxValue: this.formNew.value.maxValue,
      };

      this.querySubscriptions.push(
        this.indicatorService.new(indicator).subscribe(
          (response: HttpResponse<Indicator>) => {
            if (response) {
              console.log("response.body", response.body);
              this.router.navigate(["/"]);
            }
            this.unsubscribe(this.querySubscriptions);
          },
          (error: HttpErrorResponse) => {
            this.errorService.setError(error);
            this.unsubscribe(this.querySubscriptions);

          }
        )
      );
    }
  }

  unsubscribe(subscriptions: SubscriptionLike[]) {
    subscriptions.forEach(subscription => subscription.unsubscribe());
    subscriptions = [];
  }

  ngOnDestroy(): void {
    this.unsubscribe(this.querySubscriptions);
    this.unsubscribe(this.allLifeSubscriptions);
  }
}
