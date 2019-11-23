using MySql.Data.MySqlClient;

namespace SocialNetwork.Data.Database
{
    class Publisher
    {
        private readonly string _connectionString;
        private readonly SQLCommands _commands;

        public Publisher(string connectionString)
        {
            _connectionString = connectionString;
            _commands = new SQLCommands();
        }

        private void ExecuteNonQuery(string command)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(command, connection))
            {
                connection.Open();

                int number = cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void PublishConversation(Conversation conversation) =>
            ExecuteNonQuery(_commands.AddConversation(conversation));

        public void DeleteConversation(Conversation conversation) =>
            ExecuteNonQuery(_commands.DeleteConversation(conversation));

        public void PublishGroup(Group group) =>
            ExecuteNonQuery(_commands.AddGroup(group));

        public void PublishFriendship(User u1, User u2) =>
            ExecuteNonQuery(_commands.AddFriendship(u1, u2));

        public void DeleteFriendship(User u1, User u2) =>
            ExecuteNonQuery(_commands.DeleteFriendOfUser(u1, u2));

        public void PublishUserToGroup(User user, Group group) =>
            ExecuteNonQuery(_commands.AddUserToGroup(user, group));

        public void DeleteUserFromGroup(User user, Group group) =>
            ExecuteNonQuery(_commands.UnsubscribeUserFromGroup(user, group));

        public void PublishMessage(Conversation conversation, Message message) =>
            ExecuteNonQuery(_commands.AddMessage(message, conversation));

        public void PublishUser(User user) =>
            ExecuteNonQuery(_commands.AddUser(user));

        public void UpdateUser(User oldUser, User user) =>
            ExecuteNonQuery(_commands.UpdateUser(oldUser, user));
    }
}
