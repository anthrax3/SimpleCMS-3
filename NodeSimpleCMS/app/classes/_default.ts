import { ErrorMessages }  from "./error.messages";
import { HttpDefaults }   from "./http.defaults";
import { Http,
         Response,
         Headers,
         RequestOptions } from "@angular/http";
import { Injectable }     from "@angular/core";
import { Observable }     from "rxjs/Observable";

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';


@Injectable()
export class _default {

    public httpDefaults: HttpDefaults;

    private _apiKey: string;

    private _http: Http; 

    private _apiUrl: string;

    constructor(http: Http) {
        this._apiKey = "devtestkey";
        this._apiUrl = "http://localhost:59980";
        this.httpDefaults = new HttpDefaults();
        this._http = http;
    }

    // @param email = email to validate
    // @return bool
    public validateEmail(email: string): boolean {
        const regex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return regex.test(email);
    }

    // @param password = string to validate id = id of input field
    // @return bool (adds error message if false)
    public validatePasswordLength(password: string, id: number): boolean {
        if (password === "" || password.length < 5) {
            //$(id).addError(ErrorMessages.passwordLength);
            return false;
        }
        return true;
    }
   
    // submits an ajax get request to this.AJAX_URL + settings.url 
    // then calls settings.sucsess callback function 
    // @params settings = object for ajax, async = bool
    // @return any (if success function returns data any will be that object)
    public ajaxGet<T>(includeKey: boolean = true): Promise<T> {
        if (this.httpDefaults.url != null && this.httpDefaults.url.length > 0) {
            this.httpDefaults.url = this._apiUrl + this.httpDefaults.url;
        }
        if (includeKey) {
            let dataObject = JSON.parse(this.httpDefaults.data);
            if (dataObject == null) {
                dataObject = {
                    "apiKey": this._apiKey
                };
            } else {
                dataObject["apiKey"] = this._apiKey;
            }
            this.httpDefaults.data = JSON.stringify(dataObject); 
        }

        let headers = new Headers();
        let options = new RequestOptions({ headers: headers });
        headers.append("Content-Type", "application/json");
        return this._http.get(this.httpDefaults.url, options)
                            .toPromise()
                            .then(this.extractData)
                            .catch(this.handleError);
    }

    // submits an ajax post request to this.AJAX_URL + settings.url 
    // then calls settings.sucsess callback function 
    // @params settings = object for ajax, async = bool
    // @return any (if success function returns data any will be that object)
    public ajaxPost<T>(includeKey: boolean = true): Promise<T> {
        if (this.httpDefaults.url != null && this.httpDefaults.url.length > 0) {
            this.httpDefaults.url = this._apiUrl + this.httpDefaults.url;
        }

        if (includeKey) {
            let dataObject = JSON.parse(this.httpDefaults.data);
            if (dataObject == null) {
                dataObject = {
                    "apiKey": this._apiKey
                };
            } else {
                dataObject["apiKey"] = this._apiKey;
            }
            this.httpDefaults.data = JSON.stringify(dataObject);
        }

        let headers = new Headers();
        let options = new RequestOptions({ headers : headers}); 
        headers.append("Content-Type", "application/json");
        return this._http.post(this.httpDefaults.url, this.httpDefaults.data, options)
                            .toPromise()
                            .then(this.extractData)
                            .catch(this.handleError);
    }


    private extractData(res: Response) {
        let body = res.json();
        let response = body.data || {};

        // extract response data depending on http status code 
        // errors / messages logged to console
        if (response.length > 0 && response.httpStatusCode === 200) {
            if (response.data != null) {
                response = response.data;
            } else {
                console.log(response.message);
                response = {}
            }
        }
        if (response.length > 0 && response.httpStatusCode > 200) {
            console.log(response.errors);
            response = {};
        }

        return response;
    }

    private handleError(error: any) {
        // In a real world app, we might use a remote logging infrastructure
        // We'd also dig deeper into the error to get a better message
        let errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg); // log to console instead
        return Promise.reject(errMsg);
    }
}

// jquery functions
// $(target).addError
// @params errorMsg = message to add, field = name of field (used for targeting with LB$.updateInputField)
/*$(function() {
    $.fn.addError = function(errorMsg, field, altId) {
        if (field == undefined) field = "";
        var targetId = this;
        if (altId != undefined) {
            targetId = altId;
        }
        $(targetId).find('.error-message').remove();
        $(targetId).after("<div class=\"" + field + " error-message text-danger\">" + errorMsg + "</div>");
        $(this).addClass("input-error");
    };
});*/