import { Injectable } from '@angular/core';
import { NgProgressRef } from 'ngx-progressbar';

@Injectable({
  providedIn: 'root'
})
export class ProgressBarService {

  progressBar: NgProgressRef;
  defaultColor: string = "#375a7f";
  successColor: string = "#00bc8c";
  errorColor: string = "#e74c3c";
  currentColor: string = this.defaultColor;

  constructor() { }

  startLoading() {
    this.progressBar.start();
  }

  increment(n: number) {
    this.progressBar.inc(n);
  }

  set(n: number) {
    this.progressBar.set(n);
  }

  completeLoading() {
    this.progressBar.complete();
  }

  setSuccess() {
    this.currentColor = this.successColor;
  }

  setError() {
    this.currentColor = this.errorColor;
  }
}
