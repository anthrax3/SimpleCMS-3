using SimpleCMS.AppClasses;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.ModelBinding;

namespace SimpleCMS.Models
{
    [ModelBinder(typeof(ApiModelBinderProvider<PostRequestModel>))]
    public class PostRequestModel
    {
        [Required]
        public string ApiKey { get; set; }

        [Required]
        public Posts Post { get; set; }
    }

    [ModelBinder(typeof(ApiModelBinderProvider<ByUserRequestModel>))]
    public class ByUserRequestModel
    {
        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string Username { get; set; }
    }
}