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
            /* Gets the current user id */
            int idAuthor = data.getCurrentUser();

            if (idAuthor == -1)
                return ResponseUtils.Unauthorized("Não há usuários criados.");

            /** 
             * After getting the current user id,
             * 
             * Makes the body of the post is going to be created
             */
            string title = (string)request.body.GetValue("title");

            JObject bodyJson = (JObject)request.body.GetValue("body");

            BodyContent body = new(
                (string)bodyJson.GetValue("text"),
                (string)bodyJson.GetValue("code"),
                (string)bodyJson.GetValue("language"),
                (string)bodyJson.GetValue("image")
            );

            /* Adds the post to the global list of post */
            int postId = data.addPost(idAuthor, title, body);

            /* Makes the response */
            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + postId + ", idAuthor: " + idAuthor + " }"));
        }
        #endregion

        #region Update Post
        public static Response update(Request request, Data data)
        {
            /* Get the post by id from the route */
            Post post = data.getPostById((int)request.routeParans["idPost"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            /** 
             * After getting the post id from the route,
             * 
             * Makes the body of the post is going to be created
             */
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

            /* Update the post */
            post.title = title;
            post.body = body;
            post.setUpdateDate = DateTime.Now;

            /* Makes the response */
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
            /* Get the post by id from the route */
            Post post = data.getPostById((int)request.routeParans["idPost"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            /* Delete on cascade firts the comments and then the post */
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
            /* Get the list of posts */
            JArray postList = new();
            foreach (Post post in data.getPosts())
                postList.Add(post.serialize());

            if (postList.Count == 0)
                return ResponseUtils.Conflict("Não há posts.");

            // Reorder the list of posts to show the most recent first (Create the feed)
            JArray postListInverted = new();
            for (int i = postList.Count - 1; i >= 0; i--)
                postListInverted.Add(postList[i]);

            /* Makes the response */
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
            /* Get the post by id from the route */
            Post post = data.getPostById((int)request.routeParans.GetValue("idPost"));

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            /* Get the list of comments by post */
            JArray commentListByPost = new();
            foreach (Comment comment in post.getCommentList)
                commentListByPost.Add(comment.serialize());

            if (commentListByPost.Count == 0)
                return ResponseUtils.NotFound("Não há comentários nesse post.");

            /* Makes the response */
            return ResponseUtils.JsonSuccessResponse(commentListByPost);
        }
        #endregion

        #region Interaction
        public static Response like(Request request, Data data)
        {
            /* Get the post by id from the route */
            Post post = data.getPostById((int)request.routeParans["idPost"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            /* Get the current user */
            User user = data.getUserById(data.getCurrentUser());

            if (user == null)
                return ResponseUtils.Unauthorized("Usuário não encontrado.");

            /** 
             * Get the status of the like from the body
             * 
             * If the status is true, the user will like the post
             * If the status is false, the user will dislike the post
             */
            string statusLikeString = (string)request.body.GetValue("statusLike");
            bool statusLike = false;
            bool.TryParse(statusLikeString, out statusLike);

            if (statusLike)
            {
                /**
                 * If the user already liked the post (your id inside on array), return a conflict response
                 * 
                 * If not, add the like to the post and the userId to the array of user ids that liked the post
                 */
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

                /**
                 * If not liked post yet (your id not inside on array), but trying to dislike, return a conflict response
                 * 
                 * If liked, remove the like from the post and the userId from the array of user ids that liked the post
                 */
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
