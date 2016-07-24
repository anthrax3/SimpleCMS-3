using System.Net;
using System.Web.Http;
using SimpleCMS.ApiClasses;
using SimpleCMS.ApiModels;
using SimpleCMS.Models;
using System.Threading.Tasks;

namespace SimpleCMS.ApiControllers
{
    [AllowAnonymous]
    public class PublicAccountController : BaseApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Create(NewUserRequestModel userRequest)
        {
            if (!ModelState.IsValid)
            {
                ApiResponse.AddRangeError(ModelState.GetModelStateErrors(), HttpStatusCode.BadRequest);
                userRequest._IsValid = false;
            }

            if (userRequest._IsValid && userRequest.ValidateRequest(this))
            {
                var user = new ApplicationUser { UserName = userRequest.Username, Email = userRequest.Email };
                var result = await UserManager.CreateAsync(user, userRequest.Password);
                if (result.Succeeded)
                {
                    ApiResponse.HttpStatusCode = HttpStatusCode.Created;
                }
                else
                {
                    ApiResponse.HttpStatusCode = HttpStatusCode.NotImplemented;
                    ApiResponse.Errors.AddRange(result.Errors);
                }
            }

            return Content(ApiResponse.HttpStatusCode, ApiResponse);
        }
    }
}
