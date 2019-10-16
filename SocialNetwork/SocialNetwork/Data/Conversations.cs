using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SocialNetwork.Data
{
    class Conversations
    {
        public static void AddConversations(IEnumerable<Conversation> newConversations) =>
            conversations.AddRange(newConversations);

        private static List<Conversation> conversations = new List<Conversation>();

        public static IEnumerable<Conversation> GetConversationsByUser(User user)
        {
            IEnumerable<Conversation> Conversation = from Conversation c in conversations
                                                     where c.member1 == user || c.member2 == user
                                                     select c;
            Debug.WriteLine("There are " + Conversation.Count() + " conversations with " + user.Name);
            return Conversation;
        }
    }
}
