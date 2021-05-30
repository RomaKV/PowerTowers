import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { ErrorService } from "src/app/services/error/error.service";
import { HttpErrorResponse, HttpResponse } from "@angular/common/http";
import { SubscriptionLike } from "rxjs";
import { IndicatorService, IndicatorDto as Indicator } from "../../services/indicator/indicator.service";
import { ActivatedRoute, Params } from "@angular/router";
import { Router } from "@angular/router";


@Component({
  selector: "indicator-edit",
  templateUrl: "./indicator-edit.component.html",
  styleUrls: ["./indicator-edit.component.css"]
})
export class IndicatorEditComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  private querySubscriptions: SubscriptionLike[] = [];
  private allLifeSubscriptions: SubscriptionLike[] = [];
  private indicatorId: string;

  constructor(
    private route: ActivatedRoute,
    public indicatorService: IndicatorService,
    private errorService: ErrorService,
    private router: Router
  ) {
  }

  ngOnInit() {

    this.allLifeSubscriptions.push(
      this.route.params.subscribe((params: Params) => {
          if (this.indicatorService.indicators) {

            this.indicatorId = params["id"];
            const indicator = this.indicatorService.indicators.find(p => p.id === this.indicatorId);

            if (indicator) {
              this.form = new FormGroup({
                title: new FormControl(indicator.title, Validators.required),
                minValue: new FormControl(indicator.minValue, Validators.required),
                maxValue: new FormControl(indicator.maxValue, Validators.required),
              });
            } else {
              this.router.navigate(["/error404"]);
            }
          }
        }
      )
    );

  }

  submit() {
    if (this.form.valid) {
      const indicator: Indicator = {
        id: this.indicatorId,
        title: this.form.value.title,
        minValue: this.form.value.minValue,
        maxValue: this.form.value.maxValue,
      };

      this.querySubscriptions.push(
        this.indicatorService.update(indicator).subscribe(
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


  delete() {

    const indicator: Indicator = {
      id: this.indicatorId,
      title: "",
      minValue: 0,
      maxValue: 0,
    };

    console.log("Id", indicator);
    this.querySubscriptions.push(
      this.indicatorService.delete(indicator).subscribe(
        (response: HttpResponse<number>) => {
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


  unsubscribe(subscriptions: SubscriptionLike[]) {
    subscriptions.forEach(subscription => subscription.unsubscribe());
    subscriptions = [];
  }

  ngOnDestroy(): void {
    this.unsubscribe(this.querySubscriptions);
    this.unsubscribe(this.allLifeSubscriptions);
  }
}
