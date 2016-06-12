import { Component }          from '@angular/core';
import { RouteConfig,
    ROUTER_DIRECTIVES,
    ROUTER_PROVIDERS }        from '@angular/router-deprecated';
import { DashboardComponent } from './dashboard.component';
import { Post }               from  './classes/post';
import { PostsService }       from './services/posts.service';
import { PostComponent }      from './post.component.ts';
import { Http }               from '@angular/http';
import { _default }           from './classes/_default'; 

@Component({
    selector: 'simple-cms',
    templateUrl: 'app/app.component.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [
        ROUTER_PROVIDERS
    ]

})
@RouteConfig([
    {
        path: '/post/{id}',
        name: 'Post',
        component: PostComponent
    },
    {
        path: '/dashboard',
        name: 'Dashboard',
        component: DashboardComponent,
        useAsDefault: true
    }
])
export class AppComponent {
    public title: string;
    public posts: Post[];
    public default: _default; 
    private postService: PostsService;

    constructor(http: Http) {
        //if (page === undefined || page === null)
        //    page = 1;
        this.title = "Posts";
        this.postService = new PostsService(http);
        this.postService.getAllPosts(1)
            .then(posts => this.posts = posts);
        this.default = new _default(http); 
    } 

    public formatExcerpt(content: string): string {
        let regex = /(<([^>]+)>)/ig;
        content = content.replace(regex, ""); 
        return content.substring(0, 250) + "...";
    }
}
