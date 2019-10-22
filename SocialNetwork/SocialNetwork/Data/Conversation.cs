using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
    public class Conversation : IComparable<Conversation>, IComparable, IComparer<Conversation>
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
            
            return (c1.member1 == c2.member1 && c1.member2 == c2.member2) || (c1.member1 == c2.member2 && c1.member2 == c2.member1) ? true : false;
        }

        public static bool operator !=(Conversation c1, Conversation c2)
        {
            if (object.ReferenceEquals(c1, null))
                return !object.ReferenceEquals(c2, null);
            else if(object.ReferenceEquals(c2, null))
                return true;

            return (c1.member1 != c2.member1 || c1.member2 != c2.member2) && (c1.member1 != c2.member2 || c1.member2 != c2.member1) ? true : false;
        }

        public override bool Equals(object obj) =>
            obj is Conversation conversation &&
                ((member1 == conversation.member1 &&
                member2 == conversation.member2) ||
                (member1 == conversation.member2 &&
                member2 == conversation.member1));

        public override int GetHashCode()
        {
            var hashCode = 1206811878;
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(member1);
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(member2);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Message>>.Default.GetHashCode(messages);
            return hashCode;
        }

        public int CompareTo(object obj)
        {
            return this.member1.Name.CompareTo((obj as Conversation).member1);
        }

        public int CompareTo(Conversation other)
        {
            return this.member1.Name.CompareTo(other.member1);
        }

        public int Compare(Conversation x, Conversation y)
        {
            Message m1 = x.messages[x.messages.Count - 1];
            Message m2 = x.messages[x.messages.Count - 1];

            DateTime d1 = m1.DateTime;
            DateTime d2 = m2.DateTime;

            if (d1 > d2)
                return 1;
            if (d2 > d1)
                return -1;
            else
                return 0;
        }
    }
}
