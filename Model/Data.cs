
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
            this.addUser("Leandro", "leandromeirelles@gmail.com", "123");
            this.addUser("Lucas", "lucas@gmail.com", "123");
            this.addUser("Rogono", "rogono@gmail.com", "123");
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

        public User.User getUserByLogin(string email)
        {
            return usersList.Find(user => user.checkEmail(email));
        }

        public User.User getUserById(int id)
        {
            return usersList.Find(user => user.Id == id);
        }

        public List<User.User> getUsers()
        {
            return usersList;
        }
    }
}