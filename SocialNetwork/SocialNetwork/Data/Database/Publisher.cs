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

        public void PublishConversation(Conversation c) =>
            ExecuteNonQuery(_commands.AddConversation(c.member1.Id, c.member2.Id));

        public void DeleteConversation(Conversation c) =>
            ExecuteNonQuery(_commands.DeleteConversation(c.Id));

        public void PublishGroup(Group group) =>
            ExecuteNonQuery(_commands.AddGroup(group));

        public void PublishFriendship(User u1, User u2) =>
            ExecuteNonQuery(_commands.AddFriendship(u1.Id, u2.Id));

        public void DeleteFriendship(User u1, User u2) =>
            ExecuteNonQuery(_commands.DeleteFriendOfUser(u1.Id, u2.Id));

        public void PublishUserToGroup(User user, Group group) =>
            ExecuteNonQuery(_commands.AddUserToGroup(user.Id, group.Id));

        public void DeleteUserFromGroup(User user, Group group) =>
            ExecuteNonQuery(_commands.UnsubscribeUserFromGroup(user.Id, group.Id));

        public void PublishMessage(Conversation c, Message message) =>
            ExecuteNonQuery(_commands.AddMessage(message, c.Id));

        public void PublishUser(User user) =>
            ExecuteNonQuery(_commands.AddUser(user));

        public void UpdateUser(User oldUser, User user) =>
            ExecuteNonQuery(_commands.UpdateUser(oldUser, user));

        public void UpdateGroup(Group oldGroup, Group group) =>
            ExecuteNonQuery(_commands.UpdateGroup(oldGroup, group));
    }
}
