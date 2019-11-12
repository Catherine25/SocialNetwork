using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SocialNetwork.Data.Database
{   
    public class LocalData
    {
        public List<User> Users;
        public List<Group> Groups;
        public List<Tuple<int, int>> Friends;
        public List<Tuple<int, int>> Users_Groups;
        public List<Conversation> Conversations;
        public List<ConversationData> ConversationsData;
        public List<Message> Messages;
        public List<MessageData> MessagesData;

        public LocalData()
        {

        }

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

        public void AddEmptyConversation(User u1, User u2)
        {
            if (Conversations.Any(c => (c.member1 == u1 && c.member2 == u2) || (c.member1 == u2 && c.member2 == u1)))
                return;

            int freeId = 0;

            do
                freeId++;
            while (Conversations.Any(c => c.Id == freeId));
            
            Conversations.Add(new Conversation(freeId, u1, u2));
        }

        public void SyncWithServer(
            List<ConversationData> conversations,
            List<Group> groups,
            List<MessageData> md,
            List<Tuple<int, int>> newUserFriends,
            List<Tuple<int, int>> newUserGroups,
            List<User> newUsers)
        {
            List<Message> messages = _localData.ConvertIntoLocalClasses();
        }
    }
}
