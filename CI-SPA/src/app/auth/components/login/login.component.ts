import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { AlertService } from 'ngx-alerts';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private progService: ProgressBarService, private alertService: AlertService) { }

  ngOnInit() {
    this.progService.currentColor = this.progService.defaultColor;
  }

  onSubmit(f: NgForm) {
    this.progService.currentColor = this.progService.defaultColor;
    this.alertService.info("Logging in...");
    this.progService.startLoading();

    const loginObserver = {
      next: x => {
        this.progService.setSuccess();
        this.alertService.success("Successfully logged in.");
        this.progService.completeLoading();
      },
      error: err => {
        this.progService.setError();
        this.alertService.danger("[Failure]: Invalid username or password");
        this.progService.completeLoading();
      }
    };
    this.authService.login(f.value).subscribe(loginObserver);
  }
}
