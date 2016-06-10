using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleCMS.DAL;
using SimpleCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using SimpleCMS.ApiModels;

namespace SimpleCMS.AppClasses
{
    /// <summary>
    /// Base Controller Properties and Methods. Contains ApiResponse&lt;object&gt; ApiResponse,
    /// HttpRequest ApiRequest, ErrorMessages ErrorMessages, and UserManager&lt;ApplicationUser&gt;
    /// UserManager properties. ApplicationContext _db variable and helper methods GetModelStateErrors,
    /// ValidateRequest, and ValidateApiKey. 
    /// </summary>
    public class BaseApiController : ApiController 
    {
        public ApiResponse<object> ApiResponse { get; set; }

        public ApiRequest ApiRequest { get; set; }

        public ErrorMessages ErrorMessages { get; set; }

        protected ApplicationContext _db;

        protected UserManager<ApplicationUser> UserManager { get; set; }

        public BaseApiController()
        {
            ApiRequest = new ApiRequest
            {
                Request = HttpContext.Current.Request
            };
            ApiResponse = new ApiResponse<object>();
            _db = new ApplicationContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
        }

        /// <summary>
        ///  overload to pass mock db context for testing 
        /// </summary>
        /// <param name="db"></param>
        public BaseApiController(ApplicationContext db)
        {
            ApiRequest = new ApiRequest
            {
                Request = HttpContext.Current.Request
            };
            ApiResponse = new ApiResponse<object>();
            _db = db;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
        }

        /// <summary>
        /// Returns list of errors from an invalid ModelState 
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public IEnumerable<string> GetModelStateErrors(ModelStateDictionary modelState)
        {
            var result = new List<string>();
            var i = 0;
            foreach (var error in modelState.Select(e => e.Value.Errors))
            {
                if (error == null) continue;
                var firstOrDefault = error.Select(e => e.ErrorMessage)
                    .FirstOrDefault();
                if (firstOrDefault != null)
                    result.Add(firstOrDefault);
                i++;
            }
            return result;
        }

        /// <summary>
        /// Assuming the object parameter has an ApiKey property the api
        /// key is validated along with the ModelState of the request. With the
        /// same assumption a username is also validated. All errors are added
        /// to ApiResponse.Errors and ApiResponse._IsValid and ApiRequest.IsValid
        /// are set appropriately. 
        /// </summary>
        /// <param name="model"></param>
        public void ValidateRequest(object model)
        {
            // get ApiKey property through reflection or set to null if it doesn't exist 
            var apiKey = model.GetType().GetProperties().FirstOrDefault(p => p.Name == "ApiKey") != null ?
                            model.GetType().GetProperties().FirstOrDefault(p => p.Name == "ApiKey")
                            .GetValue(model).ToString() :
                            null; 
            
            if (string.IsNullOrEmpty(apiKey) || !ValidateApiKey(apiKey, ApiRequest.Request.Url.ToString()))
            {
                ApiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized);
            }

            // get Username property and set it null if it doesn't exist 
            var username = model.GetType().GetProperties()
                            .FirstOrDefault(p => string.Equals(p.Name, "username", 
                            StringComparison.CurrentCultureIgnoreCase)) != null ?
                            model.GetType().GetProperties()
                            .FirstOrDefault(p => string.Equals(p.Name, "username", 
                                StringComparison.CurrentCultureIgnoreCase)).GetValue(model).ToString() :
                            null;

            // only validate the username if the property is set 
            if (!string.IsNullOrEmpty(username))
            {
                // make sure username exists 
                if (!_db.Users.Any(u => u.UserName == username))
                {
                    ApiResponse.AddError(ErrorMessages.UsernameNotFound(username), HttpStatusCode.OK);
                }
            }
            
            if (ApiResponse._IsValid && !ModelState.IsValid)
            {
                ApiResponse.AddRangeError(GetModelStateErrors(ModelState), HttpStatusCode.BadRequest);
            }

            ApiRequest.IsValid = ApiResponse._IsValid;
        }

        /// <summary>
        /// Validates an apiKey based on the referrer URL 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="referrer"></param>
        /// <returns></returns>
        protected bool ValidateApiKey(string apiKey, string referrer)
        {
            var boolRtn = false;
            var validApiKey = _db.ApiAccounts.Any(a => a.ApiKey.Equals(apiKey));
            var referrerForKey = _db.ApiAccounts.FirstOrDefault(a => a.ApiKey.Equals(apiKey)).RequestURL.ToString();

            if ((referrerForKey == "*" || string.Equals(referrerForKey, referrer, StringComparison.CurrentCultureIgnoreCase)) &&
                    validApiKey)
            {
                boolRtn = true; 
            }
            ApiRequest.IsValid = boolRtn;
            return boolRtn;
        }
    }
}