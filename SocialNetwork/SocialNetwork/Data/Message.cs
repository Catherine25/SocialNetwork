using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
    public class Message
    {
        public Message(string text, DateTime dateTime, bool isFromMember1)
        {
            Text = text;
            DateTime = dateTime;
            IsFromMember1 = isFromMember1;
        }
        
        public string Text;
        public DateTime DateTime;
        public bool IsFromMember1;
    }
}
