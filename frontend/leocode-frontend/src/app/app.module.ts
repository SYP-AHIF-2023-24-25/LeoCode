import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestResultComponent } from './test-result/test-result.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ResultHistoryComponent } from './result-history/result-history.component';
import { IntroductionComponent } from './introduction/introduction.component';
import { MonacoEditorModule } from 'ngx-monaco-editor-v2';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { CreateExerciseComponent } from './create-exercise/create-exercise.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { StartScreenComponent } from './start-screen/start-screen.component';
import { LoginComponent } from './login/login.component';
import { KeycloakBearerInterceptor, KeycloakService } from 'keycloak-angular';
import { StudentStartScreenComponent } from './student-start-screen/student-start-screen.component';
import { ExerciseDetailsComponent } from './exercise-details/exercise-details.component';
import { CreateAssignmentComponent } from './create-assignment/create-assignment.component';
import { JoinAssignmentComponentComponent } from './join-assignment-component/join-assignment-component.component';
import { AssignmentOverviewComponent } from './assignment-overview/assignment-overview.component';


function initializeKeycloak(keycloak: KeycloakService) {
  return () => keycloak.init({
    config: {
      url: 'https://auth.htl-leonding.ac.at',
      realm: 'htlleonding',
      clientId: 'htlleonding-service',
    },
    initOptions: {
      onLoad: 'check-sso',
      pkceMethod: 'S256',
      flow: 'implicit',
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
    },
    enableBearerInterceptor: true,
    bearerPrefix: 'Bearer',
  });
}

@NgModule({
  declarations: [
    AppComponent,
    TestResultComponent,
    ResultHistoryComponent,
    IntroductionComponent,
    CreateExerciseComponent,
    StartScreenComponent,
    LoginComponent,
    StudentStartScreenComponent,
    ExerciseDetailsComponent,
    CreateAssignmentComponent,
    JoinAssignmentComponentComponent,
    AssignmentOverviewComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MonacoEditorModule.forRoot(),
    BrowserAnimationsModule,
    MatChipsModule,
    MatFormFieldModule,
    MatIconModule,
    MatAutocompleteModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatSelectModule,
    BrowserModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatInputModule,
    MatFormFieldModule,
    MatNativeDateModule
  ],
  providers: [KeycloakService,
    {
      provide: APP_INITIALIZER,
      useFactory: initializeKeycloak,
      deps: [KeycloakService],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: KeycloakBearerInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }