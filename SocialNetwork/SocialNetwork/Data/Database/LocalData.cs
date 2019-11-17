using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SocialNetwork.Data.Database
{   
    public class LocalData
    {
		private User CurrentUser;

        public List<User> Users { get; private set; }
        public List<Group> Groups { get; private set; }
		public List<Tuple<int, int>> Friends { get; private set; }
		public List<Tuple<int, int>> UGroups { get; private set; }
		public List<Conversation> Conversations { get; private set; }
		public List<ConversationData> ConversationsData { get; private set; }
		public List<Message> Messages { get; private set; }
		public List<MessageData> MessagesData { get; private set; }

		public void ChangeUser(User user)
		{
			CurrentUser = user;

			if (CurrentUser != null)
				Conversations = Conversations.Where(c => c.member1.Id == CurrentUser.Id || c.member2.Id == CurrentUser.Id).ToList();

            ConvertIntoLocalClasses();
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

            List<Tuple<int, int>> list1 = UGroups.FindAll(X => X.Item1 == user.Id);
            
            List<int> ids = list1.Select(x => x.Item2).ToList();
            
            foreach (int id in ids)
                groups.Add(Groups.Find(x => x.Id == id));

            return groups;
        }

        public User FindUserByName(string name) => Users.Find(X => X.Name == name);

        public void SyncWithServer (
            List<ConversationData> conversations,
            List<Group> groups,
            List<MessageData> md,
            List<Tuple<int, int>> newUserFriends,
            List<Tuple<int, int>> newUserGroups,
            List<User> newUsers)
        {
			ConversationsData = conversations;
			Groups = groups;
			MessagesData = md;
			Friends = newUserFriends;
			UGroups = newUserGroups;
			Users = newUsers;

			ConvertIntoLocalClasses();
        }
    }
}
