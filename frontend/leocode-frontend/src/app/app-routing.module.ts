import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestResultComponent } from './test-result/test-result.component';
import { ResultHistoryComponent } from './result-history/result-history.component';
import { IntroductionComponent } from './introduction/introduction.component';
import { CreateExerciseComponent } from './create-exercise/create-exercise.component';
import { StartScreenComponent } from './start-screen/start-screen.component';
import { ExerciseDetailsComponent } from './exercise-details/exercise-details.component';
import { StudentStartScreenComponent } from './student-start-screen/student-start-screen.component';
import { AuthGuard } from '../core/util/auth-guard';
import { LoginComponent } from './login/login.component';



export const routes: Routes = [
  {path: '', redirectTo: '/login', pathMatch: 'full' },
  {path: 'login', component: LoginComponent},
  {path: 'test-results', component: TestResultComponent},
  {path: 'result-history', component: ResultHistoryComponent},
  {path: 'introduction', component: IntroductionComponent},
  {path: 'create-exercise', component: CreateExerciseComponent},
  {path: 'start-screen', component: StartScreenComponent},
  {path: 'exercise-details', component: ExerciseDetailsComponent},
  {path: 'student-start-screen', component: StudentStartScreenComponent, canActivate: [AuthGuard]}
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
