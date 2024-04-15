using Newtonsoft.Json.Linq;
using System.Net;

namespace API_C_Sharp.LSharp.HTTP
{
    public class Request
    {
        private HttpListenerRequest _request;

        public JObject routeParans;
        public Method method;
        public string path;

        public JObject parameters;
        public JObject body;

        public Request(HttpListenerRequest request)
        {

            routeParans = new JObject();
            path = "";
            body = new JObject();
            parameters = new JObject();
            body = new JObject();


            _request = request;
        }

        public void bodyDecoding()
        {
            body = new();
            if (method != Method.POST && method != Method.PUT)
                return;

            StreamReader reader = new(_request.InputStream, _request.ContentEncoding);
            body = JObject.Parse(reader.ReadToEnd());
            reader.Close();
        }

        public void parameterDecoding()
        {
            string queryParamsString = _request.Url.Query;
            if (string.IsNullOrEmpty(queryParamsString))
            {
                parameters = new();
                return;
            }

            queryParamsString = queryParamsString.Substring(1);

            string[] queryParamsArray = queryParamsString.Split('&');
            JObject queryParams = new();

            foreach (string pair in queryParamsArray)
            {
                string[] keyValue = pair.Split('=');
                string key = keyValue[0];
                string value = keyValue.Length > 1 ? keyValue[1] : "";
                queryParams[key] = value;
            }

            parameters = queryParams;
        }
    }
}
