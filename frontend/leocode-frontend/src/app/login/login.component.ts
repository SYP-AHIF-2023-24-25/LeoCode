import { Component, ViewChild  } from '@angular/core';
import {KeycloakService} from "keycloak-angular";
import { Router } from '@angular/router';
import { ApiDemoComponent } from '../api-demo/api-demo.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  isLoggedIn = false;
  constructor(private keycloakService: KeycloakService, private router: Router) {
    this.checkLoginStatus();
  }

  async checkLoginStatus(): Promise<void> {
    this.isLoggedIn = await this.keycloakService.isLoggedIn();
    if (this.isLoggedIn) {
      await this.setUserData();
      this.router.navigate(['/api-demo']);
    } else {
      await this.login();
    }
  }


  async login(): Promise<void> {
    if (!this.isLoggedIn) {
      await this.keycloakService.login();
      await this.setUserData();
      this.router.navigate(['/api-demo']);
    }
  }

  async setUserData(): Promise<void> {
    const user = await this.keycloakService.loadUserProfile();
    sessionStorage.setItem('firstName', user.firstName!);
    sessionStorage.setItem('userName', user.username!);
  }

  async logout(): Promise<void> {
    if (this.isLoggedIn) {
      await this.keycloakService.logout();
      this.router.navigate(['/login']);
    }
  }
}