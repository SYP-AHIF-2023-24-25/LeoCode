import { Component, ElementRef, AfterViewInit, ViewChild, OnInit, assertInInjectionContext } from '@angular/core';

import { RestService } from '../service/rest.service';
import { TimeLoggerService } from '../service/time-logger.service';
import { ResultHistoryService } from '../service/result-history.service';
import { Result } from '../model/result';
import { Summary } from '../model/summary';
import { TestResults } from '../model/test-results';
import { ArrayOfSnippetsDto } from '../model/arrayOfSnippetsDto';
import { CodeSection } from '../model/code-sections';
import { Value } from '../model/value';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { User } from '../model/user';
import { DbService } from '../service/db-service.service';
import { ExerciseDto } from '../model/exerciseDto';
import { languages } from 'monaco-editor';
import { MatLabel } from '@angular/material/form-field';
import { Exercise } from '../model/exercise';
import { Assignment } from '../model/assignment';
import { forEach } from 'jszip';

@Component({
  selector: 'app-student-start-screen',
  templateUrl: './student-start-screen.component.html',
  styleUrls: ['./student-start-screen.component.css']
})
export class StudentStartScreenComponent implements OnInit {
 
  userName: string | null= "";
  exerciseName: string |null = "";
  creator: string | undefined = "";

  resultsAvailable: boolean = false;
  showResults: boolean = true; 
  showIntroduction: boolean = true;
  showPlayground: boolean = true;
  showSummary : boolean = true;
  showDetailedResults : boolean = true;
  showHistory : boolean = true;
 
 
     exercise : Exercise={
       name: "",
       creator: "",
       description: "",
       language: "",
       tags: [],
       zipFile: null,
       emptyZipFile: null,
       arrayOfSnippets:[],
       dateCreated: new Date(),
       dateUpdated: new Date(),
       teacher: undefined
     };

     assignments: Assignment[] = [];

     
 resultHistory:{message: string,timestamp:Date, passed:number, notPassed:number, total:number, timer:string}[] = [];



 
 
   //Code Editor
   editorOptions = { theme: 'vs-dark', language: this.exercise.language}; // language auswählen
 
   // timer
   timer: string = "";
   loading: boolean = false;
 
   // result with new json format
   result: Result = {
     Summary: {
       TotalTests: 0,
       PassedTests: 0,
       FailedTests: 0
     },
     TestResults: []
   };
 
   constructor(private rest: RestService, private resultHistoryService: ResultHistoryService, private route: ActivatedRoute, private restDb: DbService, private router: Router) {
   }
 
   ifUserName: string | null = '';
   async logout(): Promise<void> {
     sessionStorage.setItem('shouldLogOut', 'true');
     this.router.navigate(['/login']);
   }
 
   ngOnInit(): void {
     this.ifUserName = sessionStorage.getItem('ifUserName');
     this.resultHistory = this.resultHistoryService.getResultsHistory();

     this.route.queryParams.subscribe((params: Params) => {
       this.userName = params['userName'];
       this.exerciseName = params['exerciseName'];
       this.creator = params['creator'];
 
 
       if(this.userName != null && this.exerciseName != null){
         sessionStorage.setItem("userName", this.userName);
         sessionStorage.setItem("exerciseName", this.exerciseName);
       }else{
         this.userName = sessionStorage.getItem('userName');
         this.exerciseName = sessionStorage.getItem('exerciseName');
       }
     });
     this.restDb.getAssignmentsByUsername(this.ifUserName).subscribe((response: any) => {
      const data = response["$values"]; 
      if (Array.isArray(data)) {
        this.assignments = []; 
        data.forEach((assignment) => {
          this.assignments.push(assignment);
        });
    
        console.log(this.assignments);
      } else {
        console.error('Expected data to be an array, but received:', data);
      }
    }, (error) => {
      console.error('Error fetching assignments:', error);
    });
   }
   


   // parse from json new
   convertFromJsonV2(value: Value): Result {// mit neuen json format
    
    console.log("Value:"+value);
     const TotalTests: number = value.value.Summary.TotalTests;
     const PassedTests: number = value.value.Summary.PassedTests;
     const FailedTests: number = value.value.Summary.FailedTests;
 
     console.log(PassedTests);
 
     const summary: Summary = {
         TotalTests,
         PassedTests,
         FailedTests
     };
 
     const testResults: TestResults[] = value.value.TestResults.map((result: any) => {
         return {
             TestName: result.TestName,
             Outcome: result.Outcome,
             ErrorMessage: result.ErrorMessage
         };
     });
 
     const result: Result = {
         Summary: summary,
         TestResults: testResults
     };
 
     return result;
 }

