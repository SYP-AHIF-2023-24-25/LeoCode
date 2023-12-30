import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestResultComponent } from './test-result/test-result.component';
import { FormsModule } from '@angular/forms';
import { TestHistoryComponent } from './test-history/test-history.component';

@NgModule({
  declarations: [
    AppComponent,
    TestResultComponent,
    TestHistoryComponent
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
