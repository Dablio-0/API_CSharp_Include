
using API_C_Sharp.Model.Post;
using API_C_Sharp.Model.User;
using System.Security.Cryptography.X509Certificates;

namespace API_C_Sharp.Model
{
    public class Data
    {
        private List<User.User> usersList;
        private List<Friendship> friendshipsList;
        private List<Post.Post> postsList;
        private List<Comment> commentsList;
        private int currentUser = -1;

        public Data()
        {
            usersList = new();
            friendshipsList = new();
            postsList = new();
            commentsList = new();
        }

        public void alimentaAi()
        {
            this.addUser("Usu�rio 1", "usuario1@gmail.com", "123");
            this.addUser("Usu�rio 2", "usuario2@gmail.com", "123");
            this.addUser("Usu�rio 3", "usuario3@gmail.com", "123");
        }

        #region Data Users Methods
        public List<User.User> getUsers()
        {
            return usersList;
        }

        public User.User getUserByLogin(string email)
        {
            return usersList.Find(user => user.checkEmail(email));
        }

        public User.User getUserById(int id)
        {
            return usersList.Find(user => user.getId == id);
        }

        public int getCurrentUser()
        {
            return this.currentUser;
        }

        public int addUser(string name, string email, string password)
        {
            if (getUserByLogin(email) != null)
                return -1;
            else
            {
                int ID = usersList.Count();

                usersList.Add(new User.User(ID, name, email, password));

                return ID;
            }
        }

        public void login(int userId)
        {
            this.currentUser = userId;
        }

        public void logout()
        {
            this.currentUser = -1;
        }
        #endregion

        #region Data Post Methods
        public List<Post.Post> getPosts()
        {
            return postsList;
        }

        public Post.Post getPostById(int id)
        {
            return postsList.Find(post => post.getId == id);
        }

        public List<Post.Post> getListPostByUser(int id)
        {
            List<Post.Post> posts = new();

            foreach (Post.Post post in postsList)
                if (post.getIdAuthor == id)
                    posts.Add(post);

            return posts;
        }

        public int addPost(int idAuthor, string title, string body)
        {
            int ID = postsList.Count();

            postsList.Add(new Post.Post(ID, idAuthor, title, body));

            return ID;
        }

        public void deletePost(int id)
        {
            postsList.Remove(getPostById(id));
        }

        public Post.Post addPostLikeByUser(int id)
        {
            Post.Post post = getPostById(id);
            post.addLike();
            return post;
        }

        public Post.Post removePostLikeByUser(int id)
        {
            Post.Post post = getPostById(id);
            post.removeLike();
            return post;
        }
        #endregion
          
        #region Data Comment Methods
        public int addComment(int idAuthor, int idPost, string text)
        {
            int ID = commentsList.Count();

            commentsList.Add(new Comment(ID, idAuthor, idPost, text));

            return ID;
        }

        public List<Comment> getAllComments()
        {
            return commentsList;
        }

        public List<Comment> getCommentsByPost(int idPost)
        {
            List<Comment> commentsListByPost = new();
            commentsList = this.getAllComments();
            commentsList.Find(comment => comment.getIdPost == idPost);

            return commentsListByPost;
        }

        public void deleteComment(int id)
        {
            commentsList.Remove(getCommentById(id));
        }

        public Comment getCommentById(int id)
        {
            return commentsList.Find(comment => comment.getId == id);
        }

        public Comment addCommentLikeByUser(int id)
        {
            Comment comment = getCommentById(id);
            comment.addLike();
            return comment;
        }

        public Comment remvoveCommentLikeByUser(int id)
        {
            Comment comment = getCommentById(id);
            comment.removeLike();
            return comment;
        }
        #endregion
    }
}