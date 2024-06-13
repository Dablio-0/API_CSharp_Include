using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.User;
using API_C_Sharp.Model.User.Chat;
using API_C_Sharp.Utils;
using Newtonsoft.Json;
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
            /* Get the user from the ID in the route parameters */
            User userInvited = data.getUserById((int)request.routeParans["idUserInvited"]);

            if (userInvited.getId.Equals(data.getCurrentUser()))
                return ResponseUtils.Conflict("Não é possível enviar um convite para si mesmo.");

            /* Get the user by the ID of the logged in user */
            int idInviter = data.getCurrentUser();

            /* Verifies if the logged in user is already a friend of the invited user */
            if (userInvited.getFriends.Any(friend => friend.getId == idInviter))
                return ResponseUtils.Conflict("Você já é amigo desse usuário.");
            
            /* Verifies if there is already a pending friendship between the logged in user and the invited user */
            bool inviteExists = data.getFriendshipsPending()
                                    .Any(f => f.getIdInviter == idInviter && f.getIdInvited == userInvited.getId);
            if (inviteExists)
                return ResponseUtils.Conflict("Você já enviou um convite para esse usuário.");

            /** 
             * If all the validations are correct, the friendship is created with the 
             * pending status and after then returns the Json Response
             */

            int friendshipId = data.addFrienship(idInviter, userInvited.getId, FriendshipStatus.pending);

            /* Make the JSON response */
            JObject responseJson = new JObject
            {
                ["id"] = friendshipId,
                ["friendship"] = JObject.FromObject(data.getFriendshipById(friendshipId)),
                ["message"] = "Convite enviado!"
            };

            return ResponseUtils.JsonSuccessResponse(responseJson);
        }
        #endregion

        #region Accept Invite
        public static Response acceptInvite(Request request, Data data)
        {
            /* Get the friendship by the ID in the route parameters */
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Convite não encontrado.");

            /* Verifies if the friendship is already accepted */
            if (friendship.getStatus.Equals(FriendshipStatus.accepted))
                return ResponseUtils.Conflict("Convite já aceito.");
            else
            {
                /**
                 * If the friendship is not accepted, the status is changed to accepted
                 * 
                 * The both users are added to the friends list of each other
                 */

                friendship.setStatus = FriendshipStatus.accepted;

                User currentUser = data.getUserById(data.getCurrentUser());

                currentUser.getFriends.Add(data.getUserById(friendship.getIdInviter));

                User friendAdded = data.getUserById(friendship.getIdInviter);

                friendAdded.getFriends.Add(currentUser);
            }

            /* Make the JSON response */
            JObject JsonResponse = new JObject
            {
                ["id"] = friendship.getId,
                ["friendship"] = JObject.FromObject(friendship),
                ["message"] = "Convite aceito!"
            };

            return ResponseUtils.JsonSuccessResponse(JsonResponse);
        }
        #endregion

        #region Reject Invite
        public static Response rejectInvite(Request request, Data data)
        {
            /* Get the friendship by the ID in the route parameters */
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Convite recusado.");

            /* Verifies if the friendship is already declined */
            if (friendship.getStatus.Equals(FriendshipStatus.declined))
                return ResponseUtils.Conflict("Convite já recusado.");
            else
            {
                /* If the friendship is not declined, the status is changed to declined */
                friendship.setStatus = FriendshipStatus.declined;
            }
            
            /* Make the JSON response */
            JObject JsonResponse = new JObject
            {
                ["id"] = friendship.getId,
                ["friendship"] = JObject.FromObject(friendship),
                ["message"] = "Convite recusado!"
            };

            return ResponseUtils.JsonSuccessResponse(JsonResponse);
        }
        #endregion

        #region Block Friend
        public static Response blockFriend(Request request, Data data)
        {
            return new Response();
        }
        #endregion

        #region Terminate Friendship
        public static Response terminateFriendship(Request request, Data data)
        {
            /* Get the user by the ID of the logged in user */
            User currentUser = data.getUserById(data.getCurrentUser());

            if (currentUser == null)
                return ResponseUtils.NotFound("Não há usuário ativo na sessão.");

            /* Get the friend from the ID of the route parameter */
            User friendUser = data.getUserById((int)request.routeParans["idUserFriend"]);

            if (friendUser == null)
                return ResponseUtils.NotFound("Usuário não encontrado.");

            /* Verifies if the logged in user and the user from the route are different */
            if (currentUser.getId == friendUser.getId)
                return ResponseUtils.Conflict("Você não pode encerrar amizade consigo mesmo.");

            /* Get the friendship by the ID in the route parameters */
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            /* Verifies if both users are part of the friendship */
            if (!((friendship.getIdInviter == currentUser.getId && friendship.getIdInvited == friendUser.getId) ||
                  (friendship.getIdInviter == friendUser.getId && friendship.getIdInvited == currentUser.getId)))
            {
                return ResponseUtils.Conflict("Usuários não são amigos.");
            }

            /* Verifies if the friendship is a pending friendship */
            if (friendship.getStatus.Equals(FriendshipStatus.pending))
            {
                return ResponseUtils.Conflict("Não é possível encerrar uma amizade pendente.");
            }

            /** 
             * If all the validations are correct, the friendship is deleted
             * 
             * The both users are removed from the friends list of each other
             */
            data.deleteFriendship(friendship.getId);

            currentUser.getFriends.Remove(friendUser);
            friendUser.getFriends.Remove(currentUser);

            /* Make the JSON response */
            JObject JsonResponse = new JObject
            {
                ["message"] = "Amizade Encerrada."
            };

            return ResponseUtils.JsonSuccessResponse(JsonResponse);
        }
        #endregion

        #region Lit Invites by User
        public static Response listInvitesByUser(Request request, Data data)
        {
            /* Get the user by the ID of the logged in user */
            User user = data.getUserById(data.getCurrentUser());

            if (user == null)
            {
                return ResponseUtils.NotFound("Não há usuário ativo na sessão.");
            }

            /**
             * From the list of friendships, the method separates the invitations sent and received
             * 
             * In if condition the method verifies if the friendship is pending and if the user is the inviter or the invited
             * 
             * Then the friendship is added to the list of invitations sent or received
             */

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

            /* Make the JSON response */
            JObject responseJson = new JObject
            {
                ["invitationsSent"] = JArray.FromObject(invitationsSent),
                ["invitationsReceived"] = JArray.FromObject(invitationsReceived)
            };

            return ResponseUtils.JsonSuccessResponse(responseJson);
        }
        #endregion 

        #region List of Friends by User
        public static Response listFriendsByUser(Request request, Data data)
        {
            /* Get the user by the ID of the route parameter */
            User user = data.getUserById((int)request.routeParans["idUser"]);

            if (user == null)
                return ResponseUtils.NotFound("Usuário não encontrado.");

            /* Get the list of friends of the user */
            List<User> friendsList = user.getFriends;

            /* Make the Json with a active user information and the list of friends with their IDs */
            JObject userJson = new JObject
            {
                ["id"] = user.getId,
                ["name"] = user.getName,
                ["email"] = user.getEmail
            };

            JArray idsUserFriends = new JArray();

            foreach (User friend in friendsList)
            {
                idsUserFriends.Add(friend.getId);
            }

            userJson["idFriends"] = idsUserFriends;

            return ResponseUtils.JsonSuccessResponse(userJson);
        }
        #endregion
    }
}
