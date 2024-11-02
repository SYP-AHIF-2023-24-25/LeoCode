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
  
  GetAssignmentsByUsername(username: string) {
    return this.http.get(`${this.apiUrl}/Assignments?username=${username}`, httpOptions);
  }

  AddUser(username: string, firstname: string, lastname: string, isTeacher: boolean) {
    return this.http.post(`${this.apiUrl}/User?username=${username}&firstname=${firstname}&lastname=${lastname}&isTeacher=${isTeacher}`, httpOptions);
  }

  AddAssignment(exerciseName: string, creator: string, dateDue: Date, Name: string) {
    return this.http.post(`${this.apiUrl}/Assignments?exerciseName=${exerciseName}&creator=${creator}&dateDue=${dateDue}&Name=${Name}`, httpOptions);
  }

  getExerciseByUsername(username?: string, exerciseName?: string) {
    let params = new HttpParams();
    if (username) {
      params = new HttpParams().set('username', username);
    }
    if (exerciseName) {
      params = params.set('exerciseName', exerciseName);
    }
    return this.http.get<Exercise[]>(`${this.apiUrl}/Exercise`, { params });
  }

  AddExercise(arrayOfSnippets: ArrayOfSnippetsDto, exerciseName: string, introduction : string, language: string, tags: string[], username: string, dateCreated: Date, dateUpdated: Date) {
    let exercise = {
      exerciseName: exerciseName,
      introduction: introduction,
      language: language,
      tags: tags,
      username: username,
      dateCreated: dateCreated.toISOString(),
      dateUpdated: dateUpdated.toISOString()
    }
    
    return this.http.post<Exercise>(`${this.apiUrl}/exercise?name=${exercise.exerciseName}&description=${exercise.introduction}&language=${exercise.language}&tags=${exercise.tags}&username=${exercise.username}&dateCreated=${exercise.dateCreated}&dateUpdated=${exercise.dateUpdated}`, arrayOfSnippets, httpOptions);
  }

  UpdateExercise( username: string, introduction : string, language: string, tags: string[], exerciseName: string, arrayOfSnippets: ArrayOfSnippetsDto, subject: string) {
    let exercise = {
      exerciseName: exerciseName,
      introduction: introduction,
      language: language,
      tags: tags,
      username: username,
    }
    console.log("TAGS: " + exercise.tags.length);
    console.log("TAGS: " + exercise.tags[0])
    console.log("TAGS: " + exercise.tags[1])
    return this.http.put<Exercise>(`${this.apiUrl}/exercise?username=${exercise.username}&description=${exercise.introduction}&tags=${exercise.tags}&language=${exercise.language}&subject=${subject}&exerciseName=${exercise.exerciseName}`, arrayOfSnippets, httpOptions);
  }

  UpdateDetails(username: string, description: string, tags: string[], exerciseName: string, newExerciseName: string) {
    // Konvertiere das Array der Tags in einen kommagetrennten String
    const tagsString = tags.join(',');

    // Logge wichtige Informationen zur Überprüfung
    console.log("Tags length: " + tags.length);
    console.log("First Tag: " + tags[0]);

    // HTTP PUT Request an den passenden API-Endpunkt
    return this.http.put(`${this.apiUrl}/exercise/UpdateDetails?username=${username}&description=${description}&tags=${tagsString}&exerciseName=${exerciseName}&newExerciseName=${newExerciseName}`, null, httpOptions);
}


}
