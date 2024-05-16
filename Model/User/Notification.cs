using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Model.User
{

    public enum NotificationType
    {
        Invite,
        Message,
        Post,
        Comment,
    }

    public class Notification
    {
        public NotificationType type;
        public DateTime sendDate;
        public string title;
        public string description;
        public Boolean open;

        public JObject serialize()
        {
            JObject json = new();

            switch (type)
            {
                case NotificationType.Invite: json["type"] = "invite"; break;
                case NotificationType.Message: json["type"] = "mssage"; break;
                case NotificationType.Post: json["type"] = "post"; break;
                case NotificationType.Comment: json["type"] = "comment"; break;
            };

            json["sendData"] = sendDate.ToString("dd/MM/yyyy 00:00:00");
            json["title"] = title;
            json["description"] = description;
            json["open"] = open;

            return json;
        }
    }
}