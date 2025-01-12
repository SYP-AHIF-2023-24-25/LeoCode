import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CodeSection } from '../model/code-sections';

@Injectable({
  providedIn: 'root'
})
export class RestService {
  private baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  stopRunner(language: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    });
    return this.httpClient.delete(`${this.baseUrl}/api/stopRunner?language=${language}`, { headers: headers });
  }

  runTests(programName: string, code: CodeSection[], language: string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    });

    const requestBody = {
      ArrayOfSnippets: code
    };

    return this.httpClient.post(
      `${this.baseUrl}/api/runTests?exerciseName=${programName}&language=${language}`,
      requestBody,
      { headers: headers }
    );
  }

  startRunner(language: string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    });

    return this.httpClient.post(`${this.baseUrl}/api/startRunner?language=${language}`, null, { headers: headers });
  }

  uploadZipFile(file: File) {
    console.log("Uploading ZIP file");
    const formData = new FormData();
    formData.append('file', file);

    return this.httpClient.post<any>(`${this.baseUrl}/api/testTemplate`, formData);
  }

  getCode(programName: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    });

    return this.httpClient.get(`${this.baseUrl}/api/code/${programName}/`, { headers: headers });
  }

  getCodeCSharp(programName: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    });

    return this.httpClient.get(`${this.baseUrl}/api/code?exerciseName=${programName}`, { headers: headers });
  }
}
