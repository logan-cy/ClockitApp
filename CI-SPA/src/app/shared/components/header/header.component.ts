import { Component, OnInit } from '@angular/core';
import { AlertService } from 'ngx-alerts';
import { NgProgress, NgProgressRef } from 'ngx-progressbar';
import { AuthService } from '../../services/auth.service';
import { ProgressBarService } from '../../services/progress-bar.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  loggedIn: boolean = false;

  constructor(private progress: NgProgress, public progService: ProgressBarService,
    private alertService: AlertService, public authService: AuthService) { }

  ngOnInit() {
    this.progService.progressBar = this.progress.ref("progressBar");
    this.loggedIn = this.authService.loggedIn();
  }

  logOut() {
    localStorage.removeItem("token");
    this.alertService.success("You've been logged out.");
  }

}
