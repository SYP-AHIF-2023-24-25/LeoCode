import { KeycloakService } from 'keycloak-angular';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, computed, inject, Signal, signal, WritableSignal, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { finalize } from "rxjs";
import { DbService } from '../service/db-service.service';

@Component({
  selector: 'app-join-assignment-component',
  templateUrl: './join-assignment-component.component.html',
  styleUrls: ['./join-assignment-component.component.css']
})
export class JoinAssignmentComponentComponent implements OnInit {
  private readonly httpClient: HttpClient = inject(HttpClient);
  private readonly route: ActivatedRoute = inject(ActivatedRoute); // Inject ActivatedRoute
  public readonly response: WritableSignal<string | null> = signal(null);
  public readonly loading: WritableSignal<boolean> = signal(false);
  public readonly showResponse: Signal<boolean> = computed(() => this.response() !== null);
  assignmentId: number | null = null; // Variable to store the assignment ID

  constructor(private keycloakService: KeycloakService, private router: Router, private rest: DbService) { }

  async ngOnInit(): Promise<void> {
    // Get the assignment ID from the route parameters
    this.route.paramMap.subscribe(params => {
      this.assignmentId = +params.get('id')!; // Convert the string parameter to a number
      console.log('Assignment ID:', this.assignmentId); // Log the assignment ID for debugging
    });

    if (sessionStorage.getItem('shouldLogOut') === 'true') {
      sessionStorage.setItem('shouldLogOut', 'false');
      await this.keycloakService.logout();
    } else {
      const isLoggedIn = await this.keycloakService.isLoggedIn();
      if (isLoggedIn) {
        await this.setUserData();
        this.performCall('at-least-student');
      } else {
        await this.keycloakService.login();
      }
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
              // API call to create user for teacher
              this.rest.AddTeacher(ifUserName, firstname, lastname).subscribe((data: any) => {
                console.log(data);
                this.router.navigate(['/start-screen']);
              });
            } else {
              // API call to create user for student
              this.rest.AddStudent(ifUserName, firstname, lastname).subscribe((data: any) => {
                console.log(data);
              });
              // Use the assignmentId here
              console.log('Assignment ID:', this.assignmentId);
              console.log('ifUserName:', ifUserName);
              this.rest.joinAssignment(this.assignmentId!, ifUserName).subscribe((data: any) => {
                console.log(data);
              });
              this.router.navigate(['/student-start-screen']);
            }
          } else {
            // API call to create user for teacher
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
    await this.keycloakService.logout();
  }
}
