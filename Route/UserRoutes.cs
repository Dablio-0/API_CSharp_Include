using API_C_Sharp.Controller;
using API_C_Sharp.LSharp;

namespace API_C_Sharp.Route
{

    public class UserRoutes
    {
        public UserRoutes(Server app)
        {
            #region Login at System
            app.post("/login", UserController.login);
            #endregion

            #region CRUD User Routes
            app.post("/register", UserController.register);
            app.put("/user/edit/{id:int}", UserController.update);
            app.delete("user/delete/{id:int}", UserController.delete);
            #endregion

            #region List User (Search for Users) and get User by Id
            app.get("/user/list", UserController.list);
            app.get("/user/{id:int}", UserController.getUserById);
            #endregion

            #region List Post by User
            app.get("/user/{idUser:int}/post/list", UserController.listPostByUser);
            #endregion


        }
    }
}