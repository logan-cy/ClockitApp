import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployerDashboardRoutingModule } from './employer-dashboard-routing.module';
import { EditComponent } from './components/edit/edit.component';


@NgModule({
  declarations: [EditComponent],
  imports: [
    CommonModule,
    EmployerDashboardRoutingModule
  ],
  exports: [
    EditComponent
  ]
})
export class EmployerDashboardModule { }
