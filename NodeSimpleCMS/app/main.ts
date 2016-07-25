// Imports for loading & configuring the in-memory web api
import { provide }    from '@angular/core';
import { AppComponent } from './app.component.ts';
import { APP_ROUTER_PROVIDERS }  from './app.routes.ts';
import { HTTP_PROVIDERS } from '@angular/http';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
// import 'rxjs/Rx'; this will load all features
import { enableProdMode } from '@angular/core';
import { Title } from '@angular/platform-browser';

// Add these symbols to override the `LocationStrategy`
import {
    LocationStrategy,
    HashLocationStrategy
} from '@angular/common';

// The usual bootstrapping imports
import { bootstrap }      from '@angular/platform-browser-dynamic';

bootstrap(AppComponent, [
    HTTP_PROVIDERS,
    APP_ROUTER_PROVIDERS,
    { provide: LocationStrategy, useClass: HashLocationStrategy } // .../#/post
]).catch(err => console.error(err));

