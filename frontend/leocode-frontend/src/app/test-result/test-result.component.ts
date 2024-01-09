import { Component, ElementRef, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { Result } from '../model/result';
import { OriginalResult } from '../model/original-result';
import { RestService } from '../service/rest.service';
import { TimeLoggerService } from '../service/time-logger.service';
import { ResultHistoryService } from '../service/result-history.service';

import { CodeSection } from '../model/code-sections';




@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})


export class TestResultComponent  implements OnInit{

  testTemplate: string = 'export function CheckPassword(password: string): boolean{\n  Todo Implementation \n}'


  editorOptions = { theme: 'vs-dark', language: 'typescrip'};
  
  code: string = 'function x() {\nconsole.log("Hello world!");\n}';

  codeSections: CodeSection[] = [];

  timer: string = "";
  loading: boolean = false;

  result: Result = {
    message: "Test results",
    passed: 0,
    notPassed: 0,
    total: 0
  };

  constructor(private rest: RestService, private resultHistoryService: ResultHistoryService) {
  }

  ngOnInit(): void {
    this.parseTemplateToCodeSections(this.testTemplate);
  }

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

  convertFromJson(originalResult: OriginalResult): Result {
    const passed: number = originalResult.stats.passes;
    const notPassed: number = originalResult.stats.failures;
    const total: number = originalResult.stats.tests;

    const result: Result = {
      message: "Test results",
      passed,
      notPassed,
      total
    };

    return result;
  }

  resetFields() {
    this.result = {
      message: "Test results",
      passed: 0,
      notPassed: 0,
      total: 0
    };
    this.timer = "";
  }




  startTest() {
    console.log (this.codeSections);
    this.parseCodeSectionsToTemplate(this.codeSections);
    console.log (this.testTemplate);
    
    this.resetFields();
    this.loading = true;
    const timeLogger = new TimeLoggerService();
    timeLogger.start();



    this.rest.getResults().subscribe(
      (data) => {
        this.result = this.convertFromJson(data as OriginalResult);
        this.timer = timeLogger.stop();
        const logEntry = {
          message: this.result.message,
          timestamp: new Date(),
          passed: this.result.passed,
          notPassed: this.result.notPassed,
          total: this.result.total,
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
