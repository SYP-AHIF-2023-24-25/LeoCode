import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise } from '../model/exercise';
import { HttpHeaders,HttpParams } from '@angular/common/http';
import { CodeSection } from '../model/code-sections';
import { ExerciseDto } from '../model/exerciseDto';
import { ArrayOfSnippetsDto } from '../model/arrayOfSnippetsDto';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
    //,'Authorization': 'my-auth-token'
  })
}
@Injectable({
  providedIn: 'root',
})
export class DbService {

  private apiUrl ="https://localhost:7269/api"
  constructor(private http: HttpClient) { }

  getExerciseByUsername(username: string, exerciseName?: string) {
    let params = new HttpParams().set('username', username);
    if (exerciseName) {
      params = params.set('exerciseName', exerciseName);
    }
    return this.http.get<ExerciseDto[]>(`${this.apiUrl}/Exercise`, { params });
  }

  AddExercise(arrayOfSnippets: ArrayOfSnippetsDto, exerciseName: string, introduction : string, language: string, tags: string[], username: string) {
    let exercise = {
      exerciseName: exerciseName,
      introduction: introduction,
      language: language,
      tags: tags,
      username: username
    }
    
    return this.http.post<Exercise>(`${this.apiUrl}/exercise?name=${exercise.exerciseName}&description=${exercise.introduction}&language=${exercise.language}&tags=${exercise.tags}&username=${exercise.username}`, arrayOfSnippets, httpOptions);
  }

  UpdateExercise( username: string, introduction : string, language: string, tags: string[], exerciseName: string, codeSections: CodeSection[]) {
    let exercise = {
      codeSections: codeSections,
      exerciseName: exerciseName,
      introduction: introduction,
      language: language,
      tags: tags,
      username: username
    }
    return this.http.put<Exercise>(`${this.apiUrl}/exercise`, exercise, httpOptions);
  }

}
