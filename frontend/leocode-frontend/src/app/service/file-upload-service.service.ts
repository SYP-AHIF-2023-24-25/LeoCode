import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  constructor(private http: HttpClient) { }

  uploadFile(file: File, content: string) {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('content', content);
    return this.http.post('http://localhost:8000/uploadFullTemplate', formData);
  }

  async uploadCSharpTemplate(file: File, content: string):Promise<Observable<any>> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('content', content);
  
    const headers = new HttpHeaders();
    headers.set('Content-Type', 'multipart/form-data');

    return await this.http.post('http://localhost:8001/CSharpExercises/UploadTemplate', formData, { headers });

  }
}
