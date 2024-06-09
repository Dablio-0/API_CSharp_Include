using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Model.User.Chat
{
    public class Chat
    {
        private List<Message> messageList;

        public Message[] messages { get { return []; } set { } }

        public bool deleteMessage(int item) { return true; }
    }
}
