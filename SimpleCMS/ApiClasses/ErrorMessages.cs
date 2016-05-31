using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleCMS.AppClasses
{
    public class ErrorMessages
    {
        /// <summary>
        /// Invalid API key.
        /// </summary>
        public static string InvalidApiKey = "Invalid API key.";

        /// <summary>
        /// User {0} does not exist.
        /// </summary>
        public static Func<string, string> UsernameNotFound = 
            (username) => $"User {username} does not exist.";
        

        /// <summary>
        /// Post with id {0} does not exist.
        /// </summary>
        public static Func<int, string> PostNotFound = 
            (postID) => $"Post with id {postID} does not exist.";
        

        /// <summary>
        ///  Could not deserialize object of type {0} to bind to model. Verify request object is formed correctly.
        /// </summary>
        public static Func<Type, string> CouldNotBindModel =
            (modelType) =>
                $"Could not deserialize object of type {modelType} to bind to model. Verify request object is formed correctly.";

    }
}