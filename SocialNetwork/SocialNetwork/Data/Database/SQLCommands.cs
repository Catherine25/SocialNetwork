using System;

namespace SocialNetwork.Data.Database
{
	class SQLCommands
	{
		public string SelectAllUsers = "SELECT * FROM Users;";
		public int User_u_id = 0; //"u_id"
		public string User_AvatarLink = "AvatarLink";
		public string User_Username = "username";
		public string User_Bio = "Bio";

		public string SelectAllConversations = "SELECT * FROM Conversation;";
		public int Converastion_c_id = 0; //"c_id"
		public int Converastion_u1_id = 0; //"u1_id"
		public int Converastion_u2_id = 0; //"u2_id"

		public string SelectAllMessages = "SELECT * FROM Message;";
		public int Message_m_id = 0; //"m_id"
		public int Message_c_id = 0; //"Conversation_c_id"
		public string Message_message = "message";
		public DateTime Message_datetime = DateTime.MinValue; //"dt"
		public bool Message_isFormMember = false; //"isFormMember"

        public string AddUser(User user) =>
            "INSERT INTO Users (AvatarLink, username, Bio) VALUES ('" + user.AvatarLink + "', '" + user.Name + "', '" + user.Bio + "');";
        
        public string AddFriendship(int u1Id, int u2Id) =>
            "INSERT INTO Friends (Users_u_id, f_id) VALUES (" + u1Id + ", " + u2Id + ");";
		public string AddConversation(int member1Id, int member2Id) =>
			"INSERT INTO Conversation (u1_id, u2_id) VALUES (" + member1Id + ", " + member2Id + ");";
        public string AddMessage(Message m, int cId) =>
            "INSERT INTO Message (Conversation_c_id, message, dt, isFromMember1) VALUES ('" + cId + "', '" + m.Text + "', '" + m.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', " + (m.IsFromMember1 ? 1 : 0) + ");";

        public string UpdateUser(User oldUser, User newUser) =>
            "UPDATE Users SET AvatarLink='" + newUser.AvatarLink + "', username='" + oldUser.Name + "', Bio='" + newUser.Bio + "' WHERE u_id=" + oldUser.Id + ";";
    }
}
