using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Model.User
{
    public enum FriendshipStatus
    {
        pending,
        accepted,
        declined
    }

    public class Friendship
    {

        private int inviter;
        private int invited;
        public FriendshipStatus status;
        private Chat chat;

        public Friendship(int inviter, int invited, FriendshipStatus status, Chat chat)
        {
            this.inviter = inviter;
            this.invited = invited;
            this.status = FriendshipStatus.pending;
            this.chat = chat;
        }

        public int getInviter { get { return inviter; } }

        public int setInviter { set { setInviter = value; } }

        public int getInvited { get { return invited; } }

        public int setInvited { set { invited = value; } }

        public FriendshipStatus getStatus { get { return status; } }

        public FriendshipStatus setStatus { set { status = value; } }

        public Boolean End() { return true; }

        public Boolean check(int inviter, int invited) { return true; }

        public Chat Chat { get { return chat; } }

        #region Serialization for JSON
        public JObject serialize()
        {
            JObject json = new JObject();

            json["inviter"] = inviter;
            json["invited"] = invited;
            json["status"] = status.ToString();
            json["chat"] = null;

            return json;
        }
        #endregion
    }
}