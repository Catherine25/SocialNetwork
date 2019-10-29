//using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using SocialNetwork.Data.Database;

namespace SocialNetwork.Data
{
	class SQLLoader : ILoader
	{
		string connectionString;
		List<string> items = new List<string>();

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
        }

        private void ConnectToDB(string connectionString, string sqlCommand, string field)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(sqlCommand, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
							if(field == null || field == "")
							{
								string whatIsThis = reader.ToString();
								//string name = reader.().ToString();
							}
							else
							{
								// To avoid unexpected bugs access columns by name.
								items.Add(reader.GetString(reader.GetOrdinal(field)));
							}
                        }
                    }
                }
            }
        }

		public List<User> LoadFriends()
		{
			throw new NotImplementedException();
		}

		public List<Group> LoadGroups()
		{
			throw new NotImplementedException();
		}

		public User LoadUser(string name)
		{
			ConnectToDB(connectionString,
				new SQLCommands().SelectAllUsers,
				"");
			items.Find(X => X == name);
			return new User(null, null, null);
		}
    }
}
