using SimpleCMS.ApiClasses;
using SimpleCMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
