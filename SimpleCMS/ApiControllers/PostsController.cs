using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using SimpleCMS.Models;
using SimpleCMS.AppClasses;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Web.Http.Cors;

namespace SimpleCMS.Controllers
{
    [AllowAnonymous]
    public class PostsController : BaseApiController
    {
        // POST api/v1/Posts/CreatePost
        [HttpPost]
        [ResponseType(typeof (ApiResponse<object>))]
        public IHttpActionResult Create(PostRequestModel postModel)
        {
            ValidateRequest(postModel);

            // return early if bad request 
            if (!ApiRequest.IsValid)
                return Content(ApiResponse.HttpStatusCode, ApiResponse);

            // valid ApiRequest. create post
            postModel.Post.Created = DateTime.Now;
            postModel.Post.Updated = postModel.Post.Created;
            _db.Posts.Add(postModel.Post);
            _db.SaveChanges();
            ApiResponse.HttpStatusCode = HttpStatusCode.Created;
            ApiResponse.Data = null;

            return Content(ApiResponse.HttpStatusCode, ApiResponse);
        }

        // POST api/v1/Posts/Update
        [HttpPost]
        [ResponseType(typeof(ApiResponse<object>))]
        public IHttpActionResult Update(PostRequestModel postModel)
        {
            ValidateRequest(postModel);

            // return early if bad request 
            if (!ApiRequest.IsValid)
                return Content(ApiResponse.HttpStatusCode, ApiResponse);

            // valid ApiRequest. update post
            var postToUpdate = _db.Posts.First(p => p.ID == postModel.Post.ID);
            postToUpdate.ID = postModel.Post.ID;
            postToUpdate.Title = postModel.Post.Title;
            postToUpdate.Content = postModel.Post.Content;
            postToUpdate.Updated = DateTime.Now;
            postToUpdate.Attachment = postModel.Post.Attachment;
            if (postToUpdate.Attachment && !string.IsNullOrEmpty(postModel.Post.AttachmentPath))
            {
                postToUpdate.AttachmentPath = postModel.Post.AttachmentPath;
            }
            if (!string.IsNullOrEmpty(postModel.Post.Category))
            {
                postToUpdate.Category = postModel.Post.Category;
            }
            _db.SaveChanges();
            ApiResponse.HttpStatusCode = HttpStatusCode.OK;
            ApiResponse.Data = null;

            return Content(ApiResponse.HttpStatusCode, ApiResponse);
        }

        // POST api/v1/Posts/ByID/{id}
        [HttpPost]
        [ResponseType(typeof (ApiResponse<Posts>))]
        public IHttpActionResult Get([FromUri]int id, [FromBody]string apiKey)
        {
            if (!ValidateApiKey(apiKey, ApiRequest.Request.Url.Authority))
            {
                ApiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized);
            }
            
            // return early if invalid api key
            if (!ApiRequest.IsValid)
                return Content(ApiResponse.HttpStatusCode, ApiResponse);

            // return early if invalid post id 
            if (ApiRequest.IsValid && id < 0)
            {
                ApiResponse.AddError("Invalid id", HttpStatusCode.BadRequest);
                return Content(ApiResponse.HttpStatusCode, ApiResponse);
            }

            ApiResponse.Data = _db.Posts.FirstOrDefault(p => p.ID == id);
            
            return Content(ApiResponse.HttpStatusCode, ApiResponse); 
        }
        
        // POST /api/v1/Posts/AllPosts
        [HttpPost]
        [ResponseType(typeof(ApiResponse<IEnumerable<Posts>>))]
        public IHttpActionResult AllPosts([FromBody]string apiKey)
        {
            if (ValidateApiKey(apiKey, ApiRequest.Request.Url.Authority))
            {
                ApiResponse.Data = _db.Posts.ToList<Posts>();
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                ApiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized);
            }

            return Content(ApiResponse.HttpStatusCode, ApiResponse);
        }

        // POST /api/v1/Posts/ByUser
        [HttpPost]
        [ResponseType(typeof (ApiResponse<IEnumerable<Posts>>))]
        public IHttpActionResult ByUser(ByUserRequestModel userModel)
        {
            ValidateRequest(userModel);

            // return early if bad request
            if (!ApiRequest.IsValid)
                return Content(ApiResponse.HttpStatusCode, ApiResponse);
            
            // valid ApiRequest
            ApiResponse.HttpStatusCode = HttpStatusCode.OK;
            ApiResponse.Data = _db.Posts.Where(p =>
                p.ApplicationUser.UserName == userModel.Username).ToList();

            return Content(ApiResponse.HttpStatusCode, ApiResponse); 
        }
    }
}
