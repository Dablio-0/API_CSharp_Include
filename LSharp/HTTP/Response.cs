using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace API_C_Sharp.LSharp.HTTP
{

    public enum Type
    {
        JSON, Text, ERROR
    };

    public class Response
    {
        public Type type;
        public byte[] content = [];
        public HttpStatusCode statusCode = HttpStatusCode.OK;

        public Response()
        {

        }

        public Response(string content)
        {
            this.content = Encoding.UTF8.GetBytes(content);
        }

        public Response(JObject content)
        {
            this.content = Encoding.UTF8.GetBytes(content.ToString());
        }

        public Response Content(string content)
        {
            this.content = Encoding.UTF8.GetBytes(content);
            return this;
        }

        public Response Content(JObject content)
        {
            this.content = Encoding.UTF8.GetBytes(content.ToString());
            return this;
        }

        public Response JSON()
        {
            type = Type.JSON;
            return this;
        }
        public Response Text()
        {
            type = Type.Text;
            return this;
        }
        public Response ERROR()
        {
            type = Type.ERROR;
            return this;
        }

        public Response StatusCode(HttpStatusCode statusCode)
        {
            this.statusCode = statusCode;
            return this;
        }
    }
}
