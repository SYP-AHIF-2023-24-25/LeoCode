import { Component } from '@angular/core';
import { ResultHistoryService } from '../service/result-history.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-result-history',
  templateUrl: './result-history.component.html',
  styleUrls: ['./result-history.component.css']
})
export class ResultHistoryComponent {

    resultHistory:{message: string,timestamp:Date, passed:number, notPassed:number, total:number, timer:string}[] = [];
  
    constructor(private resultHistoryService: ResultHistoryService, private router: Router) { }

    ifUserName: string | null = '';
    async logout(): Promise<void> {
      sessionStorage.setItem('shouldLogOut', 'true');
      this.router.navigate(['/login']);
    }
    ngOnInit(): void {
      this.ifUserName = sessionStorage.getItem('ifUserName');

      this.resultHistory = this.resultHistoryService.getResultsHistory();
    }
    clearHistory() {
      this.resultHistoryService.clearResultsHistory();
      this.resultHistory = [];
    }
}
