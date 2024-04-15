using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Model.User
{
    public class Chat
    {
        private List<Message> messageList;

        public Message[] messages { get { return []; } set { } }

        public Boolean deleteMessage(int item) { return true; }
    }

    public class Message
    {
        private int owner;
        private string text;

        public JObject serialize { get; }
    }
}
