using API_C_Sharp.Model.User;
using API_C_Sharp.Utils;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace API_C_Sharp.Model.Post
{
    public class Post
    {
        #region Attributes
        private int id;
        private int idAuthor;
        public string title;
        public string body;
        private DateTime date;
        private DateTime updateDate;
        private int likes;
        private List<Comment> comments;
        private List<string> images;
        #endregion

        #region Constructor
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
        #endregion

        #region Gets e Sets
        public int getId { get { return id; } }

        public int setId { set { id = value; } }

        public int getIdAuthor { get { return idAuthor; } }

        public int setIdAuthor { set { idAuthor = value; } }

        public string[] image { get { return []; } set { } }

        public DateTime getDate { get { return date; } }

        public DateTime getUpdateDate { get { return updateDate; } }

        public DateTime setUpdateDate { set { updateDate = value; } }

        public int getLikes { get { return likes; } }

        public List<Comment> getCommentList { get { return comments; } }

        public List<Comment> setCommentList { set { comments = value; } }

        public List<string> getImageList { get { return images; } }

        public List<string> setImageList { set { images = value; } }
        #endregion

        #region Methods
        public void addLike()
        {
            this.likes++;
        }

        public void removeLike()
        {
            this.likes--;
        }

        public User.User getUser(Data data)
        {
            return data.getUserById(idAuthor);
        }
        #endregion

        #region Serialization for JSON
        public JObject serialize()
        {
            JObject json = new JObject();

            json["id"] = id;
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

            JArray imagesList = new();
            foreach (string image in images)
                imagesList.Add(image);

            json["images"] = imagesList;

            return json;
        }
        #endregion
    }
}
