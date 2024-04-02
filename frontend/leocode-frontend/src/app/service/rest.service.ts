import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CodeSection } from '../model/code-sections';
import { languages } from 'monaco-editor';

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

    return this.httpClient.post(`${this.baseUrl}api/runTests?exerciseName=${programName}&language=${language}`, requestBody, { headers: headers });
  }

  startRunner(language: string):Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*' // Erlaubt alle Ursprünge ()
    });

    console.log("Start Runner");
    return this.httpClient.post(`${this.baseUrl}api/startRunner?language=${language}`, null, { headers: headers });
  }

  uploadZipFile(file: File, language: string, token: string) {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('language', language);

    const headers = new HttpHeaders().set('X-XSRF-TOKEN', token); // Set the anti-forgery token

    return this.httpClient.post<any>(`${this.baseUrl}api/testTemplate`, formData, { headers });
  }
  
}
