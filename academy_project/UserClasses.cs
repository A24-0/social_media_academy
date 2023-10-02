using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace academy_project
{
    public class Message
    {
        public string value;
        public int id_author;
        public string author;
        public DateTime date_send;
        public Message(string value, int id_author, string author, DateTime date_send)
        {
            this.value = value;
            this.id_author = id_author;
            this.author = author;
            this.date_send = date_send;
        }
    }
    public class Chat
    {
        public string name;
        public Dictionary<int, Message> messages;
        public Dictionary<int, User> users;
        public DateTime? date_check = null;
        public bool new_messages = false;
        public Chat(string name)
        {
            this.name = name;
            this.messages = new Dictionary<int, Message>();
            this.users = new Dictionary<int, User>();
        }
        public Chat(string name, Dictionary<int, Message> messages, Dictionary<int, User> users)
        {
            this.name = name;
            this.messages = messages;
            this.users = users;
        }
        //Message
        public void Add_to_dictionary_message(int message_id, Message message)
        {
            messages.Add(message_id, message);
        }
        public bool ContainsMessageId(int message_id)
        {
            return messages.ContainsKey(message_id);
        }
        //Date
        public DateTime? Get_date_check()
        {
            return this.date_check;
        }
        public void Set_date_check(DateTime date_check)
        {
            this.date_check = date_check;
        }
        //Chat
        public string Get_chat_name()
        {
            return this.name;
        }
        //User
        public void Add_to_dictionary_user(int user_id, string user_name)
        {
            users.Add(user_id, new User(user_id, user_name));
        }
        public void Add_to_dictionary_user(int user_id, User user)
        {
            users.Add(user_id, user);
        }
        public bool ContainsUserId(int message_id)
        {
            return users.ContainsKey(message_id);
        }
    }
    public class User
    {
        public int id;
        public string name;
        public User(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public string get_name()
        {
            return name;
        }
    }
}
