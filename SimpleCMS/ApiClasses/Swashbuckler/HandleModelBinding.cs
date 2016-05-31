using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace SimpleCMS.AppClasses.Swashbuckler
{
    /// <summary>
    /// Since Swashbuckle does not play nicely with model binding, and assumes all requests parameters are 
    /// query string params unless [FromBody] is specified, the request parameters in an object that will be converted 
    /// to query string params (in the api documentation) unless handled by this class 
    /// </summary>
    public class HandleModelBinding : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters != null && operation.parameters.Count > 0 && !string.IsNullOrEmpty(operation.description))
            {
                string paramObjectName;
                Schema modelBinderSchema;
                Parameter modelBinderParam = null;
                if (operation.description.Contains("PostRequestModel"))
                {
                    modelBinderSchema = new Schema()
                    {
                        @ref = "#/definitions/PostRequestModel"
                    };
                    paramObjectName = operation.parameters[0].name.Substring(0, operation.parameters[0].name.IndexOf("."));
                    modelBinderParam = new Parameter()
                    {
                        description = "Post id (type positive int) and apiKey are required.",
                        @in = "body",
                        name = paramObjectName,
                        @ref = null,
                        required = true,
                        schema = modelBinderSchema
                    };
                }
                else if (operation.description.Contains("ByUserRequestModel"))
                {
                    modelBinderSchema = new Schema()
                    {
                        @ref = "#/definitions/ByUserRequestModel"
                    };
                    paramObjectName = operation.parameters[0].name.Substring(0, operation.parameters[0].name.IndexOf("."));
                    modelBinderParam = new Parameter()
                    {
                        description = "",
                        @in = "body",
                        name = paramObjectName,
                        @ref = null,
                        required = true,
                        schema = modelBinderSchema
                    };
                }

                // replace the list of each individual object property parameters
                // with one parameter that is the model binder object 
                if (modelBinderParam != null)
                {
                    operation.parameters = new List<Parameter>()
                    {
                        modelBinderParam
                    };
                }
            } // end if operation.parameters != null
        }
    }
}