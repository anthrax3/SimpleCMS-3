using SimpleCMS.ApiClasses;
using System;

namespace SimpleCMS.ApiModels
{
    public interface IRequestModel
    {
        string ApiKey { get; set; }
    }

    public interface IUserRequestModel
    {
        string Username { get; set; }

        Guid UserID { get; set; }

        string Portal { get; set; }

        bool Validate(BaseApiController controller);
    }
}
