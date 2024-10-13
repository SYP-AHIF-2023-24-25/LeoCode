import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestResultComponent } from './test-result/test-result.component';
import { ResultHistoryComponent } from './result-history/result-history.component';
import { IntroductionComponent } from './introduction/introduction.component';
import { CreateExerciseComponent } from './create-exercise/create-exercise.component';
import { StartScreenComponent } from './start-screen/start-screen.component';
import { ExerciseDetailsComponent } from './exercise-details/exercise-details.component';


const routes: Routes = [
  {path: '', redirectTo: 'start-screen', pathMatch: 'full' },
  {path: 'test-results', component: TestResultComponent},
  {path: 'result-history', component: ResultHistoryComponent},
  {path: 'introdcution', component: IntroductionComponent},
  {path: 'create-exercise', component: CreateExerciseComponent},
  {path: 'start-screen', component: StartScreenComponent},
  {path: 'exercise-details', component: ExerciseDetailsComponent}
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
