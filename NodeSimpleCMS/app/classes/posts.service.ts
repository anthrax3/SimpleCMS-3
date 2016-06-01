import { Injectable } from '@angular/core';
import { _default }   from "./_default";
import { Post }       from "./post";
import { Http, Response }       from "@angular/http";

@Injectable()
export class PostsService {

    private _default: _default;

    public post: Post;

    public postList: Post[];

    constructor(http: Http) {
        this._default = new _default(http);
        this.post = new Post();
        this.postList = null;
    }

    public getPost(id: number): Post {
        if (id > 0) {
            var successCallback =  (res:Response) => {
                let body = res.json();
                let response = body.data || {};
                if (response.length > 0 && response.httpStatusCode === 200) {
                    return response.data;
                }
            };
            this._default.httpDefaults.url = "/api/v1/Posts/Get/" + id;
            this._default.httpDefaults.callback = successCallback;
            
            let promise = this._default.ajaxPost<Post>();
            return this.post;
        }
    }

    public getAllPosts(): Post[] {
        
        this._default.httpDefaults.url = "/api/v1/Posts/AllPosts";

        let promise = this._default.ajaxPost<Post[]>();
        promise.then(
            posts => this.postList = posts
        );
        return this.postList;
    }
}