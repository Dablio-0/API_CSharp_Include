using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.User;
using API_C_Sharp.Model.User.Chat;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_C_Sharp.Controller
{
    public class ChatController
    {

        #region Send Message
        public static Response sendMessage(Request request, Data data)
        {
            /* Gets the friendship by id */
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            /* Gets the current user */
            User currentUser = data.getUserById(data.getCurrentUser());

            if (currentUser == null)
                return ResponseUtils.Unauthorized("Não há usuário ativo na sessão.");

            /** 
             * Gets the user that is going to receive the message
             * 
             * If the user is the inviter, the user received is the invited
             * If the user is the invited, the user received is the inviter
             */
            User userReceived = null;
            if (friendship.getIdInviter == currentUser.getId)
            {
                userReceived = data.getUserById(friendship.getIdInvited);
            }
            else if (friendship.getIdInvited == currentUser.getId)
            {
                userReceived = data.getUserById(friendship.getIdInviter);
            }
            else
            {
                return ResponseUtils.Conflict("Usuário não faz parte deste relacionamento.");
            }

            /* Checks if the friendship is pending, declined, blocked or terminated */
            if (friendship.getStatus.Equals(FriendshipStatus.pending) ||
                friendship.getStatus.Equals(FriendshipStatus.declined) ||
                friendship.getStatus.Equals(FriendshipStatus.blocked))
            {
                return ResponseUtils.Conflict("Você não pode enviar mensagens para um relacionamento pendente, recusado, bloqueado ou terminado.");
            }

            /* Gets the body of the message */
            JObject bodyMessageJson = (JObject)request.body.GetValue("bodyMessage");

            BodyMessage bodyMessage = new(
                (string)bodyMessageJson.GetValue("text"),
                (string)bodyMessageJson.GetValue("code"),
                (string)bodyMessageJson.GetValue("language")
            );

            /* Adds the message to the global list of messages */
            int messageId = data.addMessage(friendship.getId, currentUser.getId, userReceived.getId, bodyMessage);

            Message messageAdded = data.getMessageById(messageId);

            /* Makes the response */
            return ResponseUtils.JsonSuccessResponse(new JObject(
                new JProperty("id", messageId),
                new JProperty("idChatFriendship", friendship.getId),
                new JProperty("idAuthorMessage", currentUser.getId),
                new JProperty("idReceivedMessage", userReceived.getId),
                new JProperty("bodyMessage", bodyMessage.serialize()),
                new JProperty("status", "Mensagem enviada!")
                ));
        }
        #endregion

        #region Edit Message
        public static Response editMessage(Request request, Data data)
        {
            /* Gets the friendship by id */
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            /* Checks if the friendship is pending, declined, blocked or terminated */
            if (friendship.getStatus.Equals(FriendshipStatus.pending) ||
                friendship.getStatus.Equals(FriendshipStatus.declined) ||
                friendship.getStatus.Equals(FriendshipStatus.blocked))
            {
                return ResponseUtils.Conflict("Você não pode editar mensagens em um relacionamento pendente, recusado, bloqueado ou terminado.");
            }

            /* Gets the message by id */
            Message message = data.getMessageById((int)request.routeParans["idMessage"]);

            if (message == null)
                return ResponseUtils.NotFound("Mensagem não encontrada.");

            /* Gets the current user */
            User currentUser = data.getUserById(data.getCurrentUser());

            /* Gets all messages from the current user */
            List<Message> userMessages = data.getMessagesByUser(currentUser.getId);

            /* Checks if the message is from the current user */
            foreach (Message m in userMessages)
            {
                if (m.getId == message.getId)
                {
                    JObject bodyMessageJson = (JObject)request.body.GetValue("bodyMessage");
                    BodyMessage bodyMessage = new(
                        (string)bodyMessageJson.GetValue("text"),
                        (string)bodyMessageJson.GetValue("code"),
                        (string)bodyMessageJson.GetValue("language")
                    );

                    message.bodyMessage = bodyMessage;
                    message.setUpdateDate = DateTime.Now;

                    /* Makes the response */
                    return ResponseUtils.JsonSuccessResponse(new JObject(
                        new JProperty("id", message.getId),
                        new JProperty("idChatFriendship", friendship.getId),
                        new JProperty("idAuthotMessage", message.getIdAuthorMessage),
                        new JProperty("idReceivedMessage", message.getIdUserReceived),
                        new JProperty("bodyMessage", message.bodyMessage.serialize()),
                        new JProperty("date", message.getDate),
                        new JProperty("updateDate", message.getUpdateDate)
                    ));
                }
            }

            return ResponseUtils.Conflict("Mensagem não editarda.");
        }
        #endregion

        #region Delete Message
        public static Response deleteMessage(Request request, Data data)
        {
            /* Gets the friendship by id */
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            /* Checks if the friendship is pending, declined, blocked or terminated */
            if (friendship.getStatus.Equals(FriendshipStatus.pending) ||
                friendship.getStatus.Equals(FriendshipStatus.declined) ||
                friendship.getStatus.Equals(FriendshipStatus.blocked))
            {
                return ResponseUtils.Conflict("Você não pode deletar mensagens em um relacionamento pendente, recusado, bloqueado ou terminado.");
            }

            /* Gets the message by id */
            Message message = data.getMessageById((int)request.routeParans["idMesssage"]);

            if (message == null)
                return ResponseUtils.NotFound("Mensagem não encontrado.");

            /* Gets the current user */
            User currentUser = data.getUserById(data.getCurrentUser());

            /* Gets all messages from the current user */
            List<Message> userMessages = data.getMessagesByUser(currentUser.getId);
            foreach (Message m in userMessages)
            {
                if (m.getId == message.getId)
                {
                    /* Deletes the message in global list from data class */
                    data.deleteMessage(m.getId);

                    /* Makes the response */
                    return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + message.getId + "}"));
                }
            }
            return ResponseUtils.Conflict("Messagem não deletada.");
        }
        #endregion

        #region List Messages (All Chat)
        public static Response listMessages(Request request, Data data)
        {
            /* Gets the friendship by id */
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            /* Gets the current user */
            User currentUser = data.getUserById(data.getCurrentUser());

            if (currentUser == null)
                return ResponseUtils.Unauthorized("Não há usuário ativo na sessão.");

            /* Gets the user friend */
            User userFriend = data.getUserById((int)request.routeParans["idUserFriend"]);

            if (userFriend == null)
                return ResponseUtils.NotFound("Usuário não encontrado.");

            /* Checks if the both users are friends */
            if (!((friendship.getIdInviter == currentUser.getId && friendship.getIdInvited == userFriend.getId) ||
                  (friendship.getIdInviter == userFriend.getId && friendship.getIdInvited == currentUser.getId)))
            {
                return ResponseUtils.Conflict("Usuários não são amigos.");
            }

            /* Gets all messages from the current user */
            List<Message> userMessages = data.getMessagesByUser(currentUser.getId);

            /* Gets all messages from the user friend, both sent and received in this friendship */
            List<Message> relevantMessages = userMessages
                .Where(m => (m.getIdAuthorMessage == currentUser.getId && m.getIdUserReceived == userFriend.getId) ||
                            (m.getIdAuthorMessage == userFriend.getId && m.getIdUserReceived == currentUser.getId))
                .ToList();

            /** If this chat is empty, returns a message 
             *
             * If not, returns the list of messages
             */
            if (relevantMessages == null || relevantMessages.Count == 0)
                return ResponseUtils.NotFound("Chat vazio.");

            JArray messageList = new JArray();
            foreach (Message message in relevantMessages)
                messageList.Add(message.serialize());

            /* Makes the response */
            return ResponseUtils.JsonSuccessResponse(messageList);
        }
        #endregion
    }
}
