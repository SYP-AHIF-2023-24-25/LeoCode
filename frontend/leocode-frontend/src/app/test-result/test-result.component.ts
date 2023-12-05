import { Component } from '@angular/core';
import { Result } from '../model/result';
import { OriginalResult } from '../model/original-result';
import { RestService } from '../service/rest.service';
import { TimeLoggerService } from '../service/time-logger.service';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})
export class TestResultComponent {

  timer: String = "";

  result: Result = {
    message: "Test results",
    passed: 0,
    notPassed: 0,
    total: 0
  };


  constructor(private rest: RestService) { 
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




  startTest() {
    const timeLogger = new TimeLoggerService(); 
    timeLogger.start();

    
    this.rest.getResults().subscribe(
      (data) => {
        this.result = this.convertFromJson(data as OriginalResult);
        this.timer = timeLogger.stop();
      },
      (error) => {
        console.error("Error in API request", error);
        this.timer = timeLogger.stop();
      }
    );
  }

}
