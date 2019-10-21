using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SocialNetwork.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SocialNetwork
{
    public partial class App : Application
    {
        #region Variables
        Data.User currentUser;
        List<Data.Conversation> conversations;
        Services.NewMessagesImitator bot;
        Data.Themes themes;
        #endregion

        ///<summary> Constructor </summary>
        public App()
        {
            InitializeComponent();

            currentUser = Services.Test.GenerateUser();
            currentUser.Friends = Services.Test.GenerateUsers();
            currentUser.Groups = Services.Test.GenerateGroups();
            conversations = Services.Test.GenerateConversations(currentUser).ToList();
            themes = new Themes();
            bot = new Services.NewMessagesImitator(currentUser, DateTime.Now, TimeSpan.FromSeconds(5));
            bot.MessageGenerated += BotGeneratedMessage;

            MainPage = new MainPage(currentUser, themes.ThemesList[0], conversations);
        }

        ///<summary> Rises after NewMessagesImitator.MessageGenerated </summary>
        private void BotGeneratedMessage(Message message, User author)
        {
            Debug.WriteLine("Trying to find conversation by member1... ");

            Conversation existingConversation = conversations.Find(c=>c.member1 == author);
            
            if(existingConversation == null)
            {
                Debug.Write("Not found. Trying to find by member2...");
                conversations.Find(X=>X.member2 == author);
            }

            if(existingConversation == null)
            {
                Debug.Write("Not found. Creating new.");

                Conversation newConversation;

                if(message.IsFromMember1)
                    newConversation = new Conversation(author, currentUser, new List<Message> { message });
                else
                    newConversation = new Conversation(currentUser, author, new List<Message> { message });
                
                this.conversations.Add(newConversation);
            }
            else
            {
                Debug.Write("Found. Saving.");
                message.IsFromMember1 = existingConversation.member1 == author ? true : false;
                existingConversation.messages.Add(message);
            }
        }

        #region Overridings
        protected override void OnStart()
        {
            new Thread(bot.TryWork).Start();

            new Thread(themes.LoadRomanukeThemes).Start();
        }

        protected override void OnSleep() => bot.SuspendRequest();

        protected override void OnResume() { /* Handle when your app resumes */ }
        #endregion
    }
}
