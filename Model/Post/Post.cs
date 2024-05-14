using API_C_Sharp.Model.User;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace API_C_Sharp.Model.Post
{
    public class Post
    {
        private int id;
        private int idAuthor;
        public string title;
        public string body;
        private DateTime date;
        public DateTime updateDate;
        private int likes;
        private List<Comment> comments;
        private List<Image> images;

        public Post(int id, int idAuthor, string title, string body)
        {
            this.id = id;
            this.idAuthor = idAuthor;
            this.title = title;
            this.body = body;
            this.date = DateTime.Now;
            this.updateDate = DateTime.Now;
            this.likes = 0;
            this.comments = new();
            this.images = new();
        }

        public void addLike() { }

        public void removeLike() { }

        public List<Comment> Comment
        {
            get { return []; }
            set { }
        }

        public List<JObject> serializeComments() { return []; }

        public int getId { get { return id; } }

        public int setId { set { id = value; } }

        public string[] image { get { return []; } set { } }

        public DateTime Date { get { return date; } }

        public JObject serialize()
        {
            JObject json = new JObject();

            json["author"] = idAuthor;
            json["title"] = title;
            json["body"] = body;
            json["date"] = date.ToString("dd/MM/yyyy");
            json["updateDate"] = updateDate.ToString("dd/MM/yyyy");
            json["likes"] = likes;

            JArray commentsList = new();
            foreach (Comment comment in comments)
                commentsList.Add(comment.serialize());

            json["comments"] = commentsList;


            // verificar sobre questão de armazenar imagens com base64
            //JArray imagesList = new();
            //foreach(Image image in images)
            //    imagesList.Add(image.serialize());

            //json["images"] = imagesList;

            return json;
        }
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

        public JObject serialize()
        {
            JObject json = new();

            json["author"] = author.name;
            json["text"] = text;
            json["date"] = date.ToString("dd/MM/yyyy");
            json["updateDate"] = updateDate.ToString("dd/MM/yyyy");

            return json;
        }
    }

}
