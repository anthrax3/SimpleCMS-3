using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using SimpleCMS.Models;
using SimpleCMS.ApiModels;
using SimpleCMS.ApiClasses;
using System.Web.Http.Description;
using System.Collections.Generic;

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
            if (postModel.ValidateRequest(this, ModelState))
            {
                // valid ApiRequest. create post
                postModel.Post.Created = DateTime.Now;
                postModel.Post.Updated = postModel.Post.Created;
                _db.Posts.Add(postModel.Post);
                _db.SaveChanges();
                ApiResponse.HttpStatusCode = HttpStatusCode.Created;
                ApiResponse.Data = null;
            } 

            return ResponseContent(ApiResponse);
        }

        // POST api/v1/Posts/Update
        [HttpPost]
        [ResponseType(typeof(ApiResponse<object>))]
        public IHttpActionResult Update(PostRequestModel postModel)
        {
            if (postModel.ValidateRequest(this, ModelState))
            {
                // valid ApiRequest. update post
                var postToUpdate = _db.Posts.First(p => p.ID == postModel.Post.ID); // postModel.ValidateRequest validates Post.ID has post
                postToUpdate.ID = postModel.Post.ID;
                postToUpdate.Title = postModel.Post.Title;
                postToUpdate.Content = postModel.Post.Content;
                postToUpdate.Updated = DateTime.Now;
                postToUpdate.Attachment = postModel.Post.Attachment;
                if (postToUpdate.Attachment == true && !string.IsNullOrEmpty(postModel.Post.AttachmentPath))
                {
                    postToUpdate.AttachmentPath = postModel.Post.AttachmentPath;
                }
                _db.SaveChanges();
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
                ApiResponse.Data = null;
            }

            return ResponseContent(ApiResponse);
        }

        // POST api/v1/Posts/ByID/{id}
        //[HttpPost]
        //[ResponseType(typeof (ApiResponse<Posts>))]
        //public IHttpActionResult Get([FromUri]int? id, [FromBody]string apiKey)
        //{
        //    var isValid = true;
        //    if (!ValidateApiKey(apiKey))
        //    {
        //        ApiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized);
        //        isValid = false;
        //    }
            
        //    // return early if invalid api key
        //    if (isValid && id == null)
        //        return ResponseContent(ApiResponse)

        //    // return early if invalid post id 
        //    if (isValid && id < 0)
        //    {
        //        ApiResponse.AddError("Invalid id", HttpStatusCode.BadRequest);
        //        return ResponseContent(ApiResponse)
        //    }

        //    ApiResponse.Data = _db.Posts.FirstOrDefault(p => p.ID == id);

        //    return ResponseContent(ApiResponse);
        //}
        
        // POST /api/v1/Posts/AllPosts
        [HttpPost]
        [ResponseType(typeof(ApiResponse<IEnumerable<Posts>>))]
        public IHttpActionResult AllPosts(AllPostRequestModel postRequest)
        {
            if (postRequest.ValidateRequest(this, ModelState))
            {
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
                var totalPages = Math.Ceiling((double)_db.Posts.Count() / (int)postRequest.PageSize);
                var dataList = new Dictionary<string, object>();
                dataList.Add("posts", _db.Posts.OrderByDescending(p => p.Created)
                                            .Skip(((int)postRequest.PageNumber - 1) * (int)postRequest.PageSize)
                                            .Take((int)postRequest.PageSize)
                                            .Select(p => new
                                            {
                                                p.ID,
                                                p.Title,
                                                p.Content,
                                                p.Created,
                                                p.Visible,
                                                p.Attachment
                                            }).ToList()
                );
                dataList.Add("totalPages", totalPages);

                ApiResponse.Data = dataList;
            }
            else
            {
                ApiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized);
            }

            return ResponseContent(ApiResponse);
        }

        // POST /api/v1/Posts/GeTotalPages
        [HttpPost]
        [ResponseType(typeof(ApiResponse<object>))]
        public IHttpActionResult GetTotalPages(AllPostRequestModel postRequest)
        {
            if (postRequest.ValidateRequest(this, ModelState))
            {
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
                ApiResponse.Data = new { totalPages = Math.Ceiling((double)_db.Posts.Count() / (int)postRequest.PageSize) };
            }

            return ResponseContent(ApiResponse);
        }

        // POST /api/v1/Posts/ByUser
        [HttpPost]
        [ResponseType(typeof (ApiResponse<IEnumerable<Posts>>))]
        public IHttpActionResult ByUser(UserRequestModel userModel)
        { 
            if (userModel.ValidateRequest(this, ModelState))
            {
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
                ApiResponse.Data = _db.Posts.Where(p =>
                    p.ApplicationUser.UserName == userModel.Username).ToList();
            }

            return ResponseContent(ApiResponse);
        }
    }
}
