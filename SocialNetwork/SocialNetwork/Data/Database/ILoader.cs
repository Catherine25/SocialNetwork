using SocialNetwork.Data.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
	interface ILoader
	{
		List<User> LoadUsers();
		List<Group> LoadGroups();
        List<Tuple<int, int>> LoadUserFriends();
        List<Tuple<int, int>> LoadUserGroups();
        List<ConversationData> LoadConversationsData();
        List<MessageData> LoadMessagesData();
    }
}
