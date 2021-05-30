import { Component, OnInit, OnDestroy } from "@angular/core";
import { IndicatorService, IndicatorDto as Indicator } from "../services/indicator/indicator.service";
import { timer, SubscriptionLike } from "rxjs";
import { ErrorService } from "../services/error/error.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"]
})
export class HomeComponent implements OnInit, OnDestroy {

  private timersSubscriptions: SubscriptionLike[] = [];
  private querySubscriptions: SubscriptionLike[] = [];
  private intervalUpdate = 60000;

  constructor(
    public indicatorService: IndicatorService,
    private error: ErrorService
  ) {


  }

  ngOnInit() {
    this.error.reset();
    this.indicatorService.load = true;
    this.loadTimer();
  }


  loadTimer() {
    this.timersSubscriptions.push(
      timer(0, this.intervalUpdate).subscribe(() => {
        this.load();
      })
    );
  }

  load() {
    this.unsubscribe(this.querySubscriptions);

    this.querySubscriptions.push(
      this.indicatorService.getAll().subscribe(
        response => {
          this.error.reset();
          if (response) {
            console.log("Indicators uploaded.", response);
            this.indicatorService.indicators = response.sort((a, b) => a.title > b.title ? 1 : -1);
            this.indicatorService.load = false;
          }
        },
        error => this.error.setError(error),
        null
      )
    );
  }


  unsubscribe(subscriptions: SubscriptionLike[]) {
    subscriptions.forEach(subscription => subscription.unsubscribe());
    subscriptions = [];
  }


  ngOnDestroy(): void {
    this.unsubscribe(this.querySubscriptions);
    this.unsubscribe(this.timersSubscriptions);
  }
}
