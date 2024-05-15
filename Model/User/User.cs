﻿using Newtonsoft.Json.Linq;

namespace API_C_Sharp.Model.User
{
    public class User
    {
        /* Attributes */
        private int id;
        public string name;
        private string email;
        private string password;
        public DateOnly birthDate;
        private List<string> skills;
        private List<string> jobs;
        private List<Notification> notifications;
        public Boolean status;

        /* Constructor */
        public User(int id, string name, string email, string password)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.password = password;

            skills = new List<string>();
            jobs = new List<string>();
            notifications = new List<Notification>();
        }

        /* Gets & Sets */

        public int getId { get { return id; } }

        public int setId { set { id = value; } }

        public string getName { get { return name; } }

        public string setName { set { name = value; } }

        public string getEmail { get { return email; } }

        public string setEmail { set { email = value; } }

        public string getPassword { get { return password; } }

        public string setPassword { set { password = value; } }

        public string getBirthDate { get { return birthDate.ToString("dd/MM/yyyy"); } }

        public string setBirthDate { set { birthDate = DateOnly.Parse(value); } }

        public List<string> getSkills { get { return skills; } }

        public List<string> setSkills { set { skills = value; } }

        public List<string> getJobs { get { return jobs; } }

        public List<string> setJobs { set { jobs = value; } }

        public bool checkNotification(int item) { return true; }

        public List<Notification> getNotifications { get { return notifications; } }

        public List<Notification> setNotifications { set { notifications = value; } }

        public void addNotification(Notification notification) { }

        public bool checkEmail(string email) { return email == this.email; }

        public bool checkPassword(string password) { return password == this.password; }

        public JObject serialize()
        {
            JObject json = new JObject();

            json["id"] = id;
            json["name"] = name;
            json["email"] = email;
            json["password"] = password;
            json["birthDate"] = birthDate.ToString("dd/MM/yyyy");

            JArray skillList = new JArray();
            foreach (string skill in skills)
                skillList.Add(skill);

            json["skills"] = skillList;

            JArray jobsList = new JArray();
            foreach (string job in jobs)
                jobsList.Add(job);

            json["jobs"] = jobsList;

            JArray notificationList = new JArray();
            foreach (Notification notification in notifications)
                notificationList.Add(notification.serialize());

            json["notifications"] = notificationList;

            json["status"] = status;

            return json;
        }
    }
}


