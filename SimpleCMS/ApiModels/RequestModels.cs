using SimpleCMS.AppClasses;
using SimpleCMS.Models;
using SimpleCMS.ApiClasses;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http.ModelBinding;

namespace SimpleCMS.ApiModels
{
    #region BaseRequestModel
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
        /// Validates an apiKey based on the referrer URL 
        /// </summary
        internal virtual bool ValidateRequest(BaseApiController controller)
        {
            var boolRtn = false;
            var validApiKey = controller._db.ApiAccounts.Any(a => a.ApiKey.Equals(ApiKey));
            var referrerForKey = controller._db.ApiAccounts.FirstOrDefault(a => a.ApiKey.Equals(ApiKey)).RequestURL.ToString();

            if ((referrerForKey == "*" ||
                string.Equals(referrerForKey, controller.ApiRequest.Url.Authority, StringComparison.CurrentCultureIgnoreCase))
                && validApiKey)
            {
                boolRtn = true;
            }
            _IsValid = boolRtn;
            return boolRtn;
        }
    }
    #endregion

    #region UserRequestModels
    public class UserRequestModel : IUserRequestModel
    {
        public string Username { get; set; }

        public Guid UserID { get; set; }

        public string Portal { get; set; }

        public UserRequestModel()
        {
            this.Username = string.Empty;
            this.Portal = "any";
        }

        public UserRequestModel(string username, string portal = "any")
        {
            this.Username = username;
            this.Portal = "any";
            var user = ApplicationUserManager.GetUser(username);
            if (user != null)
            {
                this.UserID = Guid.Parse(user.Id);
            }
        }

        public UserRequestModel(string username, Guid userID, string portal = "any")
        {
            this.Username = username;
            this.UserID = userID;
            this.Portal = portal;
        }

        public bool Validate(BaseApiController controller)
        {
            var boolRtn = true;
            // only validate the username if the property is set 
            if (!string.IsNullOrEmpty(Username))
            {
                // make sure username exists 
                if (!controller._db.Users.Any(u => u.UserName == Username))
                {
                    controller.ApiResponse.AddError(ErrorMessages.UsernameNotFound(Username), HttpStatusCode.OK);
                    boolRtn = false;
                }
            }

            return boolRtn;
        }

    }

    [ModelBinder(typeof(ApiModelBinderProvider<NewUserRequestModel>))]
    public class NewUserRequestModel : RequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    [ModelBinder(typeof(ApiModelBinderProvider<UserPostsRequestModel>))]
    public class UserPostsRequestModel : RequestModel
    {
        private string _username = null; 

        [Required]
        public string Username
        {
            get { return this._username; }

            set
            {
                this._username = value;
                if (this._username != null)
                {
                    this.UserRequest = new UserRequestModel(this._username); 
                }
            }
        }

        internal IUserRequestModel UserRequest { get; set; }

        internal override bool ValidateRequest(BaseApiController controller)
        {
            var boolRtn = base.ValidateRequest(controller);
            if (boolRtn)
            {
                boolRtn = UserRequest.Validate(controller);
            }
            return boolRtn;
        }
    }
    #endregion

    #region PostsRequestModels
    [ModelBinder(typeof(ApiModelBinderProvider<PostRequestModel>))]
    public class PostRequestModel : RequestModel
    {
        [Required]
        public Posts Post { get; set; }
    }

    [ModelBinder(typeof(ApiModelBinderProvider<AllPostRequestModel>))]
    public class AllPostRequestModel : RequestModel
    {
        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override bool ValidateRequest(BaseApiController controller)
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
            boolRtn = base.ValidateRequest(controller);
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
            _IsValid = boolRtn;
            return boolRtn;
        }
    }
    #endregion

}