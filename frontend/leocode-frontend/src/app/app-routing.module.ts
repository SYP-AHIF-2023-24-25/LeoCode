import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestResultComponent } from './test-result/test-result.component';
import { ResultHistoryComponent } from './result-history/result-history.component';
import { IntroductionComponent } from './introduction/introduction.component';
import { CreateExerciseComponent } from './create-exercise/create-exercise.component';


const routes: Routes = [
  {path: '', redirectTo: 'create-exercise', pathMatch: 'full' },
  {path: 'test-results', component: TestResultComponent},
  {path: 'result-history', component: ResultHistoryComponent},
  {path: 'introdcution', component: IntroductionComponent},
  {path: 'create-exercise', component: CreateExerciseComponent}
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
