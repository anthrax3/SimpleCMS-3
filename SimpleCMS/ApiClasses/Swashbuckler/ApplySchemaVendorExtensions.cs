using SimpleCMS.AppClasses;
using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Swagger;

//namespace SimpleCMS.AppClasses.Swashbuckler
//{
//    /// <summary>
//    /// Define default values for ApiResponse&lt;T&gt; used in Swagger UI
//    /// </summary>
//    public class ApplySchemaVendorExtensions : ISchemaFilter 
//    {
//        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
//        {
//        }
//    }

//    public static class ObjectAssistant
//    {
//        public static object ToCamelCaseObject(this object obj)
//        {
//            var jsonSettings = new JsonSerializerSettings()
//            {
//                ContractResolver = new CamelCasePropertyNamesContractResolver(),
//                NullValueHandling = NullValueHandling.Include
//            };
//            var json = JsonConvert.SerializeObject(obj, jsonSettings);
//            return JsonConvert.DeserializeObject(json);
//        }
//    }
//}