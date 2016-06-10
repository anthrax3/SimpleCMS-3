using SimpleCMS.AppClasses;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.ModelBinding;

namespace SimpleCMS.Models
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
    }

    public class RequestModel
    {
        [Required]
        public string ApiKey { get; set; }
    }
}