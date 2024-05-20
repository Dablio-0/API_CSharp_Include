using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_C_Sharp.Model.Post
{
    public class Comment
    {
        #region Attributes
        private int id;
        private int idAuthorComment;
        public string text;
        public int likes;
        private DateTime date;
        public DateTime updateDate;
        #endregion

        #region Constructor
        public Comment(int id, int idAuthorComment, string text)
        {
            this.id = id;
            this.idAuthorComment = idAuthorComment;
            this.text = text;
            this.date = DateTime.Now;
            this.updateDate = DateTime.Now;
        }
        #endregion

        #region Gets e Sets
        public int getId { get { return id; } }

        public int getIdAuthorComment { get { return idAuthorComment; } }

        public string getText() { return text; }

        public string setText { set { text = value; } }

        public int getLikes { get { return likes; } }

        public DateTime getDate { get { return date; } }

        public DateTime getUpdateDate { get { return updateDate; } }

        public DateTime setUpdateDate { set { updateDate = value; } }
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

            json["authorComment"] = idAuthorComment;
            json["text"] = text;
            json["date"] = date.ToString("dd/MM/yyyy");
            json["updateDate"] = updateDate.ToString("dd/MM/yyyy");

            return json;
        }
        #endregion
    }
}
