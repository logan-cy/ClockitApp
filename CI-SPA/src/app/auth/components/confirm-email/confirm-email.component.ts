import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProgressBarService } from 'src/app/shared/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {

  emailConfirmed: boolean = false;
  urlParams: any = {};

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService, private progService: ProgressBarService) { }

  ngOnInit() {
    this.progService.startLoading();
    this.urlParams.token = this.route.snapshot.queryParamMap.get("token");
    this.urlParams.userId = this.route.snapshot.queryParamMap.get("userid");
    this.confirmEmail();
  }

  confirmEmail() {
    this.authService.confirmEmail(this.urlParams).subscribe(() => {
      this.progService.setSuccess();
      console.log("email confirmed successfully");
      this.emailConfirmed = true;
      this.progService.completeLoading();
    }, error => {
      this.progService.setError();
      console.error(error);
      this.emailConfirmed = false;
      this.progService.completeLoading();
    });
  }

}
