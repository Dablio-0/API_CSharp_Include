
using API_C_Sharp.Model.Post;
using API_C_Sharp.Model.User;
using API_C_Sharp.Model.User.Chat;
using System.Security.Cryptography.X509Certificates;

namespace API_C_Sharp.Model
{
    public class Data
    {
        private List<User.User> usersList;
        private List<Post.Post> postsList;
        private List<Friendship> friendshipsList;
        private List<Comment> commentsList;
        private List<Message> messageList;
        private int currentUser = -1;

        public Data()
        {
            usersList = new();
            friendshipsList = new();
            postsList = new();
            commentsList = new();
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

        public void removeUser(int id)
        {
            usersList.Remove(this.getUserById(id));
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

        #region Data Friendship Methods
        public int addFrienship(int idInviter, int userInvited, FriendshipStatus status, Chat chat)
        {
            int ID = friendshipsList.Count();

            friendshipsList.Add(new Friendship(ID, idInviter, userInvited, status, chat));

            return ID;
        }

        public void deleteFriendship(int id)
        {
            friendshipsList.Remove(this.getFriendshipById(id));
        }

        public Friendship modifyFriendshipStatus(int idFriendship, FriendshipStatus status)
        {
            // Obtém a amizade pelo ID
            Friendship friendship = this.getFriendshipById(idFriendship);

            // Verifica se a amizade existe
            if (friendship == null)
            {
                throw new Exception("A amizade não foi encontrada.");
            }

            // Atualiza o status da amizade
            friendship.setStatus = status;

            // Retorna a amizade atualizada
            return friendship;
        }

        public List<Friendship> getFriendships()
        {
            return friendshipsList;
        }

        public Friendship getFriendshipById(int id)
        {
            return friendshipsList.Find(friendship => friendship.getId == id);
        }

        public List<User.User> getListNotFriends(User.User invitedUser)
        {
            List<User.User> notFriends = new List<User.User>();

            foreach (User.User user in usersList)
            {
                if (!user.getFriends.Contains(invitedUser) && user.getId != invitedUser.getId)
                {
                    notFriends.Add(user);
                }
            }

            return notFriends;
        }


        public List<Friendship> getFriendshipsPending()
        {
            return friendshipsList.FindAll(friendshipsList => friendshipsList.getStatus == FriendshipStatus.pending);
        }

        public List<Friendship> getFriendshipsAccepted()
        {
            return friendshipsList.FindAll(friendshipsList => friendshipsList.getStatus == FriendshipStatus.accepted);
        }

        public List<Friendship> getFriendshipsDeclined()
        {
            return friendshipsList.FindAll(friendshipsList => friendshipsList.getStatus == FriendshipStatus.declined);
        }
        #endregion

        #region Data Meessage Methods
        public int addMessage(int idChatFriendshp, int idAuthorMessage, int idUserReceiced, BodyMessage bodyMessage)
        {
            int ID = messageList.Count();

            messageList.Add(new Message(ID, idChatFriendshp, idAuthorMessage, idUserReceiced, bodyMessage));

            return ID;
        }

        public void deleteMessage(int id)
        {
            messageList.Remove(this.getMessageById(id));
        }

        public Message getMessageById(int id)
        {
            return messageList.Find(message => message.getId == id);
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

        public int addPost(int idAuthor, string title, BodyContent body)
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
        public int addComment(int idAuthor, int idPost, BodyCommentContent bodyComment)
        {
            int ID = commentsList.Count();

            commentsList.Add(new Comment(ID, idAuthor, idPost, bodyComment));

            return ID;
        }

        public void deleteComment(int id)
        {
            commentsList.Remove(this.getCommentById(id));
        }

        public List<Comment> getAllComments()
        {
            return commentsList;
        }

        public List<Comment> getCommentsByPost(int idPost)
        {
            List<Comment> commentsListByPost = new();
            commentsList = this.getAllComments();
            commentsList.FindAll(comment => comment.getIdPost == idPost);

            return commentsListByPost;
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

        public Comment removeCommentLikeByUser(int id)
        {
            Comment comment = getCommentById(id);
            comment.removeLike();
            return comment;
        }
        #endregion
    }
}