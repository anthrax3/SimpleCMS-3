import { Component }          from '@angular/core';
import { ROUTER_DIRECTIVES }  from '@angular/router';
import { Post }               from  './classes/post';
import { PostsService }       from './services/posts.service.ts';
import { Http }               from '@angular/http';
import { _default }           from './classes/_default'; 

@Component({
    selector: 'simple-cms',
    templateUrl: 'app/app.component.html'
})
export class AppComponent {
    public title: string;
    public posts: Post[];
    public showMorePosts: boolean;
    public totalPages: number[];
    public default: _default; 
    private postService: PostsService;

    constructor(http: Http) {
        this.title = "Posts";
        this.postService = new PostsService(http);
        let pageHash = window.location.hash.substr(1);
        let pageNumber: number = 1;
        if (pageHash.length > 0) {
            pageNumber = !isNaN(parseInt(pageHash.substr(pageHash.length - 1), 10))
                ? parseInt(pageHash.substr(pageHash.length - 1), 10)
                : pageNumber;
            if (pageNumber > 1) {
                this.updatePaginationSyles(pageNumber);
            }
        }
        this.postService.getAllPosts(pageNumber)
            .then(posts => this.posts = posts);
        this.postService.getTotalPages()
            .then(totalPages => this.totalPages = totalPages);
        this.default = new _default(http); 
    } 

    public formatExcerpt(content: string): string {
        content = content.replace(/(<([^>]+)>)/ig, "").replace(/\\r\\n/ig, "");
        return content.substring(0, 250) + "...";
    }

    public getMorePosts(pageNumber: number): void {
        this.updatePaginationSyles(pageNumber);
        // save current posts in session 
        this._setSessionStorage(this.posts); 
        // check session for current posts 
        let storagePosts = window.sessionStorage.getItem("page-" + pageNumber);
        if (storagePosts != null) {
            this.posts = JSON.parse(storagePosts);
        } else { // get posts through api if not in session 
            // prevent page jumping after loading 
            $(".posts").hide();
            setTimeout(function () {
                $(".posts").show();
            }, 500);
            this.postService.getAllPosts(pageNumber)
                .then(posts => this.posts = posts);
        }
    } 

    public updatePaginationSyles(pageNumber: number): void {
        $("[class^='page-']").parent().removeClass("active");
        $(".page-" + pageNumber).parent().addClass("active");
    }

    private _setSessionStorage(data: any, key?:string): void {
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


