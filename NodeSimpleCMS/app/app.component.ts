import { Component }          from '@angular/core';
import { RouteConfig,
    ROUTER_DIRECTIVES,
    ROUTER_PROVIDERS }        from '@angular/router-deprecated';
import { DashboardComponent } from './dashboard.component';
import { Post }               from  './classes/post';
import { PostsService }       from './classes/posts.service';
import { PostComponent }      from './post.component.ts';
import { Http } from '@angular/http';
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
    public title = "Posts";
    public posts: Post[];
    private postService: PostsService;

    constructor(http:Http) {
        this.postService = new PostsService(http);
        this.posts = this.postService.getAllPosts(); 
    } 
}
