using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace academy_project
{
    public class Chat
    {
        public string Name { get; set; }
        public List<Message> Messages { get; set; }

        public Chat(string name, List<Message> messages)
        {
            Name = name;
            Messages = messages;
        }
        public Chat(string name)
        {
            Name = name;
        }
    }
}
