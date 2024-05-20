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
            #region CRUD Post Routes
            app.post("/post/publish", PostController.create);
            app.put("/post/{idPost:int}/edit", PostController.update);
            app.delete("/post/{idPost:int}/delete", PostController.delete);
            #endregion

            #region List Comments (Feed)
            app.get("/feed", PostController.feed);
            #endregion

            #region Like Post
            app.post("/feed/post/{idPost:int}/like", PostController.like);
            #endregion
        }
    }
}
