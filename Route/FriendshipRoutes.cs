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
            app.post("/friendship/send", FriendshipController.sendFriendshipInvite);
            app.post("/friendship/{idFriendship:int}/accept", FriendshipController.acceptInvite);
            app.post("/friendship/{idFriendship:int}/reject", FriendshipController.rejectInvite);
            #endregion

            #region List Friendship
            app.get("/user/idUser:int}/friendship/list", FriendshipController.listFriendship);
            #endregion
        }
    }
}
