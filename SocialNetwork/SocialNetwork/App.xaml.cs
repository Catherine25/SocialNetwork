using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MySql.Data.MySqlClient;
using SocialNetwork.Data;
using SocialNetwork.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork
{
    public partial class App : Application
    {
        #region Variables
        NewMessagesImitator _bot;
        Themes _themes;
        User _currentUser;

        List<Conversation> _conversations = new List<Conversation>();
        #endregion

        ///<summary> Constructor </summary>
        public App()
        {
            InitializeComponent();

            _currentUser = Test.GenerateUser();
            _currentUser.Friends = Test.GenerateUsers();
            _currentUser.Groups = Test.GenerateGroups();
            _themes = new Themes();
            _bot = new NewMessagesImitator(_currentUser, DateTime.Now, TimeSpan.FromSeconds(5));
            _bot.MessageGenerated += BotGeneratedMessage;

            MainPage = new MainPage(_currentUser, _themes.ThemesList[0], _conversations);

            List<Conversation> conversations = Test.GenerateConversations(_currentUser).ToList();
            foreach (Conversation c in conversations)
                TryAddConversation(c);
            string cs = @"server=35.180.63.12;userid=Admin;password=3555serg;database=Kate";

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                Console.WriteLine("MySQL version : {0}", conn.ServerVersion);

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());

            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void TryAddConversation(Conversation conversation)
        {
            Debug.WriteLine("TryAddConversation() running");

            User user1 = conversation.member1;
            User user2 = conversation.member2;

            Debug.WriteLine("user1 = " + user1 + " user2 = " + user2);

            Conversation existing;
            existing = _conversations.Find(X => X.member1.Name == user1.Name && X.member2.Name == user2.Name);

            if (existing == null)
            {
                Debug.WriteLine("existing conversation not found");
                existing = _conversations.Find(X => X.member1.Name == user2.Name && X.member2.Name == user1.Name);
            }

            if (existing == null)
            {
                Debug.WriteLine("existing conversation not found again, adding");
                _conversations.Add(conversation);
            }
            else
            {
                Debug.WriteLine("existing conversation_1 found");
                existing.messages.AddRange(conversation.messages);
            }
        }

        ///<summary> Rises after NewMessagesImitator.MessageGenerated </summary>
        private void BotGeneratedMessage(Message message, User author)
        {
            message.IsFromMember1 = true;
            TryAddConversation(new Conversation(author, _currentUser, new List<Message> { message }));
        }

        #region Overridings
        protected override void OnStart()
        {
            new Thread(_bot.TryWork).Start();

            new Thread(_themes.LoadRomanukeThemes).Start();
        }

        protected override void OnSleep() => _bot.SuspendRequest();

        protected override void OnResume() { /* Handle when your app resumes */ }
        #endregion
    }
}
