using API_C_Sharp.Model.Post;
using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Model.User.Chat
{
    public class Chat
    {
        private List<Message> messageList;

        public Chat()
        {
            this.messageList = new List<Message>();
        }

        public List<Message> getMessageList { get { return messageList; } }

        public List<Message> setMessageList { set { messageList = value; } }

        public bool deleteMessage(int item) { return true; }
    }
}
