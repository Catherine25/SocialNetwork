//using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using SocialNetwork.Data.Database;

namespace SocialNetwork.Data
{
	struct UserFields
	{
		public string name;
		public string link;
		public string bio;
	}

	struct GroupFields
	{
		public string title;
	}

	class SQLLoader : ILoader
	{
		string connectionString;
		List<User> users = new List<User>();
		SQLCommands sqlCommands = new SQLCommands();

		public SQLLoader()
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

			LoadUsers(connectionString);
        }

		private string GetDataByReader(MySqlDataReader reader, string fieldName) => reader.GetString(reader.GetOrdinal(fieldName));

        private void LoadUsers(string connectionString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().SelectAllUsers, connection))
            {
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    if (reader.HasRows) // Check is the reader has any rows at all before starting to read.
						while (reader.Read())
							users.Add(
								ConstructUser(
									new UserFields
									{
										name = GetDataByReader(reader, sqlCommands.User_Username),
										bio = GetDataByReader(reader, sqlCommands.User_Bio),
										link = GetDataByReader(reader, sqlCommands.User_AvatarLink)
									}));
            }
        }

		private void LoadGroups(string connectionString)
		{
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().SelectAllUsers, connection))
			{
				connection.Open();

				using (MySqlDataReader reader = cmd.ExecuteReader())
					if (reader.HasRows) // Check is the reader has any rows at all before starting to read.
						while (reader.Read())
							users.Add(
								ConstructUser(
									new UserFields
									{
										name = GetDataByReader(reader, sqlCommands.User_Username),
										bio = GetDataByReader(reader, sqlCommands.User_Bio),
										link = GetDataByReader(reader, sqlCommands.User_AvatarLink)
									}));
			}
		}

		private User ConstructUser(UserFields userFields) => new User(userFields.link, userFields.name, userFields.bio);

		public List<User> LoadFriends()
		{
			throw new NotImplementedException();
		}

		public List<Group> LoadGroups()
		{
			throw new NotImplementedException();
		}

		public User LoadUser(string name) => users.Find(X => X.Name == name);
	}
}
