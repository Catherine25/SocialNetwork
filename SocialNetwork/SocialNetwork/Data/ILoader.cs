using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
	interface ILoader
	{
		User LoadUser();
		List<User> LoadFriends();
		List<Group> LoadGroups();
	}
}
