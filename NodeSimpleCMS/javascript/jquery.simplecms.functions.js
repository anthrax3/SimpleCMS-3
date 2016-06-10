"use strict";

// Namespace SimpleCMS for custom jquery functions 
// contains helpful extension functions to be called like
// $(selector).SimpleCMS('funcitonName', [ 'param', 'anotherParam' ]); 
// depending on the funciton there may not be any parameters. 
$.fn.SimpleCMS = function (callbackName, params) {
    const NAMESPACE = "SimpleCMS";
    // logger object to handle errors 
    var logger = {};
    logger.missingCallback = function () {
        console.log("%cError: parameter callbackName cannot be empty for $()." + NAMESPACE + "(callbackName)", "color:red");
        return;
    };
    logger.callbackNotFound = function (callbackName) {
        console.log("%cError: callback function " + callbackName + " not found in $()." + NAMESPACE + "(callbackName)", "color:red");
        return;
    };
    logger.invalidParams = function (callbackName, numParams) {
        var msg = "Error: " + callbackName + " function requires at least " + numParams + " parameter";
        if (numParams > 1)
            msg += "s";
        console.log("%c" + msg, "color:red");
        return;
    };
    logger.jqueryNotFound = function () {
        console.log("%cError: jquery parameter in $(this).fn not found", "color:red");
        return;
    };
    // validate parameters 
    var isValid = true;
    if (callbackName === undefined) {
        logger.missingCallback();
        isValid = false;
    }
    if ($(this).length === 0) {
        logger.jqueryNotFound();
        isValid = false;
    }
    // return early if base parameters invalid
    if (!isValid) {
        return;
    }
    // initialize params if empty (some functions may not have parameters)
    if (params === undefined)
        params = [];
    // add "this" scope to params so callback functions can manipulate 
    // "this" of parent function 
    params.push($(this));
    // callbacks object for callback functions
    var callbacks = {};
    // @params[0] = array of objects { display : "" : value: ""} for Select List 
    // @params[1] = string of "display" or "value" to sort on (optional)
    // @params[2] = string type to sort on (string for display. string, float, or 
    //              integer for values) (optional)
    // @return status object of DOM manipulatin 
    callbacks.setListOptions = function (params) {
        if (params.length < 2) {
            return { msg : "invalid params", numParams: 1 };
        }
        // handle overloading the function 
        var arrayList = params[0];
        var jquery = params[1];
        if (params.length > 2) {
            var sortOn = params[1];
            jquery = params[2];
        }
        if (params.length > 3) {
            var sortType = params[2];
            jquery = params[3];
        }
        // validate params 
        if (sortOn === undefined)
            sortOn = false;
        if (sortType === undefined)
            sortType = false;
        // sort arrayList according to display / value or sort type  
        if (typeof (sortOn) === "string" && sortOn != false) {
            if (sortOn === "display" && sortType === "string") {
                arrayList.sort(function (a, b) {
                    // set two items to compare case insensitive 
                    var nameA = a.display.toUpperCase();
                    var nameB = b.display.toUpperCase();
                    if (nameA < nameB) {
                        return -1;
                    }
                    if (nameA > nameB) {
                        return 1;
                    }
                    // names must be equal
                    return 0;
                });
            } else if (sortOn === "values") { // sort by values
                switch (sortType) {
                    case "float":
                        arrayList.sort(function (a, b) {
                            // set two items to compare
                            var nameA = parseFloat(a.value);
                            var nameB = parseFloat(b.value);
                            if (isNaN(nameA) || isNaN(nameB)) {
                                return 0;
                            }
                            if (nameA < nameB) {
                                return -1;
                            }
                            if (nameA > nameB) {
                                return 1;
                            }
                            // names must be equal
                            return 0;
                        });
                        break;
                    case "integer":
                        arrayList.sort(function (a, b) {
                            // set two items to compare
                            var nameA = parseInt(a.value, 10);
                            var nameB = parseInt(b.value, 10);
                            if (isNaN(nameA) || isNaN(nameB))
                                return 0;
                            if (nameA < nameB)
                                return -1;
                            if (nameA > nameB)
                                return 1;
                            // names must be equal
                            return 0;
                        });
                        break;
                    case "string": 
                    default:
                        arrayList.sort(function (a, b) {
                            // set two items to compare case insensitive 
                            var nameA = a.display.toUpperCase();
                            var nameB = b.display.toUpperCase();
                            if (nameA < nameB) {
                                return -1;
                            }
                            if (nameA > nameB) {
                                return 1;
                            }
                            // names must be equal
                            return 0;
                        });
                        break;
                } // end switch sortType
            } // end sortType === "values"
        } // end typeof sortOn
        
        // build out html for select list input   
        var listHtml = [];
        $(arrayList).each(function (i, listItem) {
            listHtml.push("<option value=\"" + listItem.value + "\">" + listItem.display + "</option>");
        });
        // bind html to $(this) of jquery function call 
        jquery.html(listHtml.join(""));
        return { msg : "OK" };
    };
    // @params[0] = string error message 
    // @return status object of DOM manipulation 
    callbacks.addError = function (params) {
        if (params.length != 2) {
            return { msg : "invalid params", numParams: 1 };
        }
        // set params
        var errorMsg = params[0];
        var jquery = params[1];
        // add error classes and message
        jquery.find('.error-message').remove();
        jquery.after("<div class=\"error-message text-danger\">" + errorMsg + "</div>");
        jquery.addClass("input-error");
        return { msg : "OK" };
    };
    // clears all error messages on a page. If a form id is
    // passed the form fields will also be cleared
    // @param formid = id of form (optional)
    // @return status object of DOM manipulation 
    callbacks.clearErrors = function (params) {
        var jquery = params[0];
        if (params.length === 2) {
            var formId = params[0];
            jquery = params[1];
        }
        $(".input-error").removeClass("input-error");
        $(".error-message").remove();
        if (formId != undefined) {
            var form = document.getElementById(jquery.attr("id"));
            form.reset();
        }
        return { msg: "OK" };
    };
    // clears input errors and values in a modal form
    // @params[0] $(this) of $.fn.SimpleCMS (default/always set)
    // @return status object of DOM manipulation 
    callbacks.clearModalErrors = function (params) {
        var jquery = params[0];
        jquery.on('hidden.bs.modal', function () {
            var id = jquery.find('form').attr('id');
            $("#" + id).SimpleCMS("clearErrors"); 
        });
        return { msg : "OK" };
    };
    // Clears input errors on change
    // @params[0] $(this) of $.fn.SimpleCMS (default/always set)
    // @return status object of DOM manipulation 
    callbacks.updateInputField = function (params) {
        var jquery = params[0];
        jquery.change(function () {
            if (this.value != "") {
                $(this).removeClass("input-error");
                $("." + field.substring(1) + ".error-message").remove();
            }
        });
        return { msg : "OK" };
    };
    // adds a dismissable alert to an element
    // @params[0] type = name of what is being updated
    // @params[1] action = update or delete
    // @return status object of DOM manipulation 
    callbacks.alertMsg = function (params) {
        if (params.length != 3) {
            return { msg : "invalid params", numParams: 2 };
        }
        var type = params[0];
        var action = params[1];
        var jquery = params[2];
        var deleteMsg = [
            '<div class="alert alert-success alert-dismissible" role="alert">',
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close">',
            '<span aria-hidden="true">&times;</span></button>',
            '<strong>Success!</strong> ' + type + ' has been deleted.',
            '</div>'
        ];
        var updateMsg = [ 
            '<div class="alert alert-success alert-dismissible" role="alert">',
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close">',
            '<span aria-hidden="true">&times;</span></button>',
            '<strong>Success!</strong> ' + type + ' has been updated.',
            '</div>'
        ];
        if (action === 'delete') jquery.html(deleteMsg.join(""));
        if (action === 'update') jquery.html(updateMsg.join(""));
        return { msg : "OK" };
    };
    
    // call callback function and log errors 
    if (callbacks.hasOwnProperty(callbackName)) {
        var status = callbacks[callbackName](params);
        if (status.msg === "invalid params")
            logger.invalidParams(callbackName, status.numParams);
    } else {
        logger.callbackNotFound(callbackName);
    }
    return;
}; // end $.fn.customNamespace 
