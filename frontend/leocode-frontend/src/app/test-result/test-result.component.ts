import { Component, ElementRef,AfterViewInit, ViewChild } from '@angular/core';
import { Result } from '../model/result';
import { OriginalResult } from '../model/original-result';
import { RestService } from '../service/rest.service';
import { TimeLoggerService } from '../service/time-logger.service';
import { ResultHistoryService } from '../service/result-history.service';

import * as monaco from 'monaco-editor';


@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})


export class TestResultComponent implements AfterViewInit{
  
  @ViewChild('editorContainer') editorContainer!: ElementRef;

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
    this.initMonacoEditor();
  }

 /* private initMonacoEditor() {
    const container = this.editorContainer.nativeElement;

    if (container) {
      monaco.editor.create(container, {
        value: 'console.log("Hello, Monaco Editor!");',
        language: 'javascript',
      });
    }
  }*/

  
 /* private initMonacoEditor() {
    const container = this.editorContainer.nativeElement;
  
    if (container) {
      // Using Monaco's AMD loader directly
      monaco.loader.require(['vs/editor/editor.main'], () => {
        const editor = monaco.editor.create(container, {
          value: 'console.log("Hello, Monaco Editor!");',
          language: 'javascript',
        });
      });
    }
  }*/
  
 private initMonacoEditor() {
    const container = this.editorContainer.nativeElement;
  
    if (container) {
      // Ensure that the Monaco loader is loaded
      if ((window as any).monaco) {
        this.createEditor(container);
      } else {
        const loaderScript = document.createElement('script');
        loaderScript.type = 'text/javascript';
        loaderScript.src = 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.20.0/min/vs/loader.js';
        //loaderScript.src = '../../../node_modules/monaco-editor/min/vs/loader.js';
        loaderScript.onload = () => this.createEditor(container);
        document.head.appendChild(loaderScript);
      }
    }
  }
  
  private createEditor(container: HTMLElement) {
    (window as any).require.config({ paths: { 'vs': 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.20.0/min/vs' } });
    (window as any).require(['vs/editor/editor.main'], () => {
      const editor = (window as any).monaco.editor.create(container, {
        value: 'console.log("Hello, Monaco Editor!");',
        language: 'typescript',
        theme: 'vs-dark',
        //readOnly: true,
        automaticLayout: true
      });
    });
  }

  



  convertFromJson(originalResult: OriginalResult): Result {
    const passed : number = originalResult.stats.passes;
    const notPassed : number = originalResult.stats.failures;
    const total : number = originalResult.stats.tests;

    const result: Result = {
        message: "Test results",
        passed,
        notPassed,
        total
    };

    return result;
  }

  resetFields(){
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
    
        this.resultHistoryService.addResult(logEntry.message,logEntry.passed, logEntry.notPassed, logEntry.total, logEntry.timer);    
        this.loading = false;  
      },
      (error) => {
        console.error("Error in API request", error);
        this.timer = timeLogger.stop();
        this.loading  = false;
      }
    );
  }

  
}
