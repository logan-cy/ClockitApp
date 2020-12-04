import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { AlertService } from 'ngx-alerts';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private authService: AuthService, private progService: ProgressBarService, private alertService: AlertService) { }

  ngOnInit() {
    this.progService.currentColor = this.progService.defaultColor;
  }

  onSubmit(f: NgForm) {
    this.progService.currentColor = this.progService.defaultColor;
    this.alertService.info("Working...");

    this.progService.startLoading();
    const registerObserver = {
      next: x => {
        this.progService.setSuccess();
        this.alertService.success("Your account was successfully registered; please check your email for confirmation.");
        this.progService.completeLoading();
      },
      error: err => {
        this.progService.setError();
        console.log(err);
        const msg = err.error.errors[0].description
        this.alertService.danger(`[Failure]: ${msg}`);
        this.progService.completeLoading();
      }
    };
    this.authService.register(f.value).subscribe(registerObserver);
  }
}
