using API_C_Sharp.LSharp.HTTP;
using API_C_Sharp.Model;
using API_C_Sharp.Model.Post;
using API_C_Sharp.Model.User;
using API_C_Sharp.Model.User.Chat;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
                friendship.getStatus.Equals(FriendshipStatus.blocked) ||
                friendship.getStatus.Equals(FriendshipStatus.terminated))
            {
                return ResponseUtils.Conflict("Você não pode enviar mensagens para um relacionamento pendente, recusado, bloqueado ou terminado.");
            }

            // Obtém o corpo da mensagem a partir da requisição
            JObject bodyMessageJson = (JObject)request.body.GetValue("bodyComment");

            BodyMessage bodyMessage = new(
                (string)bodyMessageJson.GetValue("text"),
                (string)bodyMessageJson.GetValue("code"),
                (string)bodyMessageJson.GetValue("language")
            );

            // Adiciona a mensagem no chat
            int messageId = data.addMessage(friendship.getId, currentUser.getId, userReceived.getId, bodyMessage);

            Message messageAdded = data.getMessageById(messageId);

            friendship.getChat.getMessageList.Add(messageAdded);

            JObject JsonResponse = new JObject
            {
                ["id"] = messageId,
                ["idChatFriendship"] = friendship.getId,
                ["idAuthotMessage"] = currentUser.getId,
                ["idReceivedMessage"] = userReceived.getId,
                ["bodyMessage"] = JObject.FromObject(bodyMessage)
                ["status"] = "Messagem enviada!"
            };

            return ResponseUtils.JsonSuccessResponse(JsonResponse);
        }


        public static Response editMessage(Request request, Data data)
        {
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            Message message = data.getMessageById((int)request.routeParans["idMessage"]);

            if (message == null)
                return ResponseUtils.NotFound("Mensagem não encontrada.");

            foreach (Message m in friendship.getChat.getMessageList)
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

            return ResponseUtils.NotFound("Este comentário não existe na lista de comentários desse post.");
        }

        public static Response deleteMessage(Request request, Data data)
        {
            Friendship friendship = data.getFriendshipById((int)request.routeParans["idFriendship"]);

            if (friendship == null)
                return ResponseUtils.NotFound("Relacionamento não encontrado.");

            Message message = data.getMessageById((int)request.routeParans["idMesssage"]);

            if (message == null)
                return ResponseUtils.NotFound("Mensagem não encontrado.");

            foreach (Message m in friendship.getChat.getMessageList)
            {
                if (m.getId == message.getId)
                {
                    friendship.getChat.getMessageList.Remove(m);

                    //return ResponseUtils.JsonSuccessResponse(JObject.Parse("{" +
                    //                    "id:" + comment.getId + ", " +
                    //                    "idAuthor: " + comment.getIdAuthorComment + ", " +
                    //                    "idPost: " + comment.getIdPost + ", " +
                    //                    "body: " + comment.bodyComment.serialize() + ", " +
                    //                    " }"));

                    return ResponseUtils.JsonSuccessResponse(JObject.Parse("{id:" + message.getId + "}"));
                }
            }
            return ResponseUtils.NotFound("Comentário não encontrado.");
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
