using SocialNetwork.Data.Database;
using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
	class GeneratorLoader : ILoader
	{
        public List<ConversationData> LoadConversationsData()
        {
            throw new NotImplementedException();
        }

        public List<User> LoadFriends() => Test.GenerateUsers();

		public List<Group> LoadGroups() => Test.GenerateGroups();

        public List<MessageData> LoadMessagesData()
        {
            throw new NotImplementedException();
        }

        public User LoadUser() => Test.GenerateUser();

		public User LoadUser(string name)
		{
			throw new NotImplementedException();
		}

        public List<Tuple<int, int>> LoadUserFriends()
        {
            throw new NotImplementedException();
        }

        public List<Tuple<int, int>> LoadUserGroups()
        {
            throw new NotImplementedException();
        }

        public List<User> LoadUsers()
        {
            throw new NotImplementedException();
        }
    }
}
