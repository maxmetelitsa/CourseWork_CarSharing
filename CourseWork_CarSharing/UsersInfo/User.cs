using System.Collections.Generic;

namespace CourseWork_CarSharing.UsersInfo
{
    public class User
    {
        private static int lastID = 0;
        public int ID { get; }
        string name;
        string surname;
        string email;
        string password;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public User(string name, string surname, string email, string password)
        {
            ID = ++lastID;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
        }
        public static List<User> CreateListOfUsers()
        {
            List <User> users = new List<User>();
            return users;
        }
        public void AddUser(List<User> users, User user)
        {
            if (user != null)
            {
                users.Add(user);
            }
        }

        public void RemoveUser(List<User> users, User user)
        {
            if (user != null)
            {
                users.Remove(user);
            }
        }

        public User GetUserByID(List<User> users, int id)
        {
            return users.Find(u => u.ID == id);
        }

        public List<User> GetUsersByName(List<User> users, string name)
        {
            return users.FindAll(u => u.Name == name);
        }

        public List<User> GetUsersBySurname(List<User> users, string surname)
        {
            return users.FindAll(u => u.Surname == surname);
        }

        public List<User> GetUsersByEmail(List<User> users, string email)
        {
            return users.FindAll(u => u.Email == email);
        }

        public List<User> GetUsersByPassword(List<User> users, string password)
        {
            return users.FindAll(u => u.Password == password);
        }
    }
}
