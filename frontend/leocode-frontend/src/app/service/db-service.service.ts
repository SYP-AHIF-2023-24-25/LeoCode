import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Exercise } from '../model/exercise';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { CodeSection } from '../model/code-sections';
import { ExerciseDto } from '../model/exerciseDto';
import { ArrayOfSnippetsDto } from '../model/arrayOfSnippetsDto';
import { Observable } from 'rxjs/internal/Observable';
import { Assignment } from '../model/assignment';
import { environment } from '../../environments/environment'; // Import environment

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
  }),
};

@Injectable({
  providedIn: 'root',
})
export class DbService {
  private apiUrl = environment.dbUrl; // Use environment variable for base URL

  constructor(private http: HttpClient) {}

  GetExerciseForStudentAssignment(
    language: any,
    exerciseName: any,
    student: string | null
  ) {
    return this.http.get<ExerciseDto>(
      `${this.apiUrl}/exercise/GetExerciseForStudentAssignment?language=${language}&exerciseName=${exerciseName}&student=${student}`,
      httpOptions
    );
  }

  GetAssignmentsByUsernameForTeacher(username: string) {
    return this.http.get(
      `${this.apiUrl}/Assignments?username=${username}`,
      httpOptions
    );
  }

  AddStudent(username: string, firstname: string, lastname: string) {
    return this.http.post(
      `${this.apiUrl}/Student?username=${username}&firstname=${firstname}&lastname=${lastname}`,
      httpOptions
    );
  }

  AddTeacher(username: string, firstname: string, lastname: string) {
    return this.http.post(
      `${this.apiUrl}/Teacher?username=${username}&firstname=${firstname}&lastname=${lastname}`,
      httpOptions
    );
  }

  AddAssignment(
    exerciseName: string,
    creator: string,
    dateDue: Date,
    Name: string
  ): Observable<any> {
    const formattedDate = dateDue.toISOString();
    return this.http.post(
      `${this.apiUrl}/Assignments?exerciseName=${exerciseName}&creator=${creator}&dateDue=${formattedDate}&Name=${Name}`,
      httpOptions,
      { responseType: 'text' as 'json' }
    );
  }

  joinAssignment(assignmentId: number, studentName: string): Observable<any> {
    console.log(
      'Joining assignment with ID ' + assignmentId + ' as ' + studentName
    );
    return this.http.post(`${this.apiUrl}/Assignments/JoinAssignment`, {
      assignmentId,
      ifStudentName: studentName,
    });
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

  AddExercise(
    arrayOfSnippets: ArrayOfSnippetsDto,
    exerciseName: string,
    introduction: string,
    language: string,
    tags: string[],
    username: string,
    dateCreated: Date,
    dateUpdated: Date
  ) {
    let exercise = {
      exerciseName: exerciseName,
      introduction: introduction,
      language: language,
      tags: tags,
      username: username,
      dateCreated: dateCreated.toISOString(),
      dateUpdated: dateUpdated.toISOString(),
    };

    return this.http.post<Exercise>(
      `${this.apiUrl}/exercise?name=${exercise.exerciseName}&description=${exercise.introduction}&language=${exercise.language}&tags=${exercise.tags}&username=${exercise.username}&dateCreated=${exercise.dateCreated}&dateUpdated=${exercise.dateUpdated}`,
      arrayOfSnippets,
      httpOptions
    );
  }

  UpdateExercise(
    username: string,
    teacher: string,
    introduction: string,
    language: string,
    tags: string[],
    exerciseName: string,
    arrayOfSnippets: ArrayOfSnippetsDto,
    subject: string,
    dateCreated: Date,
    dateUpdated: Date,
    total: number,
    passed: number,
    failed: number
  ) {
    const queryParams = `student=${username}&teacher=${teacher}&description=${encodeURIComponent(
      introduction
    )}&language=${language}&subject=${subject}&exerciseName=${exerciseName}&dateCreated=${dateCreated.toISOString()}&dateUpdated=${dateUpdated.toISOString()}&tags=${tags}&total=${total}&passed=${passed}&failed=${failed}`;

    return this.http.put<Exercise>(
      `${this.apiUrl}/exercise?${queryParams}`,
      arrayOfSnippets,
      httpOptions
    );
  }

  UpdateDetails(
    username: string,
    description: string,
    tags: string[],
    exerciseName: string,
    newExerciseName: string
  ) {
    return this.http.put(
      `${this.apiUrl}/exercise/UpdateDetails?username=${username}&description=${description}&exerciseName=${exerciseName}&newExerciseName=${newExerciseName}`,
      tags,
      httpOptions
    );
  }

  getAssignmentsByUsername(username: string | null) {
    return this.http.get<Assignment[]>(
      `${this.apiUrl}/Assignments/GetAssignmentsByUsername?username=${username}`
    );
  }
}
