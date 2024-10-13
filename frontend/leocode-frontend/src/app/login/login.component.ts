import { KeycloakService } from 'keycloak-angular';
import { Router } from '@angular/router';
import { Component, computed, inject, Signal, signal, WritableSignal, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { finalize } from "rxjs";

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
  constructor(private keycloakService: KeycloakService, private router: Router) {

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
          if (res === 'You are at least a student') {
            if (sessionStorage.getItem('ifUserName') === 'if200183') {
              this.router.navigate(['/start-screen']);
              //this.router.navigate(['/student-start-screen']);
            } else {
              this.router.navigate(['/student-start-screen']);
            }
          } else {
            this.router.navigate(['/start-screen']);
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
    sessionStorage.setItem('ifUserName', user.username!);
  }

  async logout(): Promise<void> {
    await this.keycloakService.logout();
    //this.router.navigate(['/login']);
  }
}
