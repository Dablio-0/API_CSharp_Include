using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_C_Sharp.Controller
{
    public class CommentController
    {
        #region create comment
        public static void create(Request request, Data data)
        {
            int idAuthor = data.getCurrentUser();
            int idPost = (int)request.parameters.GetValue("idPost"); //o id do post será recebido via query params
            string text = (string)request.text.GetValue("text");

            int commentId = data.addComment(idAuthor, idPost, text);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                "id:" + commentId + ", " +
                "idAuthor: " + idAuthor + ", " +
                "idPost: " + idPost + ", " +
                "text: " + text + ", " +
            " }"));
        }
        #endregion

        public static void update()
        {
            // Update a comment
        }

        public static void delete()
        {
            // Delete a comment
        }
    }
}
