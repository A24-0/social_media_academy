﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace academy_project
{
    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public Message(string sender, string content)
        {
            Sender = sender;
            Content = content;
        }
    }
}