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
        public async Task<IHttpActionResult> Create(CreateUserRequestModel userRequest)
        {
            if (userRequest.ValidateRequest(this, ModelState))
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

            return ResponseContent(ApiResponse);
        }
    }
}
