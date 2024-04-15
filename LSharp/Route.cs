using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace API_C_Sharp.LSharp
{
    public delegate Response ICallback(Request request, Data dataInstance);

    public class Route
    {
        private HTTP.Method _method;
        private ICallback _callback;
        private string _routePathPatternInRegex;
        private string _routePathPattern;

        public Route(HTTP.Method method, string path, ICallback callback)
        {
            _method = method;
            _callback = callback;
            _routePathPattern = path;
            string pattern = @"\{[^{}]+:([^{}]+)\}";
            string replacement = @"[^/]+";
            string regexPattern = $"^{Regex.Replace(path, pattern, replacement)}$";
            _routePathPatternInRegex = regexPattern;
        }

        public string method
        {
            get
            {
                switch (_method)
                {
                    case HTTP.Method.GET:
                        return "GET";
                    case HTTP.Method.POST:
                        return "POST";
                    case HTTP.Method.PUT:
                        return "PUT";
                    case HTTP.Method.DELETE:
                        return "DELETE";
                }

                return "";
            }
        }

        public Boolean pathIsEquals(String path)
        {
            path = path.Trim('?').Split('?')[0];

            Match match = Regex.Match(path, _routePathPatternInRegex);

            if (match.Success)
            {
                string[] partsRoute = _routePathPattern.Trim('/').Split('/');
                string[] partsPath = path.Trim('/').Split('/');

                if (partsRoute.Length != partsPath.Length)
                {
                    return false;
                }

                for (int i = 0; i < partsRoute.Length; i++)
                {
                    string partRoute = partsRoute[i];
                    string partPath = partsPath[i];

                    if (partRoute.StartsWith("{") && partRoute.EndsWith("}"))
                    {
                        string type = partRoute.Split(':')[1].TrimEnd('}');
                        string value = partPath;

                        if (type == "int")
                        {
                            return int.TryParse(value, out _);
                        }
                        else if (type == "str")
                        {
                            return !string.IsNullOrEmpty(value);
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public Boolean check(HttpListenerRequest request)
        {
            return request.HttpMethod == this.method && this.pathIsEquals(request.RawUrl ?? "");
        }

        public JObject getRouteParans(String path)
        {
            JObject routeParans = new JObject();

            string[] partsRoute = _routePathPattern.Trim('/').Split('/');
            string[] partsPath = path.Trim('/').Split('/');

            if (partsRoute.Length != partsPath.Length)
                throw new Exception("Route paths not compatible.");

            for (int i = 0; i < partsRoute.Length; i++)
            {
                string partRoute = partsRoute[i];
                string partPath = partsPath[i];

                if (partRoute.StartsWith("{") && partRoute.EndsWith("}"))
                {
                    string name = partRoute.Split(':')[0].TrimStart('{');
                    string type = partRoute.Split(':')[1].TrimEnd('}');
                    string value = partPath;


                    if (type == "int" && int.TryParse(value, out _))
                        routeParans[name] = int.Parse(value);
                    else if (type == "str" && !string.IsNullOrEmpty(value))
                        routeParans[name] = value;
                }
            }

            return routeParans;
        }

        public Response make(HttpListenerRequest request, Data dataInstance)
        {
            Request requestInstance = new Request(request);

            requestInstance.method = _method;
            requestInstance.path = _routePathPattern;
            requestInstance.routeParans = getRouteParans(request.RawUrl ?? "");

            requestInstance.bodyDecoding();
            requestInstance.parameterDecoding();

            return _callback(requestInstance, dataInstance);
        }
    }


}

