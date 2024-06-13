using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Model.User
{
    #region Status for a Friendship (enum class)
    public enum FriendshipStatus
    {
        pending,
        accepted,
        declined,
        blocked
    }
    #endregion

    public class Friendship
    {
        #region Attributes
        private int id;
        private int idInviter;
        private int idInvited;
        public FriendshipStatus status;
        #endregion

        #region Constructor
        public Friendship(int id, int idInviter, int idInvited, FriendshipStatus status)
        {
            this.id = id;
            this.idInviter = idInviter;
            this.idInvited = idInvited;
            this.status = FriendshipStatus.pending;
        }
        #endregion

        #region Gets e Sets
        public int getId { get { return id; } }

        public int setId { set { id = value; } }

        public int getIdInviter { get { return idInviter; } }

        public int setIdInviter { set { idInviter = value; } }

        public int getIdInvited { get { return idInvited; } }

        public int setIdInvited { set { idInvited = value; } }

        public FriendshipStatus getStatus { get { return status; } }

        public FriendshipStatus setStatus { set { status = value; } }
        #endregion 

        #region Serialization for JSON
        public JObject serialize()
        {
            JObject json = new JObject();

            json["inviter"] = idInviter;
            json["invited"] = idInvited;
            json["status"] = status.ToString();

            return json;
        }
        #endregion
    }
}