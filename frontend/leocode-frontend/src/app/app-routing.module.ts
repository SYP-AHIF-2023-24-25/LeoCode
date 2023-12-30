import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestResultComponent } from './test-result/test-result.component';
import { ResultHistoryComponent } from './result-history/result-history.component';


const routes: Routes = [
  {path: '', redirectTo: 'test-results', pathMatch: 'full' },
  {path: 'test-results', component: TestResultComponent},
  {path: 'result-history', component: ResultHistoryComponent}
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
