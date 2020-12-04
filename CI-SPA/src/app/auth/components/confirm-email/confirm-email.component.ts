import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { AlertService } from 'ngx-alerts';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {

  emailConfirmed: boolean = false;
  urlParams: any = {};

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService, private progService: ProgressBarService, private alertService: AlertService) { }

  ngOnInit() {
    this.progService.currentColor = this.progService.defaultColor;
    this.progService.startLoading();
    this.urlParams.token = this.route.snapshot.queryParamMap.get("token");
    this.urlParams.userId = this.route.snapshot.queryParamMap.get("userid");
    this.confirmEmail();
  }

  confirmEmail() {
    this.progService.currentColor = this.progService.defaultColor;
    this.alertService.info("Working...");

    this.authService.confirmEmail(this.urlParams).subscribe(() => {
      this.progService.setSuccess();
      this.alertService.success("Your email address was successfully confirmed.");
      this.emailConfirmed = true;
      this.progService.completeLoading();
    }, error => {
      this.progService.setError();
      console.log(error.error.errors[0].Description);
      this.alertService.danger("Unable to confirm your email address; please see console.");
      this.emailConfirmed = false;
      this.progService.completeLoading();
    });
  }

}
