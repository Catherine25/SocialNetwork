using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
    public class Message
    {
        public Message(int id, string text, DateTime dateTime, bool isFromMember1)
        {
            Id = id;
            Text = text;
            DateTime = dateTime;
            IsFromMember1 = isFromMember1;
        }

        public int Id;
        public string Text;
        public DateTime DateTime;
        public bool IsFromMember1;

        public override string ToString() => "[" + DateTime + "] " + Text + " isFromMember1: " + IsFromMember1;
    }
}
