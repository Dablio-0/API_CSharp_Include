using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_C_Sharp.Controller
{
    public class ChatController
    {
        #region Messages Interaction
        public static Response sendMessage(Request request, Data data)
        {
            return new Response();
        }

        public static Response editMessage(Request request, Data data)
        {
            return new Response();
        }

        public static Response deleteMessage(Request request, Data data)
        {
            return new Response();
        }
        #endregion

        #region List Messaages (All Chat)
        public static Response listMessages(Request request, Data data)
        {
            return new Response();
        }
        #endregion
    }
}
