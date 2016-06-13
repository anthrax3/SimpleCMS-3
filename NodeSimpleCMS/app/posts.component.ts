import { Component }          from '@angular/core';
import { RouteConfig, RouteParams,
    ROUTER_DIRECTIVES }        from '@angular/router-deprecated';
import { Post }               from  './classes/post';
import { PostsService }       from './services/posts.service';
import { PostComponent }      from './post.component.ts';
import { Http }               from '@angular/http';
import { _default }           from './classes/_default';

@Component({
    selector: 'simple-cms',
    templateUrl: 'app/app.component.html',
    directives: [ROUTER_DIRECTIVES]
})
export class PostsComponent {
    public title: string;
    public posts: Post[];
    public default: _default;
    private _postService: PostsService;
    
    constructor(http: Http, private _routeParams: RouteParams) {
        let page: number = parseInt(this._routeParams.get("page"), 10);
        this.title = "Posts";
        this._postService = new PostsService(http);
        this._postService.getAllPosts(page)
            .then(posts => this.posts = posts);
        this.default = new _default(http);
    }
}


