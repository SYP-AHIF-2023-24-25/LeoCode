import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  private uploadUrl = environment.uploadUrl; // Upload service base URL
  private cSharpUrl = environment.cSharpUrl; // C# service base URL

  constructor(private http: HttpClient) { }

  uploadFile(file: File, content: string) {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('content', content);
    return this.http.post(`${this.uploadUrl}uploadFullTemplate`, formData);
  }

  async uploadCSharpTemplate(file: File, content: string) {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('content', content);

    const headers = new HttpHeaders();
    headers.set('Content-Type', 'multipart/form-data');

    return await this.http.post(`${this.cSharpUrl}CSharpExercises/UploadTemplate`, formData, { headers });
  }
}
