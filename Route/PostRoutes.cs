using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_C_Sharp.Controller;
using API_C_Sharp.LSharp;

namespace API_C_Sharp.Route
{
    public class PostRoutes
    {
        public PostRoutes(Server app)
        {
            app.post("/post/publish", PostController.create);
            app.get("/feed", PostController.feed);
            app.get("/post/list", PostController.list);
            //app.get("/post/{id:int}", PostController.getPostById);
        }
    }
}
