import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RestService {

  private baseUrl: string = 'http://localhost:5080/';

  constructor(private httpClient: HttpClient) { }

  runTests(language: string, ProgramName: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*' // Erlaubt alle Ursprünge ()
    });

    const body = { language, ProgramName };

    return this.httpClient.post(`${this.baseUrl}/runtestssecondbackend`, body, { headers: headers });
  }
}
