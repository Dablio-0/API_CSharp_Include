using API_C_Sharp.LSharp.HTTP;
using Newtonsoft.Json.Linq;
using System.Net;

namespace API_C_Sharp.Utils
{
    class ResponseUtils
    {

        public static Response JsonSuccessResponse(JObject content)
        {
            var response = new JObject();

            response["error"] = false;
            response["content"] = content;

            return new Response(response.ToString()).JSON().StatusCode(HttpStatusCode.OK);
        }

        public static Response JsonSuccessResponse(JArray content)
        {
            var response = new JObject();

            response["error"] = false;
            response["content"] = content;

            return new Response(response.ToString()).JSON().StatusCode(HttpStatusCode.OK);
        }

        private static Response Error(string message, HttpStatusCode code)
        {
            var response = new JObject();

            response["error"] = true;
            response["content"] = message;

            return new Response(response.ToString()).ERROR().StatusCode(code);
        }

        public static Response Unauthorized(string message)
        {
            return Error(message, HttpStatusCode.Unauthorized);
        }
        public static Response Conflict(string message)
        {
            return Error(message, HttpStatusCode.Conflict);
        }

        public static Response NotFound(string message)
        {
            return Error(message, HttpStatusCode.NotFound);
        }


    }
}
