import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ResultHistoryService {

  public resultHistory:{message: string,timestamp:Date, passed:number, notPassed:number, total:number, timer:string}[] = [];

  addResult(message: string, passed:number, notPassed:number, total:number, timer:string){
    const timestamp = new Date();
    this.resultHistory.push({message,timestamp,passed,notPassed,total,timer});
  }

  getResultsHistory(){
    return this.resultHistory;
  }

  clearResultsHistory(){
    this.resultHistory = [];
  }

}
