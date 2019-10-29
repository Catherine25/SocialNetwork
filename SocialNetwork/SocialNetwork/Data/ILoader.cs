using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
	interface ILoader
	{
		User LoadUser(string name);
		List<User> LoadFriends();
		List<Group> LoadGroups();
	}
}
