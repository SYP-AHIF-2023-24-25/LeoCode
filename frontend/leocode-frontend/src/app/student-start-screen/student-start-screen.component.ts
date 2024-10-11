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
  username: string | null = '';

  constructor(private keycloakService: KeycloakService, private router: Router) {}

  ngOnInit(): void {
    this.firstName = sessionStorage.getItem('firstName');
    this.username = sessionStorage.getItem('username');
  }

  async logout(): Promise<void> {
    await this.keycloakService.logout();
    this.router.navigate(['/login']);
  }
}
