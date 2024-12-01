import { KeycloakService } from 'keycloak-angular';
import { Router } from '@angular/router';
import { Component, computed, inject, Signal, signal, WritableSignal, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { finalize } from "rxjs";
import { DbService } from '../service/db-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit  {
  private readonly httpClient: HttpClient = inject(HttpClient);
  public readonly response: WritableSignal<string | null> = signal(null);
  public readonly loading: WritableSignal<boolean> = signal(false);
  public readonly showResponse: Signal<boolean> = computed(() => this.response() !== null);
  constructor(private keycloakService: KeycloakService, private router: Router, private rest: DbService) {

  }

  async ngOnInit(): Promise<void> {
    if (sessionStorage.getItem('shouldLogOut') === 'true') {
      sessionStorage.setItem('shouldLogOut', 'false');
      await this.keycloakService.logout();
    } else {
      const isLoggedIn = await this.keycloakService.isLoggedIn();
      if (isLoggedIn) {
        await this.setUserData();
      } else {
        await this.keycloakService.login();
      }
      this.performCall('at-least-student');
    }
  }

  public performCall(action: string): void {
    const route = `http://localhost:5050/api/demo/${action}`;

    this.loading.set(true);

    // bearer token is automatically added by the interceptor
    this.httpClient.get(route, { responseType: "text" })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (res) => {
          this.response.update(() => res);
          const ifUserName: string = sessionStorage.getItem('ifUserName') || '';
          const firstname: string = sessionStorage.getItem('firstName') || '';
          const lastname: string = sessionStorage.getItem('lastName') || '';
          if (res === 'You are at least a student') {
            if (ifUserName === 'if200183') {
              //api call für user createn für teacher
              this.rest.AddTeacher(ifUserName, firstname, lastname).subscribe((data: any) => {
                console.log(data);
                this.router.navigate(['/start-screen']);
              });
            } else {
              //api call für user createn für student
              this.rest.AddStudent(ifUserName, firstname, lastname).subscribe((data: any) => {
                console.log(data);
                this.router.navigate(['/student-start-screen']);
              });
            }
          } else {
            //api call für user createn für teacher
            this.rest.AddStudent(ifUserName, firstname, lastname).subscribe((data: any) => {
              console.log(data);
              this.router.navigate(['/start-screen']);
            });
          }
        },
        error: err => {
          this.response.update(() => `Backend says no: ${err.status}`);
          this.router.navigate(['/start-screen']);
        }
      });
  }

  async setUserData(): Promise<void> {
    const user = await this.keycloakService.loadUserProfile();
    sessionStorage.setItem('firstName', user.firstName!);
    sessionStorage.setItem('lastName', user.lastName!);
    sessionStorage.setItem('ifUserName', user.username!);
  }

  async logout(): Promise<void> {
    /*sessionStorage.setItem('ifUserName', '');
    sessionStorage.setItem('firstName', '');
    sessionStorage.setItem('lastName', '');*/
    await this.keycloakService.logout();
    //this.router.navigate(['/login']);
  }
}
