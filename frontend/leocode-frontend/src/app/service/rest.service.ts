import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CodeSection } from '../model/code-sections';

@Injectable({
  providedIn: 'root'
})
export class RestService {
  stopRunner(language: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*' // Erlaubt alle Ursprünge ()
    });
    if (language === 'Typescript') {
      return this.httpClient.delete(`${this.baseUrl}api/stopRunner?language=${language}`, { headers: headers });
    } else if (language === 'CSharp') {
      return this.httpClient.delete(`${this.baseUrl}api/stopRunner?language=${language}`, { headers: headers });
    } else if (language === 'Java') {
      return this.httpClient.delete(`${this.baseUrl}api/stopRunner?language=${language}`, { headers: headers });
    } else {
      return new Observable<any>(); // Return an empty observable
    }
  }

  private baseUrl: string = 'http://localhost:5080/';

  constructor(private httpClient: HttpClient) { }

  runTests(programName: string, code:CodeSection[], language: string):Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*' // Erlaubt alle Ursprünge ()
    });

    const requestBody = {
      "ArrayOfSnippets": code
    }

    if (language === 'Typescript') {
      return this.httpClient.post(`${this.baseUrl}api/runTsTests?exerciseName=${programName}`, requestBody, { headers: headers });
    } else if (language === 'CSharp') {
      return this.httpClient.post(`${this.baseUrl}api/runCSharpTests?exerciseName=${programName}`, requestBody, { headers: headers });
    } else if (language === 'Java') {
      return this.httpClient.post(`${this.baseUrl}api/runJavaTests?exerciseName=${programName}`, requestBody, { headers: headers });
    } else {
      return new Observable<any>(); // Return an empty observable
    }
  }

  startRunner(language: string):Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*' // Erlaubt alle Ursprünge ()
    });

    console.log("Start Runner");

    if (language === 'Typescript') {
      console.log("Jaa");
      return this.httpClient.post(`${this.baseUrl}api/startTsRunner`, {}, { headers: headers });
    } else if (language === 'CSharp') {
      return this.httpClient.post(`${this.baseUrl}api/startCSharpRunner`, null, { headers: headers });
    } else if (language === 'Java') {
      return this.httpClient.post(`${this.baseUrl}api/startJavaRunner`, null, { headers: headers });
    } else {
      return new Observable<any>(); // Return an empty observable
    }
    
  }
}
