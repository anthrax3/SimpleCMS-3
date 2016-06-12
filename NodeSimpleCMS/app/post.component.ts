import { Component }          from '@angular/core';
import { RouteConfig,
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

    constructor(public id: number, http: Http) {
        this.postService = new PostsService(http);
        this.post = this.postService.getPost(id); 
        this.default = new _default(http);   
    } 
}
