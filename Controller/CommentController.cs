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

            Comment commentCreated = data.getCommentById(commentId);

            Post post = data.getPostById(idPost);

            post.getCommentList.Add(commentCreated);

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

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            Comment comment = data.getCommentById((int)request.routeParans["idComment"]);

            if (comment == null)
                return ResponseUtils.NotFound("Comentário não encontrado.");

            User user = data.getUserById(data.getCurrentUser());

            string statusLikeString = (string)request.body.GetValue("statusLike");
            bool statusLike = false;
            bool.TryParse(statusLikeString, out statusLike);

            if (statusLike)
            {
                if (comment.getLikesIdUser.Contains(user.getId))
                    return ResponseUtils.Conflict("Você já curtiu esse comentário.");

                data.addCommentLikeByUser((int)request.routeParans["idPost"]);
                comment.getLikesIdUser.Add(user.getId);

                return ResponseUtils.JsonSuccessResponse((JObject.Parse("{" +
                    "idPost:" + post.getId + ", " +
                    "idComment:" + comment.getId + ", " +
                    "likes:" + comment.getLikes + "}")));
            }
            else
            {
                data.removePostLikeByUser((int)request.routeParans["idPost"]);
                comment.getLikesIdUser.Remove(user.getId);

                return ResponseUtils.JsonSuccessResponse((JObject.Parse("{" +
                    "idPost:" + post.getId + ", " +
                    "idComment:" + comment.getId + ", " +
                    "likes:" + comment.getLikes + "}")));
            }
        }
    }
}
