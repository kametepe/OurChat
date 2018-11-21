﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurChat.Models
{
    public class ChatMessage
    {

        public string UserName { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }

        public string UserImage { get; set; }
    }
}
