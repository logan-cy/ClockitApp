import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ProgressBarService } from 'src/app/shared/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private progService: ProgressBarService) { }

  ngOnInit() {
  }

  onSubmit(f: NgForm) {
    this.progService.startLoading();

    const loginObserver = {
      next: x => {
        this.progService.setSuccess();
        console.log("User logged in");
        this.progService.completeLoading();
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
