//using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using SocialNetwork.Data.Database;
using System.Threading;

namespace SocialNetwork.Data
{
	class SQLLoader : ILoader
	{
        private LocalData _localData;
        private string connectionString;
        private SQLCommands sqlCommands = new SQLCommands();

		public SQLLoader(LocalData localData)
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

            new Thread(SyncWithServer).Start();
        }

        public List<User> LoadUsers()
        {
            List<User> users = new List<User>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().SelectAllUsers, connection))
            {
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    if (reader.HasRows) // Check is the reader has any rows at all before starting to read.
                        while (reader.Read())
                            users.Add(new User(
                                reader.GetInt32(reader.GetOrdinal("u_id")),
                                reader.GetString(reader.GetOrdinal(sqlCommands.User_AvatarLink)),
                                reader.GetString(reader.GetOrdinal(sqlCommands.User_Username)),
                                reader.GetString(reader.GetOrdinal(sqlCommands.User_Bio))));

                connection.Close();
            }

            return users;
        }

		public List<Group> LoadGroups()
		{
            List<Group> groups = new List<Group>();

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().SelectAllGroups, connection))
			{
				connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    if (reader.HasRows) // Check is the reader has any rows at all before starting to read.
                        while (reader.Read())
                            groups.Add(new Group(
                                reader.GetInt32(reader.GetOrdinal("g_id")),
                                reader.GetString(reader.GetOrdinal(sqlCommands.Group_Title)),
                                reader.GetString(reader.GetOrdinal(sqlCommands.Group_Description)),
                                reader.GetString(reader.GetOrdinal(sqlCommands.Group_AvatarLink))));
                
                connection.Close();
            }

            return groups;
		}

        public List<Tuple<int, int>> LoadUserFriends()
        {
            List<Tuple<int, int>> friends = new List<Tuple<int, int>>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().SelectAllFriends, connection))
            {
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    if (reader.HasRows) // Check is the reader has any rows at all before starting to read.
                        while (reader.Read())
                            friends.Add(new Tuple<int, int>(
                                reader.GetInt32(reader.GetOrdinal("Users_u_id")),
                                reader.GetInt32(reader.GetOrdinal("f_id"))));
            }

            return friends;
        }

        public List<Tuple<int, int>> LoadUserGroups()
        {
            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().SelectAllusers_groups, connection))
            {
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    if (reader.HasRows) // Check is the reader has any rows at all before starting to read.
                        while (reader.Read())
                            pairs.Add(new Tuple<int, int>(reader.GetInt32(reader.GetOrdinal("Users_u_id")),
                                reader.GetInt32(reader.GetOrdinal("Groups_g_id"))));
            }

            return pairs;
        }

        public List<ConversationData> LoadConversationsData()
        {
            List<ConversationData> pairs = new List<ConversationData>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().SelectAllConversations, connection))
            {
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    if (reader.HasRows) // Check is the reader has any rows at all before starting to read.
                        while (reader.Read())
                            pairs.Add(new ConversationData
                            {
                                c_id = reader.GetInt32(reader.GetOrdinal("c_id")),
                                u1_id = reader.GetInt32(reader.GetOrdinal("u1_id")),
                                u2_id = reader.GetInt32(reader.GetOrdinal("u2_id"))
                            });
            }

            return pairs;
        }

        public List<MessageData> LoadMessagesData()
        {
            List<MessageData> pairs = new List<MessageData>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().SelectAllMessages, connection))
            {
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    if (reader.HasRows) // Check is the reader has any rows at all before starting to read.
                        while (reader.Read())
                            pairs.Add(new MessageData
                            {
                                m_id = reader.GetInt32(reader.GetOrdinal("m_id")),
                                c_id = reader.GetInt32(reader.GetOrdinal("Conversation_c_id")),
                                text = reader.GetString(reader.GetOrdinal("message")),
                                dt = reader.GetDateTime(reader.GetOrdinal("dt")),
                                isFromMember1 = reader.GetBoolean(reader.GetOrdinal("isFromMember1"))
                            });
            }

            return pairs;
        }

        private void SyncWithServer()
        {
            DateTime initial = DateTime.Now;
            TimeSpan timer = TimeSpan.FromSeconds(5);

            List<ConversationData> newConversations;
            List<Group> newGroups;
            List<MessageData> newMessages;
            List<Tuple<int, int>> newUserFriends;
            List<Tuple<int, int>> newUserGroups;
            List<User> newUsers;

            while (true)
            {
                if (initial + timer <= DateTime.Now)
                {
                    newConversations = LoadConversationsData();
                    newGroups = LoadGroups();
                    newMessages = LoadMessagesData();
                    newUserFriends = LoadUserFriends();
                    newUserGroups = LoadUserGroups();
                    newUsers = LoadUsers();
                    _localData.SyncWithServer(newConversations, newGroups, newMessages, newUserFriends, newUserGroups, newUsers);
                }
            }
        }
    }
}
