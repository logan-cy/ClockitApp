import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  model: any = {};

  constructor(private route: ActivatedRoute, private authService: AuthService) { }

  ngOnInit() {
    this.model.token = this.route.snapshot.queryParamMap.get("token");
    this.model.userId = this.route.snapshot.queryParamMap.get("userid");
  }

  changePassword() {
    console.log(this.model);
    this.authService.changePassword(this.model).subscribe(() => {
      console.log("success - password changed");
    }, error => {
      console.error(error);
    });
  }
}
