import { Component } from '@angular/core';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})
export class TestResultComponent {
  result = {
    message: 'Test-Projekt',
    passed: 3,
    notPassed: 0,
    total: 3
  };


startTest() {}
}
