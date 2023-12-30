import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestResultComponent } from './test-result/test-result.component';
import { TestHistoryComponent } from './test-history/test-history.component';

const routes: Routes = [
  {path: '', redirectTo: 'test-results', pathMatch: 'full' },
  {path: 'test-results', component: TestResultComponent},
  {path: 'test-history', component: TestHistoryComponent}
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
