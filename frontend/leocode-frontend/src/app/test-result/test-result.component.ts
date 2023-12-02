import { Component } from '@angular/core';
import { Result } from '../model/result';
import { OriginalResult } from '../model/original-result';

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


  constructor() { 
    const originalJson = {
      "stats": {
        "suites": 1,
        "tests": 3,
        "passes": 3,
        "pending": 0,
        "failures": 0,
        "start": "2023-11-16T08:20:11.947Z",
        "end": "2023-11-16T08:20:11.952Z",
        "duration": 5
      },
      "tests": [
        {
          "title": "should return true for a valid password",
          "fullTitle": "CheckPassword Function should return true for a valid password",
          "file": "/usr/src/project/test/passwordChecker.spec.js",
          "duration": 1,
          "currentRetry": 0,
          "speed": "fast",
          "err": {}
        },
        {
          "title": "should return false for an invalid password (too short)",
          "fullTitle": "CheckPassword Function should return false for an invalid password (too short)",
          "file": "/usr/src/project/test/passwordChecker.spec.js",
          "duration": 0,
          "currentRetry": 0,
          "speed": "fast",
          "err": {}
        },
        {
          "title": "should return false for an invalid password (too long)",
          "fullTitle": "CheckPassword Function should return false for an invalid password (too long)",
          "file": "/usr/src/project/test/passwordChecker.spec.js",
          "duration": 0,
          "currentRetry": 0,
          "speed": "fast",
          "err": {}
        }
      ],
      "pending": [],
      "failures": [],
      "passes": [
        {
          "title": "should return true for a valid password",
          "fullTitle": "CheckPassword Function should return true for a valid password",
          "file": "/usr/src/project/test/passwordChecker.spec.js",
          "duration": 1,
          "currentRetry": 0,
          "speed": "fast",
          "err": {}
        },
        {
          "title": "should return false for an invalid password (too short)",
          "fullTitle": "CheckPassword Function should return false for an invalid password (too short)",
          "file": "/usr/src/project/test/passwordChecker.spec.js",
          "duration": 0,
          "currentRetry": 0,
          "speed": "fast",
          "err": {}
        },
        {
          "title": "should return false for an invalid password (too long)",
          "fullTitle": "CheckPassword Function should return false for an invalid password (too long)",
          "file": "/usr/src/project/test/passwordChecker.spec.js",
          "duration": 0,
          "currentRetry": 0,
          "speed": "fast",
          "err": {}
        }
      ]
    };
    const originalResult: OriginalResult = originalJson as OriginalResult;
    this.result = this.convertFromJson(originalResult);
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
    // TODO : Reques befehl zu server/backend schicken und messen wie lange es dauert bis die antwort kommt
    this.timer = 0;
    const intervalId = setInterval(() => {
      this.timer++;
    }, 1000);

    // TODO : Reques befehl zu server/schicken und das json file welches man zurück bekommt convertieren und in result speichern

    clearInterval(intervalId);

  }
}
