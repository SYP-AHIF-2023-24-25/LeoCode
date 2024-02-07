import { Stats } from './stats';
import { Test } from './test';
export interface OriginalResult {
    stats: Stats;
    tests: Test[];
    pending: any[];
    failures: any[];
    passes: Test[];
}

/*
{
  "Summary": {
    "TotalTests": 2,
    "PassedTests": 1,
    "FailedTests": 1
  },
  "TestResults": [
    {
      "TestName": "T01_checkPassword_ok",
      "Outcome": "Passed",
      "ErrorMessage": null
    },
    {
      "TestName": "T02_checkPassword_invalid",
      "Outcome": "Failed",
      "ErrorMessage": null
    }
  ]
}*/
