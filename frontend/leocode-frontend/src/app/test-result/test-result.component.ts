import { Component, ElementRef, AfterViewInit, ViewChild, OnInit } from '@angular/core';

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

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})

export class TestResultComponent  implements OnInit{

 userName: string | null= "";
 exerciseName: string |null = "";
 creator: string | undefined = "";

  exercise : ExerciseDto = {
    name: "",
    creator: "",
    description: "",
    language: "",
    tags: [],
    arrayOfSnippets: [],
    dateCreated: new Date(),
    dateUpdated: new Date()
  }

  //Code Editor
 // testTemplate: string = 'export function CheckPassword(password: string): boolean{\n  Todo Implementation \n}'
  editorOptions = { theme: 'vs-dark', language: 'typescrip'}; // language auswählen
 // codeSections: CodeSection[] = [];


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

     if(this.userName != null && this.exerciseName != null){
      this.restDb.getExerciseByUsername(this.creator, this.exerciseName).subscribe((data: ExerciseDto[]) => {
        this.exercise = data[0];
        console.log("TAGS")
        console.log(this.exercise.tags.length);
        console.log(this.exercise.creator);
        console.log(this.exercise.dateCreated);
    });
    }
  }
  
  // parse from json new
  convertFromJsonV2(value: Value): Result {// mit neuen json format
   
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
    let usrname = "Default";
    console.log(this.exercise.name);
    console.log(this.exercise.tags.length);
    this.restDb.UpdateExercise(usrname, this.exercise.description, this.exercise.language, this.exercise.tags, this.exercise.name, arrayOfSnippets, subject).subscribe();
  }
}