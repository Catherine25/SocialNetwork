using SocialNetwork.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SocialNetwork.Data
{
    static class Messages
    {
        static Messages()
        {
        }

        public static void AddMessages(IEnumerable<Conversation> newConversations) => conversations.AddRange(newConversations);

        private static List<Conversation> conversations = new List<Conversation>();

        public static IEnumerable<Conversation> GetConversationsByUser(User user)
        {
            IEnumerable<Conversation> conversation = from Conversation c in conversations
                                           where c.member1 == user || c.member2 == user
                                           select c;
            return conversation;
        }
    }
}
