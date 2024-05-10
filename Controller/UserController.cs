using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.User;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;

namespace API_C_Sharp.Controller
{
    public class UserController
    {

        /* Login and Logout */
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

            data.login(user.getId);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + user.getId + "}"));
        }

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

        public static Response update(Request request, Data data)
        {
            User user = data.getUserById((int)request.routeParans.GetValue("id"));

            if (user == null)
                return ResponseUtils.NotFound("Usuario não existe.");

            string name = (string)request.body.GetValue("name");
            string email = (string)request.body.GetValue("email");

            if (!Email.IsValid(email))
                return ResponseUtils.Unauthorized("Email inválido.");

            string password = (string)request.body.GetValue("password");

            if (!Password.IsValid(password))
                return ResponseUtils.Unauthorized("Senha inválida.");

            string birthDate = (string)request.body.GetValue("birthDate");

            JArray skills = (JArray)request.body.GetValue("skills");
            List<string> skillsList = new();

            foreach (string skill in skills)
                skillsList.Add(skill.ToString());

            JArray jobs = (JArray)request.body.GetValue("jobs");
            List<string> jobsList = new();

            foreach (string job in jobs)
                jobsList.Add(job.ToString());

            user.setName = name;
            user.setEmail = email;
            user.setPassword = password;
            user.setBirthDate = birthDate;
            user.setSkills = skillsList;
            user.jobs = jobsList;
            return ResponseUtils.JsonSuccessResponse(user.serialize());
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
            foreach (Notification notification in user.getNotifications)
                notificationsList.Add(notification.serialize().ToString());

            return ResponseUtils.JsonSuccessResponse(JObject.Parse(notificationsList.ToString()));
        }

    }
}
