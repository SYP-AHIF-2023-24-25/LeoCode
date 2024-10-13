import { Component, computed, inject, Signal, signal, WritableSignal } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { finalize } from "rxjs";
import { Router } from '@angular/router';


@Component({
  selector: 'app-api-demo',
  templateUrl: './api-demo.component.html',
  styleUrls: ['./api-demo.component.css']
})
export class ApiDemoComponent {
  private readonly httpClient: HttpClient = inject(HttpClient);
  public readonly response: WritableSignal<string | null> = signal(null);
  public readonly loading: WritableSignal<boolean> = signal(false);
  public readonly showResponse: Signal<boolean> = computed(() => this.response() !== null);
  constructor(private router: Router) {
    this.performCall('at-least-student');
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
}
