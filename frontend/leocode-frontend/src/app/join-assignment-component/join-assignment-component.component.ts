import { KeycloakService } from 'keycloak-angular';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, computed, inject, Signal, signal, WritableSignal, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { finalize } from "rxjs";
import { DbService } from '../service/db-service.service';
import { environment } from 'src/environments/environment';
import { createLeoUser, LeoUser } from 'src/core/util/leo-token';

@Component({
  selector: 'app-join-assignment-component',
  templateUrl: './join-assignment-component.component.html',
  styleUrls: ['./join-assignment-component.component.css']
})
export class JoinAssignmentComponentComponent implements OnInit {
  private readonly httpClient: HttpClient = inject(HttpClient);
    public readonly response: WritableSignal<string | null> = signal(null);
    public readonly loading: WritableSignal<boolean> = signal(false);
    public readonly showResponse: Signal<boolean> = computed(() => this.response() !== null);
    assignmentId: number | null = null;
    private readonly route: ActivatedRoute = inject(ActivatedRoute);
    constructor(private keycloakService: KeycloakService, private router: Router, private rest: DbService) {
  
    }
  
    async ngOnInit(): Promise<void> {

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
        } else {
          await this.keycloakService.login();
        }
        const user: LeoUser = await createLeoUser(this.keycloakService);
        if (user.username === 'if200183' || user.username === undefined || user.username === '' || !user.username?.startsWith('if')) {
          this.rest.AddTeacher(user.username!, user.firstName!, user.lastName!).subscribe((data: any) => {
                  console.log(data);
                  this.router.navigate(['/start-screen']);
                });
        } else {
          this.rest.AddStudent(user.username!, user.firstName!, user.lastName!).subscribe((data: any) => {
                  console.log(data);
          });
          this.rest.joinAssignment(this.assignmentId!, user.username!).subscribe((data: any) => {
            console.log(data);
          });
          this.sleep(5000);
          this.router.navigate(['/student-start-screen']);
        }
      }
    }

    sleep(ms: number): Promise<void> {
      return new Promise(resolve => setTimeout(resolve, ms));
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
