import { Component }          from '@angular/core';
import { RouteConfig,
    ROUTER_DIRECTIVES,
    ROUTER_PROVIDERS }   from '@angular/router-deprecated';
import { DashboardComponent } from './dashboard.component';
import { Post }               from  './classes/post';
import { PostsService }       from './classes/posts.service';
import { PostComponent }      from './post.component.ts';

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

    constructor() {
        this.postService = new PostsService();
        // mock posts to test angular 
        //this.posts = this.postService.getAllPosts(); 
        this.posts = [];
        for (let i = 0; i < 5; i++) {
            this.posts.push({
                id: i,
                title: "Mock post title number " + i,
                content:
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque elementum felis lorem, et cursus nisl porttitor et. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis rhoncus fringilla dictum. Aenean fringilla imperdiet velit, in finibus augue posuere sit amet. Sed eget semper lacus. Cras mollis nulla a consequat condimentum. Duis fringilla ligula quis justo malesuada volutpat. Pellentesque interdum vulputate neque, vel accumsan nisl pharetra in. Donec venenatis mauris vel convallis ornare. Morbi urna nibh, euismod quis elit nec, sollicitudin rutrum ex.",
                category: "default",
                attachment: false,
                attachmentPath: null,
                comments: null
            });
        } // end for loop 
    } // end constructor 
}
