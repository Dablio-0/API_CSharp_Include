using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.Post;
using API_C_Sharp.Model.User;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_C_Sharp.Controller
{
    public class PostController
    {
        #region Publish Post
        public static Response create(Request request, Data data)
        {
            int idAuthor = data.getCurrentUser();

            if (idAuthor == -1)
                return ResponseUtils.Unauthorized("Não há usuários criados.");

            string title = (string)request.body.GetValue("title");

            JObject bodyJson = (JObject)request.body.GetValue("body");

            BodyContent body = new(
                (string)bodyJson.GetValue("text"),
                (string)bodyJson.GetValue("code"),
                (string)bodyJson.GetValue("language"),
                (string)bodyJson.GetValue("image")
            );

            int postId = data.addPost(idAuthor, title, body);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + postId + ", idAuthor: " + idAuthor + " }"));
        }
        #endregion

        #region Update Post
        public static Response update(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["idPost"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            string title = (string)request.body.GetValue("title");

            JObject bodyJson = (JObject)request.body.GetValue("body");
            BodyContent body = new(
                (string)bodyJson.GetValue("text"),
                (string)bodyJson.GetValue("code"),
                (string)bodyJson.GetValue("language"),
                (string)bodyJson.GetValue("image")
            );

            JArray images = (JArray)request.body.GetValue("images");

            if (images != null)
            {
                List<string> imagesList = new();

                foreach (JToken image in images)
                    imagesList.Add((string)image);

                post.setImageList = imagesList;
            }

            post.title = title;
            post.body = body;
            post.setUpdateDate = DateTime.Now;

            return ResponseUtils.JsonSuccessResponse(new JObject(
                new JProperty("id", post.getId),
                new JProperty("idAuthor", post.getIdAuthor),
                new JProperty("title", post.title),
                new JProperty("body", post.body.serialize()),
                new JProperty("date", post.getDate),
                new JProperty("updateDate", post.getUpdateDate)
                ));
        }
        #endregion

        #region Delete Post
        public static Response delete(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["idPost"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            foreach (Comment c in post.getCommentList)
            {
                data.deleteComment(c.getId);
            }

            data.deletePost(post.getId);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + post.getId + "}"));
        }
        #endregion

        #region Feed of Social Network
        public static Response feed(Request request, Data data)
        {
            Console.WriteLine(data.getPosts());

            JArray postList = new();
            foreach (Post post in data.getPosts())
                postList.Add(post.serialize());

            if (postList.Count == 0)
                return ResponseUtils.Conflict("Não há posts.");

            // Reorder the list of posts to show the most recent first
            JArray postListInverted = new();
            for (int i = postList.Count - 1; i >= 0; i--)
                postListInverted.Add(postList[i]);

            return ResponseUtils.JsonSuccessResponse(postListInverted);
        }
        #endregion

        #region View a unique post
        public static Response getPostById(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["idPost"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            return ResponseUtils.JsonSuccessResponse(post.serialize());
        }
        #endregion

        #region List of Comments by Post
        public static Response getPostCommentList(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans.GetValue("idPost"));

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            JArray commentListByPost = new();
            foreach (Comment comment in post.getCommentList)
                commentListByPost.Add(comment.serialize());

            if (commentListByPost.Count == 0)
                return ResponseUtils.NotFound("Não há comentários nesse post.");

            return ResponseUtils.JsonSuccessResponse(commentListByPost);
        }
        #endregion

        #region Interaction
        public static Response like(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["idPost"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            User user = data.getUserById(data.getCurrentUser());

            if (user == null)
                return ResponseUtils.Unauthorized("Usuário não encontrado.");

            string statusLikeString = (string)request.body.GetValue("statusLike");
            bool statusLike = false;
            bool.TryParse(statusLikeString, out statusLike);

            if (statusLike)
            {
                if (post.getLikesIdUser.Contains(user.getId))
                    return ResponseUtils.Conflict("Você já curtiu esse post.");

                data.addPostLikeByUser((int)request.routeParans["idPost"]);
                post.getLikesIdUser.Add(user.getId);

                return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                    "\"idPost\":" + post.getId + ", " +
                    "\"likes\":" + post.getLikes + "}"));
            }
            else
            {
                if (!post.getLikesIdUser.Contains(user.getId))
                    return ResponseUtils.Conflict("Não é possível tirar o like duas vezes ou você não deu like ainda.");


                data.removePostLikeByUser((int)request.routeParans["idPost"]);
                post.getLikesIdUser.Remove(user.getId);

                return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                    "\"idPost\":" + post.getId + ", " +
                    "\"likes\":" + post.getLikes + "}"));
            }
        }
        #endregion
    }
}
