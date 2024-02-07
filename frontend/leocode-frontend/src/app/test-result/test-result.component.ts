import { Component, ElementRef, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { Result } from '../model/result';
import { OriginalResult } from '../model/original-result';
import { RestService } from '../service/rest.service';
import { TimeLoggerService } from '../service/time-logger.service';
import { ResultHistoryService } from '../service/result-history.service';
import { ResultV2 } from '../model/result-v2';
import { Summary } from '../model/summary';
import { TestResults } from '../model/test-results';

import { CodeSection } from '../model/code-sections';
import { Value } from '../model/value';




@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})


export class TestResultComponent  implements OnInit{

  //Code Editor
  testTemplate: string = 'export function CheckPassword(password: string): boolean{\n  Todo Implementation \n}'
  editorOptions = { theme: 'vs-dark', language: 'typescrip'}; // language auswählen
  codeSections: CodeSection[] = [];

  // timer
  timer: string = "";
  loading: boolean = false;

  // result with old json format
  result: Result = {
    message: "Test results",
    passed: 0,
    notPassed: 0,
    total: 0
  };
  // result with new json format
  resultV2: ResultV2 = {
    Summary: {
      TotalTests: 0,
      PassedTests: 0,
      FailedTests: 0
    },
    TestResults: []
  };

  constructor(private rest: RestService, private resultHistoryService: ResultHistoryService) {
  }

  ngOnInit(): void {
    this.parseTemplateToCodeSections(this.testTemplate);
  }

// Code Editor Funcions
  parseTemplateToCodeSections(template: string) {

    let stringCodeSections: string[]= template.split("\n");

    let codeSections: CodeSection[] = [];


    for(let i = 0; i < stringCodeSections.length; i++) {
      if(stringCodeSections[i].includes("Todo Implementation")) {
        codeSections.push({ code: stringCodeSections[i], readonly: false });
      }else {
        codeSections.push({ code: stringCodeSections[i], readonly: true });
      }
    }

    this.codeSections = codeSections;
  }
  
  parseCodeSectionsToTemplate(codeSections: CodeSection[]) {
      let template: string = "";
      for(let i = 0; i < codeSections.length; i++) {
        template += codeSections[i].code + "\n";
      }
      this.testTemplate = template;
    }

  // parse from json new
  convertFromJsonV2(value: Value): ResultV2 {// mit neuen json format
    const TotalTests: number = value.results.Summary.TotalTests;
    const PassedTests: number = value.results.Summary.PassedTests;
    const FailedTests: number = value.results.Summary.FailedTests;

    const summary: Summary = {
        TotalTests,
        PassedTests,
        FailedTests
    };
    
    /*const testResultsV2: TestResults[] = new Array<TestResults>();
   for (let i = 0; i < value.results.TestResults.length; i++) {
        const TestName: string = value.results.TestResults[i].TestName;
        const Outcome: string = value.results.TestResults[i].Outcome;
        const ErrorMessage: string = value.results.TestResults[i].ErrorMessage;

        testResultsV2.push({
            TestName,
            Outcome,
            ErrorMessage
        });
    }*/

    const testResults: TestResults[] = value.results.TestResults.map((result: any) => {
        return {
            TestName: result.TestName,
            Outcome: result.Outcome,
            ErrorMessage: result.ErrorMessage
        };
    });

    const result: ResultV2 = {
        Summary: summary,
        TestResults: testResults
    };

    return result;
}


    //reset fields for new json format
    resetFieldsV2() {
      this.resultV2 = {
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
  startTestV2() {
    console.log(this.codeSections);
    this.parseCodeSectionsToTemplate(this.codeSections);
    console.log(this.testTemplate);

    this.resetFieldsV2();
    this.loading = true;
    const timeLogger = new TimeLoggerService();
    timeLogger.start();

    this.rest.runTests('Typescript', 'PasswordChecker').subscribe(
        (data) => {
            const d = data.data.value.data;
            this.resultV2 = this.convertFromJsonV2(d as Value);
            
            this.timer = timeLogger.stop();
            const logEntry = {
                message: `Unitests : ${this.resultV2.Summary.PassedTests}/${this.resultV2.Summary.TotalTests} test completed.`,
                timestamp: new Date(),
                passed: this.resultV2.Summary.PassedTests,
                notPassed: this.resultV2.Summary.FailedTests,
                total: this.resultV2.Summary.TotalTests,
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
}

}