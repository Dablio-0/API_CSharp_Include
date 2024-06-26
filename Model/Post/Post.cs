﻿using API_C_Sharp.Model.User;
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
        public BodyContent body;
        private DateTime date;
        private DateTime updateDate;
        private int likes;
        private List<Comment> comments;
        private List<string> images;
        private List<int> likesIdUser;
        #endregion

        #region Constructor
        public Post(int id, int idAuthor, string title, BodyContent body)
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
            this.likesIdUser = new();
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

            /* Body Element */
            JObject bodyJson = new();
            bodyJson["text"] = body.text;
            bodyJson["code"] = body.code;
            bodyJson["language"] = body.language;
            bodyJson["image"] = body.image;
            json["body"] = bodyJson;

            json["date"] = date.ToString("dd/MM/yyyy HH:mm:ss");
            json["updateDate"] = updateDate.ToString("dd/MM/yyyy HH:mm:ss");
            json["likes"] = likes;

            JArray commentsList = new();
            foreach (Comment comment in comments)
                commentsList.Add(comment.serialize());

            json["comments"] = commentsList;

            JArray imagesList = new();
            foreach (string image in images)
                imagesList.Add(image);

            json["images"] = imagesList;

            JArray likesIdUserList = new();
            foreach (int id in likesIdUser)
                likesIdUserList.Add(id);

            json["likesIdUser"] = likesIdUserList;

            return json;
        }
        #endregion
    }
}
