//using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Data
{
	class SQLLoader : ILoader
	{
        public SQLLoader()
		{
			string server = "35.180.63.12";
			string database = "Kate";
			string uid = "Admin";
			string password = "3555serg";
			string connectionString =
                "SERVER=" + server +
                "; PORT = 3306 ;" +
                "DATABASE=" + database + ";" +
                "UID=" + uid + ";" +
                "PASSWORD=" + password + ";";

            LoadUsersFromDB(connectionString);
        }

        private void LoadUsersFromDB(string connectionString)
        {
            ConnectToDB(connectionString, SelectAllUsers, User_Username);
        }

        private void ConnectToDB(string connectionString, string sqlCommand, string field)
        {
            List<string> items = new List<string>();

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
                            // To avoid unexpected bugs access columns by name.
                            items.Add(reader.GetString(reader.GetOrdinal(field)));
                        }
                    }
                }
            }
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }

		public IEnumerable<User> LoadFriends()
		{
			throw new NotImplementedException();
		}

		public List<Group> LoadGroups()
		{
			throw new NotImplementedException();
		}

		public User LoadUser()
		{
			throw new NotImplementedException();
		}

		List<User> ILoader.LoadFriends()
		{
			throw new NotImplementedException();
		}
    }
}
