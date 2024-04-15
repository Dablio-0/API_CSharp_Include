using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Model.Post
{
    public class Post
    {
        private User.User author;
        public string Title;
        public string body;
        private DateTime date;
        public DateTime updateDate;
        private int likes;
        private List<Comment> comments;
        private List<Image> images;

        public void addLike() { }

        public void removeLike() { }

        public List<Comment> Comment
        {
            get { return []; }
            set { }
        }

        public List<JObject> serializeComments() { return []; }

        public string[] image { get { return []; } set { } }

        public DateTime Date { get { return date; } }
    }

    class Image()
    {

    }

    public class Comment
    {
        private User.User author;
        public string text;
        private DateTime date;
        public DateTime updateDate;

        public DateTime Date { get { return date; } }

        public JObject serialize { get { return []; } }
    }
}
