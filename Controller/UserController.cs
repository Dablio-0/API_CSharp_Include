using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.Post;
using API_C_Sharp.Model.User;
using API_C_Sharp.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Data;
using System.Formats.Tar;

namespace API_C_Sharp.Controller
{
    public class UserController
    {
        #region Login
        public static Response login(Request request, Data data)
        {
            /* Get values from json */
            string email = (string)request.body.GetValue("email");
            string password = (string)request.body.GetValue("password");

            /* Check email pattern */
            if (!Email.IsValid(email))
                return ResponseUtils.Unauthorized("Email inválido.");

            /* Check if user exists */
            User user = data.getUserByLogin(email);

            if (user == null)
                return ResponseUtils.Unauthorized("Usuário ou senha inválidos.");

            /* Check password pattern */
            if (!user.checkPassword(password))
                return ResponseUtils.Unauthorized("Usuário ou senha inválidos.");

            /* Initialize user session login */
            data.login(user.getId);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + user.getId + "}"));
        }

        public static Response userLogged(Request request, Data data)
        {
            /* Get the current user by id */
            User user = data.getUserById(data.getCurrentUser());

            if (user == null)
                return ResponseUtils.NotFound("Não há nenhum login ativo.");

            /* Makes the Json response */
            JObject JsonResponse = new JObject
            {
                ["id"] = user.getId,
                ["email"] = user.getEmail
            };

            return ResponseUtils.JsonSuccessResponse(JsonResponse);
        }
        #endregion

        #region CRUD User
        public static Response register(Request request, Data data)
        {
            /* Get values from json */
            string name = (string)request.body.GetValue("name");
            string email = (string)request.body.GetValue("email");
            string password = (string)request.body.GetValue("password");

            /* Check email pattern */
            if (!Email.IsValid(email))
                return ResponseUtils.Unauthorized("Email inválido.");

            /* Add new user */
            int userId = data.addUser(name, email, password);

            /* Check if email is already in use */
            if (userId == -1)
                return ResponseUtils.Conflict("Este email já está sendo usado por outro usuário.");

            /* Initialize user session login */
            data.login(userId);

            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + userId + "}"));
        }

        public static Response update(Request request, Data data)
        {
            /* Get the user by id from the route */
            User user = data.getUserById(data.getCurrentUser());

            if (user == null)
                return ResponseUtils.NotFound("Usuario não existe.");

            /** 
             * Get values from json body
             */

            string name = (string)request.body.GetValue("name");
            string email = (string)request.body.GetValue("email");

            string imageIconProfile = (string)request.body.GetValue("imageIconProfile");

            string birthDate = (string)request.body.GetValue("birthDate");

            JArray skills = (JArray)request.body.GetValue("skills");
            List<string> skillsList = new();

            foreach (string skill in skills)
                skillsList.Add(skill.ToString());

            JArray jobs = (JArray)request.body.GetValue("jobs");
            List<string> jobsList = new();

            foreach (string job in jobs)
                jobsList.Add(job.ToString());

            /* Set new values */
            user.setName = name;
            user.setEmail = email;
            user.setImageIconProfile = imageIconProfile;
            user.setBirthDate = birthDate;
            user.setSkills = skillsList;
            user.setJobs = jobsList;

            /* Return the user updated */
            return ResponseUtils.JsonSuccessResponse(user.serialize());
        }

        public static Response delete(Request request, Data data)
        {
            /* Get the user by id from the route */
            User user = data.getUserById((int)request.routeParans.GetValue("id"));

            if (user == null)
                return ResponseUtils.NotFound("Usuario não existe.");

            /* Remove the user from the list */
            data.getUsers().Remove(user);

            /* Return the user updated */
            return ResponseUtils.JsonSuccessResponse(JObject.Parse("{message: 'Usuario deletado com sucesso.'}"));
        }
        #endregion

        #region All Users
        public static Response list(Request request, Data data)
        {
            /* Get all users from the global list in data class */
            List<User> usersList = data.getUsers();

            if (usersList.Count == 0)
            {
                return ResponseUtils.NotFound("Não há usuários criados.");
            }

            JArray usersArray = new JArray();

            /** 
             * Serialize all users and add to the array
             */
            foreach (User user in usersList)
            {
                JObject userJson = new JObject
                {
                    ["id"] = user.getId,
                    ["name"] = user.getName,
                    ["email"] = user.getEmail,
                    ["imageIconProfile"] = user.getImageIconProfile,
                    ["birthDate"] = user.getBirthDate,
                    ["skills"] = JArray.FromObject(user.getSkills),
                    ["jobs"] = JArray.FromObject(user.getJobs)
                };

                usersArray.Add(userJson);
            }

            /* Return the list of users */
            return ResponseUtils.JsonSuccessResponse(usersArray);
        }
        #endregion

        #region Get User by ID
        public static Response getUserById(Request request, Data data)
        {
            /* Get the user by id from the route */
            User user = data.getUserById((int)request.routeParans.GetValue("id"));

            if (user == null)
                return ResponseUtils.NotFound("Usuario não existe.");

            /* Makes the Json response to serialize the unique user is going to return */
            JObject JsonResponse = new JObject
            {
                ["id"] = user.getId,
                ["name"] = user.getName,
                ["email"] = user.getEmail,
                ["imageIconProfile"] = user.getImageIconProfile,
                ["birthDate"] = user.getBirthDate,
                ["skills"] = JArray.FromObject(user.getSkills),
                ["jobs"] = JArray.FromObject(user.getJobs)
            };

            /* Return the user */
            return ResponseUtils.JsonSuccessResponse(JsonResponse);
        }
        #endregion

        #region List of Post by User
        public static Response listPostByUser(Request request, Data data)
        {
            /* Get the user by id from the route */
            User user = data.getUserById((int)request.routeParans.GetValue("idUser"));

            if (user == null)
            {
                return ResponseUtils.NotFound("Usuario não existe.");
            }

            /**
             * Get all posts from the global list in data class
             * 
             * And filter the posts by the user id
             * */

            List<Post> posts = new List<Post>();
            foreach (Post post in data.getPosts())
            {
                if (post.getIdAuthor == user.getId)
                    posts.Add(post);
            }

            /* Makes the Json response to serialize all posts by the user */
            JArray postListByUser = new();
            foreach (Post post in posts)
                postListByUser.Add(post.serialize());

            /* Return the list of posts by the user */
            return ResponseUtils.JsonSuccessResponse(postListByUser);
        }
        #endregion

        #region Notifications
        //public static Response getUserFriendship(Request request, Data data)
        //{
        //    User user = data.getUserById((int)request.routeParans.GetValue("id"));

        //    if (user == null)
        //        return ResponseUtils.NotFound("Usuario não existe.");

        //    List<string> friendsList = new();
        //    foreach (User friend in user.getFriends)
        //        friendsList.Add(friend.serialize().ToString());

        //    return ResponseUtils.JsonSuccessResponse(JObject.Parse(friendsList.ToString()));

        //}
        #endregion

        #region Get User notifications
        public static Response getUserNotifications(Request request, Data data)
        {
            User user = data.getUserById((int)request.routeParans.GetValue("id"));

            if (user == null)
                return ResponseUtils.NotFound("Usuario não existe.");

            List<string> notificationsList = new();
            foreach (Notification notification in user.getNotifications)
                notificationsList.Add(notification.serialize().ToString());

            return ResponseUtils.JsonSuccessResponse(JObject.Parse(notificationsList.ToString()));
        }
        #endregion
    }
}
