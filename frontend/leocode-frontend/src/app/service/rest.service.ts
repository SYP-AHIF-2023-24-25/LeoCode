import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RestService {

  private baseUrl: string = 'http://localhost:5080/';

  constructor(private httpClient: HttpClient) { }

  runTests(language: string, ProgramName: string,):Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*' // Erlaubt alle Ursprünge ()
    });


    return this.httpClient.post(`${this.baseUrl}runtestssecondbackend?language=${language}&ProgramName=${ProgramName}`, null, { headers: headers });
  }
}
