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

		public string AddUser = "INSERT INTO Users (AvatarLink, username, Bio) VALUES (?, ?, ?);";
		public string AddGroup = "INSERT INTO Groups (AvatarLink, Title, Description) VALUES (?, ?, ?);";
		public string AddUserToGroup = "INSERT INTO users_groups (Users_u_id, Groups_g_id) VALUES (?, ?);";
		public string AddFriendship = "INSERT INTO Friends (Users_u_id, f_id) VALUES (?, ?);";
		public string AddConversation(Conversation conversation) =>
			"INSERT INTO Conversation (u1_id, u2_id) VALUES (" + conversation.member1.Id + ", " + conversation.member2.Id + ");";
		public string AddMessage = "INSERT INTO Message (Conversation_c_id, message, dt, isFormMember) VALUES (?, ?, ?, ?);";

		public static string UpdateUser = "UPDATE Users SET AvatarLink=?, username=?, Bio=? WHERE u_id=?;";
		public static string UpdateGroup = "UPDATE Groups SET AvatarLink=?, Title=?, Description=? WHERE g_id=?;";

		public static string DeleteUser = "DELETE FROM Users where u_id=?;";
		public static string DeleteGroup = "DELETE FROM Groups where g_id=?;";
		public static string DeleteFriendOfUser = "DELETE FROM Friends where Users_u_id=? and f_id=?;";
		public static string UnsubscribeUserFromGroup = "DELETE FROM users_groups where Groups_g_id=? and Users_u_id=?;";
		public static string DeleteConveration = "DELETE FROM Conversation where c_id=?;";
	}
}
