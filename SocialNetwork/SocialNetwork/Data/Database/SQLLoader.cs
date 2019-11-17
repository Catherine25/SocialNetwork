//using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using SocialNetwork.Data.Database;
using System.Threading;
using System.Linq;

namespace SocialNetwork.Data
{
	public class SQLLoader
	{
        private Publisher _publisher;
        private LocalData _localData;
        private string connectionString;
        private SQLCommands sqlCommands = new SQLCommands();
		private TimeSpan timer;
		private bool suspended = true;

		public SQLLoader(LocalData localData, TimeSpan timeSpan)
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

			timer = timeSpan;
			_localData = localData;
            _publisher = new Publisher(connectionString);

			SyncWithServerImmediately();
            new Thread(SyncWithServer).Start();
        }

        #region Single Update Request

        public void AddEmptyConversation(Conversation newC)
        {
            if (_localData.Conversations.Any(c =>
                (c.member1 == newC.member1 && c.member2 == newC.member2) ||
                (c.member1 == newC.member2 && c.member2 == newC.member1)))
                return;

            _publisher.PublishConversation(newC);
        }

        public void DeleteConversation(Conversation conversation) => _publisher.DeleteConversation(conversation);

        public void AddNewFriend(User u1, User u2) =>
            _publisher.PublishFriendship(u1, u2);

        public void AddNewMessage(Message message, Conversation conversation) =>
            _publisher.PublishMessage(conversation, message);

        #endregion

        #region Collections

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

		#endregion Collections

		public void SetupTimer(TimeSpan span) => timer = span;

		private void SyncWithServer()
        {
            DateTime initial = DateTime.Now;

            while (!suspended)
                if (initial + timer <= DateTime.Now)
				{
					SyncWithServerImmediately();
					initial = DateTime.Now;
				}
        }

		private void SyncWithServerImmediately() => _localData.SyncWithServer(
			LoadConversationsData(),
			LoadGroups(),
			LoadMessagesData(),
			LoadUserFriends(),
			LoadUserGroups(),
			LoadUsers());
	}
}
