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
        public static Response sendMessage(Request request, Data data)
        {
            #region Message Interaction
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            User currentUser = data.getUserById(data.getCurrentUser());

            if (currentUser == null)
                return ResponseUtils.Unauthorized("Não há usuário ativo na sessão.");

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

            // Verifica o status da amizade
            if (friendship.getStatus.Equals(FriendshipStatus.pending) ||
                friendship.getStatus.Equals(FriendshipStatus.declined) ||
                friendship.getStatus.Equals(FriendshipStatus.blocked))
            {
                return ResponseUtils.Conflict("Você não pode enviar mensagens para um relacionamento pendente, recusado, bloqueado ou terminado.");
            }

            // Obtém o corpo da mensagem a partir da requisição
            JObject bodyMessageJson = (JObject)request.body.GetValue("bodyMessage");

            BodyMessage bodyMessage = new(
                (string)bodyMessageJson.GetValue("text"),
                (string)bodyMessageJson.GetValue("code"),
                (string)bodyMessageJson.GetValue("language")
            );

            // Adiciona a mensagem no chat
            int messageId = data.addMessage(friendship.getId, currentUser.getId, userReceived.getId, bodyMessage);

            Message messageAdded = data.getMessageById(messageId);

            return ResponseUtils.JsonSuccessResponse(new JObject(
                new JProperty("id", messageId),
                new JProperty("idChatFriendship", friendship.getId),
                new JProperty("idAuthorMessage", currentUser.getId),
                new JProperty("idReceivedMessage", userReceived.getId),
                new JProperty("bodyMessage", bodyMessage.serialize()),
                new JProperty("status", "Mensagem enviada!")
                ));
        }

        public static Response editMessage(Request request, Data data)
        {
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            if (friendship.getStatus.Equals(FriendshipStatus.pending) ||
                friendship.getStatus.Equals(FriendshipStatus.declined) ||
                friendship.getStatus.Equals(FriendshipStatus.blocked))
            {
                return ResponseUtils.Conflict("Você não pode editar mensagens em um relacionamento pendente, recusado, bloqueado ou terminado.");
            }

            Message message = data.getMessageById((int)request.routeParans["idMessage"]);

            if (message == null)
                return ResponseUtils.NotFound("Mensagem não encontrada.");

            //alterar para pegar da lista geral

            User currentUser = data.getUserById(data.getCurrentUser());

            List<Message> userMessages = data.getMessagesByUser(currentUser.getId);

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

        public static Response deleteMessage(Request request, Data data)
        {
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            if (friendship.getStatus.Equals(FriendshipStatus.pending) ||
                friendship.getStatus.Equals(FriendshipStatus.declined) ||
                friendship.getStatus.Equals(FriendshipStatus.blocked))
            {
                return ResponseUtils.Conflict("Você não pode deletar mensagens em um relacionamento pendente, recusado, bloqueado ou terminado.");
            }

            Message message = data.getMessageById((int)request.routeParans["idMesssage"]);

            if (message == null)
                return ResponseUtils.NotFound("Mensagem não encontrado.");

            // alterar para pegar da lista geral

            User currentUser = data.getUserById(data.getCurrentUser());

            List<Message> userMessages = data.getMessagesByUser(currentUser.getId);
            foreach (Message m in userMessages)
            {
                if (m.getId == message.getId)
                {
                    data.deleteMessage(m.getId);

                    return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + message.getId + "}"));
                }
            }
            return ResponseUtils.Conflict("Messagem não deletada.");
        }
        #endregion

        #region List Messages (All Chat)
        public static Response listMessages(Request request, Data data)
        {
            // Obtém a amizade pelo ID
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            // Obtém o usuário atual da sessão
            User currentUser = data.getUserById(data.getCurrentUser());

            if (currentUser == null)
                return ResponseUtils.Unauthorized("Não há usuário ativo na sessão.");

            // Obtém o amigo a partir do ID do parâmetro da rota
            User userFriend = data.getUserById((int)request.routeParans["idUserFriend"]);

            if (userFriend == null)
                return ResponseUtils.NotFound("Usuário não encontrado.");

            // Verifica se ambos os usuários fazem parte da amizade
            if (!((friendship.getIdInviter == currentUser.getId && friendship.getIdInvited == userFriend.getId) ||
                  (friendship.getIdInviter == userFriend.getId && friendship.getIdInvited == currentUser.getId)))
            {
                return ResponseUtils.Conflict("Usuários não são amigos.");
            }

            // Obtém todas as mensagens do usuário logado
            List<Message> userMessages = data.getMessagesByUser(currentUser.getId);

            // Filtra as mensagens que envolvem o amigo especificado
            List<Message> relevantMessages = userMessages
                .Where(m => (m.getIdAuthorMessage == currentUser.getId && m.getIdUserReceived == userFriend.getId) ||
                            (m.getIdAuthorMessage == userFriend.getId && m.getIdUserReceived == currentUser.getId))
                .ToList();

            if (relevantMessages == null || relevantMessages.Count == 0)
                return ResponseUtils.NotFound("Chat vazio.");

            JArray messageList = new JArray();
            foreach (Message message in relevantMessages)
                messageList.Add(message.serialize());

            return ResponseUtils.JsonSuccessResponse(messageList);
        }
        #endregion
    }
}
