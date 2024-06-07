using API_C_Sharp.Controller;
using API_C_Sharp.LSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_C_Sharp.Route
{
    public class FriendshipRoutes
    {
        public FriendshipRoutes(Server app)
        {
            #region Invite Routes
            app.post("/friendship/send/{idUserInvited:int}", FriendshipController.sendFriendshipInvite);
            app.post("/friendship/invites/{idFriendship:int}/accept", FriendshipController.acceptInvite);
            app.post("/friendship/invites/{idFriendship:int}/reject", FriendshipController.rejectInvite);

            // list invites by user
            app.get("/friendship/invites/{idCurrentUser:int}", FriendshipController.listInvites);
            #endregion

            #region List Friendship
            app.get("/friendship/user/{idUser:int}/list", FriendshipController.listFriendship);
            #endregion
        }
    }
}
