import { Component, OnInit } from '@angular/core';
import { AlertService } from 'ngx-alerts';
import { NgProgress, NgProgressRef } from 'ngx-progressbar';
import { ProgressBarService } from '../../services/progress-bar.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private progress: NgProgress, public progService: ProgressBarService, private alertService: AlertService) { }

  ngOnInit() {
    this.progService.progressBar = this.progress.ref("progressBar")
  }

}
