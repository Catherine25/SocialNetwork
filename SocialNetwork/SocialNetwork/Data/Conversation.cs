using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
    public class Conversation
    {
        public Conversation(User user1, User user2, List<Message> newMessages)
        {
            member1 = user1;
            member2 = user2;

            messages = newMessages ?? throw new Exception();
        }

        public readonly User member1;
        public readonly User member2;
        public readonly List<Message> messages;

        public static bool operator ==(Conversation c1, Conversation c2)
        {
            if (object.ReferenceEquals(c1, null))
                return object.ReferenceEquals(c2, null);
            else if(object.ReferenceEquals(c2, null))
                return false;
            
            return c1.member1 == c2.member1 && c1.member2 == c2.member2 ? true : false;
        }

        public static bool operator !=(Conversation c1, Conversation c2)
        {
            if (object.ReferenceEquals(c1, null))
                return !object.ReferenceEquals(c2, null);
            else if(object.ReferenceEquals(c2, null))
                return true;

            return c1.member1 != c2.member1 || c1.member2 != c2.member2 ? true : false;
        }

        public override bool Equals(object obj) =>
            obj is Conversation conversation &&
                member1 == conversation.member1 &&
                member2 == conversation.member2;

        public override int GetHashCode()
        {
            var hashCode = 1206811878;
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(member1);
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(member2);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Message>>.Default.GetHashCode(messages);
            return hashCode;
        }
    }
}
