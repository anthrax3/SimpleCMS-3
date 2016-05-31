import { Injectable } from '@angular/core';
import { _default } from "./_default";
import { Post } from "./post";

@Injectable()
export class PostsService {

    private _default: _default;

    public post: Post;

    public postList: Post[];

    constructor() {
        this._default = new _default();
        this.post = new Post();
        this.postList = null;
    }

    public getPost(id: number): Post {
        if (id > 0) {
            var successCallback = response => {
                if (response.httpStatusCode === 200) {
                    return response.data;
                }
            };
            this._default.ajaxDefaults.url = "/api/v1/Posts/Get/" + id.toString();
            this._default.ajaxDefaults.success = successCallback;
            this._default.ajaxDefaults.async = false; 
            this.post = this._default.ajaxPost();
            return this.post;
        }
    }

    public getAllPosts(): Post[] {
        var successCallback = response => {
            if (response.httpStatusCode === 200) {
                return response.data;
            }
        }
        this._default.ajaxDefaults.url = "/api/v1/Posts/AllPosts";
        this._default.ajaxDefaults.success = successCallback; 
        this._default.ajaxDefaults.async = false; 
        this.post = this._default.ajaxPost();
        return this.postList;
    }
}