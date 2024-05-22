using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_C_Sharp.Model.Post
{
    public class Comment
    {
        #region Attributes
        private int id;
        private int idAuthorComment;
        private int idPost;
        public string text;
        public int likes;
        private DateTime date;
        public DateTime updateDate;
        private List<int> likesIdUser;
        #endregion

        #region Constructor
        public Comment(int id, int idAuthorComment, int idPost, string text)
        {
            this.id = id;
            this.idAuthorComment = idAuthorComment;
            this.idPost = idPost;
            this.text = text;
            this.date = DateTime.Now;
            this.updateDate = DateTime.Now;
            this.likesIdUser = new();
        }
        #endregion

        #region Gets e Sets
        public int getId { get { return id; } }

        public int getIdAuthorComment { get { return idAuthorComment; } }

        public int getIdPost { get { return idPost; } }

        public int setIdPost { set { idPost = value; } }

        public string getText() { return text; }

        public string setText { set { text = value; } }

        public int getLikes { get { return likes; } }

        public DateTime getDate { get { return date; } }

        public DateTime getUpdateDate { get { return updateDate; } }

        public DateTime setUpdateDate { set { updateDate = value; } }

        public List<int> getLikesIdUser { get { return likesIdUser; } }

        public List<int> setLikesIdUser { set { likesIdUser = value; } }
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
        #endregion

        #region Serialization for JSON
        public JObject serialize()
        {
            JObject json = new();
            json["id"] = id;
            json["idAuthorComment"] = idAuthorComment;
            json["idPost"] = idPost;
            json["text"] = text;
            json["likes"] = likes;
            json["date"] = date.ToString("dd/MM/yyyy");
            json["updateDate"] = updateDate.ToString("dd/MM/yyyy");

            JArray likesIdUser = new();
            foreach (int id in likesIdUser)
                likesIdUser.Add(id);

            json["likesIdUser"] = likesIdUser;

            return json;
        }
        #endregion
    }
}
