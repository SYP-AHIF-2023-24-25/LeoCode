import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CodeSection } from '../model/code-sections';

@Injectable({
  providedIn: 'root'
})
export class RestService {

  private baseUrl: string = 'http://localhost:5080/';

  constructor(private httpClient: HttpClient) { }

  runTests(ProgramName: string, code:CodeSection[]):Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*' // Erlaubt alle Ursprünge ()
    });

    const requestBody = {
      "ArrayOfSnippets": code
    }


    return this.httpClient.post(`${this.baseUrl}api/runtest?exerciseName=${ProgramName}`, requestBody, { headers: headers });
  }
}
