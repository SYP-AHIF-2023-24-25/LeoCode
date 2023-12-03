import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RestService {

  baseUrl: string = 'http://localhost:5080';

  constructor(private httpClient: HttpClient) { }

  getResults() {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*' // Erlaubt alle Ursprünge (*)
    });

    return this.httpClient.get(`${this.baseUrl}/runtests`, { headers: headers });
  }
}
