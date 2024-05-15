
using API_C_Sharp.Model.User;

namespace API_C_Sharp.Model
{
    public class Data
    {
        private List<User.User> usersList;
        private List<Friendship> friendshipsList;
        private List<Post.Post> postsList;
        private int currentUser = -1;

        public Data()
        {
            usersList = new();
            friendshipsList = new();
            postsList = new();
        }

        public void alimentaAi()
        {
            this.addUser("Usuário 1", "usuario1@gmail.com", "123");
            this.addUser("Usuário 2", "usuario2@gmail.com", "123");
            this.addUser("Usuário 3", "usuario3@gmail.com", "123");
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

        public int getCurrentUser()
        {
            return this.currentUser;
        }

        public void logout()
        {
            this.currentUser = -1;
        }

        public User.User getUserByLogin(string email)
        {
            return usersList.Find(user => user.checkEmail(email));
        }

        public User.User getUserById(int id)
        {
            return usersList.Find(user => user.getId == id);
        }

        public List<User.User> getUsers()
        {
            return usersList;
        }

        public int addPost(int idAuthor, string title, string body)
        {
            int ID = postsList.Count();

            postsList.Add(new Post.Post(ID, idAuthor, title, body));

            return ID;
        }

        public Post.Post getPostById(int id)
        {
            return postsList.Find(post => post.getId == id);
        }

        public List<Post.Post> getPosts()
        {
            return postsList;
        }


    }
}