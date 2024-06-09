using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.User;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_C_Sharp.Controller
{
    public class FriendshipController
    {
        //public static Response sendFriendshipInvite(Request request, Data data)
        //{
        //    // Create a new friendship invite
        //    User userInvited = data.getUserById((int)request.routeParans["idUserInvited"]);
        //    int idInviter = data.getCurrentUser();

        //    List<User> listNotFriends = data.getListNotFriends(userInvited);

        //    foreach (User user in listNotFriends)
        //        if (!user.getId.Equals(idInviter))
        //            return ResponseUtils.Conflict("Você ja é amigo desse usuário.");

        //    Friendship newInvite = new Friendship(idInviter, userInvited.getId, FriendshipStatus.pending, null);

        //    if (List<Friendship>.ReferenceEquals(listNotFriends, newInvite))
        //        return ResponseUtils.Conflict("Você ja enviou um convite para esse usuário.");

        //    JObject responseJson = new JObject
        //    {
        //        ["friendship"] = JObject.FromObject(newInvite),
        //        ["message"] = "Convite enviado!"
        //    };

        //    return ResponseUtils.JsonSuccessResponse(responseJson);
        //}

        public static Response sendFriendshipInvite(Request request, Data data)
        {
            // Obtém o usuário a ser convidado e o usuário logado
            User userInvited = data.getUserById((int)request.routeParans["idUserInvited"]);
            int idInviter = data.getCurrentUser();

            // Verifica se o usuário logado já é amigo do usuário convidado
            if (userInvited.getFriends.Any(friend => friend.getId == idInviter))
            {
                return ResponseUtils.Conflict("Você já é amigo desse usuário.");
            }

            // Verifica se já existe um convite pendente
            bool inviteExists = data.getFriendshipsPending()
                                    .Any(f => f.getIdInviter == idInviter && f.getIdInvited == userInvited.getId);
            if (inviteExists)
            {
                return ResponseUtils.Conflict("Você já enviou um convite para esse usuário.");
            }

            // Cria um novo convite de amizade
            int friendshipId = data.addFrienship(idInviter, userInvited.getId, FriendshipStatus.pending, null);

            JObject responseJson = new JObject
            {
                ["friendship"] = JObject.FromObject(data.getFriendshipById(friendshipId)),
                ["message"] = "Convite enviado!"
            };

            return ResponseUtils.JsonSuccessResponse(responseJson);
        }

        public static Response acceptInvite(Request request, Data data)
        {
            return new Response();
        }

        public static Response rejectInvite(Request request, Data data)
        {
            return new Response();
        }

        public static Response listInvites(Request request, Data data)
        {
            return new Response();
        }

        public static Response listFriendship(Request request, Data data)
        {
            return new Response();
        }
    }
}
