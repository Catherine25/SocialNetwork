using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
    public class Conversation
    {
        public Conversation(int id, User user1, User user2)
        {
            Id = id;
            member1 = user1;
            member2 = user2;
        }

        public readonly int Id;
        public readonly User member1;
        public readonly User member2;
        public List<Message> messages;
    }
}
