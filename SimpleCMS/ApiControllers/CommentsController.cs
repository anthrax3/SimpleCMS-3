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
        //[HttpPost]
        //[ResponseType(typeof(ApiResponse<Comments>))]
        //public IHttpActionResult ByPost(PostRequestModel postRequest)
        //{
        //    if (postRequest.ValidateRequest(this, ModelState)) {
        //    // valid ApiRequest
        //    ApiResponse.HttpStatusCode = HttpStatusCode.OK;
        //    var postComments = _db.Comments.Where(c => c.posts.Any(p => p.ID == postID)).ToList<Object>();
        //    if (postComments.Count == 0)
        //    {
        //        ApiResponse.Data = null; 
        //        ApiResponse.Messages.Add(ErrorMessages.NoComments);
        //    }
        //    else
        //    {
        //        ApiResponse.Data = postComments;
        //    }
          
        //    return ResponseContent(ApiResponse)
        //}
    }
}
