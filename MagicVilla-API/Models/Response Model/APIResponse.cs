using System.Net;

namespace MagicVilla_API.Models
{
    /// <summary>
    /// THIS CLASS IS A DEFAULT RETURN-TYPE FOR OUR API
    /// 
    /// Typically API will return One standard response
    /// That standard response contains multiple properties Like
    /// * what is the model?
    /// * Status
    /// * Are there any error
    /// * Result and more
    /// </summary>
    public class APIResponse
    {
        // This returns the statuscode of the API (Ex. 201,404,etc)
        public HttpStatusCode StatusCode { get; set; }

        // This returns the status of API like successful or unsuccessful
        public bool isSuccess { get; set; }

        // If the code is unsuccessful, What are the error messages
        public List<string> ErrorMessages { get; set; }

        // This contains the result
        public object Result { get; set; }
    }
}
