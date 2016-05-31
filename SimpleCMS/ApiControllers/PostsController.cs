using System.Linq;
using System.Net;
using System.Web.Http;
using SimpleCMS.Models;
using SimpleCMS.AppClasses;
using System.Web.Http.Description;
using System.Collections.Generic;

namespace SimpleCMS.Controllers
{
    public class PostsController : BaseApiController
    {
        // POST api/v1/Posts/CreatePost
        [HttpPost]
        [ResponseType(typeof (ApiResponse<object>))]
        public IHttpActionResult Create(PostRequestModel postModel)
        {
            ValidateRequest(postModel); 

            if (ApiRequest.IsValid)
            {
                _db.Posts.Add(postModel.Post);
                _db.SaveChanges();
                ApiResponse.HttpStatusCode = HttpStatusCode.Created;
                ApiResponse.Data = null;
            }

            return Content(ApiResponse.HttpStatusCode, ApiResponse);
        }

        // POST api/v1/Posts/Update
        [HttpPost]
        [ResponseType(typeof(ApiResponse<object>))]
        public IHttpActionResult Update(PostRequestModel postModel)
        {
            ValidateRequest(postModel);

            if (ApiRequest.IsValid)
            {
                var postToUpdate = _db.Posts.First(p => p.ID == postModel.Post.ID);
                postToUpdate = postModel.Post;
                _db.SaveChanges();
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
            }

            return Content(ApiResponse.HttpStatusCode, ApiResponse);
        }

        // POST api/v1/Posts/ByID/{id}
        [HttpPost]
        [ResponseType(typeof (ApiResponse<Posts>))]
        public IHttpActionResult Get([FromUri]int id, [FromBody]string apiKey)
        {
            if (!ValidateApiKey(apiKey, ApiRequest.Request.Url.Authority.ToString()))
            {
                ApiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized);
                ApiRequest.IsValid = false;
            }

            if (ApiRequest.IsValid && id < 1)
            {
                ApiResponse.AddError("Invalid id", HttpStatusCode.BadRequest);
                ApiRequest.IsValid = false;
            }

            if (ApiRequest.IsValid)
            {
                ApiResponse.Data = _db.Posts.FirstOrDefault(p => p.ID == id);
            }

            return Content(ApiResponse.HttpStatusCode, ApiResponse); 
        }
        
        // POST /api/Posts/AllPosts
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

        // POST api/Posts/PostsByUser

        [HttpPost]
        [ResponseType(typeof (ApiResponse<IEnumerable<Posts>>))]
        public IHttpActionResult ByUser(ByUserRequestModel userModel)
        {
            ValidateRequest(userModel); 

            if (ApiResponse._IsValid)
            {
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
                ApiResponse.Data = _db.Posts.Where(p =>
                    p.ApplicationUser.UserName == userModel.Username)
                    .ToList<Posts>() as IEnumerable<Posts>;
            }

            return Content(ApiResponse.HttpStatusCode, ApiResponse); 
        }
    }
}
