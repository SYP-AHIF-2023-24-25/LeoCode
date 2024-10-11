import { Component } from '@angular/core';
import {KeycloakService} from "keycloak-angular";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  isLoggedIn = false;

  constructor(private keycloakService: KeycloakService) {
    this.keycloakService.isLoggedIn().then(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
    });
    this.keycloakService.getToken().then(token => {
      console.log(token);
    });
  }

  async login(): Promise<void> {
    if (this.isLoggedIn) {
      return
    }
    await this.keycloakService.login();

  }

  async logout(): Promise<void> {
    if (!this.isLoggedIn) {
      return;
    }
    await this.keycloakService.logout();
  }
}