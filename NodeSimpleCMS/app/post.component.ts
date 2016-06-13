import { Component }          from '@angular/core';
import { RouteConfig, RouteParams,
    ROUTER_DIRECTIVES,
    ROUTER_PROVIDERS }   from '@angular/router-deprecated';
import { DashboardComponent } from './dashboard.component';
import { Post }               from  './classes/post';
import { PostsService }       from './services/posts.service';
import { Http }  from '@angular/http'
import { _default} from './classes/_default'; 

@Component({
    selector: 'simple-cms',
    templateUrl: 'app/post.component.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [
        ROUTER_PROVIDERS
    ]

})
export class PostComponent {
    public title: string;
    public post: Post;
    private postService: PostsService;
    public default: _default; 

    constructor(http: Http, private _routeParams: RouteParams) {
        let id = parseInt(this._routeParams.get("id"));
        this.postService = new PostsService(http);
        this.post = this.postService.getPost(id); 
        this.default = new _default(http);   
    } 
}
