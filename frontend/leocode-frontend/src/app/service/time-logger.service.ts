import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TimeLoggerService {
  private startTime: number | null = null;

  start() {
    this.startTime = performance.now();
  }

  stop(): String {
    if (this.startTime !== null) {
      const endTime = performance.now();
      const elapsedMilliseconds = endTime - this.startTime;
      const elapsedSeconds = elapsedMilliseconds / 1000;

      // Format the seconds with a comma as the decimal separator
      const formattedSeconds = elapsedSeconds.toLocaleString('en-US', { minimumFractionDigits: 2 });
      this.startTime = null;
      return `${formattedSeconds}`;
      
    } else {
      return "Die Zeitmessung wurde nicht gestartet."
    }
  }
}