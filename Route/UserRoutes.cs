using API_C_Sharp.Controller;
using API_C_Sharp.LSharp;

namespace API_C_Sharp.Route
{

    public class UserRoutes
    {
        public UserRoutes(Server app)
        {
            app.post("/login", UserController.login);

            app.post("/register", UserController.register);
            app.put("/user/edit/{id:int}", UserController.update);

            app.get("/user/list", UserController.list);
            app.get("/user/{id:int}", UserController.getUserById);

            app.get("/user/{id:int}/friendship", UserController.getUserFriendship);
            app.get("/user/{id:int}/notification", UserController.getUserNotification);
        }
    }
}