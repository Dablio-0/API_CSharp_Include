using API_C_Sharp.Model.Post;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_C_Sharp.Model.User.Chat
{
    public class Message
    {
        #region Attributes
        private int id;
        private int idChatFriendship;
        private int idAuthorMessage;
        private int idUserReceived;
        public BodyMessage bodyMessage;
        private DateTime date;
        public DateTime updateDate;
        #endregion

        #region Constructor
        public Message(int id, int idChatFriendship, int idAuthorMessage, int idUserReceived, BodyMessage bodyMessage)
        {
            this.id = id;
            this.id = idChatFriendship;
            this.idAuthorMessage = idAuthorMessage;
            this.idUserReceived = idUserReceived;
            this.bodyMessage = bodyMessage;
            this.date = DateTime.Now;
            this.updateDate = DateTime.Now;
        }
        #endregion

        #region Gets e Sets
        public int getId { get { return id; } }

        public int getIdChatFriendship { get { return idChatFriendship; } }

        public int setIdChatFriendship { set { idChatFriendship = value; } }

        public int getIdAuthorMessage { get { return idAuthorMessage; } }

        public int setIdAuthorMessage { set { idAuthorMessage = value; } }

        public int getIdUserReceived { get { return idUserReceived; } }

        public int setIdUserReceived { set { idUserReceived = value; } }

        public DateTime getDate { get { return date; } }

        public DateTime getUpdateDate { get { return updateDate; } }

        public DateTime setUpdateDate { set { updateDate = value; } }
        #endregion

        #region Serialization for JSON
        public JObject serialize()
        {
            JObject json = new();
            json["id"] = id;
            json["idChatFriendship"] = idChatFriendship;
            json["idAuthorMessage"] = idAuthorMessage;
            json["idUserReceived"] = idUserReceived;

            /* BodyComment Element */
            JObject bodyMessageJson = new();
            bodyMessageJson["text"] = bodyMessage.text;
            bodyMessageJson["code"] = bodyMessage.code;
            bodyMessageJson["language"] = bodyMessage.language;
            json["body"] = bodyMessageJson;

            json["date"] = date.ToString("dd/MM/yyyy HH:mm:ss");
            json["updateDate"] = updateDate.ToString("dd/MM/yyyy HH:mm:ss");

            return json;
        }
        #endregion
    }
}
