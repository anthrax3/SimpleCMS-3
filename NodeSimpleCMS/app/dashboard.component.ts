import { Component } from '@angular/core';
import { RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS } from '@angular/router-deprecated';

@Component({
  selector: 'simple-cms',
  templateUrl: 'app/dashboard.component.html',
  providers: [
    ROUTER_PROVIDERS
  ]
})
export class DashboardComponent {
  title = 'Dashbaord';
}