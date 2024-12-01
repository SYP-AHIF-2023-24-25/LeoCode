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
import { languages } from 'monaco-editor';
import { MatLabel } from '@angular/material/form-field';
import { Exercise } from '../model/exercise';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})

export class TestResultComponent  implements OnInit{

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

 resultHistory:{message: string,timestamp:Date, passed:number, notPassed:number, total:number, timer:string}[] = [];

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

    mergedCodeSections: CodeSection[] = [];


  //Code Editor
  editorOptions = { 
   
    theme: 'vs-dark', 
    language: this.exercise.language.toLowerCase(), 
    automaticLayout: true,  
    lineNumbers: 'on',
    minimap: { enabled: false }, 
    wordWrap: 'on' ,
    readonly: false
  }; 
  readonlyEditorOptions = {
    theme: 'vs-dark', 
    language: this.exercise.language.toLowerCase(), 
    automaticLayout: true,  
    lineNumbers: 'on',
    minimap: { enabled: false }, 
    wordWrap: 'on' ,
    readonly: true
  }; 

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

     if(this.userName != null && this.exerciseName != null){
      this.restDb.getExerciseByUsername(this.creator, this.exerciseName).subscribe((data: Exercise[]) => {
        this.exercise = data[0];
        console.log("TAGS")
        console.log(this.exercise.tags.length);
        console.log(this.exercise.creator);
        console.log(this.exercise.dateCreated);
        console.log(this.exercise.language);
        this.editorOptions.language = this.exercise.language.toLowerCase();
        this.readonlyEditorOptions.language = this.exercise.language.toLowerCase();
        console.log(this.editorOptions.language);

        this.mergeCodeSections();
    });
    }

  }
  

  clearHistory() {
    this.resultHistoryService.clearResultsHistory();
    this.resultHistory = [];
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


    // merge the new code into th exercise.arrayOfSnippets
    this.exercise.arrayOfSnippets.forEach((section) => {
      if(section.readonlySection == false){
        this.mergedCodeSections.forEach((section2) => {
          if(section2.readonlySection == false){
            section.code = section2.code;
          }
        });
      } 
    });
    
    console.log(this.exercise.language);
    console.log(this.exercise.arrayOfSnippets);
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
    let usrname = sessionStorage.getItem('ifUserName');
    console.log(this.exercise.name);
    console.log(this.exercise.tags.length);
    if (usrname) {
      //this.restDb.UpdateExercise(usrname, this.exercise.description, this.exercise.language, this.exercise.tags, this.exercise.name, arrayOfSnippets, subject).subscribe();
    } else {
      console.error("Username is null");
    }
  }

  mergeCodeSections(): void {
    setTimeout(() => {  // Verarbeitung im Hintergrund, um den UI-Thread nicht zu blockieren
      let mergedSectionStringBeforEditSection = "";
      let editSection = "";
      let foundEditableSection = false;
      let mergedCodeSectionsAfterEditSection = "";
  
      this.exercise.arrayOfSnippets.forEach((section) => {
        if (section.readonlySection && !foundEditableSection) {
          mergedSectionStringBeforEditSection += section.code;
        } else if (!section.readonlySection && !foundEditableSection) {
          foundEditableSection = true;
          editSection = section.code;
        } else {
          mergedCodeSectionsAfterEditSection += section.code;
        }
      });
  
      this.mergedCodeSections = [
        {
          code: mergedSectionStringBeforEditSection,
          readonlySection: true,
          fileName: "Before Editable Section"
        },
        {
          code: editSection,
          readonlySection: false,
          fileName: "Editable Section"
        },
        {
          code: mergedCodeSectionsAfterEditSection,
          readonlySection: true,
          fileName: "After Editable Section"
        }
      ];
    }, 0); // asynchron verarbeiten
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