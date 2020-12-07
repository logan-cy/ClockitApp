import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EmployerService {

  employersUrl = "http://localhost:5000/api/employers/";

  constructor(private http: HttpClient) { }

  update(model: any) {
    return this.http.put(this.employersUrl, model);
  }
}
