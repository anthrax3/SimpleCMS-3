using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using SimpleCMS.ApiClasses;
using SimpleCMS.Models;
using SimpleCMS.ApiModels;
using System.Web.Http.Description;

namespace SimpleCMS.Controllers
{
    public class CommentsController : BaseApiController
    {
        // POST api/v1/Comments/ByPost/{id}
        [HttpPost]
        [ResponseType(typeof(ApiResponse<Comments>))]
        public IHttpActionResult ByPost([FromUri]int id, [FromBody]string apiKey)
        {
            var postID = id; // to distinguish between comment and post id 
            var isValid = true;
            if (!ValidateApiKey(apiKey))
            {
                ApiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized);
                isValid = false;
            }

            if (isValid && !_db.Posts.Any(p => p.ID == postID))
            {
                ApiResponse.Messages.Add(ErrorMessages.PostNotFound(postID));
                ApiResponse.HttpStatusCode = HttpStatusCode.OK;
                isValid = false;
            }

            // return early if bad request 
            if (!isValid)
                return Content(ApiResponse.HttpStatusCode, ApiResponse);
            
            // valid ApiRequest
            ApiResponse.HttpStatusCode = HttpStatusCode.OK;
            var postComments = _db.Comments.Where(c => c.posts.Any(p => p.ID == postID)).ToList<Object>();
            if (postComments.Count == 0)
            {
                ApiResponse.Data = null; 
                ApiResponse.Messages.Add(ErrorMessages.NoComments);
            }
            else
            {
                ApiResponse.Data = postComments;
            }
          
            return Content(ApiResponse.HttpStatusCode, ApiResponse);
        }
    }
}
