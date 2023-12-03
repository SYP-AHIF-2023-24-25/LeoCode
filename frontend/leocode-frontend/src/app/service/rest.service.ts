import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RestService {

  baseUrl: string = 'http://localhost:7169';

  constructor(private httpClient: HttpClient) { }

  getResults() {
    return this.httpClient.get(`${this.baseUrl}/runtests`);
  }
}
