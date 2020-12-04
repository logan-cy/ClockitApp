import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { AlertService } from 'ngx-alerts';
import { error } from 'console';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {


  constructor(private authService: AuthService, private progService: ProgressBarService, private alertService: AlertService) { }

  ngOnInit() {
    this.progService.currentColor = this.progService.defaultColor;
  }

  onSubmit(f: NgForm) {
    this.progService.currentColor = this.progService.defaultColor;
    this.alertService.info("Working...");
    this.progService.startLoading();
    const resetPasswordObserver = {
      next: x => {
        this.progService.setSuccess();
        this.alertService.success("Please check your email to change your password.");
        this.progService.completeLoading();
      },
      error: err => {
        this.progService.setError();
        let msg;
        if (err.status === 500) {
          msg = "No user was found with the supplied email address.";
        }
        if (err.status === 401) {
          msg = "You don't have permission to perform this action.";
        }
        this.alertService.danger(`[Failure] ${msg}`);
        this.progService.completeLoading();
      }
    };
    this.authService.resetPassword(f.value).subscribe(resetPasswordObserver);
  }
}
