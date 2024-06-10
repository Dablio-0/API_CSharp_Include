using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Model.User
{
    public enum FriendshipStatus
    {
        pending,
        accepted,
        declined,
        blocked,
        terminated
    }

    public class Friendship
    {
        private int id;
        private int idInviter;
        private int idInvited;
        public FriendshipStatus status;
        private Chat.Chat chat;

        public Friendship(int id, int idInviter, int idInvited, FriendshipStatus status, Chat.Chat chat)
        {
            this.id = id;
            this.idInviter = idInviter;
            this.idInvited = idInvited;
            this.status = FriendshipStatus.pending;
            this.chat = chat;
        }

        public int getId { get { return id; } }

        public int setId { set { id = value; } }

        public int getIdInviter { get { return idInviter; } }

        public int setIdInviter { set { idInviter = value; } }

        public int getIdInvited { get { return idInvited; } }

        public int setIdInvited { set { idInvited = value; } }

        public FriendshipStatus getStatus { get { return status; } }

        public FriendshipStatus setStatus { set { status = value; } }

        public Chat.Chat getChat { get { return chat; } }

        public Chat.Chat setChat { set { chat = value; } }

        public Boolean End() { return true; }

        public Boolean check(int inviter, int invited) { return true; }


        #region Serialization for JSON
        public JObject serialize()
        {
            JObject json = new JObject();

            json["inviter"] = idInviter;
            json["invited"] = idInvited;
            json["status"] = status.ToString();
            json["chat"] = null;

            return json;
        }
        #endregion
    }
}