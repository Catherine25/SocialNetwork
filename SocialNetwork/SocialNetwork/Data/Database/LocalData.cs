using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SocialNetwork.Data.Database
{   
    public class LocalData
    {
        string connectionString;
        public LocalData()
        {
            string server = "35.180.63.12";
            string database = "Kate";
            string uid = "Admin";
            string password = "3555serg";

            connectionString =
                "SERVER=" + server +
                "; PORT = 3306 ;" +
                "DATABASE=" + database + ";" +
                "UID=" + uid + ";" +
                "PASSWORD=" + password + ";";

            _loader = new SQLLoader(connectionString);
            _publisher = new Publisher(connectionString);
        }

        public enum DataType { ConversationsData, Groups, MessagesData, UserFriends, UserGroups, Users, All }

        private User CurrentUser;
        private SQLLoader _loader;
        private Publisher _publisher;
        private List<Conversation> _conversations;
        private List<Message> _messages;
        
        
        #region Collections

        public List<User> GetUsers() => _loader.LoadUsers();
        public List<Group> GetGroups() => _loader.LoadGroups();
        public List<Tuple<int, int>> GetFriends() => _loader.LoadUserFriends();
        public List<Tuple<int, int>> GetUGroups() => _loader.LoadUserGroups();
        public List<Conversation> GetConversations()
        {
            ConvertIntoLocalClasses();
            return _conversations;
        }
        public List<ConversationData> GetConversationsData() => _loader.LoadConversationsData();
        public List<Message> GetMessages()
        {
            ConvertIntoLocalClasses();
            return _messages;
        }
        public List<MessageData> GetMessagesData() => _loader.LoadMessagesData();

        #endregion

        #region Single Update Request

        public void AddEmptyConversation(Conversation newC)
        {
            var conversations = GetConversations();
            if (conversations.Any(c =>
                (c.member1 == newC.member1 && c.member2 == newC.member2) ||
                (c.member1 == newC.member2 && c.member2 == newC.member1)))
                return;

            _publisher.PublishConversation(newC);
        }

        public void DeleteConversation(Conversation conversation) => _publisher.DeleteConversation(conversation);

        public void AddNewFriend(User u1, User u2)
        {
            var friends = FindFriendsOfUser(u1);
            if (friends.Any(f => f.Id == u2.Id))
                return;
            _publisher.PublishFriendship(u1, u2);
        }

        public void DeleteFriend(User u1, User u2) =>
            _publisher.DeleteFriendship(u1, u2);
        public void AddNewGroup(Group group)
        {
            var groups = GetGroups();
            if (groups.Any(g => g.Id == group.Id))
                return;
            _publisher.PublishGroup(group);
        }

        public void AddUserToGroup(User user, Group group)
        {
            var groups = FindGroupsOfUser(user);
            if (groups.Any(g => g.Id == group.Id))
                return;
            _publisher.PublishUserToGroup(user, group);
        }

        public void DeleteUserFromGroup(User user, Group group) =>
            _publisher.DeleteUserFromGroup(user, group);

        public void AddNewMessage(Message message, Conversation conversation)
        {
            message.Text = message.Text.Replace("'", "");
            _publisher.PublishMessage(conversation, message);
        }

        public void AddNewUser(User user) =>
            _publisher.PublishUser(user);
        public void UpdateUser(User oldUser, User user) =>
            _publisher.UpdateUser(oldUser, user);
        public void UpdateGroup(Group oldGroup, Group group) =>
            _publisher.UpdateGroup(oldGroup, group);

        #endregion

        #region Update

        public User Update(User user) =>
            GetUsers().First(u => u.Name == user.Name);
        public Conversation Update(Conversation conversation) =>
            GetConversations().First(c => c.Id == conversation.Id);

        #endregion
        public void ChangeUser(User user) => CurrentUser = user;

        public List<User> FindFriendsOfUser(User user)
        {
            var totalList = GetFriends();
            var users = GetUsers();
            List<User> friends = new List<User>();

            List<Tuple<int, int>> list1 = totalList.FindAll(X => X.Item1 == user.Id);
            List<Tuple<int, int>> list2 = totalList.FindAll(X => X.Item2 == user.Id);

            List<int> ids = new List<int>();
            ids.AddRange(list1.Select(x => x.Item2));
            ids.AddRange(list2.Select(x => x.Item1));

            foreach(int id in ids)
                friends.Add(users.Find(x => x.Id == id));

            return friends;
        }

        public List<Conversation> FindConversationsOfUser(User user) =>
            GetConversations().Where(c => c.member1.Id == user.Id || c.member2.Id == user.Id).ToList();

        public void ConvertIntoLocalClasses()
        {
            var conversations = new List<Conversation>();
            var conversationsData = GetConversationsData();
            var messagesData = GetMessagesData();
            var users = GetUsers();

            foreach (ConversationData cd in conversationsData)
            {
                Conversation conversation = new Conversation (
                    cd.c_id,
                    users.Find(c=>c.Id == cd.u1_id),
                    users.Find(c=>c.Id == cd.u2_id));
                conversations.Add(conversation);
            }

            foreach (MessageData md in messagesData)
            {
                Message message = new Message(md.m_id, md.text, md.dt, md.isFromMember1);
                Conversation existingConversation = conversations.Find(c => c.Id == md.c_id);
                List<Message> existingMessages = existingConversation.messages;
                
                if(existingMessages != null)
                    existingMessages.Add(message);
                else
                    existingMessages = new List<Message> { message };

                existingConversation.messages = existingMessages;
            }

            _conversations = conversations;
        }

        public List<Group> FindGroupsOfUser(User user)
        {
            var ugroups = GetUGroups();
            var groups = GetGroups();

            List<Group> resGroups = new List<Group>();

            List<Tuple<int, int>> list1 = ugroups.FindAll(X => X.Item1 == user.Id);
            
            List<int> ids = list1.Select(x => x.Item2).ToList();
            
            foreach (int id in ids)
                resGroups.Add(groups.Find(x => x.Id == id));

            return resGroups;
        }

        public User FindUserByName(string name)
        {
            var users = GetUsers();
            return users.Find(X => X.Name == name);
        }
    }
}
