using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
    class Conversation
    {
        public Conversation(User user1, User user2, List<Message> newMessages)
        {
            member1 = user1;
            member2 = user2;

            if (newMessages != null)
                if (newMessages.Count != 0)
                    messages = newMessages;
                else throw new Exception();
            else throw new Exception();
        }
        public readonly User member1;
        public readonly User member2;
        public readonly List<Message> messages;
    }
}
