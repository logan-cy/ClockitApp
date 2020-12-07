import { Injectable, NgModule } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (localStorage.getItem("token") != null) {
      var token = localStorage.getItem("token");
      const conedRequest = request.clone({
        headers: request.headers.set("Authorization", `Bearer ${token}`)
      });
      return next.handle(conedRequest).pipe(
        tap(
          success => { },
          error => {
            if (error.status === 401) {
              localStorage.removeItem("token");
              this.router.navigateByUrl("/login");
            }
          }
        ));
    }
    else {
      return next.handle(request.clone());
    }
  }
}
