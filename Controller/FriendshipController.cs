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
        #region Create Friendship Instance (Pendingg Status)
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
                ["id"] = friendshipId,
                ["friendship"] = JObject.FromObject(data.getFriendshipById(friendshipId)),
                ["message"] = "Convite enviado!"
            };

            return ResponseUtils.JsonSuccessResponse(responseJson);
        }
        #endregion

        public static Response acceptInvite(Request request, Data data)
        {
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Convite não encontrado.");

            if (friendship.getStatus.Equals(FriendshipStatus.accepted))
                return ResponseUtils.Conflict("Convite já aceito.");
            else
            {
                friendship.setStatus = FriendshipStatus.accepted;

                User currentUser = data.getUserById(data.getCurrentUser());

                currentUser.getFriends.Add(data.getUserById(friendship.getIdInviter));
            }

            JObject JsonResponse = new JObject
            {
                ["id"] = friendship.getId,
                ["friendship"] = JObject.FromObject(friendship),
                ["message"] = "Convite aceito!"
            };

            return ResponseUtils.JsonSuccessResponse(JsonResponse);
        }

        public static Response rejectInvite(Request request, Data data)
        {
            return new Response();
        }

        public static Response blockFriend(Request request, Data data)
        {
            return new Response();
        }

        public static Response terminateFriendship(Request request, Data data)
        {
            return new Response();
        }

        public static Response listInvitesByUser(Request request, Data data)
        {
            User user = data.getUserById((int)request.routeParans["idCurrentUser"]);

            if (user == null)
            {
                return ResponseUtils.NotFound("Usuário não encontrado.");
            }

            List<Friendship> friendshipsList = data.getFriendships();
            List<Friendship> invitationsSent = new();
            List<Friendship> invitationsReceived = new();

            foreach (Friendship friendship in friendshipsList)
            {
                if (friendship.getStatus == FriendshipStatus.pending)
                {
                    if (friendship.getIdInvited.Equals(user.getId))
                        invitationsReceived.Add(friendship);
                    else if (friendship.getIdInviter.Equals(user.getId))
                        invitationsSent.Add(friendship);
                }
            }

            JObject responseJson = new JObject
            {
                ["invitationsSent"] = JArray.FromObject(invitationsSent),
                ["invitationsReceived"] = JArray.FromObject(invitationsReceived)
            };

            return ResponseUtils.JsonSuccessResponse(responseJson);
        }

        public static Response listFriendshipByUser(Request request, Data data)
        {
            User user = data.getUserById((int)request.routeParans["idCurrentUser"]);

            if (user == null)
                return ResponseUtils.NotFound("Usuário não encontrado.");

            List<User> friends = user.getFriends;

            return ResponseUtils.JsonSuccessResponse(JArray.FromObject(friends));
        }
    }
}
