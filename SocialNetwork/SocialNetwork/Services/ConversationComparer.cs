using System;
using System.Collections.Generic;
using System.Text;
using SocialNetwork.Data;

namespace SocialNetwork.Services
{
    class ConversationComparer : IComparer<Data.Conversation>
    {
        public int Compare(Conversation x, Conversation y)
        {
            Message m1 = x.messages[x.messages.Count - 1];
            Message m2 = x.messages[x.messages.Count - 1];

            DateTime dateTime1 = m1.DateTime;
            DateTime dateTime2 = m2.DateTime;


        }
    }
}
