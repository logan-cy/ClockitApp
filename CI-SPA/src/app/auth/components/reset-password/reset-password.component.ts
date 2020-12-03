import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProgressBarService } from 'src/app/shared/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {


  constructor(private authService: AuthService, private progService: ProgressBarService) { }

  ngOnInit() {
    this.progService.currentColor = this.progService.defaultColor;
  }

  onSubmit(f: NgForm) {
    this.progService.startLoading();
    const resetPasswordObserver = {
      next: x => {
        this.progService.setSuccess();
        console.log("Check email to change password.");
        this.progService.completeLoading();
      },
      error: err => {
        this.progService.setError();
        console.error(err);
        this.progService.completeLoading();
      }
    };
    this.authService.resetPassword(f.value).subscribe(resetPasswordObserver);
  }
}
