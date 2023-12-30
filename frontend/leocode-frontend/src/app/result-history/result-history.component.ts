import { Component } from '@angular/core';
import { ResultHistoryService } from '../service/result-history.service';

@Component({
  selector: 'app-result-history',
  templateUrl: './result-history.component.html',
  styleUrls: ['./result-history.component.css']
})
export class ResultHistoryComponent {

    resultHistory:{message: string,timestamp:Date, passed:number, notPassed:number, total:number, timer:string}[] = [];
  
    constructor(private resultHistoryService: ResultHistoryService ) { }

    ngOnInit(): void {
      this.resultHistory = this.resultHistoryService.getResultsHistory();
    }
    clearHistory() {
      this.resultHistoryService.clearResultsHistory();
      this.resultHistory = [];
    }
}
