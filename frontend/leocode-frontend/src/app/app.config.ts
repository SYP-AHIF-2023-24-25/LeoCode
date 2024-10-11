import { APP_INITIALIZER, ApplicationConfig, Provider } from '@angular/core';
import { provideRouter } from '@angular/router';
import { KeycloakBearerInterceptor, KeycloakService } from 'keycloak-angular';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app-routing.module';  // Importiere deine Routen

// Funktion zur Initialisierung von Keycloak
function initializeKeycloak(keycloak: KeycloakService) {
  return () => keycloak.init({
    config: {
      url: 'https://auth.htl-leonding.ac.at/',
      realm: 'htlleonding',
      clientId: 'htlleonding-service',
    },
    initOptions: {
      onLoad: 'check-sso',
      pkceMethod: 'S256',
      flow: 'implicit',
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
    },
    enableBearerInterceptor: true,
    bearerPrefix: 'Bearer',
  });
}

// Provider für Keycloak Bearer Interceptor
const KeycloakBearerInterceptorProvider: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: KeycloakBearerInterceptor,
  multi: true
};

// Provider für die Keycloak-Initialisierung
const KeycloakInitializerProvider: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initializeKeycloak,
  multi: true,
  deps: [KeycloakService]
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    KeycloakInitializerProvider,
    KeycloakBearerInterceptorProvider,
    KeycloakService,
    provideRouter(routes),
    provideAnimations()
  ]
};