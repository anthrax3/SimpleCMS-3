using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace SimpleCMS.ApiClasses
{
    public static class ModelStateAssistant
    {
        /// <summary>
        /// Returns list of errors from an invalid ModelState 
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetModelStateErrors(this ModelStateDictionary modelState)
        {
            var result = new List<string>();
            foreach (var error in modelState.Select(e => e.Value.Errors))
            {
                if (error == null) continue;
                var firstOrDefault = error.Select(e => e.ErrorMessage).FirstOrDefault();
                if (firstOrDefault == null) continue;
                result.Add(firstOrDefault);
            }
            return result;
        }
    }

}