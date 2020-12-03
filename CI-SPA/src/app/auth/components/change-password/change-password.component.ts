import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  model: any = {};

  constructor(private route: ActivatedRoute, private authService: AuthService, private progService: ProgressBarService) { }

  ngOnInit() {
    this.progService.startLoading();
    this.model.token = this.route.snapshot.queryParamMap.get("token");
    this.model.userId = this.route.snapshot.queryParamMap.get("userid");

    this.progService.completeLoading();
  }

  changePassword() {
    this.progService.startLoading();

    this.authService.changePassword(this.model).subscribe(() => {
      this.progService.setSuccess();
      console.log("success - password changed");
      this.progService.completeLoading();
    }, error => {
      this.progService.setError();
      console.error(error);
      this.progService.completeLoading();
    });
  }
}
