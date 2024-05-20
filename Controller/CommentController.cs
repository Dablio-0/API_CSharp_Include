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
        public static Response create(Request request, Data data)
        {
            // Create a new comment
            return new Response();
        }

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
