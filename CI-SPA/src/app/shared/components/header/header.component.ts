import { Component, OnInit } from '@angular/core';
import { NgProgress, NgProgressRef } from 'ngx-progressbar';
import { ProgressBarService } from '../../progress-bar.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private progress: NgProgress, private progService: ProgressBarService) { }

  ngOnInit() {
    this.progService.progressBar = this.progress.ref("progressBar")
  }

}
