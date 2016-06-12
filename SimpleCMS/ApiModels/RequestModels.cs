using SimpleCMS.AppClasses;
using SimpleCMS.DAL;
using SimpleCMS.Models;
using SimpleCMS.ApiClasses;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.ModelBinding;

namespace SimpleCMS.ApiModels
{
    [ModelBinder(typeof(ApiModelBinderProvider<PostRequestModel>))]
    public class PostRequestModel : RequestModel
    {
        [Required]
        public Posts Post { get; set; }
    }

    [ModelBinder(typeof(ApiModelBinderProvider<ByUserRequestModel>))]
    public class ByUserRequestModel : RequestModel
    {
        [Required]
        public string Username { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="apiResponse"></param>
        /// <param name="apiRequest"></param>
        /// <param name="db"></param>
        public bool ValidateRequest(ApiResponse<object> apiResponse, HttpRequest apiRequest, ApplicationContext db)
        {
            var boolRtn = true; 
            if (string.IsNullOrEmpty(ApiKey) || !ValidateApiKey(apiResponse, apiRequest, db))
            {
                apiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized);
                boolRtn = false;
            }

            // only validate the username if the property is set 
            if (!string.IsNullOrEmpty(Username))
            {
                // make sure username exists 
                if (!db.Users.Any(u => u.UserName == Username))
                {
                    apiResponse.AddError(ErrorMessages.UsernameNotFound(Username), HttpStatusCode.OK);
                }
            }
           
            _IsValid = boolRtn;
            return boolRtn;
        }
        
    }

    [ModelBinder(typeof(ApiModelBinderProvider<AllPostRequestModel>))]
    public class AllPostRequestModel : RequestModel
    {
        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiResponse"></param>
        /// <param name="apiRequest"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool ValidateRequest(ApiResponse<object> apiResponse, HttpRequest apiRequest, ApplicationContext db)
        {
            var boolRtn = true;
            // set defaults 
            if (PageNumber == null)
            {
                PageNumber = 1; 
            }
            if (PageSize == null)
            {
                PageSize = 5;
            }
            if (!ValidateApiKey(apiResponse, apiRequest, db))
            {
                apiResponse.AddError(ErrorMessages.InvalidApiKey, HttpStatusCode.Unauthorized); 
                boolRtn = false;
            }
            // make sure nothing set to zero (prevent divide by zero errors down the line) 
            if (PageNumber == 0)
            {
                apiResponse.AddError("PageNumber cannot be zero", HttpStatusCode.BadRequest);
                boolRtn = false;
            }
            if (PageSize == 0)
            {
                apiResponse.AddError("PageSize cannot be zero", HttpStatusCode.BadRequest);
                boolRtn = false;
            }
            _IsValid = boolRtn;  
            return boolRtn;
        }
    }

    public class RequestModel
    {
        [Required]
        public string ApiKey { get; set; }

        internal bool _IsValid { get; set; }

        public RequestModel()
        {
            _IsValid = true;
        }

        /// <summary>
        /// Validates an apiKey based on the referrer URL 
        /// </summary>
        /// <param name="apiResponse"></param>
        /// <param name="apiRequest"></param>
        /// <returns></returns>
        public bool ValidateApiKey(ApiResponse<object> apiResponse, HttpRequest apiRequest, ApplicationContext db)
        {
            var boolRtn = false;
            var validApiKey = db.ApiAccounts.Any(a => a.ApiKey.Equals(ApiKey));
            var referrerForKey = db.ApiAccounts.FirstOrDefault(a => a.ApiKey.Equals(ApiKey)).RequestURL.ToString();

            if ((referrerForKey == "*" || string.Equals(referrerForKey, apiRequest.Url.Authority, StringComparison.CurrentCultureIgnoreCase)) &&
                    validApiKey)
            {
                boolRtn = true;
            }
            _IsValid = boolRtn;
            return boolRtn;
        }
    }
}