using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.User;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Controller
{
    public class UserController
    {

        public static Response register(Request request, Data data)
        {
            string name = (string)request.body.GetValue("name");
            string email = (string)request.body.GetValue("email");
            string password = (string)request.body.GetValue("password");

            if (!Email.IsValid(email))
                return ResponseUtils.Unauthorized("Email inválido.");

            int userId = data.addUser(name, email, password);

            if (userId == -1)
                return ResponseUtils.Conflict("Este email já está sendo usado por outro usuário.");

            data.login(userId);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + userId + "}"));
        }

        public static Response login(Request request, Data data)
        {
            string email = (string)request.body.GetValue("email");
            string password = (string)request.body.GetValue("password");

            if (!Email.IsValid(email))
                return ResponseUtils.Unauthorized("Email inválido.");

            User user = data.getUserByLogin(email);

            if (user == null)
                return ResponseUtils.Unauthorized("Usuário não encontrado.");

            if (!user.checkPassword(password))
                return ResponseUtils.Unauthorized("Usuário não encontrado.");

            data.login(user.Id);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + user.Id + "}"));
        }

        public static Response list(Request request, Data data)
        {
            Console.WriteLine(data.getUsers());

            JArray usersList = new();
            foreach (User user in data.getUsers())
                usersList.Add(user.serialize());

            if (usersList.Count == 0)
                return ResponseUtils.JsonSuccessResponse(JObject.Parse("[]"));


            return ResponseUtils.JsonSuccessResponse(usersList);
        }

        public static Response getUserById(Request request, Data data)
        {
            User user = data.getUserById((int)request.routeParans.GetValue("id"));

            if (user == null)
                return ResponseUtils.NotFound("Usuario não existe.");

            return ResponseUtils.JsonSuccessResponse(user.serialize());
        }

        public static Response getUserFriendship(Request request, Data data)
        {
            User user = data.getUserById((int)request.routeParans.GetValue("id"));

            if (user == null)
                return ResponseUtils.NotFound("Usuario não existe.");

            return new Response("teste");
        }

        public static Response getUserNotification(Request request, Data data)
        {
            User user = data.getUserById((int)request.routeParans.GetValue("id"));

            if (user == null)
                return ResponseUtils.NotFound("Usuario não existe.");

            List<string> notificationsList = new();
            foreach (Notification notification in user.Notifications)
                notificationsList.Add(notification.serialize().ToString());

            return ResponseUtils.JsonSuccessResponse(JObject.Parse(notificationsList.ToString()));
        }

    }
}
