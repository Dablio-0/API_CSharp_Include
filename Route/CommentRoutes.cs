using API_C_Sharp.Controller;
using API_C_Sharp.LSharp;

namespace API_C_Sharp.Route
{
    public class CommentRoutes
    {
        public CommentRoutes(Server app)
        {
            #region CRUD Routes
            app.post("/post/{idPost:int}/comment/publish", CommentController.create);
            app.put("/post/{idPost:int}/comment/{idComment:int}/edit", CommentController.update);
            app.delete("/post/{idPost:int}/comment/{idComment:int}/delete", CommentController.delete);
            #endregion

            #region List Comments by Post
            app.get("/post/{idPost:int}/comment/list", CommentController.listCommentsByPost);
            #endregion

            #region Like Comment
            app.post("/post/{idPost:int}/comment/{idComment:int}/like", CommentController.like);
            #endregion
        }
    }
}
