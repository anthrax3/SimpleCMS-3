"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var _default_1 = require("../classes/_default");
var post_1 = require("../classes/post");
var logger_1 = require("../classes/logger");
var PostsService = (function () {
    function PostsService(http) {
        this._default = new _default_1._default(http);
        this.post = new post_1.Post();
        this.postList = null;
        this.totalPages = 1;
    }
    PostsService.prototype.getPost = function (id) {
        if (id > 0) {
            this._default.httpDefaults.url = "/api/v1/Posts/Get/" + id;
            this._default.httpDefaults.includeKey = true;
            var callbackFunction = function extractData(res) {
                var response = res.json();
                var post;
                ;
                // extract response data depending on http status code 
                // errors / messages logged to console
                if (response.httpStatusCode === 200) {
                    if (response.data != null) {
                        post = new post_1.Post(parseInt(String(post["id"]), 10), post["title"], post["content"], post["created"], String(post["visible"]).toLowerCase() === "true", String(post["attachment"]).toLowerCase() === "true");
                    }
                    else {
                        response = {};
                    }
                    logger_1.Logger.LogMessages(response.messages);
                } // end if response.httpStatusCode === 200
                if (response.httpStatusCode > 200) {
                    logger_1.Logger.LogErrors(response.errors);
                    response = {};
                } // end if response.httpStatusCode > 200 
                return post;
            }; // end callbackFunction 
            return this._default.post(callbackFunction);
        } // end if id > 0
        return new post_1.Post();
    };
    PostsService.prototype.getAllPosts = function (pageNumber) {
        if (typeof (pageNumber) === "undefined" || isNaN(pageNumber))
            pageNumber = 1;
        this._default.httpDefaults.url = "/api/v1/Posts/AllPosts";
        this._default.httpDefaults.includeKey = true;
        this._default.httpDefaults.data = JSON.stringify({
            "PageNumber": pageNumber,
            "PageSize": 5
        });
        var scopedTotalPages = this.totalPages;
        var callbackFunction = function extractData(res) {
            var response = res.json();
            var posts = [];
            // extract response data depending on http status code 
            // errors / messages logged to console
            if (response.httpStatusCode === 200) {
                if (response.data != null && response.data["posts"] != null) {
                    for (var i = 0, post; post = response.data["posts"][i++];) {
                        if (String(post["id"]).length > 0) {
                            posts.push(new post_1.Post(parseInt(post["id"], 10), post["title"], post["content"], post["created"], String(post["visible"]).toLowerCase() === "true", String(post["attachment"]).toLowerCase() === "true"));
                        }
                    }
                    scopedTotalPages = !isNaN(parseInt(response.data["totalPages"], 10)) ? parseInt(response.data["totalPages"]) : 1;
                }
                else {
                    response = {};
                }
                logger_1.Logger.LogMessages(response.messages);
            } // end response.httpStatusCode === 200 
            if (response.httpStatusCode > 200) {
                logger_1.Logger.LogErrors(response.errors);
                response = {};
            } // end if response.httpStatusCode > 200
            return posts;
        }; // end callbackFunction 
        this.totalPages = scopedTotalPages; // add updated totalPages back to this
        return this._default.post(callbackFunction);
    };
    PostsService.prototype.getTotalPages = function () {
        this._default.httpDefaults.url = "/api/v1/Posts/GetTotalPages";
        this._default.httpDefaults.includeKey = true;
        this._default.httpDefaults.data = JSON.stringify({
            "PageNumber": 1,
            "PageSize": 5
        });
        var getTotalPagesCallback = function extractDataTotalPages(res) {
            var response = res.json();
            var pagesList = [];
            // extract response data depending on http status code 
            // errors / messages logged to console
            if (response.httpStatusCode === 200) {
                if (response.data != null && response.data["totalPages"] != null) {
                    var totalPages = response.data["totalPages"];
                    for (var i = 1; i <= totalPages; i++) {
                        pagesList.push(i);
                    }
                }
                else {
                    pagesList = [];
                }
                logger_1.Logger.LogMessages(response.messages);
            } // end response.httpStatusCode === 200 
            if (response.httpStatusCode > 200) {
                logger_1.Logger.LogErrors(response.errors);
                pagesList = [];
            } // end if response.httpStatusCode > 200
            console.log(pagesList);
            return pagesList;
        }; // end callbackFunction 
        return this._default.post(getTotalPagesCallback);
    };
    PostsService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], PostsService);
    return PostsService;
}());
exports.PostsService = PostsService;
//# sourceMappingURL=posts.service.js.map