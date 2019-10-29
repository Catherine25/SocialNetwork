using SocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
	class GeneratorLoader : ILoader
	{
		public List<User> LoadFriends() => Test.GenerateUsers();

		public List<Group> LoadGroups() => Test.GenerateGroups();

		public User LoadUser() => Test.GenerateUser();

		public User LoadUser(string name)
		{
			throw new NotImplementedException();
		}
	}
}
