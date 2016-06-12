using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleCMS.DAL;
using SimpleCMS.Models;
using System.Web;
using System.Web.Http;
using SimpleCMS.ApiModels;
using System.Linq;
using System;

namespace SimpleCMS.ApiClasses
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

        public HttpRequest ApiRequest { get; set; }
        
        protected ApplicationContext _db;

        protected UserManager<ApplicationUser> UserManager { get; set; }

        public BaseApiController()
        {
            ApiRequest = HttpContext.Current.Request;
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
            ApiRequest = HttpContext.Current.Request;
            ApiResponse = new ApiResponse<object>();
            _db = db;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
        }

        /// <summary>
        /// Validates an apiKey based on the referrer URL 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        protected bool ValidateApiKey(string apiKey)
        {
            var boolRtn = false;
            var validApiKey = _db.ApiAccounts.Any(a => a.ApiKey.Equals(apiKey));
            var referrerForKey = _db.ApiAccounts.FirstOrDefault(a => a.ApiKey.Equals(apiKey)).RequestURL.ToString();

            if ((referrerForKey == "*" || string.Equals(referrerForKey, ApiRequest.Url.Authority, StringComparison.CurrentCultureIgnoreCase)) &&
                    validApiKey)
            {
                boolRtn = true;
            }
            return boolRtn;
        }
    }
}