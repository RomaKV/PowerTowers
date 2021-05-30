import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { HomeComponent } from "./home/home.component";
import { CalcPercentPipe } from "./pipes/calc-percent.pipe";
import { CalcStateColorPipe } from "./pipes/calc-state-color.pipe";
import { CalcHeightIndicatorPipe } from "./pipes/calc-height-indicator.pipe";
import { Error404PageComponent } from "./error-404-page/error-404-page.component";
import { IndicatorEditComponent } from "./indicator/indicator-edit/indicator-edit.component";
import { IndicatorNewComponent } from "./indicator/indicator-new/indicator-new.component";
import { ErrorMessageComponent } from "./error-message/error-message.component";


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CalcPercentPipe,
    CalcStateColorPipe,
    CalcHeightIndicatorPipe,
    Error404PageComponent,
    IndicatorEditComponent,
    IndicatorNewComponent,
    ErrorMessageComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: "", component: HomeComponent },
      { path: "edit/:id", component: IndicatorEditComponent },
      { path: "new", component: IndicatorNewComponent },
      { path: "error404", component: Error404PageComponent },
      { path: "**", redirectTo: "/error404" }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
