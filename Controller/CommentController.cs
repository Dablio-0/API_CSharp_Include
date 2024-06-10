using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.Post;
using API_C_Sharp.Model.User;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Controller
{
    public class CommentController
    {
        #region create comment
        public static Response create(Request request, Data data)
        {
            int idAuthor = data.getCurrentUser();

            if (idAuthor == -1)
                return ResponseUtils.Unauthorized("Não há usuários criados.");

            Post post = data.getPostById((int)request.routeParans["idPost"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            JObject bodyCommentJson = (JObject)request.body.GetValue("bodyComment");
            BodyCommentContent bodyComment = new(
                (string)bodyCommentJson.GetValue("text"),
                (string)bodyCommentJson.GetValue("code"),
                (string)bodyCommentJson.GetValue("language"),
                (string)bodyCommentJson.GetValue("image")
            );

            int commentId = data.addComment(idAuthor, post.getId, bodyComment);

            Comment commentCreated = data.getCommentById(commentId);

            post.getCommentList.Add(commentCreated);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                "id:" + commentId + ", " +
                "idAuthor: " + idAuthor + ", " +
                "idPost:" + post.getId + ", " +
            " }"));
        }
        #endregion

        #region Update Comment
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
                    JObject bodyCommentJson = (JObject)request.body.GetValue("bodyComment");
                    BodyCommentContent bodyComment = new(
                        (string)bodyCommentJson.GetValue("text"),
                        (string)bodyCommentJson.GetValue("code"),
                        (string)bodyCommentJson.GetValue("language"),
                        (string)bodyCommentJson.GetValue("image")
                    );

                    comment.bodyComment = bodyComment;
                    comment.setUpdateDate = DateTime.Now;


                    return ResponseUtils.JsonSuccessResponse(new JObject(
                        new JProperty("id", comment.getId),
                        new JProperty("idAuthor", comment.getIdAuthorComment),
                        new JProperty("body", comment.bodyComment.serialize()),
                        new JProperty("date", comment.getDate),
                        new JProperty("updateDate", comment.getUpdateDate)
                        ));

                    //return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                    //      "id:" + comment.getId + ", " +
                    //      "idAuthor: " + comment.getIdAuthorComment + ", " +
                    //      "idPost: " + comment.getIdPost + ", " +
                    //      "bodyComment: " + comment.bodyComment.serialize() + ", " +
                    //      "date: " + comment.getDate + ", " +
                    //      "updateDate: " + comment.getUpdateDate + ", " +
                    //      " }"));
                }
            }

            return ResponseUtils.NotFound("Este comentário não existe na lista de comentários desse post.");
        }
        #endregion

        #region Delete Comment
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

                    //return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                    //                    "id:" + comment.getId + ", " +
                    //                    "idAuthor: " + comment.getIdAuthorComment + ", " +
                    //                    "idPost: " + comment.getIdPost + ", " +
                    //                    "body: " + comment.bodyComment.serialize() + ", " +
                    //                    " }"));

                    return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + comment.getId + "}"));
                }
            }
            return ResponseUtils.NotFound("Comentário não encontrado.");
        }
        #endregion

        #region List Comments by Post
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
        #endregion

        #region Interaction
        public static Response like(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["idPost"]);

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

                data.addCommentLikeByUser((int)request.routeParans["idComment"]);
                comment.getLikesIdUser.Add(user.getId);

                return ResponseUtils.JsonSuccessResponse((JObject.Parse("{" +
                    "idPost:" + post.getId + ", " +
                    "idComment:" + comment.getId + ", " +
                    "likes:" + comment.getLikes + "}")));
            }
            else
            {
                if (!comment.getLikesIdUser.Contains(user.getId))
                    return ResponseUtils.Conflict("Não é possível tirar o like duas vezes ou você não deu like ainda.");

                data.removeCommentLikeByUser((int)request.routeParans["idComment"]);
                comment.getLikesIdUser.Remove(user.getId);

                return ResponseUtils.JsonSuccessResponse((JObject.Parse("{" +
                    "idPost:" + post.getId + ", " +
                    "idComment:" + comment.getId + ", " +
                    "likes:" + comment.getLikes + "}")));
            }
        }
        #endregion
    }
}
