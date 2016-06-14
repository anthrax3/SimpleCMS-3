import { Injectable }     from "@angular/core";
import { Http, Response } from "@angular/http";
import { _default }       from "../classes/_default";
import { Post }           from "../classes/post";
import { Logger }         from "../classes/logger";

@Injectable()
export class PostsService {

    private _default: _default;

    public post: Post;

    public postList: Post[];

    public totalPages: number;

    constructor(http: Http) {
        this._default = new _default(http);
        this.post = new Post();
        this.postList = null;
        this.totalPages = 1;
    }

    public getPost(id: number): any {
        if (id > 0) {
            this._default.httpDefaults.url = "/api/v1/Posts/Get/" + id;
            this._default.httpDefaults.includeKey = true;
            let callbackFunction = function extractData(res: Response): any {
                let response = res.json();
                let post: Post;;
                // extract response data depending on http status code 
                // errors / messages logged to console
                if (response.httpStatusCode === 200) {
                    if (response.data != null) {
                        post = new Post(parseInt(String(post["id"]), 10), post["title"], post["content"], post["created"], String(post["visible"]).toLowerCase() === "true", String(post["attachment"]).toLowerCase() === "true");
                    } else {
                        response = {};
                    }
                    Logger.LogMessages(response.messages);
                } // end if response.httpStatusCode === 200
                if (response.httpStatusCode > 200) {
                    Logger.LogErrors(response.errors);
                    response = {};
                } // end if response.httpStatusCode > 200 

                return post;
            } // end callbackFunction 

            return this._default.post<Post>(callbackFunction); 
        } // end if id > 0
        return new Post(); 
    }

    public getAllPosts(pageNumber?: number): any {
        if (typeof(pageNumber) === "undefined" || isNaN(pageNumber))
            pageNumber = 1;  

        this._default.httpDefaults.url = "/api/v1/Posts/AllPosts";
        this._default.httpDefaults.includeKey = true; 
        this._default.httpDefaults.data = JSON.stringify({
            "PageNumber": pageNumber,
            "PageSize" : 5
        });
        let scopedTotalPages = this.totalPages;
        let callbackFunction = function extractData(res: Response): any {
            let response = res.json();
            let posts = [];
            // extract response data depending on http status code 
            // errors / messages logged to console
            if (response.httpStatusCode === 200) {
                if (response.data != null && response.data["posts"] != null) {
                    for (var i = 0, post; post = response.data["posts"][i++];) {
                        if (String(post["id"]).length > 0) {
                            posts.push(new Post(parseInt(post["id"], 10), post["title"], post["content"], post["created"], String(post["visible"]).toLowerCase() === "true", String(post["attachment"]).toLowerCase() === "true"));
                        }
                    }
                    scopedTotalPages = !isNaN(parseInt(response.data["totalPages"], 10)) ? parseInt(response.data["totalPages"]) : 1;
                } else {
                    response = {}
                }
                Logger.LogMessages(response.messages); 
            } // end response.httpStatusCode === 200 
            if (response.httpStatusCode > 200) {
                Logger.LogErrors(response.errors);
                response = {};
            } // end if response.httpStatusCode > 200

            return posts;
        } // end callbackFunction 
        this.totalPages = scopedTotalPages; // add updated totalPages back to this
        return this._default.post<Post[]>(callbackFunction);
    }

    public getTotalPages(): any {
        this._default.httpDefaults.url = "/api/v1/Posts/GetTotalPages";
        this._default.httpDefaults.includeKey = true;
        this._default.httpDefaults.data = JSON.stringify({
            "PageNumber": 1,
            "PageSize": 5
        });
        let getTotalPagesCallback = function extractDataTotalPages(res: Response): any {
            let response = res.json();
            let pagesList = [];
            // extract response data depending on http status code 
            // errors / messages logged to console
            if (response.httpStatusCode === 200) {
                if (response.data != null && response.data["totalPages"] != null) {
                    let totalPages = response.data["totalPages"];
                    for (var i = 1; i <= totalPages; i++) {
                        pagesList.push(i);
                    }
                } else {
                    pagesList = [];
                }
                Logger.LogMessages(response.messages);
            } // end response.httpStatusCode === 200 
            if (response.httpStatusCode > 200) {
                Logger.LogErrors(response.errors);
                pagesList = [];
            } // end if response.httpStatusCode > 200
            console.log(pagesList);
            return pagesList;
        } // end callbackFunction 

        return this._default.post<Object>(getTotalPagesCallback);
    }
    
}