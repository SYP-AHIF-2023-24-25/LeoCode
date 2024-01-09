import { Component, ElementRef, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { Result } from '../model/result';
import { OriginalResult } from '../model/original-result';
import { RestService } from '../service/rest.service';
import { TimeLoggerService } from '../service/time-logger.service';
import { ResultHistoryService } from '../service/result-history.service';

import * as monaco from 'monaco-editor';
import { CodeSection } from './code-sections';




@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})


export class TestResultComponent implements AfterViewInit {
  editorOptions = { theme: 'vs-dark', language: 'javascript' };
  code: string = 'function x() {\nconsole.log("Hello world!");\n}';

  @ViewChild('editorContainer') editorContainer!: ElementRef;
  editor!: monaco.editor.IStandaloneCodeEditor;

  codeSections: CodeSection[] = [
    { code: "console.log();", readonly: true },
    { code: "", readonly: false },
    { code: "console.log();", readonly: true }
  ];


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

  ngAfterViewInit() {
    //this.initMonacoEditor();
  }
  /*
    private initMonacoEditor() {

      if (this.editorContainer instanceof HTMLElement) {
        this.editor = monaco.editor.create(this.editorContainer, {
          value: 'console.log("Hello, Monaco Editor!");',
          language: 'typescript',
          theme: 'vs-dark',
          automaticLayout: true
        });
      } else {
        console.error('Editor container not found or not an HTMLElement.');
      }
    }
  */



  getTemplateAsString() {

  }

  getEditorCode() {
    if (this.editor) {
      const code = this.editor.getValue();
      console.log(code);
      //console.log(passwordChecker);
      return code;
    } else {
      console.error('Editor not initialized.');
      return '';
    }
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
