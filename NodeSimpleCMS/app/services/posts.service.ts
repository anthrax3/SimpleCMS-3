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

    constructor(http: Http) {
        this._default = new _default(http);
        this.post = new Post();
        this.postList = null;
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
        if (typeof(pageNumber) === "number")
            pageNumber = 1;  

        this._default.httpDefaults.url = "/api/v1/Posts/AllPosts";
        this._default.httpDefaults.includeKey = true; 
        this._default.httpDefaults.data = JSON.stringify({
            "PageNumber": pageNumber,
            "PageSize" : 5
        });
        let callbackFunction = function extractData(res: Response): any {
            let response = res.json();
            let posts = [];
            // extract response data depending on http status code 
            // errors / messages logged to console
            if (response.httpStatusCode === 200) {
                if (response.data != null) {
                    for (var i = 0, post; post = response.data[i++];) {
                        posts.push(new Post(parseInt(post["id"], 10), post["title"], post["content"], post["created"], String(post["visible"]).toLowerCase() === "true", String(post["attachment"]).toLowerCase() === "true"));
                    }
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
   
        return this._default.post<Post[]>(callbackFunction);
    }

    // callback method used for http requests 
    // @param Response 
    // @returns 
    public extractData<T>(res: Response): T {
        let response = res.json();

        // extract response data depending on http status code 
        // errors / messages logged to console
        if (response.HttpStatusCode === 200) {
            if (response.Data != null) {
                response = response.Data;
            } else {
                console.log(response.Message);
                response = {}
            }
        }
        if (response.length > 0 && response.HttpStatusCode > 200) {
            console.log(response.Errors);
            response = {};
        }

        return response;
    }
}