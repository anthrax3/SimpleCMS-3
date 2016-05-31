using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using SimpleCMS.AppClasses;

namespace SimpleCMS.Controllers
{
    public class CommentsController : BaseApiController
    {
        // POST api/v1/Comments/ByPost/{id}
        public IHttpActionResult ByPost([FromUri]int id, [FromBody]string apiKey)
        {
            var postID = id; // to distinguish between comment and post id 

            if (!ValidateApiKey(apiKey, ApiRequest.Request.Url.ToString()))
            {
                ApiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized);
            }

            if (ApiResponse._IsValid && !_db.Posts.Any(p => p.ID == postID))
            {
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
                ApiResponse.Messages.Add(ErrorMessages.PostNotFound(postID));
            }

            if (ApiResponse._IsValid)
            {
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
                var postComments = _db.Comments.Where(c => c.posts.Any(p => p.ID == postID)).ToList<Object>();
                if (postComments == null)
                {
                    ApiResponse.Data = null; 
                    ApiResponse.Messages.Add("This post does not have any comments.");
                }
                else
                {
                    ApiResponse.Data = postComments;
                }
            }

            return Content(ApiResponse.HttpStatusCode, ApiResponse);
        }
    }
}
