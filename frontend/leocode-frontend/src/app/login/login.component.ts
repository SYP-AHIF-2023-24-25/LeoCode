import { Component, OnInit } from '@angular/core';
import { KeycloakService } from 'keycloak-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit  {
  //isLoggedIn = false;
  constructor(private keycloakService: KeycloakService, private router: Router) {
    //this.checkLoginStatus();
  }

  async ngOnInit(): Promise<void> {
    if (sessionStorage.getItem('shouldLogOut') === 'true') {
      sessionStorage.setItem('shouldLogOut', 'false');
      await this.keycloakService.logout();
    } else {
      const isLoggedIn = await this.keycloakService.isLoggedIn();
      if (isLoggedIn) {
        await this.setUserData();
        this.router.navigate(['/api-demo']);
      } else {
        await this.keycloakService.login();
        this.router.navigate(['/api-demo']);
      }
    }
  }

  /*async checkLoginStatus(): Promise<void> {
    this.isLoggedIn = await this.keycloakService.isLoggedIn();
    if (this.isLoggedIn) {
      await this.setUserData();
      this.router.navigate(['/api-demo']);
    } else {
      await this.login();
    }
  }*/


  /*async login(): Promise<void> {
    if (!this.isLoggedIn) {
      await this.keycloakService.login();
      await this.setUserData();
      this.router.navigate(['/api-demo']);
    }
  }*/

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
