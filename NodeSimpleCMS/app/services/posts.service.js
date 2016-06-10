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
var PostsService = (function () {
    function PostsService(http) {
        this._default = new _default_1._default(http);
        this.post = new post_1.Post();
        this.postList = null;
    }
    PostsService.prototype.getPost = function (id) {
        var _this = this;
        if (id > 0) {
            this._default.httpDefaults.url = "/api/v1/Posts/Get/" + id;
            this._default.httpDefaults.includeKey = true;
            var promise = this._default.post();
            promise.then(function (post) { return _this.post = post; });
            return this.post;
        }
    };
    PostsService.prototype.getAllPosts = function () {
        this._default.httpDefaults.url = "/api/v1/Posts/AllPosts";
        this._default.httpDefaults.includeKey = true;
        var callbackFunction = function extractData(res) {
            var response = res.json();
            var posts = [];
            // extract response data depending on http status code 
            // errors / messages logged to console
            if (response.HttpStatusCode === 200) {
                if (response.Data != null) {
                    for (var i = 0, post; post = response.Data[i++];) {
                        posts.push(new post_1.Post(parseInt(post["ID"], 10), post["Title"], post["Content"], post["Created"], String(post["Visible"]).toLowerCase() === "true", String(post["Attachment"]).toLowerCase() === "true"));
                    }
                }
                else {
                    console.log(response.Message);
                    response = {};
                }
            }
            if (response.length > 0 && response.HttpStatusCode > 200) {
                console.log(response.Errors);
                response = {};
            }
            return posts;
        };
        return this._default.post(callbackFunction);
    };
    // callback method used for http requests 
    // @param Response 
    // @returns 
    PostsService.prototype.extractData = function (res) {
        var response = res.json();
        // extract response data depending on http status code 
        // errors / messages logged to console
        if (response.HttpStatusCode === 200) {
            if (response.Data != null) {
                response = response.Data;
            }
            else {
                console.log(response.Message);
                response = {};
            }
        }
        if (response.length > 0 && response.HttpStatusCode > 200) {
            console.log(response.Errors);
            response = {};
        }
        return response;
    };
    PostsService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], PostsService);
    return PostsService;
}());
exports.PostsService = PostsService;
//# sourceMappingURL=posts.service.js.map