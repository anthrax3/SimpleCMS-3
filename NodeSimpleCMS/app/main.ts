// Imports for loading & configuring the in-memory web api
import { provide }    from '@angular/core';

import { ROUTER_PROVIDERS } from '@angular/router-deprecated';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
// import 'rxjs/Rx'; this will load all features
import { enableProdMode } from '@angular/core';
import { Title } from '@angular/platform-browser';

// The usual bootstrapping imports
import { bootstrap }      from '@angular/platform-browser-dynamic';
import { HTTP_PROVIDERS } from '@angular/http';

import { AppComponent }   from './app.component';

bootstrap(AppComponent, [
    HTTP_PROVIDERS
]);
