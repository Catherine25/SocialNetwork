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

        public static void AddMessages(IEnumerable<Message> newMessages) => messages.AddRange(newMessages);

        private static List<Message> messages = new List<Message>();

        public static IEnumerable<Message> GetMessagesByUser(User user)
        {
            IEnumerable<Message> message = from Message m in messages
                                           where m.Reciever == user || m.Sender == user
                                           select m;
            return message;
        }
    }
}
