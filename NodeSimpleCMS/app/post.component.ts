import { Component }          from '@angular/core';
import { RouteConfig,
    ROUTER_DIRECTIVES,
    ROUTER_PROVIDERS }   from '@angular/router-deprecated';
import { DashboardComponent } from './dashboard.component';
import { Post }               from  './classes/post';
import { PostsService }       from './services/posts.service';
import { Http }  from '@angular/http'
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

    constructor(public id: number, http: Http) {
        this.postService = new PostsService(http);
        // mock posts to test angular 
        //this.post = this.postService.getPost(id); 
        for (var i = 0; i < 5; i++) {
            this.post = {
                id: i,
                title: "Mock post title number " + i,
                content: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque elementum felis lorem, et cursus nisl porttitor et. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis rhoncus fringilla dictum. Aenean fringilla imperdiet velit, in finibus augue posuere sit amet. Sed eget semper lacus. Cras mollis nulla a consequat condimentum. Duis fringilla ligula quis justo malesuada volutpat. Pellentesque interdum vulputate neque, vel accumsan nisl pharetra in. Donec venenatis mauris vel convallis ornare. Morbi urna nibh, euismod quis elit nec, sollicitudin rutrum ex.",
                category: "default",
                attachment: false,
                attachmentPath: null,
                comments: null
            };
        } // end for loop 
    } // end constructor 
}
