using SimpleCMS.AppClasses;
using SimpleCMS.Models;
using SimpleCMS.ApiClasses;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http.ModelBinding;
using System.Collections.Generic;

namespace SimpleCMS.ApiModels
{
    #region BaseRequestModel
    /// <summary>
    /// Base Reqeust Model that contains ApiKey, _IsValid, and a virtual Validate
    /// Request that validates ModelState, apiKey, and referrer.
    /// </summary>
    public class RequestModel : IRequestModel
    {
        [Required]
        public string ApiKey { get; set; }

        internal bool _IsValid { get; set; }

        public RequestModel()
        {
            _IsValid = true;
        }

        /// <summary>
        /// Validates the ModelState, apikey and referrer url and adds errors / messages 
        /// to controller.ApiResponse as needed 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        internal virtual bool ValidateRequest(BaseApiController controller, ModelStateDictionary modelState)
        {
            var boolRtn = false;
            if (!modelState.IsValid)
            {
                controller.ApiResponse.AddRangeError(modelState.GetModelStateErrors(), HttpStatusCode.BadRequest);
                _IsValid = false;
            }
            var apiAccount = controller._db.ApiAccounts.FirstOrDefault(a => a.ApiKey.Equals(ApiKey));
            var referrerForKey = string.Empty;
            if (apiAccount != null)
            {
                referrerForKey =  apiAccount.RequestURL.ToString(); 
            }

            if (_IsValid && 
                ((referrerForKey == "*" ||
                string.Equals(referrerForKey, controller.ApiRequest.Url.Authority, StringComparison.CurrentCultureIgnoreCase))))
            {
                boolRtn = true;
            }
            _IsValid = boolRtn;
            return boolRtn;
        }
    }
    #endregion

    #region UserRequestModels
    /// <summary>
    /// UserRequestModel extends RequestModel and contains Username, UserID. Username or 
    /// UserId are required and are validate with ValidateResult (along with apikey and
    /// referer); 
    /// </summary>
    [ModelBinder(typeof (ApiModelBinderProvider<UserRequestModel>))]
    public class UserRequestModel : RequestModel, IUserRequestModel, IValidatableObject
    {
        public string Username { get; set; }

        public Guid UserID { get; set; }

        /// <summary>
        /// Validates that Username or UserId are set 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Username) || UserID.Equals(Guid.Empty))
            {
                yield return new ValidationResult(ErrorMessages.UserNameAndUserIdEmpty);
            }
        }

        /// <summary>
        /// Validates ModelState, username, apikey, and referrer and adds errors / messages 
        /// to controller.ApiResponse as needed 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        internal override bool ValidateRequest(BaseApiController controller, ModelStateDictionary modelState)
        {
            var boolRtn = base.ValidateRequest(controller, modelState);
            // only validate the username if base request (apikey and referrer) are valid 
            if (boolRtn)
            {
                // make sure username exists 
                if (string.IsNullOrEmpty(Username) || !controller._db.Users.Any(u => u.UserName == Username))
                {
                    controller.ApiResponse.AddError(ErrorMessages.UsernameNotFound(Username), HttpStatusCode.OK);
                    boolRtn = false;
                }
            }

            return boolRtn;
        }

    }

    /// <summary>
    /// Used for creating a user (contains Username, Email, and Password)
    /// </summary>
    [ModelBinder(typeof(ApiModelBinderProvider<CreateUserRequestModel>))]
    public class CreateUserRequestModel : RequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
    #endregion

    #region PostsRequestModels
    [ModelBinder(typeof(ApiModelBinderProvider<PostRequestModel>))]
    public class PostRequestModel : RequestModel
    {
        [Required]
        public Posts Post { get; set; }

        internal override bool ValidateRequest(BaseApiController controller, ModelStateDictionary modelState)
        {
            var boolRtn = base.ValidateRequest(controller, modelState);

            if (boolRtn)
            {
                if (controller._db.Posts.All(p => p.ID != Post.ID))
                {
                    controller.ApiResponse.Messages.Add(ErrorMessages.PostNotFound(Post.ID));
                    controller.ApiResponse.HttpStatusCode = HttpStatusCode.OK;
                    boolRtn = false;
                }
            }

            return boolRtn;
        }
    }

    /// <summary>
    /// Request model used for returning all posts. Has optional parameters of 
    /// PageNumber and PageSize used for pagination.
    /// </summary>
    [ModelBinder(typeof(ApiModelBinderProvider<AllPostRequestModel>))]
    public class AllPostRequestModel : RequestModel
    {
        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        /// <summary>
        /// Validates ModelState, apikey, referrer, pageNumber, and pageSize and adds errors / messages 
        /// to controller.ApiResponse as needed 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        internal override bool ValidateRequest(BaseApiController controller, ModelStateDictionary modelState)
        {
            var boolRtn = base.ValidateRequest(controller, modelState); ;
           
            // only continue if base request (ModelState, apikey, referrer) is valid 
            if (boolRtn)
            {
                // set defaults 
                PageNumber = PageNumber == null ? 1 : PageNumber;
                PageSize = PageSize == null ? 5 : PageSize;
                
                // make sure nothing set to zero (prevent divide by zero errors down the line) 
                if (PageNumber == 0)
                {
                    controller.ApiResponse.AddError("PageNumber cannot be zero", HttpStatusCode.BadRequest);
                    boolRtn = false;
                }
                if (PageSize == 0)
                {
                    controller.ApiResponse.AddError("PageSize cannot be zero", HttpStatusCode.BadRequest);
                    boolRtn = false;
                }
            }
            _IsValid = boolRtn;
            return boolRtn;
        }
    }
    #endregion

}