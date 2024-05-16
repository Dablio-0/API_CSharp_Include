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
            string title = (string)request.body.GetValue("title");
            string body = (string)request.body.GetValue("body");

            int postId = data.addPost(idAuthor, title, body);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + postId + ", idAuthor: " + idAuthor + " }"));
        }
        #endregion

        #region Update Post
        public static Response update(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["id"]);

            if (post == null)
            {
                return ResponseUtils.NotFound("Post não encontrado.");
            }

            string title = (string)request.body.GetValue("title");
            string body = (string)request.body.GetValue("body");

            JArray images = (JArray)request.body.GetValue("images");
            List<string> imagesList = new();

            foreach (JToken image in images)
                imagesList.Add((string)image);

            post.title = title;
            post.body = body;
            post.setImageList = imagesList;
            post.setUpdateDate = DateTime.Now;

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                "id:" + post.getId + ", " +
                "idAuthor: " + post.getIdAuthor + ", " +
                "title: " + post.title + ", " +
                "body: " + post.body + ", " +
                "date: " + post.getDate + ", " +
                "updateDate: " + post.getUpdateDate + ", " +
                " }"));
        }
        #endregion

        #region Delete Post
        public static Response delete(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["id"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

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
                return ResponseUtils.JsonSuccessResponse(JObject.Parse("[]"));

            // inverter a lista de post para fazer o feed com as ultimas postagens feitas
            JArray postListInverted = new();
            for (int i = postList.Count - 1; i >= 0; i--)
                postListInverted.Add(postList[i]);

            return ResponseUtils.JsonSuccessResponse(postListInverted);
        }
        #endregion

        #region All Post
        public static Response list(Request request, Data data)
        {
            Console.WriteLine(data.getPosts());

            JArray postList = new();
            foreach (Post post in data.getPosts())
                postList.Add(post.serialize());

            if (postList.Count == 0)
                return ResponseUtils.JsonSuccessResponse(JObject.Parse("[]"));

            return ResponseUtils.JsonSuccessResponse(postList);
        }
        #endregion

        #region View a unique post
        public static Response getPostById(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["id"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            return ResponseUtils.JsonSuccessResponse(post.serialize());
        }
        #endregion

        #region List of Comments of a Post
        public static Response getPostCommentList(Request request, Data data)
        {
            Post post = data.getPostById((int)request.routeParans["id"]);

            if (post == null)
                return ResponseUtils.NotFound("Post não encontrado.");

            JArray commentList = new();
            foreach (Comment comment in post.getCommentList)
                commentList.Add(comment.serialize());

            if (commentList.Count == 0)
                return ResponseUtils.NotFound("Não há comentários nesse post.");


            return ResponseUtils.JsonSuccessResponse(JObject.Parse("[]"));
        }
        #endregion
    }
}
