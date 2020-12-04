import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { AlertService } from 'ngx-alerts';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  model: any = {};

  constructor(private route: ActivatedRoute, private authService: AuthService, private progService: ProgressBarService, private alertService: AlertService) { }

  ngOnInit() {
    this.progService.currentColor = this.progService.defaultColor;
    this.progService.startLoading();
    this.model.token = this.route.snapshot.queryParamMap.get("token");
    this.model.userId = this.route.snapshot.queryParamMap.get("userid");

    this.progService.completeLoading();
  }

  changePassword() {
    this.progService.currentColor = this.progService.defaultColor;
    this.alertService.info("Working...");
    this.progService.startLoading();

    this.authService.changePassword(this.model).subscribe(() => {
      this.progService.setSuccess();
      this.alertService.success("Your password was changed successfully.");
      this.progService.completeLoading();
    }, err => {
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
    });
  }
}
