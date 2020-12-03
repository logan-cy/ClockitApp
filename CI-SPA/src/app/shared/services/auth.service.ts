import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  authUrl = "http://localhost:5000/api/auth/";
  employersUrl = "http://localhost:5000/api/employers/";
  confirmEmailUrl = "http://localhost:4200/confirm-email/";
  changePasswordUrl = "http://localhost:4200/change-password/";

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(`${this.authUrl}login`, model)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user.result.succeeded) {
            localStorage.setItem("token", user.token);
          }
        })
      )
  }

  register(model: any) {
    const headers = new HttpHeaders({
      "confirmEmailUrl": this.confirmEmailUrl
    });
    const options = { headers: headers };
    return this.http.post(`${this.employersUrl}create`, model, options);
  }

  confirmEmail(model: any) {
    return this.http.post(`${this.authUrl}confirmemail`, model);
  }

  resetPassword(model: any) {
    const headers = new HttpHeaders({
      "changePasswordUrl": this.changePasswordUrl
    });
    const options = { headers: headers };
    return this.http.post(`${this.authUrl}resetpassword`, model, options);
  }

  changePassword(model: any) {
    return this.http.post(`${this.authUrl}changepassword`, model);
  }
}
