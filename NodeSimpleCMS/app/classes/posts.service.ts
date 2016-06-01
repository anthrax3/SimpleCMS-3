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
            this._default.httpDefaults.url = "/api/v1/Posts/Get/" + id;
            
            let promise = this._default.post<Post>();
            promise.then(
                post => this.post = post
            );

            return this.post;
        }
    }

    public getAllPosts(): Post[] {
        this._default.httpDefaults.url = "/api/v1/Posts/AllPosts";

        let promise = this._default.post<Post[]>();
        promise.then(
            posts => this.postList = posts
        );

        return this.postList;
    }
}