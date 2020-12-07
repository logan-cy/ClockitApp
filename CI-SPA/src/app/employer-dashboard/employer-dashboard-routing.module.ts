import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthguardService } from '../auth/authguard.service';
import { EditComponent } from './components/edit/edit.component';

const routes: Routes = [
  { path: "employer/edit", component: EditComponent, canActivate: [AuthguardService] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployerDashboardRoutingModule { }
