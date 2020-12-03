import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { connectableObservableDescriptor } from 'rxjs/internal/observable/ConnectableObservable';
import { ProgressBarService } from 'src/app/shared/progress-bar.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private authService: AuthService, private progService: ProgressBarService) { }

  ngOnInit() {
    this.progService.currentColor = this.progService.defaultColor;
  }

  onSubmit(f: NgForm) {
    this.progService.startLoading();
    const registerObserver = {
      next: x => {
        this.progService.setSuccess();
        console.log("User registered");
        this.progService.completeLoading();
      },
      error: err => {
        this.progService.setError();
        console.error(err);
        this.progService.completeLoading();
      }
    };
    this.authService.register(f.value).subscribe(registerObserver);
  }
}
