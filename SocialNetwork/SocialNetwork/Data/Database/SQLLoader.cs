using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using SocialNetwork.Data.Database;

namespace SocialNetwork.Data
{
    public class SQLLoader
	{
        public enum DataType { ConversationsData, Groups, MessagesData, UserFriends, UserGroups, Users, All }

        private string _connectionString;
        private SQLCommands sqlCommands = new SQLCommands();

        public SQLLoader(string connectionString) => _connectionString = connectionString;

        #region Collections

        public List<User> LoadUsers()
        {
            List<User> users = new List<User>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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

			using (MySqlConnection connection = new MySqlConnection(_connectionString))
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

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
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

		#endregion Collections
    }
}
