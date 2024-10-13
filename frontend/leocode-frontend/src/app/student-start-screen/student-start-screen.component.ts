import { Component, OnInit } from '@angular/core';
import { KeycloakService } from 'keycloak-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-student-start-screen',
  templateUrl: './student-start-screen.component.html',
  styleUrls: ['./student-start-screen.component.css']
})
export class StudentStartScreenComponent implements OnInit {
  firstName: string | null = '';
  ifUserName: string | null = '';

  constructor(private keycloakService: KeycloakService, private router: Router) {}

  ngOnInit(): void {
    this.firstName = sessionStorage.getItem('firstName');
    this.ifUserName = sessionStorage.getItem('ifUserName');
  }

  async logout(): Promise<void> {
    sessionStorage.setItem('shouldLogOut', 'true');
    this.router.navigate(['/login']);
  }
}
