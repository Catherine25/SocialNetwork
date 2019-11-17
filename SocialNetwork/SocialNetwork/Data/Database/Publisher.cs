using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data.Database
{
    class Publisher
    {
        private string _connectionString;

        public Publisher(string connectionString) => _connectionString = connectionString;

        public void PublishConversation(Conversation conversation)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().AddConversation(conversation), connection))
            {
                connection.Open();

                int number = cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void DeleteConversation(Conversation conversation)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().DeleteConversation(conversation), connection))
            {
                connection.Open();

                int number = cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void PublishFriendship(User u1, User u2)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().AddFriendship(u1, u2), connection))
            {
                connection.Open();

                int number = cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void DeleteFriendship(User u1, User u2)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().DeleteFriendOfUser(u1, u2), connection))
            {
                connection.Open();

                int number = cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void PublishUserToGroup(User user, Group group)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().AddUserToGroup(user, group), connection))
            {
                connection.Open();

                int number = cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void DeleteUserFromGroup(User user, Group group)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().UnsubscribeUserFromGroup(user, group), connection))
            {
                connection.Open();

                int number = cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void PublishMessage(Conversation conversation, Message message)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(new SQLCommands().AddMessage(message, conversation), connection))
            {
                connection.Open();

                int number = cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
