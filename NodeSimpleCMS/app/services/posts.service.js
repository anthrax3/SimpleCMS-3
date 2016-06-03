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
        var _this = this;
        this._default.httpDefaults.url = "/api/v1/Posts/AllPosts";
        this._default.httpDefaults.includeKey = true;
        var promise = this._default.post();
        promise.then(function (posts) { return _this.postList = posts; });
        return this.postList;
    };
    PostsService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], PostsService);
    return PostsService;
}());
exports.PostsService = PostsService;
//# sourceMappingURL=posts.service.js.map