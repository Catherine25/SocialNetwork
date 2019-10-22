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

            DateTime d1 = m1.DateTime;
            DateTime d2 = m2.DateTime;

            if (d1.Ticks > d2.Ticks)
                return 1;
            if (d2.Ticks > d1.Ticks)
                return -1;
            else
                return 0;
            
        }
    }
}
