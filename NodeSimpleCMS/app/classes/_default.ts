import { ErrorMessages } from './error.messages';
import { AjaxDefaults } from './ajax.defaults';

var $; // "mock" jquery for compiler 

export class _default {

    public ajaxDefaults: AjaxDefaults;

    private _apiKey: string;

    constructor() {
        this._apiKey = "devtestkey";
        this.ajaxDefaults = new AjaxDefaults();
    }

    // doamin for ajax requests 
    // @return string
    public AJAX_URL(): string {
        return "http://localhost";
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
        if (password.trim() === "" || password.trim().length < 5) {
            $(id).addError(ErrorMessages.passwordLength);
            return false;
        }
        return true;
    }

    // clears all error messages on a page. If a form id is
    // passed the form fields will also be cleared
    // @param formid = id of form (optional)
    // @return void
    public clearErrors(formID: string): void {
        $(".input-error").removeClass("input-error");
        $(".error-message").remove();
        if (formID != undefined) {
            var form = document.getElementById(formID);
            //form.reset();
        }
    }

    // clears input errors and values when a bootstrap modal 
    // that has a form closes. 
    // @param modal = id of modal
    // @return void
    public clearModalErrors(modal: string): void {
        $(modal).on("hidden.bs.modal", function () {
            var id = $(this).find("form").attr("id");
            this.clearErrors(id);
        });
    }

    // Clears input errors on change
    // @param field = id of input
    // @return void
    public updateInputField(field: string): void {
        $(field).change(function () {
            if (this.value != "") {
                $(this).removeClass("input-error");
                $("." + field.substring(1) + ".error-message").remove();
            }
        });
    }

    // submits an ajax get request to this.AJAX_URL + settings.url 
    // then calls settings.sucsess callback function 
    // @params settings = object for ajax, async = bool
    // @return any (if success function returns data any will be that object)
    public ajaxGet(includeKey: boolean = true): any {
        this.ajaxDefaults.type = "GET";
        if (this.ajaxDefaults.url != null && this.ajaxDefaults.url.length > 0) {
            this.ajaxDefaults.url = this.AJAX_URL() + this.ajaxDefaults.url;
        }
        if (includeKey) {
            let dataObject = JSON.parse(this.ajaxDefaults.data);
            if (dataObject == null) {
                dataObject = {
                    "apiKey": this._apiKey
                };
            } else {
                dataObject["apiKey"] = this._apiKey;
            }
            this.ajaxDefaults.data = JSON.stringify(dataObject); 
        }
        $.ajax(this.ajaxDefaults);
    }

    // submits an ajax post request to this.AJAX_URL + settings.url 
    // then calls settings.sucsess callback function 
    // @params settings = object for ajax, async = bool
    // @return any (if success function returns data any will be that object)
    public ajaxPost(includeKey: boolean = true): any {
        this.ajaxDefaults.type = "POST";
        if (this.ajaxDefaults.url != null && this.ajaxDefaults.url.length > 0) {
            this.ajaxDefaults.url = this.AJAX_URL() + this.ajaxDefaults.url;
        }

        if (includeKey) {
            let dataObject = JSON.parse(this.ajaxDefaults.data);
            if (dataObject == null) {
                dataObject = {
                    "apiKey": this._apiKey
                };
            } else {
                dataObject["apiKey"] = this._apiKey;
            }
            this.ajaxDefaults.data = JSON.stringify(dataObject);
        }

        return $.ajax(this.ajaxDefaults);
    }

    // adds a dismissable alert to an element
    // @params type = name of what is being updated, action = update or delete, msgId = id of element to update
    public alertMsg(type: string, action: string, msgID: string): void {
        if (type == undefined) return;
        const deleteMsg = `<div class="alert alert-success alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Success!</strong> ${type} has been deleted.</div>`;
        const updateMsg = `<div class="alert alert-success alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Success!</strong> ${type} has been updated.</div>`;
        if (action === "delete") $(msgID).html(deleteMsg);
        if (action === "update") $(msgID).html(updateMsg);
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