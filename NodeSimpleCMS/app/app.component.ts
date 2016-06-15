import { Component }          from '@angular/core';
import { RouteConfig, RouteParams, Router,
    ROUTER_DIRECTIVES, ROUTER_PROVIDERS }        from '@angular/router-deprecated';
import { DashboardComponent } from './dashboard.component';
import { Post }               from  './classes/post';
import { PostsService }       from './services/posts.service';
import { PostComponent }      from './post.component.ts';
import { PostsComponent }     from './posts.component.ts';
import { Http }               from '@angular/http';
import { _default }           from './classes/_default'; 

@Component({
    selector: 'simple-cms',
    templateUrl: 'app/app.component.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [
        ROUTER_PROVIDERS,
        PostsService,
        PostsComponent,
        PostComponent,
        DashboardComponent
        ]
})
@RouteConfig([
    {
        path: '/dashboard',
        as: 'Dashboard',
        component: DashboardComponent
    },
    {
        path: '/posts/:page',
        as: 'Posts',
        component: PostsComponent
    },
    {
        path: '/post/:id',
        as: 'Post',
        component: PostComponent
    }
])
export class AppComponent {
    public title: string;
    public posts: Post[];
    public showMorePosts: boolean;
    public totalPages: number[];
    public default: _default; 
    private postService: PostsService;

    constructor(http: Http,  private _router: Router) {
        this.title = "Posts";
        this.postService = new PostsService(http);
        this.postService.getAllPosts(1)
            .then(posts => this.posts = posts);
        this.postService.getTotalPages()
            .then(totalPages => this.totalPages = totalPages);
        this.default = new _default(http); 
    } 

    public formatExcerpt(content: string): string {
        let regex = /(<([^>]+)>)/ig;
        content = content.replace(regex, "").replace(/\\r\\n/ig, "");
        return content.substring(0, 250) + "...";
    }

    public getMorePosts(pageNumber: number): void {
        $("[class^='page-']").parent().removeClass("active");
        $(".page-" + pageNumber).parent().addClass("active");
        // save current post in session 
        this._setSessionlStorage(this.posts); 
        let storagePosts = window.sessionStorage.getItem("page-" + pageNumber);
        if (storagePosts != null) {
            this.posts = JSON.parse(storagePosts);
        } else { // get posts through api if not in session 
            // prevent page jumping after loading 
            $(".posts").hide().height("600px");
            setTimeout(function () {
                $(".posts").show();
            }, 400);
            this.postService.getAllPosts(pageNumber)
                .then(posts => this.posts = posts);
        }
    } 

    private _setSessionlStorage(data: any, key?:string): void {
        if (key == null) {
            key = window.location.hash.substr(1);
            if (key.length === 0) {
                key = "page-1";
            }
        } // end if key == null
        if (data != undefined && key.length > 0) {
            window.sessionStorage.setItem(key, JSON.stringify(data));
        }
    }
}


