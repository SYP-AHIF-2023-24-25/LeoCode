import { Component } from '@angular/core';
import { Result } from '../model/result';
import { OriginalResult } from '../model/original-result';
import { RestService } from '../service/rest.service';
import { TimeLoggerService } from '../service/time-logger.service';
import { ResultHistoryService } from '../service/result-history.service';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})
export class TestResultComponent {

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