 clearHistory() {
  this.resultHistoryService.clearResultsHistory();
  this.resultHistory = [];
  }

 
 
     //reset fields for new json format
     resetFields() {
       this.result = {
         Summary: {
           TotalTests: 0,
           PassedTests: 0,
           FailedTests: 0
         },
         TestResults: []
       };
       this.timer = "";
     }
 
   // start tests for new json format
   startTest() {
     this.resetFields();
     this.loading = true;
     this.resultsAvailable = false;
     const timeLogger = new TimeLoggerService();
     timeLogger.start();
 
 
     //this.rest.runTests('PasswordChecker', this.codeSections, "Typescript").subscribe(
     console.log(this.exercise.language);
       this.rest.runTests(this.exercise.name, this.exercise.arrayOfSnippets, this.exercise.language).subscribe(
         (data) => {
           console.log(data);
           
             const d = data
             console.log(d);
             this.result = this.convertFromJsonV2(d as Value);
             
             this.timer = timeLogger.stop();
             console.log(this.result.Summary.TotalTests);
             const logEntry = {
                 message: `Unitests : ${this.result.Summary.PassedTests}/${this.result.Summary.TotalTests} test completed.`,
                 timestamp: new Date(),
                 passed: this.result.Summary.PassedTests,
                 notPassed: this.result.Summary.FailedTests,
                 total: this.result.Summary.TotalTests,
                 timer: this.timer
             };
 
             this.resultHistoryService.addResult(logEntry.message, logEntry.passed, logEntry.notPassed, logEntry.total, logEntry.timer);
             this.loading = false;
             this.resultsAvailable = true;
         },
         (error) => {
             console.error("Error in API request", error);
             this.timer = timeLogger.stop();
             this.loading = false;
            
         }
     );
     let subject = "";
     if(this.exercise.tags.includes("WMC")){
       subject = "WMC";
     }else if(this.exercise.tags.includes("POSE")){
       subject = "POSE";
     }else if(this.exercise.tags.includes("TYPESCRIPT")){
       subject = "TYPESCRIPT";
     }
     let arrayOfSnippets: ArrayOfSnippetsDto = {
       snippets: this.exercise.arrayOfSnippets
     }

     console.log(this.exercise.name);
     console.log(this.exercise.tags.length);

     if(this.userName != null){
      this.restDb.UpdateExercise(this.userName, this.exercise.description, this.exercise.language, this.exercise.tags, this.exercise.name, arrayOfSnippets, subject).subscribe();
     }
}

   loadAssignment(assignment: any): void {
    // Extract exercise details from the assignment
    const exercise = assignment.exercise;
    

    // Check if exercise exists and extract its properties
    if (exercise) {
        this.exercise.name = exercise.name; 
        this.exercise.description = exercise.description; 
        if(exercise.language = 1){
          this.exercise.language = "CSharp"
        } else if(exercise.language = 0){
          this.exercise.language = "Java";
        }else{
          this.exercise.language= "TypeScript";
        }
                
        // Ensure arrayOfSnippets is correctly populated
        if (exercise.arrayOfSnippets && exercise.arrayOfSnippets.snippets && exercise.arrayOfSnippets.snippets.$values) {
            this.exercise.arrayOfSnippets = exercise.arrayOfSnippets.snippets.$values; 
            console.log(this.exercise.arrayOfSnippets);
        } else {
            console.warn('exercise.arrayOfSnippets does not contain the expected format:', exercise.arrayOfSnippets);
            this.exercise.arrayOfSnippets = []; 
        }

        this.exercise.tags= exercise.tags.$values;
        this.exercise.teacher = exercise.teacher;
        console.log(exercise.tags);
        this.editorOptions.language = this.exercise.language; 
        sessionStorage.setItem("exerciseName",this.exercise.name);

        console.log('Loaded assignment with exercise:', this.exercise);
    } else {
        console.error('No exercise found in assignment:', assignment);
    }
  }

  toggleResults() {
    this.showResults = !this.showResults;
  }

  toggleIntroduction() {
    this.showIntroduction = !this.showIntroduction;  // Umschalten zwischen true und false
  }

  togglePlayground() {
    this.showPlayground = !this.showPlayground;
  }
   // Toggle the visibility of the summary
   toggleSummary() {
    this.showSummary = !this.showSummary;
  }

  // Toggle the visibility of the detailed test results
  toggleDetailedResults() {
    this.showDetailedResults = !this.showDetailedResults;
  }

  // Toggle the visibility of the result history
  toggleHistory() {
    this.showHistory = !this.showHistory;
  }

 }
