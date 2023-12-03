import { Component } from '@angular/core';
import { Result } from '../model/result';
import { OriginalResult } from '../model/original-result';
import { RestService } from '../service/rest.service';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})
export class TestResultComponent {

  timer: number = 0;

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
    // TODO : Request befehl zu server/backend schicken und messen wie lange es dauert bis die antwort kommt
    //TODO: Fix timer
    this.timer = 0;
    const intervalId = setInterval(() => {
      this.timer++;
    }, 1000);

    // TODO : Request befehl zu server/schicken und das json file welches man zurück bekommt convertieren und in result speichern
    this.rest.getResults().subscribe((data) => {
      this.result = this.convertFromJson(data as OriginalResult);
    });
    
    clearInterval(intervalId);

  }
}
