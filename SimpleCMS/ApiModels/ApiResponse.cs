using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;

namespace SimpleCMS.ApiModels
{
    /// <summary>
    /// Contains properties and methods used to form an API response. Data property
    /// is of generic type T. In the BaseController it is set as an Object, but for
    /// the ResponseType attribute on a controller method use the appropriate model 
    /// class for Swagger documentation to be created properly. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// HttpStatusCode of response. Setting HttpStatusCode also
        /// adds the string value to Messages list.
        /// </summary>
        private HttpStatusCode _httpStatusCode;
        public HttpStatusCode HttpStatusCode
        {
            get
            {
                return _httpStatusCode; 
            }
            set
            {
                _httpStatusCode = value; 
                if (!Messages.Contains(value.ToString()))
                {
                    Messages.Add(value.ToString());
                }
            }
        }

        /// <summary>
        /// List of error messages for response 
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// List of messages for resonse. First message is always 
        /// the string value of HttpStatusCode.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Data object of generic type T
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Constructor instantiating a List&lt;string&gt; for Errors and
        /// Messages and sets _IsValid to true.
        /// </summary>
        public ApiResponse()
        {
            Errors = new List<string>();

            Messages = new List<string>();
        }

        /// <summary>
        /// Method that adds an error message, sets the httpStatusCode, and 
        /// sets _IsValid to false 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="httpStatusCode"></param>
        public void AddError(string errorMessage, HttpStatusCode httpStatusCode)
        {
            Errors.Add(errorMessage);
            HttpStatusCode = httpStatusCode; 
        }

        /// <summary>
        /// Method that adds an IEnumerable&lt;string&gt; to Errors, sets the httpStatusCode,
        /// and sets _IsValid to false 
        /// </summary>
        /// <param name="errorMessages"></param>
        /// <param name="httpStatusCode"></param>
        public void AddRangeError(IEnumerable<string> errorMessages, HttpStatusCode httpStatusCode)
        {
            Errors.AddRange(errorMessages);
            HttpStatusCode = httpStatusCode;
        }
    }
}