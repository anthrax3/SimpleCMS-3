using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SimpleCMS.AppClasses
{
    /// <summary>
    /// Generic Model Binder Provider for binding parameters in a Model (generic type T) to WebAPI Controller methods.
    /// To bind a Model to a controller method add the data annotation [ModelBinder(typeof(ApiModelBinderProvider&lt;T&gt;))] above the 
    /// class's decleration or in front of the method parameter (where T is the type of the Model Class you are attampting to bind). 
    /// </summary>
    /// <typeparam name="T">Type parameter of Model Class that you are attempting to bind</typeparam>
    public class ApiModelBinderProvider<T> : ModelBinderProvider where T : new()
    {
        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            return new ApiModelBinder<T>();
        }
    }

    /// <summary>
    /// Generic Model Binder called by ApiModelBinderProvider&lt;T&gt; used to pass model paramaters to a WebAPI Controller method.
    /// Generic type T of ApiModelBinder must be the same as the generic type used in ApiModelBinderProvider.  
    /// </summary>
    /// <typeparam name="T">Type parameter of Modal Class that you area attempting to bind</typeparam>
    public class ApiModelBinder<T> : IModelBinder where T : new()
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var boolRtn = true;
            // create object of type T 
            var modelObject = GetObject();
            // get object of type T properties
            var modelPorperties = modelObject.GetType().GetProperties() as IEnumerable<PropertyInfo>;

            // read content from request 
            var requestContent = actionContext.Request.Content.ReadAsStringAsync().Result;
            // if request body doesn't have any content try to get parameters from URL 
            if (string.IsNullOrEmpty(requestContent))
            {
                // get the requested URL 
                var requestURL = actionContext.Request.RequestUri.ToString();
                // take out part of the URL that we don't need 
                requestURL = requestURL.Substring(requestURL.IndexOf('?') + 1);

                try
                {
                    // loop through params in request and attempt to set their corresponding property value 
                    var requestParams = requestURL.Split('&');
                    foreach (var param in requestParams)
                    {
                        var paramProp = string.Empty;
                        // get parameter name 
                        if (param.Substring(0, param.IndexOf('=')).Contains('.'))
                        {
                            // param is of format T.paramName=paramValue 
                            paramProp = HttpUtility.UrlDecode(param.Substring(param.IndexOf('.') + 1, param.IndexOf('=') - param.IndexOf('.') - 1));
                        }
                        else
                        {
                            // param is of format paramName=paramValue
                            paramProp = HttpUtility.UrlDecode(param.Substring(0, param.IndexOf('=') - 1));
                        }
                        // get property in object type T that equals the parameter name 
                        var propToSet = modelPorperties.FirstOrDefault(p => string.Equals(p.Name, paramProp, StringComparison.CurrentCultureIgnoreCase));
                        // set property value if it exists 
                        if (propToSet != null)
                        {
                            var paramValue = HttpUtility.UrlDecode(param.Substring(param.IndexOf('=') + 1));
                            if (!string.IsNullOrEmpty(paramValue))
                            {
                                if (paramValue.GetType() == propToSet.PropertyType)
                                {
                                    propToSet.SetValue(modelObject, paramValue);
                                }
                                else
                                {
                                    var newTypeVal = Convert.ChangeType(paramValue, propToSet.PropertyType);
                                    propToSet.SetValue(modelObject, newTypeVal);
                                }
                            }
                        }
                    } // end foreach param in requestParams 
                }
                catch (Exception ex)
                {
                    boolRtn = false;
                }
            }
            else // requestContent not null. Create Model object from JSON 
            {
                try
                {
                    modelObject = JsonConvert.DeserializeObject<T>(requestContent);
                }
                catch (JsonReaderException ex)
                {
                    boolRtn = false;
                }
            }

            // only bind the model if an exception was not thrown 
            if (boolRtn)
            {
                bindingContext.Model = modelObject;
            }

            // Validate Model and update bindingContext.ModelState appropriately
            var modelStateErrors = ValidateRequestModel(bindingContext.Model);
            if (modelStateErrors.Any())
            {
                foreach (var modelError in modelStateErrors)
                {
                    bindingContext.ModelState.AddModelError(string.Empty, modelError.ErrorMessage);
                }
            }

            return boolRtn;
        }

        /// <summary>
        /// Returns object of generic type T
        /// </summary>
        /// <returns></returns>
        public static T GetObject()
        {
            return new T();
        }

        /// <summary>
        /// Validates a model according to the DataAnnotations on the model. If there are errors 
        /// they are returned in an IList&lt;ValidationResult&gt;
        /// </summary>
        /// <param name="model"></param>
        public static IList<ValidationResult> ValidateRequestModel(object model)
        {
            var modelValidationResult = new List<ValidationResult>();
            if (model != null)
            {
                var modelValidationContext = new ValidationContext(model, null, null);

                Validator.TryValidateObject(model, modelValidationContext, modelValidationResult);
            }
            else
            {
                modelValidationResult.Add(
                    new ValidationResult(ErrorMessages.CouldNotBindModel(typeof(T)))
                );
            }

            return modelValidationResult;
        }
    }
}