using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.Post;
using API_C_Sharp.Model.User;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_C_Sharp.Controller
{
    public class CommentController
    {
        #region create comment
        public static void create(Request request, Data data)
        {
            int idAuthor = data.getCurrentUser();
            int idPost = (int)request.parameters.GetValue("idPost");
            string text = (string)request.body.GetValue("text");

            int commentId = data.addComment(idAuthor, idPost, text);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                "id:" + commentId + ", " +
                "idAuthor: " + idAuthor + ", " +
                "idPost: " + idPost + ", " +
                "text: " + text + ", " +
            " }"));        
        }
        #endregion

        public static Response update(Request request, Data data)
        {
            // Update a comment
            return new Response();
        }

        public static Response delete(Request request, Data data)
        {
            // Delete a comment
            return new Response();
        }

        public static Response listCommentsByPost(Request request, Data data)
        {
            // List all comments from a post
            return new Response();
        }

        public static Response like(Request request, Data data)
        {
            // Like a comment
            return new Response();
        }
    }
}
