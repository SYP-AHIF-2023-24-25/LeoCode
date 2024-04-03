import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

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
}
