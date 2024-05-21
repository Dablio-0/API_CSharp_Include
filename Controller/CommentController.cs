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
        public static Response create(Request request, Data data)
        {
            int idAuthor = data.getCurrentUser();
            int idPost = (int)request.routeParans.GetValue("idPost");
            string text = (string)request.body.GetValue("text");

            int commentId = data.addComment(idAuthor, idPost, text);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                "id:" + commentId + ", " +
                "idAuthor: " + idAuthor + ", " +
                "idPost:" + idPost + ", " +
            " }"));
        }
        #endregion

        public static Response update(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["idPost"]);
            Comment comment = data.getCommentById((int)request.routeParans["idComment"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            if (comment == null)
                return ResponseUtils.NotFound("Comentário não encontrado.");


            foreach (Comment c in post.getCommentList)
            {
                if (c.getId == comment.getId)
                {
                    string text = request.body.GetValue("text").ToString();
                    comment.text = text;

                    return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                                               "id:" + comment.getId + ", " +
                                               "idAuthor: " + comment.getIdAuthorComment + ", " +
                                               "idPost: " + comment.getIdPost + ", " +
                                               "text: " + comment.text + ", " +
                                               " }"));
                }
            }

            return ResponseUtils.NotFound("Este comentário não existe na lista de comentários desse post.");
        }


        public static Response delete(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["idPost"]);
            Comment comment = data.getCommentById((int)request.routeParans["idComment"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            if (comment == null)
                return ResponseUtils.NotFound("Comentário não encontrado.");

            foreach (Comment c in post.getCommentList)
            {
                if (c.getId == comment.getId)
                {
                    post.getCommentList.Remove(c);
                    return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                                        "id:" + comment.getId + ", " +
                                        "idAuthor: " + comment.getIdAuthorComment + ", " +
                                        "idPost: " + comment.getIdPost + ", " +
                                        "text: " + comment.text + ", " +
                                        " }"));
                }
            }
            return ResponseUtils.NotFound("Comentário não encontrado.");
        }

        public static Response listCommentsByPost(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["idPost"]);

            if (post == null)
            {
                return ResponseUtils.NotFound("Post não encontrado.");
            }

            List<Comment> comments = new List<Comment>();
            foreach (Comment comment in data.getAllComments())
            {
                if (comment.getIdPost == post.getId)
                    comments.Add(comment);
            }

            JArray commentListByPost = new();
            foreach (Comment comment in comments)
                commentListByPost.Add(comment.serialize());

            return ResponseUtils.JsonSuccessResponse(commentListByPost);
        }

        public static Response like(Request request, Data data)
        {
            Post post = data.addPostLikeByUser((int)request.routeParans["idPost"]);
            Comment comment = data.getCommentById((int)request.routeParans["idComment"]);
            return new Response();
        }
    }
}
