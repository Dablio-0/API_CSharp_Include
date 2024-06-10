using API_C_Sharp.Controller;
using API_C_Sharp.LSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_C_Sharp.Route
{
    public class ChatRoutes
    {
        public ChatRoutes(Server app)
        {
            #region Messages Interaction
            app.post("chat/friendship/{idFriendship:int}/message", ChatController.sendMessage);
            app.put("chat/frienship/{idFriendship:int}/message/{idMessage:int}/edit", ChatController.editMessage);
            app.delete("chat/frienship/{idFriendship:int}/message/{idMessage:int}/delete", ChatController.editMessage);
            #endregion

            #region List Messages (All Chat)
            app.get("chat/friendship/{idFriendship:int}/listMessage", ChatController.listMessages);
            #endregion
        }
    }
}
