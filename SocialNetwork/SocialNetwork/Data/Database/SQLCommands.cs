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

		public string SelectAllGroups = "SELECT * FROM Groups;";
		public int Group_g_id = 0; //"g_id"
		public string Group_AvatarLink = "AvatarLink";
		public string Group_Title = "Title";
		public string Group_Description = "Description";

		public string SelectAllusers_groups = "SELECT * FROM users_groups;";
		public int Groups_g_id = 0; // "Groups_g_id"
		public int Users_u_id = 0; // "Users_u_id"

		public string SelectAllFriends = "SELECT * FROM Friends;";
		public int Friends_u_id = 0; //"Users_u_id"
		public int Friends_f_id = 0; //"f_id"

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
        public string AddGroup(Group group) =>
            "INSERT INTO Groups (AvatarLink, Title, Description) VALUES ('" + group.AvatarLink + "', '" + group.Title + "', '" + group.Description + "');";
        public string AddUserToGroup(int uId, int gId) =>
            "INSERT INTO users_groups (Users_u_id, Groups_g_id) VALUES (" + uId + ", " + gId + ");";
        public string AddFriendship(int u1Id, int u2Id) =>
            "INSERT INTO Friends (Users_u_id, f_id) VALUES (" + u1Id + ", " + u2Id + ");";
		public string AddConversation(int member1Id, int member2Id) =>
			"INSERT INTO Conversation (u1_id, u2_id) VALUES (" + member1Id + ", " + member2Id + ");";
        public string AddMessage(Message m, int cId) =>
            "INSERT INTO Message (Conversation_c_id, message, dt, isFromMember1) VALUES ('" + cId + "', '" + m.Text + "', '" + m.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', " + (m.IsFromMember1 ? 1 : 0) + ");";

        public string UpdateUser(User oldUser, User newUser) =>
            "UPDATE Users SET AvatarLink='" + newUser.AvatarLink + "', username='" + oldUser.Name + "', Bio='" + newUser.Bio + "' WHERE u_id=" + oldUser.Id + ";";
        public string UpdateGroup(Group oldGroup, Group group) =>
            "UPDATE Groups SET AvatarLink='" + group.AvatarLink + "', Title='" + oldGroup.Title + "', Description='" + group.Description + "' WHERE g_id=" + oldGroup.Id + ";";

		public string DeleteUser = "DELETE FROM Users where u_id=?;";
		public string DeleteGroup = "DELETE FROM Groups where g_id=?;";
        public string DeleteFriendOfUser(int u1Id, int u2Id) =>
            "DELETE FROM Friends where Users_u_id=" + u1Id + " and f_id=" + u2Id + ";";
        public string UnsubscribeUserFromGroup(int uId, int gId) =>
            "DELETE FROM users_groups where Groups_g_id=" + gId + " and Users_u_id=" + uId + ";";
        public string DeleteConversation(int cId) =>
            "DELETE FROM Conversation where c_id=" + cId + ";";
	}
}
