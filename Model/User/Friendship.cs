namespace API_C_Sharp.Model.User
{
    public enum FriendshipStatus
    {
        Pending,
        Accepted,
        Declined
    }

    public class Friendship
    {

        private int inviter;
        private int invited;
        public FriendshipStatus status;
        private Chat chat;

        public Boolean End() { return true; }

        public Boolean check(int inviter, int invited) { return true; }

        public Chat Chat { get { return chat; } }
    }
}