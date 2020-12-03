import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private progService: ProgressBarService) { }

  ngOnInit() {
    this.progService.currentColor = this.progService.defaultColor;
  }

  onSubmit(f: NgForm) {
    this.progService.startLoading();

    const loginObserver = {
      next: x => {
        console.log("User logged in");
        this.progService.completeLoading();
        this.progService.setSuccess();
      },
      error: err => {
        this.progService.setError();
        console.error(err);
        this.progService.completeLoading();
      }
    };
    this.authService.login(f.value).subscribe(loginObserver);
  }
}
