import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestResultComponent } from './test-result/test-result.component';
import { FormsModule } from '@angular/forms';
import { ResultHistoryComponent } from './result-history/result-history.component';
import { IntroductionComponent } from './introduction/introduction.component';


@NgModule({
  declarations: [
    AppComponent,
    TestResultComponent,
    ResultHistoryComponent,
    IntroductionComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
