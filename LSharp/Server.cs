using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using System.Net;

namespace API_C_Sharp.LSharp
{
    public class Server
    {
        private string _url;
        private HttpListener _listener = new HttpListener();
        private List<Route> _routeList = new List<Route>();
        private Data _dataInstance;

        public Server(int port, string url = "localhost")
        {
            _url = "http://" + url + ":" + port + "/";
        }

        public async void Start(Data dataInstance)
        {
            _dataInstance = dataInstance;
            _listener.Prefixes.Add(_url);
            _listener.Start();

            while (_listener.IsListening)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse listenerResponse = context.Response;

                if (request.HttpMethod == "OPTIONS")
                {
                    listenerResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                    listenerResponse.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                    listenerResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
                    listenerResponse.StatusCode = 200;
                    listenerResponse.Close();
                }
                else
                {
                    Route route = _routeList.Find(route => route.check(request));

                    if (route == null)
                    {
                        listenerResponse.StatusCode = (int)HttpStatusCode.NotFound;
                        listenerResponse.Close();
                    }
                    else
                    {
                        Response response = route.make(request, _dataInstance);

                        switch (response.type)
                        {
                            case HTTP.Type.JSON:
                                listenerResponse.ContentType = "application/json";
                                break;
                            case HTTP.Type.Text:
                                listenerResponse.ContentType = "text/html";
                                break;
                            case HTTP.Type.ERROR:
                                listenerResponse.ContentType = "application/json";
                                break;
                        }

                        listenerResponse.StatusCode = (int)response.statusCode;

                        byte[] buffer = response.content;

                        listenerResponse.Headers.Add("Access-Control-Allow-Origin", "*");

                        listenerResponse.ContentLength64 = buffer.Length;
                        Stream output = listenerResponse.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                    }
                }
            }
        }

        public void Close()
        {
            _listener.Close();
        }

        private void addRoute(HTTP.Method method, string path, ICallback callback)
        {
            _routeList.Add(new Route(method, path, callback));
        }

        public void get(string path, ICallback callback)
        {
            this.addRoute(HTTP.Method.GET, path, callback);
        }

        public void post(string path, ICallback callback)
        {
            this.addRoute(HTTP.Method.POST, path, callback);
        }

        public void put(string path, ICallback callback)
        {
            this.addRoute(HTTP.Method.PUT, path, callback);
        }

        public void delete(string path, ICallback callback)
        {
            this.addRoute(HTTP.Method.DELETE, path, callback);
        }


    }
}