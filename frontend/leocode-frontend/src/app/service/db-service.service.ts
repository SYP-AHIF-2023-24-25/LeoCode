import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise } from '../model/exercise';
import { HttpHeaders,HttpParams } from '@angular/common/http';
import { CodeSection } from '../model/code-sections';
import { ExerciseDto } from '../model/exerciseDto';
import { ArrayOfSnippetsDto } from '../model/arrayOfSnippetsDto';
import { Observable } from 'rxjs/internal/Observable';
import { Assignment } from '../model/assignment';

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

  GetAssignmentsByUsernameForTeacher(username: string) {
    return this.http.get(`${this.apiUrl}/Assignments?username=${username}`, httpOptions);
  }

  AddUser(username: string, firstname: string, lastname: string, isTeacher: boolean) {
    return this.http.post(`${this.apiUrl}/User?username=${username}&firstname=${firstname}&lastname=${lastname}&isTeacher=${isTeacher}`, httpOptions);
  }

  AddAssignment(exerciseName: string, creator: string, dateDue: Date, Name: string): Observable<any> {
    const formattedDate = dateDue.toISOString();
    return this.http.post(`${this.apiUrl}/Assignments?exerciseName=${exerciseName}&creator=${creator}&dateDue=${formattedDate}&Name=${Name}`, httpOptions, { responseType: 'text' as 'json' });
  }



  joinAssignment(assignmentId: number, studentName: string): Observable<any> {
    console.log("Joining assignment with ID " + assignmentId + " as " + studentName);
    return this.http.post(`${this.apiUrl}/Assignments/JoinAssignment`, { assignmentId, ifStudentName: studentName });
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

  UpdateExercise(
    username: string,
    introduction: string,
    language: string,
    tags: string[],
    exerciseName: string,
    arrayOfSnippets: ArrayOfSnippetsDto,
    subject: string
  ) {
    const queryParams = `username=${username}&description=${introduction}&language=${language}&subject=${subject}&exerciseName=${exerciseName}`;
    console.log(tags);
    const body = {
      tags: tags,
      arrayOfSnippets: arrayOfSnippets
    };
  
    return this.http.put<Exercise>(
      `${this.apiUrl}/exercise?${queryParams}`,
      body,
      httpOptions
    );
  }
  

  UpdateDetails(username: string, description: string, tags: string[], exerciseName: string, newExerciseName: string) {

    // Logge wichtige Informationen zur Überprüfung
    console.log("Tags length: " + tags.length);
    console.log("First Tag: " + tags[0]);
    console.log(tags);

    // HTTP PUT Request an den passenden API-Endpunkt
    return this.http.put(`${this.apiUrl}/exercise/UpdateDetails?username=${username}&description=${description}&exerciseName=${exerciseName}&newExerciseName=${newExerciseName}`, tags, httpOptions);
  }

  getAssignmentsByUsername(username: string|null) {
    return this.http.get<Assignment[]>(`${this.apiUrl}/Assignments/GetAssignmentsByUsername?username=${username}`);
  }


}
