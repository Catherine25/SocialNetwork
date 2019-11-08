using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNetwork.Data.Database
{   
    public class LocalData
    {
        private List<User> users;
        private List<Group> groups;
        private List<Tuple<int, int>> friends;
        private List<Tuple<int, int>> users_groups;
        private List<Conversation> conversations;
        private List<ConversationData> conversationsData;
        private List<Message> messages;
        private List<MessageData> messagesData;

        public List<User> Users { get => users; set => users = value; }
        public List<Group> Groups { get => groups; set => groups = value; }
        public List<Tuple<int, int>> Friends { get => friends; set => friends = value; }
        public List<Tuple<int, int>> Users_Groups { get => users_groups; set => users_groups = value; }
        public List<Conversation> Conversations { get => conversations; set => conversations = value; }
        public List<ConversationData> ConversationsData { get => conversationsData; set => conversationsData = value; }
        public List<Message> Messages { get => messages; set => messages = value; }
        public List<MessageData> MessagesData { get => messagesData; set => messagesData = value; }

        public void LoadFromMessagesData(List<MessageData> messagesData)
        {
            foreach (var item in messagesData)
                Messages.Add(new Message(
                    item.m_id,
                    item.text,
                    item.dt,
                    item.isFromMember1));
        }

        public List<User> FindFriendsOfUser(User user)
        {
            List<User> friends = new List<User>();

            List<Tuple<int, int>> list1 = Friends.FindAll(X => X.Item1 == user.Id);
            List<Tuple<int, int>> list2 = Friends.FindAll(X => X.Item2 == user.Id);

           List<int> ids = new List<int>();
            ids.AddRange(list1.Select(x => x.Item2));
            ids.AddRange(list2.Select(x => x.Item1));

            foreach(int id in ids)
                friends.Add(Users.Find(x => x.Id == id));

            return friends;
        }

        public void ConvertIntoLocalClasses()
        {
            Conversations = new List<Conversation>();
            foreach (ConversationData cd in ConversationsData)
            {
                Conversation conversation = new Conversation (
                    cd.c_id,
                    Users.Find(c=>c.Id == cd.u1_id),
                    Users.Find(c=>c.Id == cd.u2_id));
                Conversations.Add(conversation);
            }

            foreach (MessageData md in MessagesData)
            {
                Message message = new Message(md.m_id, md.text, md.dt, md.isFromMember1);
                Conversation existingConversation = Conversations.Find(c => c.Id == md.c_id);
                List<Message> existingMessages = existingConversation.messages;
                
                if(existingMessages != null)
                    existingMessages.Add(message);
                else
                    existingMessages = new List<Message> { message };

                existingConversation.messages = existingMessages;
            }
        }

        public List<Group> FindGroupsOfUser(User user)
        {
            List<Group> groups = new List<Group>();

            List<Tuple<int, int>> list1 = Users_Groups.FindAll(X => X.Item1 == user.Id);
            
            List<int> ids = list1.Select(x => x.Item2).ToList();
            
            foreach (int id in ids)
                groups.Add(Groups.Find(x => x.Id == id));

            return groups;
        }

        public User FindUserByName(string name) => Users.Find(X => X.Name == name);
    }
}
